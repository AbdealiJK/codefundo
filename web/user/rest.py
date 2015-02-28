# Django
from django.utils.html import strip_tags
from django.conf import settings
from django.views.decorators.csrf import csrf_exempt
from django.core.files import File
# Apps
from rest_framework import viewsets
from rest_framework import status
from rest_framework.response import Response
from rest_framework.renderers import JSONRenderer
from annoying.functions import get_object_or_None
from user.models import TempUser, Room, Size, BoundingBox, CONFIGURATION_CHOICES
from user.serializers import TempUserSerializer, RoomSerializer, SizeSerializer, BoundingBoxSerializer
from user.utils import *
# Python
import HTMLParser
import urllib
import os
import random
import datetime

MIN_KEY = 100000
MAX_KEY = 999999

class TempUserViewSet(viewsets.ViewSet):
    """
        Information about a specific user
    """
    def list(self, request):
        _key = request.POST.get('key', None)
        if _key == None:
            users = TempUser.objects.all()
            users_data = TempUserSerializer(users, many=True).data
            return Response(viewset_response("done", users_data))
        else:
            user = get_object_or_None(TempUser, key=_key)
            if user == None:
                return Response(viewset_response(
                    "We could not find any such user", {}))
            user_data = UserSerializer(user).data
            return Response(viewset_response("done", user_data))

    def create(self, request):
        _type = request.POST.get('type', None)
        if _type == 'create':
            # THe size is always calculated with portrait mode.
            _size_x = request.POST.get('size_x', None)
            _size_y = request.POST.get('size_y', None)

            try:
                _size_y = float(_size_y)
                _size_x = float(_size_x)
                assert(_size_y > 0 and _size_x > 0)
            except TypeError:
                return Response(viewset_response(
                    "Size data given is invalid", {}))
            except AssertionError:
                return Response(viewset_response(
                    "Size data given cannot be negative", {}))

            r = 0
            max_trials = 100

            # Delete all old users
            expiry_date = datetime.datetime.now() + datetime.timedelta(minutes=60*12, days=0)
            TempUser.objects.filter(date_created__gte=expiry_date).delete()

            # Get a unique key
            while r < MIN_KEY or r > (MAX_KEY+1) or \
                get_object_or_None(TempUser, key=r) != None:

                r = random.randint(MIN_KEY, MAX_KEY+1)
                max_trials -= 1
                if max_trials < 0:
                    return Response(viewset_response(
                        "Sorry, all our slots are full. Try again later",
                        {}))
            # Create a user and save size, bbox and key
            user = TempUser.objects.create(key=r)
            user.bounding_box = BoundingBox.objects.create()
            user.size = Size.objects.create(width=_size_x, height=_size_y)
            user.save()
            user.size.save()
            user_data = TempUserSerializer(user).data
            return Response(viewset_response("done", user_data))
        else:
            return Response(viewset_response("Invalid type given", {}))


class RoomViewSet(viewsets.ViewSet):
    """
        Information about a specific room

        GET :
            Gives data about all rooms
    """
    def list(self, request):
        _id = request.POST.get('id', None)
        if _id == None:
            rooms = Room.objects.all()
            rooms_data = RoomSerializer(rooms, many=True).data
            return Response(viewset_response("done", rooms_data))
        else:
            room = get_object_or_None(Room, id=_id)
            if room == None:
                return Response(viewset_response(
                    "We could not find any such room", {}))
            room_data = RoomSerializer(room).data
            return Response(viewset_response("done", room_data))

    def create(self, request):
        _key = request.POST.get('key', None)
        _type = request.POST.get('type', None)
        if _key == None:
            return Response(viewset_response(
                "Your key is required to edit a room", {}))
        user = get_object_or_None(TempUser, key=_key)
        if user == None:
            return Response(viewset_response(
                "We could not find you! Try refreshing your key",
                {}))
        if _type == "create":
            _configuration = request.POST.get('configuration', None)

            # Delete all old rooms
            expiry_date = datetime.datetime.now() + datetime.timedelta(minutes=60*12, days=0)
            Room.objects.filter(date_created__gte=expiry_date).delete()

            room = Room.objects.create()
            room.user.add(user)

            all_configs = [str(CONFIGURATION_CHOICES[i][0]) for i in xrange(len(CONFIGURATION_CHOICES))]
            if str(_configuration) not in all_configs:
                return Response(viewset_response(
                    "The configuration specified is not valid",
                    {}))
            room.configuration = _configuration
            room.save()
            room_data = RoomSerializer(room).data
            return Response(viewset_response("done", room_data))
        elif _type == "add":
            _room_id = request.POST.get('room_id', None)
            _position = request.POST.get('position', None)
            room = get_object_or_None(Room, id=_room_id)

            # Check if room exists
            if room == None:
                return Response(viewset_response(
                    "We could not find any such room", {}))
            _new_key = request.POST.get('new_key', None)

            # Check that the user exists and is not in any other room
            new_user = get_object_or_None(TempUser, key=_new_key)
            if new_user == None:
                return Response(viewset_response(
                    "We could not find the user to add", {}))
            if new_user.room.count() > 0:
                return Response(viewset_response(
                    "This user is already in another room. You may want to refresh his token",
                    {}))

            # Save the position that needs to be set and add ot the room
            new_user.position = _position
            new_user.save()
            room.user.add(new_user)

            room_data = RoomSerializer(room).data
            return Response(viewset_response('done', room_data))
        elif _type == 'file':
            _room_id = request.POST.get('room_id', None)
            room = get_object_or_None(Room, id=_room_id)
            if room == None:
                return Response(viewset_response(
                    'We could not find any such room', {}))
            _file = request.FILES.get('file', None)
            if _file == None:
                return Response(viewset_response(
                    'A file is needed', {}))
            room.shared_file.save(_file.name, _file, save=True)
            # TODO : rename file to avoid clashes
            room.save()
            room_data = RoomSerializer(room).data
            return Response(viewset_response("done", room_data))

class EventViewSet(viewsets.ViewSet):
    def list(self, request):
        _room_id = request.POST.get('room_id', None)
        _user_key = request.POST.get('user_key', None)

        # Check if given data exists
        room = get_object_or_None(Room, id=_room_id)
        if room == None:
            return Response(viewset_response(
                "We could not find any such room", {}))
        user = get_object_or_None(TempUser, key=_user_key)
        if user == None:
            return Response(viewset_response(
                "We could not find any such user", {}))
        if room not in user.room.all():
            return Response(viewset_response(
                "The user is not in the given room", {}))

        bbox = user.bounding_box
        bbox_data = BoundingBoxSerializer(bbox).data
        return Response(viewset_response("done", bbox_data))

    def create(self, request):
        _room_id = request.POST.get('room_id', None)
        _user_key = request.POST.get('user_key', None)

        _bbox_x1 = request.POST.get('bbox_x1', None)
        _bbox_y1 = request.POST.get('bbox_y1', None)
        _bbox_x2 = request.POST.get('bbox_x2', None)
        _bbox_y2 = request.POST.get('bbox_y2', None)

        # Check if given data exists
        room = get_object_or_None(Room, id=_room_id)
        if room == None:
            return Response(viewset_response(
                "We could not find any such room", {}))
        user = get_object_or_None(TempUser, key=_user_key)
        if user == None:
            return Response(viewset_response(
                "We could not find any such user", {}))
        if room not in user.room.all():
            return Response(viewset_response(
                "The user is not in the given room", {}))

        # Save user information
        user.bounding_box.x1 = _bbox_x1
        user.bounding_box.x2 = _bbox_x2
        user.bounding_box.y1 = _bbox_y1
        user.bounding_box.y2 = _bbox_y2
        user.bounding_box.save()
        user.save()

        # This handles all the computations needed and saves the required data 
        # in TempUser models - it modifies the room, user, size tables
        calculate_room(room, user)

        bbox = user.bounding_box
        bbox_data = BoundingBoxSerializer(bbox).data
        return Response(viewset_response("done", bbox_data))




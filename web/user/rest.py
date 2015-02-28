# Django
from django.utils.html import strip_tags
from django.conf import settings
from django.views.decorators.csrf import csrf_exempt
# Apps
from rest_framework import viewsets
from rest_framework import status
from rest_framework.response import Response
from rest_framework.renderers import JSONRenderer
from annoying.functions import get_object_or_None
from user.models import TempUser, Room, Size, CONFIGURATION_CHOICES
from user.serializers import TempUserSerializer, RoomSerializer, SizeSerializer
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
            user = TempUser.objects.create(key=r)
            print user
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

            all_configs = [CONFIGURATION_CHOICES[i][0] for i in xrange(len(CONFIGURATION_CHOICES))]
            if _configuration not in all_configs:
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

            # CHeck that the user exists and is not in any other room
            new_user = get_object_or_None(TempUser, key=_new_key)
            if new_user == None:
                return Response(viewset_response(
                    "We could not find the user to add", {}))
            if new_user.rooms.count() > 0:
                return Response(viewset_response(
                    "This user is already in another room. You may want to refresh his token",
                    {}))

            # Save the position that needs to be set and add ot the room
            new_user.position = _position
            new_user.save()
            room.user.add(new_user)

            room_data = RoomSerializer(room).data
            return Response(viewset_response("done", room_data))
        elif _type == "file":
            _room_id = request.POST.get('room_id', None)
            room = get_object_or_None(Room, id=_room_id)
            if room == None:
                return Response(viewset_response(
                    "We could not find any such room", {}))
            _file = request.FILES.get('file', None)
            print _file
            room.shared_file = _file
            # TODO : rename file to avoid clashes
            room.save()
            return Response(viewset_response("done", room_data))

class SizeViewSet(viewsets.ViewSet):
    """
        Information about a specific room
    """
    def list(self, request):
        rooms = Room.objects.all()
        rooms_data = RoomSerializer(rooms, many=True).data
        return Response(viewset_response("done", rooms_data))


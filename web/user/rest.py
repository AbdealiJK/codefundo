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
from user.models import TempUser, Room, Size
from user.serializers import TempUserSerializer, RoomSerializer, SizeSerializer
from user.utils import *
# Python
import HTMLParser
import urllib
import os
import random

MIN_KEY = 100000
MAX_KEY = 999999

class TempUserViewSet(viewsets.ViewSet):
    """
        Information about a specific user
    """
    def list(self, request):
        tempusers = TempUser.objects.all()
        tempusers_data = TempUserSerializer(tempusers, many=True).data
        return Response(viewset_response("done", tempusers_data))

    def create(self, request):
        r = 0
        max_trials = 100
        TempUser.objects.filter(date_created)
        while r < MIN_KEY and r > MAX_KEY+1 and
            get_object_or_None(TempUser, key=r) != None:

            r = random.randint(MIN_KEY, MAX_KEY+1)
            max_trials -= 1
            if max_trials < 0:
                return Response(viewset_response(
                    "Sorry, all our slots are full. Try again later", {}))
        user = TempUser.objects.create(key=r)
        user_data = TempUserSerializer(user).data
        return Response(viewset_response("done", user_data))

class RoomViewSet(viewsets.ViewSet):
    """
        Information about a specific room
    """
    def list(self, request):
        rooms = Room.objects.all()
        rooms_data = RoomSerializer(rooms, many=True).data
        return Response(viewset_response("done", rooms_data))

    def create(self, request):
        _key = request.POST.get('key', None)
        _type = request.POST.get('type', None)
        if key == None:
            return Response(viewset_response(
                "Your key is required to create a room", {}))
        user = get_object_or_None(TempUser, key=_key)
        if user == None:
            return Response(viewset_response(
                "We could not find you! Try refreshing your key",
                {}))
        if _type == "create":
            room = Room.objects.create()
            room.user.add(user)
            room_data = RoomSerializer(room).data
            return Response(viewset_response("done", room_data))
        elif _type == "add":
            _room_id = request.POST.get('room_id', None)
            room = get_object_or_None(Room, id=_room_id)
            if room == None:
                return Response(viewset_response(
                    "We could not find any such room", {}))
            _new_key = request.POST.get('new_key', None)

            new_user = get_object_or_None(TempUser, key=_new_key)
            if new_user == None:
                return Response(viewset_response(
                    "We could not find the user to add", {}))
            room.user.add(new_user)
            room_data = RoomSerializer(room).data
            return Response(viewset_response("done", room_data))

class SizeViewSet(viewsets.ViewSet):
    """
        Information about a specific room
    """
    def list(self, request):
        rooms = Room.objects.all()
        rooms_data = RoomSerializer(rooms, many=True).data
        return Response(viewset_response("done", rooms_data))







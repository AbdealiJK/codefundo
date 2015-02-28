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
        while r < MIN_KEY and r > MAX_KEY:
            r = random.randint(MIN_KEY, MAX_KEY)

            if get_object_or_None(TempUser, key=r) == None:
                # Got a random key which nobody has
                break
            max_trials -= 1
            if max_trials < 0:
                return Response(viewset_response(
                    "Sorry, all our slots are full. Try again later", {}))
        user = TempUser.objects.create(key=r)
        user_data = TempUserSerializer(user).data
        return Response(viewset_response("done", tempusers_data))

class RoomViewSet(viewsets.ViewSet):
    """
        Information about a specific room
    """
    def list(self, request):
        rooms = Room.objects.all()
        rooms_data = RoomSerializer(rooms, many=True).data
        return Response(viewset_response("done", rooms_data))

class SizeViewSet(viewsets.ViewSet):
    """
        Information about a specific room
    """
    def list(self, request):
        rooms = Room.objects.all()
        rooms_data = RoomSerializer(rooms, many=True).data
        return Response(viewset_response("done", rooms_data))







from rest_framework import serializers
from django.contrib.auth.models import User
from user.models import TempUser, Room, Size


class TempUserKeySerializer(serializers.ModelSerializer):
    user_id = serializers.IntegerField(source='id')
    class Meta:
        model = TempUser
        fields = ('user_id', 'key')

class RoomSerializer(serializers.ModelSerializer):
    users = TempUserKeySerializer(source="user", many=True)
    room_id = serializers.IntegerField(source="id")
    class Meta:
        model = Room
        fields = ('room_id', 'users')

class TempUserSerializer(serializers.ModelSerializer):
    rooms = RoomSerializer(source="room", many=True)
    user_id = serializers.IntegerField(source='id')
    class Meta:
        model = TempUser
        fields = ('user_id', 'key', 'rooms', )

class SizeSerializer(serializers.ModelSerializer):
    class Meta:
        model = Size

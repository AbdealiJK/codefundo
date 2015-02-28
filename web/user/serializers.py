from rest_framework import serializers
from django.contrib.auth.models import User
from user.models import TempUser, Room, Size


class BoundingBox(serializers.Serializer):
    x1 = serializers.IntegerField()
    x2 = serializers.IntegerField()
    y1 = serializers.IntegerField()
    y2 = serializers.IntegerField()

class SizeSerializer(serializers.ModelSerializer):
    class Meta:
        model = Size
        fields = ('width', 'height', )

class TempUserKeySerializer(serializers.ModelSerializer):
    # user_id = serializers.IntegerField(source='id')
    class Meta:
        model = TempUser
        fields = ('key', 'position', 'size', )

class RoomSerializer(serializers.ModelSerializer):
    users = TempUserKeySerializer(source="user", many=True)
    room_id = serializers.IntegerField(source="id")
    class Meta:
        model = Room
        fields = ('room_id', 'users', 'configuration', )

class TempUserSerializer(serializers.ModelSerializer):
    rooms = RoomSerializer(source="room", many=True)
    size = SizeSerializer()
    # user_id = serializers.IntegerField(source='id')
    class Meta:
        model = TempUser
        fields = ('key', 'rooms', 'position', 'size', )

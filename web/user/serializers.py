from rest_framework import serializers
from django.contrib.auth.models import User
from user.models import TemoUser, Room, Size

class TempUserSerializer(serializers.ModelSerializer):
	class Meta:
		model = TempUser
		# fields = ('id', 'key', '')

class RoomSerializer(serializers.ModelSerializer):
	class Meta:
		model = Room

class SizeSerializer(serializers.ModelSerializer):
	class Meta:
		model = Size

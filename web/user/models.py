# Django
from django.db import models
from django.contrib.auth.models import User
from django.conf import settings
from django.dispatch import receiver
from django.db.models.signals import post_save
from django.db.models import Q
from django.utils import timezone
from django.core.urlresolvers import reverse
from django.core.signing import TimestampSigner, BadSignature, SignatureExpired
# Apps
# Python
import datetime

# Department Models
class TempUser(models.Model):
    """
        Stores the data about temporary users
    """
    key = models.CharField(max_length=15, blank=True, null=True)
    size = models.ForeignKey(Size, null=True, blank=True, related_name='tempuser')

class Room(models.Model):
	"""
        A room where multiple tempusers are connected
    """
    users = models.ManyToManyField(TempUser, null=True, blank=True, related_name='room')

class Size(models.Model):
	"""
        The size class
    """
    width = models.FloatField()
    height = models.FloatField()


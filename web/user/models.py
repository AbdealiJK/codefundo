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

class Size(models.Model):
    """
        The size class
    """
    width = models.FloatField()
    height = models.FloatField()

class TempUser(models.Model):
    """
        Stores the data about temporary users
    """
    key = models.CharField(max_length=15, blank=True, null=True)
    size = models.ForeignKey(Size, null=True, blank=True, related_name='tempuser')
    date_created = models.DateTimeField(auto_now_add=True)

    def __str__(self):
        return str(self.key)

class Room(models.Model):
    """
        A room where multiple tempusers are connected
    """
    user = models.ManyToManyField(TempUser, null=True, blank=True, related_name='room')
    date_created = models.DateTimeField(auto_now_add=True)
    shared_file = models.FileField(upload_to='/shared/')
    def __str__(self):
        return str(self.id)

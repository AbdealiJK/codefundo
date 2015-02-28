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

CONFIGURATION_CHOICES = (
    (0, 'NO_CONFIG'),
    (1, '2_HORIZ_CONFIG'),
    (2, '2_VERTICAL_CONFIG'),
    (3, '4_PORTRAIT_CONFIG'),
    (4, '4_LANDSCAPE_CONFIG'),
)

class BoundingBox(models.Model):
    """
        A class to store the data about a rectangle
        It is assumed the (x1, y1) is top left when phone is held in portrait mode
    """
    x1 = models.IntegerField(default=0)
    x2 = models.IntegerField(default=0)
    y1 = models.IntegerField(default=0)
    y2 = models.IntegerField(default=0)

    def copy(self, b):
        self.x1 = b.x1
        self.x2 = b.x2
        self.y1 = b.y1
        self.y2 = b.y2

class Size(models.Model):
    """
        The size class.
        Width implies in the x direction
        Height implies in the y direction

        The width/height or x/y is with respect to portrait mode always
    """
    width = models.FloatField(null=True, blank=True)
    height = models.FloatField(null=True, blank=True)

class TempUser(models.Model):
    """
        Stores the data about temporary users
    """
    key = models.CharField(max_length=15, blank=True, null=True)
    size = models.ForeignKey(Size, null=True, blank=True, related_name='tempuser')
    date_created = models.DateTimeField(auto_now_add=True)

    position = models.IntegerField(default=0)
    bounding_box = models.ForeignKey(BoundingBox, null=True, blank=True, related_name='tempuser')

    def __str__(self):
        return str(self.key)

class Room(models.Model):
    """
        A room where multiple tempusers are connected
    """
    user = models.ManyToManyField(TempUser, null=True, blank=True, related_name='room')
    date_created = models.DateTimeField(auto_now_add=True)
    shared_file = models.FileField(upload_to='shared/', default=settings.MEDIA_URL+'/logo.jpg')

    configuration = models.IntegerField(max_length=1, choices=CONFIGURATION_CHOICES, default=0)
    size = models.ForeignKey(Size, null=True, blank=True, related_name='room')

    def __str__(self):
        return str(self.id)

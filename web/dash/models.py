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

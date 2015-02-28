# Django
from django.shortcuts import get_object_or_404, render_to_response, redirect, HttpResponseRedirect, render
from django.contrib.auth import authenticate, login
from django.contrib import messages
from django.conf import settings
from django.contrib.auth.decorators import login_required
from django.templatetags.static import static
# Apps
from misc.utils import *  #Import miscellaneous functions
from misc import strings
from misc.constants import HOSTEL_CHOICES, BRANCH_CHOICES
# Decorators
from django.views.decorators.csrf import csrf_exempt
from rest_framework.decorators import api_view, permission_classes
# Models
from django.contrib.auth.models import User, check_password
from apps.users.models import ERPProfile, UserProfile, Dept, Subdept
from apps.walls.models import Wall, Post
# Forms
from forms import LoginForm, UserProfileForm, ERPProfileForm, UserForm
from apps.users.forms import LoginForm,UserProfileForm,UserForm
# View functions
# REST API
from apps.api.serializers import UserSerializer
from rest_framework import serializers
from rest_framework.permissions import AllowAny, IsAuthenticated
from rest_framework.response import Response
from rest_framework import status
from rest_framework.authtoken.models import Token
# Misc
from annoying.functions import get_object_or_None
# Python
import os

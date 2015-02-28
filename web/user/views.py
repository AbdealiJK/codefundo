# Django
from django.shortcuts import get_object_or_404, render_to_response, redirect, HttpResponseRedirect, render
from django.contrib.auth import authenticate, login
from django.contrib import messages
from django.conf import settings
from django.contrib.auth.decorators import login_required
from django.templatetags.static import static
# Apps
from django.views.decorators.csrf import csrf_exempt
from annoying.functions import get_object_or_None
# Python
import os


def index(request):
    return HttpResponse("Hello, world. You're at the polls index.")
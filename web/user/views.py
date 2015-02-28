# Django
from django.shortcuts import get_object_or_404, render_to_response, redirect, HttpResponseRedirect, render
from django.contrib.auth import authenticate, login
from django.contrib import messages
from django.conf import settings
from django.contrib.auth.decorators import login_required
from django.templatetags.static import static
from django.core.urlresolvers import reverse
from django.core.context_processors import csrf
# Apps
from django.views.decorators.csrf import csrf_exempt
from annoying.functions import get_object_or_None
# Python
import os
import urllib

def index(request):
    return HttpResponse("Hello, world. You're at the polls index.")

def microsoft_redirect(request):
	# encode the url
	redirect_url = urllib.quote_plus(settings.SITE_URL + reverse('register_microsoft'))

	# create a unique state value for CSRF validation
	request.session['microsoft_state'] = unicode(csrf(request)['csrf_token'])

	# redirect to microsoft for approval
	url = 'https://login.live.com/oauth20_authorize.srf?' \
		+ 'client_id=' + settings.MICROSOFT_CLIENT_ID \
		+ '&redirect_uri=' + redirect_url \
		+ '&scope=wl.signin%20wl.basic%20wl.emails' \
		+ '&state=' + request.session['microsoft_state'] \
		+ '&response_type=code'

	return url
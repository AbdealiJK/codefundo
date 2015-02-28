from django.conf.urls import patterns, include, url
from django.contrib import admin
from django.contrib.staticfiles.urls import staticfiles_urlpatterns

from rest_framework.routers import DefaultRouter
from user import rest

router = DefaultRouter()
router.register(r'tempuser', rest.TempUserViewSet, base_name="tempuser")
router.register(r'room', rest.RoomViewSet, base_name="room")

urlpatterns = patterns('',
    # Examples:

	url(r'^admin/', include(admin.site.urls)),
	url(r'^user/', include('user.urls')),
	url('', include('social.apps.django_app.urls', namespace='social')),

	url(r'^api/', include(router.urls)),
)

urlpatterns += staticfiles_urlpatterns()
from django.conf.urls import patterns, include, url
from django.contrib import admin
from django.conf.urls.static import static
from django.conf import settings
from rest_framework.routers import DefaultRouter
from user import rest

router = DefaultRouter()
router.register(r'tempuser', rest.TempUserViewSet, base_name="tempuser")
router.register(r'room', rest.RoomViewSet, base_name="room")
router.register(r'event', rest.EventViewSet, base_name="event")

urlpatterns = patterns('',
    # Examples:

	url(r'^admin/', include(admin.site.urls)),
	url(r'^user/', include('user.urls')),
	url('', include('social.apps.django_app.urls', namespace='social')),

	url(r'^api/', include(router.urls)),
)

urlpatterns += static(settings.STATIC_URL,
    document_root=settings.STATIC_ROOT)

urlpatterns += patterns('',
    (r'^' + settings.MEDIA_URL + '(?P<path>.*)$',
        'django.views.static.serve', {
            'document_root': settings.MEDIA_ROOT
        }
))

urlpatterns += patterns('',
    (r'^static/(?P<path>.*)$',
        'django.views.static.serve', {
            'document_root': settings.STATIC_ROOT
        }
))
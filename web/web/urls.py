from django.conf.urls import patterns, include, url
from django.contrib import admin

urlpatterns = patterns('',
    # Examples:

    # url(r'^blog/', include('blog.urls')),
	url(r'^$', 'dash.views.home', name='home'),
    url(r'^admin/', include(admin.site.urls)),
	url(r'^user/', include('user.urls')),
	url(r'^dash/', include('dash.urls')),
	url('', include('social.apps.django_app.urls', namespace='social')),
)

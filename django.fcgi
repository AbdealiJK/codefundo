#!/home/user/public_html/webops/hackathon/hybriddevs/venv/bin/python

import sys
import os


base_path = '/home/user/public_html/webops/hackathon/hybriddevs'
dirs = [
	base_path + '/venv/bin',
	base_path + '/venv/lib',
	base_path + '/venv/lib/python2.7',
	base_path + '/venv/lib/python2.7/site-packages',
	base_path + '/web/',
	base_path + '/web/web',
]

for d in dirs:
	sys.path.insert(0, d)

os.environ['DJANGO_SETTINGS_MODULE'] = 'web.settings'

from django.core.servers.fastcgi import runfastcgi
runfastcgi(method="threaded", daemonize="false")


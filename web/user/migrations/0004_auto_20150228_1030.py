# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('user', '0003_room_shared_file'),
    ]

    operations = [
        migrations.AlterField(
            model_name='room',
            name='shared_file',
            field=models.FileField(default=b'settings.MEDIA_ROOT/logo.jpg', upload_to=b'/shared/'),
            preserve_default=True,
        ),
    ]

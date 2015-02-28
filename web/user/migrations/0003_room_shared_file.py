# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('user', '0002_auto_20150228_0916'),
    ]

    operations = [
        migrations.AddField(
            model_name='room',
            name='shared_file',
            field=models.FileField(default='null', upload_to=b'/shared/'),
            preserve_default=False,
        ),
    ]

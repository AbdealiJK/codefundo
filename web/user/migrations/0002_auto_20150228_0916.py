# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
import datetime
from django.utils.timezone import utc


class Migration(migrations.Migration):

    dependencies = [
        ('user', '0001_initial'),
    ]

    operations = [
        migrations.AddField(
            model_name='room',
            name='date_created',
            field=models.DateTimeField(default=datetime.datetime(2015, 2, 28, 9, 16, 4, 510393, tzinfo=utc), auto_now_add=True),
            preserve_default=False,
        ),
        migrations.AddField(
            model_name='tempuser',
            name='date_created',
            field=models.DateTimeField(default=datetime.datetime(2015, 2, 28, 9, 16, 16, 766333, tzinfo=utc), auto_now_add=True),
            preserve_default=False,
        ),
    ]

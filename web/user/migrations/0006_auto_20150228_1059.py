# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('user', '0005_room_size'),
    ]

    operations = [
        migrations.AddField(
            model_name='room',
            name='configuration',
            field=models.IntegerField(default=0, max_length=1, choices=[(0, b'NO_CONFIG'), (1, b'2_HORIZ_CONFIG'), (2, b'2_VERTICAL_CONFIG'), (3, b'4_TABLE_CONFIG')]),
            preserve_default=True,
        ),
        migrations.AddField(
            model_name='tempuser',
            name='position',
            field=models.IntegerField(default=0),
            preserve_default=True,
        ),
    ]

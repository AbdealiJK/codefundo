# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('user', '0009_auto_20150228_1501'),
    ]

    operations = [
        migrations.AlterField(
            model_name='room',
            name='configuration',
            field=models.IntegerField(default=0, max_length=1, choices=[(0, b'NO_CONFIG'), (1, b'2_LANDSCAPE_CONFIG'), (2, b'2_PORTRAIT_CONFIG'), (3, b'4_PORTRAIT_CONFIG'), (4, b'4_LANDSCAPE_CONFIG')]),
            preserve_default=True,
        ),
    ]

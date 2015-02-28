# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('user', '0006_auto_20150228_1059'),
    ]

    operations = [
        migrations.AlterField(
            model_name='room',
            name='configuration',
            field=models.IntegerField(default=0, max_length=1, choices=[(0, b'NO_CONFIG'), (1, b'2_HORIZ_CONFIG'), (2, b'2_VERTICAL_CONFIG'), (3, b'4_PORTRAIT_CONFIG'), (4, b'4_LANDSCAPE_CONFIG')]),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='size',
            name='height',
            field=models.FloatField(null=True, blank=True),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='size',
            name='width',
            field=models.FloatField(null=True, blank=True),
            preserve_default=True,
        ),
    ]

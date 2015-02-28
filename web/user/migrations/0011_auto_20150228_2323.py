# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('user', '0010_auto_20150228_1521'),
    ]

    operations = [
        migrations.AlterField(
            model_name='boundingbox',
            name='x1',
            field=models.FloatField(default=0),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='boundingbox',
            name='x2',
            field=models.FloatField(default=0),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='boundingbox',
            name='y1',
            field=models.FloatField(default=0),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='boundingbox',
            name='y2',
            field=models.FloatField(default=0),
            preserve_default=True,
        ),
    ]

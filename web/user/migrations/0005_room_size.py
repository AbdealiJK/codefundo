# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('user', '0004_auto_20150228_1030'),
    ]

    operations = [
        migrations.AddField(
            model_name='room',
            name='size',
            field=models.ForeignKey(related_name='room', blank=True, to='user.Size', null=True),
            preserve_default=True,
        ),
    ]

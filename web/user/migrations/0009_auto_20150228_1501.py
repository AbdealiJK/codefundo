# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('user', '0008_auto_20150228_1223'),
    ]

    operations = [
        migrations.AlterField(
            model_name='room',
            name='shared_file',
            field=models.FileField(default=b'media//logo.jpg', upload_to=b'shared/'),
            preserve_default=True,
        ),
    ]

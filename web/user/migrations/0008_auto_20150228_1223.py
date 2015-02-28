# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('user', '0007_auto_20150228_1118'),
    ]

    operations = [
        migrations.CreateModel(
            name='BoundingBox',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('x1', models.IntegerField(default=0)),
                ('x2', models.IntegerField(default=0)),
                ('y1', models.IntegerField(default=0)),
                ('y2', models.IntegerField(default=0)),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.AddField(
            model_name='tempuser',
            name='bounding_box',
            field=models.ForeignKey(related_name='tempuser', blank=True, to='user.BoundingBox', null=True),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='room',
            name='shared_file',
            field=models.FileField(default=b'/mnt/data/webops/codefundo/web/media/logo.jpg', upload_to=b'/shared/'),
            preserve_default=True,
        ),
    ]

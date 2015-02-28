from user.models import TempUser, Room, Size, BoundingBox, CONFIGURATION_CHOICES

def viewset_response(message,data):
    temp={}
    temp['status']=0
    temp['message']=message
    temp['data']=data
    if not message:
        temp['status']=1
        temp['message']='done'
    return temp

def calculate_room(room, user):
    """
        Calculates the various bounding boxes to show in a room for
        all users.

        room : The room in which the event occured
        user : The user who is the master right now
    """
    all_configs = [str(CONFIGURATION_CHOICES[i][0]) for i in xrange(len(CONFIGURATION_CHOICES))]
    all_config_names = [str(CONFIGURATION_CHOICES[i][1]) for i in xrange(len(CONFIGURATION_CHOICES))]

    if room.configuration == 0: # NO_CONFIG
        return
    elif room.configuration == 1: # 2_HORIZ_CONFIG
        # Here, there will be one phone on top and another phone at the
        # bottom. Both will be in landscape mode.

        if user.position == 0: # Top screen
            bottom = room.user.filter(position=1)
            bbox1 = bottom.bounding_box
            bbox0 = user.bounding_box
            # Common edge
            bbox1.x2 = bbox0.x1
            bbox1.y2 = bbox0.y2
            bbox1.x1 = bbox0.
            bbox1.y1 = bbox0.
        return
    elif room.configuration == 2: # 2_VERTICAL_CONFIG
        return
    elif room.configuration == 3: # 4_PORTRAIT_CONFIG
        return
    elif room.configuration == 4: # 4_LANDSCAPE_CONFIG
        return

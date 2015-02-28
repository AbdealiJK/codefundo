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

    rows = 0
    cols = 0

    if room.configuration == 0: # NO_CONFIG
        return
    elif room.configuration == 1: # 2_LANDSCAPE_CONFIG
        # Temp variables
        user0 = room.user.filter(position=0)
        user1 = room.user.filter(position=1)
        bbox1 = user1.bounding_box
        bbox0 = user0.bounding_box
        size1 = user1.size
        size0 = user0.size
        if user.position == 0: # Top screen
            bbox1.x2 = bbox0.x1
            bbox1.y2 = bbox0.y2
            bbox1.x1 = bbox0.x1 + int((bbox0.x1 - bbox0.x2) * 1.0/ size0.width * size1.width)
            bbox1.y1 = bbox0.y1
        elif user.position == 1: # Bottom screen
            bbox0.x1 = bbox1.x2
            bbox0.y1 = bbox1.y2
            bbox0.x1 = bbox1.x2 + int((bbox1.x2 - bbox1.x1) * 1.0/ size1.width * size0.width)
            bbox0.y1 = bbox1.y1
        rows = 2
        cols = 1
        landscape = False
        return
    elif room.configuration == 2: # 2_PORTRAIT_CONFIG
        rows = 1
        cols = 2
        landscape = False
    elif room.configuration == 3: # 4_PORTRAIT_CONFIG
        rows = 2
        cols = 2
        landscape = False
    elif room.configuration == 4: # 4_LANDSCAPE_CONFIG
        rows = 2
        cols = 2
        landscape = True

    users = room.user.all().order_by('position')
    bboxs = map(lambda x: x.bounding_box, user)
    sizes = map(lambda x: x.size, user)
    dones = [False for i in xrange(len(users))]
    master = user.position

    calculate_recursive(room, master, rows, cols, users, bboxs, sizes, dones, landscape)

def calculate_recursive(room, master, rows, cols, users,
                        bboxs, sizes, dones, landscape):
    """
        Assumes the master is already calibrated.
        Will use the master and calibrate all 4-neighbours
        Now will assume the 4 neighbours are masters each
    """
    top = master - cols
    if top >= 0 and not dones[top]:
        user1 = user[master]
        user0 = user[top]
        calc_doublet(user0, user1, 0, landscape, False)
        dones[top] = True
    bottom = master + cols
    if bottom < rows * cols and not dones[bottom]:
        user1 = user[master]
        user0 = user[bottom]
        calc_doublet(user0, user1, 1, landscape, False)
        dones[bottom] = True

    left = master - rows
    if left >= 0 and not dones[left]:
        user1 = user[master]
        user0 = user[left]
        calc_doublet(user0, user1, 0, landscape, True)
        dones[left] = True
    right = master + rows
    if right < rows * cols and not dones[right]:
        user1 = user[right]
        user0 = user[master]
        calc_doublet(user0, user1, 1, landscape, True)
        dones[right] = True

    calculate_recursive(room, top, rows, cols, users, bboxs, sizes, dones, landscape)
    calculate_recursive(room, bottom, rows, cols, users, bboxs, sizes, dones, landscape)
    calculate_recursive(room, left, rows, cols, users, bboxs, sizes, dones, landscape)
    calculate_recursive(room, right, rows, cols, users, bboxs, sizes, dones, landscape)

def calc_doublet(user0, user1, master, landscape, leftright):
    """
        user0 is on top, user1 is at the bottom
    """
    bbox1 = user1.bounding_box
    bbox0 = user0.bounding_box
    size1 = user1.size
    size0 = user0.size
    topbottom = not leftright
    if master == 0 and landscape and leftright:
        bbox1.x1 = bbox0.x1
        bbox1.x2 = bbox0.x2
        bbox1.y1 = bbox0.y2
        bbox1.y2 = bbox0.y2 + int((bbox0.y2 - bbox0.y1) * 1.0/ size0.width * size1.width)
    elif master == 1 and landscape and leftright:
        bbox0.x1 = bbox1.x1
        bbox0.x2 = bbox1.x2
        bbox0.y1 = bbox1.y1 + int((bbox1.y1 - bbox1.y2) * 1.0/ size0.width * size1.width)
        bbox0.y2 = bbox1.y1
    elif master == 0 and not landscape and leftright:
        bbox1.y1 = bbox0.y1
        bbox1.y2 = bbox0.y2
        bbox1.x1 = bbox0.x2
        bbox1.x2 = bbox0.x2 + int((bbox0.x2 - bbox0.x1) * 1.0/ size0.width * size1.width)
    elif master == 1 and not landscape and leftright:
        bbox0.y1 = bbox1.y1
        bbox0.y2 = bbox1.y2
        bbox0.x1 = bbox1.x1 + int((bbox1.x1 - bbox1.x2) * 1.0/ size0.width * size1.width)
        bbox0.x2 = bbox1.x1
    elif master == 0 and landscape and not leftright:
        bbox1.y1 = bbox0.y1
        bbox1.y2 = bbox0.y2
        bbox1.x1 = bbox0.x1 + int((bbox0.x1 - bbox0.x2) * 1.0/ size0.width * size1.width)
        bbox1.x2 = bbox0.x1
    elif master == 1 and landscape and not leftright:
        bbox0.y1 = bbox1.y1
        bbox0.y2 = bbox1.y2
        bbox0.x1 = bbox1.x2
        bbox0.x2 = bbox1.x2 + int((bbox1.x2 - bbox1.x1) * 1.0/ size0.width * size1.width)
    elif master == 0 and not landscape and not leftright:
        bbox1.x1 = bbox0.x1
        bbox1.x2 = bbox0.x2
        bbox1.y1 = bbox0.y2
        bbox1.y2 = bbox0.y2 + int((bbox0.y2 - bbox0.y1) * 1.0/ size0.width * size1.width)
    elif master == 1 and not landscape and not leftright:
        bbox0.x1 = bbox1.x1
        bbox0.x2 = bbox1.x2
        bbox0.y1 = bbox1.y1 + int((bbox1.y1 - bbox1.y2) * 1.0/ size0.width * size1.width)
        bbox0.y2 = bbox1.y1
    bbox0.save()
    bbox1.save()
    user0.save()
    user1.save()

def calc_top_bottom_landscape(user0, user1, master):
    """
        user0 is on top, user1 is at the bottom
    """
    bbox1 = user1.bounding_box
    bbox0 = user0.bounding_box
    size1 = user1.size
    size0 = user0.size
    if master == 0: # Top screen
        bbox1.y1 = bbox0.y1
        bbox1.y2 = bbox0.y2
        bbox1.x1 = bbox0.x1 + int((bbox0.x1 - bbox0.x2) * 1.0/ size0.width * size1.width)
        bbox1.x2 = bbox0.x1
    elif master == 1: # Bottom screen
        bbox0.y1 = bbox1.y1
        bbox0.y2 = bbox1.y2
        bbox0.x1 = bbox1.x2
        bbox0.x2 = bbox1.x2 + int((bbox1.x2 - bbox1.x1) * 1.0/ size1.width * size0.width)
    bbox0.save()
    bbox1.save()
    user0.save()
    user1.save()

def calc_top_bottom_portrait(user0, user1):
    """
        user0 is on top, user1 is at the bottom
    """
    bbox1 = user1.bounding_box
    bbox0 = user0.bounding_box
    size1 = user1.size
    size0 = user0.size
    if user.position == 0: # Top screen
        bbox1.x2 = bbox0.x1
        bbox1.y2 = bbox0.y2
        bbox1.x1 = bbox0.x1 + int((bbox0.x1 - bbox0.x2) * 1.0/ size0.width * size1.width)
        bbox1.y1 = bbox0.y1
    elif user.position == 1: # Bottom screen
        bbox0.x1 = bbox1.x2
        bbox0.y2 = bbox1.y2
        bbox0.x1 = bbox1.x2 + int((bbox1.x2 - bbox1.x1) * 1.0/ size1.width * size0.width)
        bbox0.y1 = bbox1.y1
    bbox0.save()
    bbox1.save()
    user0.save()
    user1.save()


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

def calculate_room(room):
    print room

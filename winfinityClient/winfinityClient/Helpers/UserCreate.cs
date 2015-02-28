using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winfinityClient.Helpers
{
    public class UserOfRoom
    {
        public string key { get; set; }
        public int position { get; set; }
    }

    public class Room
    {
        public int room_id { get; set; }
        public List<UserOfRoom> users { get; set; }
        public int configuration { get; set; }
        public string shared_file { get; set; }
    }

    public class DataOfRooms
    {
        public string key { get; set; }
        public List<Room> rooms { get; set; }
        public int position { get; set; }
    }

    public class UserCreate
    {
        public int status { get; set; }
        public string message { get; set; }
        public DataOfRooms data { get; set; }
    }
}

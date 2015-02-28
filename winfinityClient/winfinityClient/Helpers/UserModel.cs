using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winfinityClient.Helpers
{
    public class User
    {
        public string key { get; set; }
    }

    public class Room
    {
        public int room_id { get; set; }
        public List<User> users { get; set; }
    }

    public class Datum
    {
        public string key { get; set; }
        public List<Room> rooms { get; set; }
    }

    public class UserModel
    {
        public int status { get; set; }
        public string message { get; set; }
        public List<Datum> data { get; set; }
    }
}

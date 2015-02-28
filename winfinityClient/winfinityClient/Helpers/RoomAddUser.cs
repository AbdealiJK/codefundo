using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winfinityClient.Helpers
{
    public class UserA
    {
        public string key { get; set; }
        public int position { get; set; }
    }

    public class Data
    {
        public int room_id { get; set; }
        public List<UserA> users { get; set; }
        public int configuration { get; set; }
        public string shared_file { get; set; }
    }

    public class RoomAddUser
    {
        public int status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
}

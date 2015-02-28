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

    public class DataofRoom
    {
        public int room_id { get; set; }
        public List<User> users { get; set; }
    }

    public class RoomCreate
    {
        public int status { get; set; }
        public string message { get; set; }
        public DataofRoom data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winfinityClient.Helpers
{
    public class DataofUser
    {
        public string key { get; set; }
        public List<object> rooms { get; set; }
    }

    public class UserCreate
    {
        public int status { get; set; }
        public string message { get; set; }
        public DataofUser data { get; set; }
    }
}

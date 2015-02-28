using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winfinityClient.Helpers
{
    class EventResult
    {
        public int status { get; set; }
        public string message { get; set; }
        public BoundBox data { get; set; }
    }
}

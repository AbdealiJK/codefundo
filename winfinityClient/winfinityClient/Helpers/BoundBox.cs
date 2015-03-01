using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace winfinityClient.Helpers
{
    public class BoundBox
    {
        public double x1;
        public double x2;
        public double y1;
        public double y2;

        public void Pan(double cx, double cy)
        {
            x1 += cx;
            x2 += cx;
            y1 += cy;
            y2 += cy;
        }
    }
}

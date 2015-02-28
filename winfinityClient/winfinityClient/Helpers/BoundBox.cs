using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace winfinityClient.Helpers
{
    class BoundBox
    {
        public double X1;
        public double X2;
        public double Y1;
        public double Y2;

        public void Pan(double cx, double cy)
        {
            X1 += cx;
            X2 += cx;
            Y1 += cy;
            Y2 += cy;
        }
    }
}

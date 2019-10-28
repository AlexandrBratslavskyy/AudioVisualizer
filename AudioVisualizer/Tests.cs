using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioVisualizer
{
    public class Tests
    {
        static public Display TestDisplay()
        {
            Display display = new Display();
            int N = 10000, A = 200, f = 80;
            for (int t=0; t< N; t++)
            {
                display.Add(t, (int)(A * Math.Cos(2 * Math.PI * f * t /N)));
            }
            return display;
        }
    }
}

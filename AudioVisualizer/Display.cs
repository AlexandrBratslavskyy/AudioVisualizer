using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AudioVisualizer
{
    public class Display
    {

        // Statics

        static private long ZOOM = 10;
        static public void ZoomIn()
        {
            if (ZOOM != 1)
                ZOOM--;
        }
        static public void ZoomOut()
        {
            ZOOM++;
        }
        static public long DrawChart(Canvas canvas, S s)
        {
            //clear previous
            canvas.Children.Clear();

            //line showing signal zero
            double Y = canvas.ActualHeight / 2;
            Line zeroline = new Line();

            zeroline.X1 = 0;
            zeroline.Y1 = Y;
            zeroline.X2 = s.Size() - ZOOM;
            zeroline.Y2 = Y;

            zeroline.Stroke = new SolidColorBrush(Colors.Black);
            zeroline.StrokeThickness = 1.0;

            canvas.Children.Add(zeroline);

            //compensate for limited space
            double c = 1.0;
            if (Y < s.GetMax())
                c = s.GetMax() / Y;

            //display on canvas
            long i = 0;
            for (long next = 0; next < s.Size() - ZOOM; i++)
            {
                Line line = new Line();
                int c1 = s.Get(next), c2 = s.Get(next += ZOOM);

                line.X1 = i;
                line.Y1 = c * (Y - c1);
                line.X2 = i;
                line.Y2 = c * (Y - c2);

                line.Stroke = new SolidColorBrush(Colors.Black);
                line.StrokeThickness = 1.0;

                canvas.Children.Add(line);
            }
            return i;
        }
    }
}

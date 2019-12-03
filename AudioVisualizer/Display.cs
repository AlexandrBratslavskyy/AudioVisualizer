using System;
using System.Windows.Controls;
using System.Windows.Media;
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
                --ZOOM;
        }
        static public void ZoomOut()
        {
            ++ZOOM;
        }
        static public long Get()
        {
            return ZOOM;
        }
        static public void DrawTimeDomain(ref Canvas canvas, ref S s)
        {
            //clear previous
            canvas.Children.Clear();

            //over scrollbar
            double Y = canvas.ActualHeight / 2;

            //display on canvas
            long i = 0;
            for (long next = 0; next < s.Size() - ZOOM; )
            {
                Line line = new Line();
                double c1 = s.Get(next), c2 = s.Get(next += ZOOM);

                line.X1 = i;
                line.X2 = ++i;
                line.Y1 = Y - c1 / 150;
                line.Y2 = Y - c2 / 150;

                line.Stroke = new SolidColorBrush(Colors.Blue);
                line.StrokeThickness = 1.0;

                canvas.Children.Add(line);
            }
            canvas.Width = i;
        }
        static public void DrawFrequencyDomain(ref Canvas canvas, ref A a, ref long N)
        {
            //clear previous
            canvas.Children.Clear();

            //line showing signal zero
            double Y = canvas.ActualHeight, X = canvas.ActualWidth / N;

            //display filters
            long i = 0;
            for (double l = 0; l < canvas.ActualWidth && i < a.Size(); l+=X)
            {
                //display lines on canvas
                Line line = new Line();

                //display values
                line.X1 = l + X / 2;
                line.X2 = l + X / 2;
                line.Y1 = Y - Math.Abs(a.GetAmp(i++)) / 25;
                line.Y2 = Y;

                line.Stroke = new SolidColorBrush(Colors.Red);
                line.StrokeThickness = X;

                // add both - order matters I think
                canvas.Children.Add(line);
            }
        }
    }
}

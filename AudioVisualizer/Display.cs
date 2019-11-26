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
        static public void DrawTimeDomain(Canvas canvas, S s)
        {
            //clear previous
            canvas.Children.Clear();

            //over scrollbar                        //compensate for limited space, only shows positives
            double Y = canvas.ActualHeight * 0.945, max = (Math.Abs(s.GetMax()) + Math.Abs(s.GetMin())) / Y;

            //display on canvas
            long i = 0;
            for (long next = 0; next < s.Size() - ZOOM; i++)
            {
                Line line = new Line();
                double c1 = s.Get(next), c2 = s.Get(next += ZOOM);

                line.X1 = i;
                line.X2 = i;
                line.Y1 = Y - c1 / max;
                line.Y2 = Y - c2 / max;

                line.Stroke = new SolidColorBrush(Colors.Blue);
                line.StrokeThickness = 1.0;

                canvas.Children.Add(line);
            }
            canvas.Width = i;
        }
        static public void DrawFrequencyDomain(Canvas canvas, A a, long N)
        {
            //clear previous
            canvas.Children.Clear();

            //line showing signal zero
            double Y = canvas.ActualHeight, X = canvas.ActualWidth / N;

            //display filters
            long i = 0;
            for (double l = 0; l < canvas.ActualWidth; l+=X)
            {
                //display lines on canvas
                Line line = new Line(), value = new Line();

                line.X1 = l;
                line.X2 = l;
                line.Y1 = 0;
                line.Y2 = Y;

                line.Stroke = new SolidColorBrush(Colors.Black);
                line.StrokeThickness = 1.0;

                //display values
                value.X1 = l + X / 2;
                value.X2 = l + X / 2;
                value.Y1 = Y - Math.Abs(a.Get(i++).getReal());
                value.Y2 = Y;

                value.Stroke = new SolidColorBrush(Colors.Red);
                value.StrokeThickness = X;

                // add both - order matters I think
                canvas.Children.Add(value);
                canvas.Children.Add(line);
            }
        }
    }
}

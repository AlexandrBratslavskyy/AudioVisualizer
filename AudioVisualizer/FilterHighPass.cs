using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;

namespace AudioVisualizer
{
    public class FilterHighPass : Filter
    {
        /*
            create a high pass filter based on user selection
            user selects a frequency cutoff
            all frequencies after freq cutoff are removed

            freq = (f(frequency bins) * SamplingRate) / NumSamples

                                                      Nyquist
            [0, 0, 0, 0, 0, 0,            1, 1, 1, 1,    1,    1, 1, 1, 1, 0, 0, 0, 0, 0]
                            freq(cutoff) ---------------> <---------------
        */
        public override A CreateFilter(long frequencyBin1, long frequencyBin2, long N)
        {
            long nyquistLimit = N / 2;
            //error checking
            if (frequencyBin1 > nyquistLimit)
            {
                frequencyBin1 = nyquistLimit - (frequencyBin1 - nyquistLimit);
            }

            long difference = ((nyquistLimit - frequencyBin1) * 2) + 1;
            A filter = new A();

            long i;

            //beginning of filter
            for (i = 0; i <= frequencyBin1; i++)
            {
                filter.Add(0, 0);
            }

            //middle of filter
            for (; i <= frequencyBin1 + difference; i++)
            {
                filter.Add(1, 1);
            }

            //end of filter
            for (; i < N; i++)
            {
                filter.Add(0, 0);
            }

            //return filter;
            return filter;
        }

        //drawing and dragging
        public override void DrawFilter(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            left1.Visibility = Visibility.Visible;
            left2.Visibility = Visibility.Collapsed;
            right1.Visibility = Visibility.Visible;
            right2.Visibility = Visibility.Collapsed;

            Canvas.SetLeft(left1, 0);
            Canvas.SetLeft(right1, canvas.ActualWidth);

            rect1.Visibility = Visibility.Visible;
            rect2.Visibility = Visibility.Collapsed;

            rect1.Width = canvas.ActualWidth;

            Canvas.SetLeft(rect1, 0);
        }
        public override void DragFilterLeft1(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            Point m = Mouse.GetPosition(canvas);
            if (m.X >= 0 && m.X <= canvas.ActualWidth / 2)
            {
                Canvas.SetLeft(left1, Canvas.GetLeft(left1) + e.HorizontalChange);
                Canvas.SetLeft(right1, canvas.ActualWidth - Canvas.GetLeft(left1));

                rect1.Width = Canvas.GetLeft(right1) - (int)Canvas.GetLeft(left1);

                Canvas.SetLeft(rect1, Canvas.GetLeft(left1));
            }
            else if (m.X >= canvas.ActualWidth / 2)
            {
                Canvas.SetLeft(left1, canvas.ActualWidth / 2);
                Canvas.SetLeft(right1, canvas.ActualWidth / 2);

                rect1.Width = 0;

                Canvas.SetLeft(rect1, canvas.ActualWidth / 2);
            }
            else
            {
                Canvas.SetLeft(left1, 0);
                Canvas.SetLeft(right1, canvas.ActualWidth);

                rect1.Width = canvas.ActualWidth;

                Canvas.SetLeft(rect1, 0);
            }
        }
        public override void DragFilterLeft2(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            throw new NotImplementedException("High Pass doesn't have second Thumb");
        }
        public override void DragFilterRight1(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            Point m = Mouse.GetPosition(canvas);
            if (m.X >= canvas.ActualWidth / 2 && m.X <= canvas.ActualWidth)
            {
                Canvas.SetLeft(right1, Canvas.GetLeft(right1) + e.HorizontalChange);
                Canvas.SetLeft(left1, canvas.ActualWidth - Canvas.GetLeft(right1));
                
                rect1.Width = Canvas.GetLeft(right1) - (int)Canvas.GetLeft(left1);

                Canvas.SetLeft(rect1, Canvas.GetLeft(left1));
            }
            else if (m.X <= canvas.ActualWidth / 2)
            {
                Canvas.SetLeft(right1, canvas.ActualWidth / 2);
                Canvas.SetLeft(left1, canvas.ActualWidth / 2);

                rect1.Width = 0;

                Canvas.SetLeft(rect1, canvas.ActualWidth / 2);
            }
            else
            {
                Canvas.SetLeft(right1, canvas.ActualWidth);
                Canvas.SetLeft(left1, 0);
                
                rect1.Width = canvas.ActualWidth;

                Canvas.SetLeft(rect1, 0);
            }
        }
        public override void DragFilterRight2(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            throw new NotImplementedException("High Pass doesn't have second Thumb");
        }
        public override void DropFilterLeft1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException("TODO");
        }
        public override void DropFilterLeft2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException("High Pass doesn't have second Thumb");
        }
        public override void DropFilterRight1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException("TODO");
        }
        public override void DropFilterRight2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException("High Pass doesn't have second Thumb");
        }
    }
}

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
    public class FilterLowPass : Filter
    {
        /*
            create a low pass filter based on user selection
            user selects a frequency cutoff
            all frequencies after freq cutoff are removed

            freq = (f(frequency bins) * SamplingRate) / NumSamples

                                                      Nyquist
            [1, 1, 1, 1, 1, 1,            0, 0, 0, 0,    0,    0, 0, 0, 0, 1, 1, 1, 1, 1]
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
                filter.Add(1, 1);
            }

            //middle of filter
            for (; i <= frequencyBin1 + difference; i++)
            {
                filter.Add(0, 0);
            }

            //end of filter
            for (; i < N; i++)
            {
                filter.Add(1, 1);
            }

            //return filter;
            return filter;
        }

        //drawing and dragging
        public override void DrawFilter(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            double width = canvas.ActualWidth, bin = width / Filter.getSize();

            left1.Visibility = Visibility.Visible;
            left2.Visibility = Visibility.Collapsed;
            right1.Visibility = Visibility.Visible;
            right2.Visibility = Visibility.Collapsed;

            Canvas.SetLeft(left1, width / 2);
            Canvas.SetLeft(right1, width / 2 + bin);

            rect1.Visibility = Visibility.Visible;
            rect2.Visibility = Visibility.Visible;

            this.DrawRect(left1, left2, right1, right2, rect1, rect2);
        }
        protected override void DrawRect(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2)
        {
            rect1.Width = Canvas.GetLeft(left1);
            rect2.Width = Canvas.GetLeft(left1);

            Canvas.SetLeft(rect2, Canvas.GetLeft(right1));
        }
        public override void DragFilterLeft1(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            double width = canvas.ActualWidth, bin = width / Filter.getSize(), pos = Mouse.GetPosition(canvas).X;

            if (pos >= bin && pos <= width / 2)
            {
                Canvas.SetLeft(left1, Canvas.GetLeft(left1) + e.HorizontalChange);
                Canvas.SetLeft(right1, width - Canvas.GetLeft(left1) + bin);
            }
            else if (pos >= width / 2)
            {
                Canvas.SetLeft(left1, width / 2);
                Canvas.SetLeft(right1, width / 2 + bin);
            }
            else
            {
                Canvas.SetLeft(left1, bin);
                Canvas.SetLeft(right1, width);
            }

            this.DrawRect(left1, left2, right1, right2, rect1, rect2);
        }
        public override void DragFilterLeft2(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            throw new NotImplementedException("Low Pass doesn't have second Thumb");
        }
        public override void DragFilterRight1(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            double width = canvas.ActualWidth, bin = width / Filter.getSize(), pos = Mouse.GetPosition(canvas).X;

            if (pos >= width / 2 + bin && pos <= width)
            {
                Canvas.SetLeft(right1, Canvas.GetLeft(right1) + e.HorizontalChange);
                Canvas.SetLeft(left1, width - Canvas.GetLeft(right1) + bin);
            }
            else if (pos <= width / 2 + bin)
            {
                Canvas.SetLeft(right1, width / 2 + bin);
                Canvas.SetLeft(left1, width / 2);
            }
            else
            {
                Canvas.SetLeft(right1, width);
                Canvas.SetLeft(left1, bin);
            }

            this.DrawRect(left1, left2, right1, right2, rect1, rect2);
        }
        public override void DragFilterRight2(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            throw new NotImplementedException("Low Pass doesn't have second Thumb");
        }
        public override void DropFilterLeft1(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException("TODO");
        }
        public override void DropFilterLeft2(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException("Low Pass doesn't have second Thumb");
        }
        public override void DropFilterRight1(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            double width = canvas.ActualWidth, bin = width / Filter.getSize(), binNumber = Filter.getSize() / 2 + 1;

            for (double i = binNumber, distance = width; i <= Filter.getSize(); ++i)
            {
                if (Math.Abs(i * bin - Canvas.GetLeft(right1)) < distance)
                {
                    distance = Math.Abs(i * bin - Canvas.GetLeft(right1));
                    binNumber = i;
                }
            }

            Canvas.SetLeft(right1, binNumber * bin);
            Canvas.SetLeft(left1, width - Canvas.GetLeft(right1) + bin);

            this.DrawRect(left1, left2, right1, right2, rect1, rect2);

        }
        public override void DropFilterRight2(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException("Low Pass doesn't have second Thumb");
        }
    }
}

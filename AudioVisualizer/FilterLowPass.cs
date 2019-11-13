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
        public FilterLowPass(long N) : base(N) { }
        /*
            create a low pass filter based on user selection
            user selects a frequency cutoff
            all frequencies after freq cutoff are removed

            freq = (f(frequency bins) * SamplingRate) / NumSamples

                                                      Nyquist
            [1, 1, 1, 1, 1, 1,            0, 0, 0, 0,    0,    0, 0, 0, 0, 1, 1, 1, 1, 1]
                            freq(cutoff) ---------------> <---------------
        */
        public override void CreateFilter()
        {
            A filter = new A();

            long i;

            //beginning of filter
            for (i = 0; i <= fl1; i++)
            {
                filter.Add(1, 1);
            }

            //middle of filter
            for (; i <= fr1; i++)
            {
                filter.Add(0, 0);
            }

            //end of filter
            for (; i < N; i++)
            {
                filter.Add(1, 1);
            }

            //return filter;
            Filter.WEIGHTS = Algorithms.ReverseDFT(filter);
        }

        //drawing and dragging
        public override void DrawFilter(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            this.left1 = left1;
            this.left2 = left2;
            this.right1 = right1;
            this.right2 = right2;
            this.rect1 = rect1;
            this.rect2 = rect2;
            this.canvas = canvas;

            double width = canvas.ActualWidth, bin = width / N;

            left1.Visibility = Visibility.Visible;
            left2.Visibility = Visibility.Collapsed;
            right1.Visibility = Visibility.Visible;
            right2.Visibility = Visibility.Collapsed;

            Canvas.SetLeft(left1, width / 2);
            Canvas.SetLeft(right1, width / 2 + bin);

            rect1.Visibility = Visibility.Visible;
            rect2.Visibility = Visibility.Visible;

            DrawRect();

            fl1 = N / 2;
            fr1 = N / 2 + 1;

            CreateFilter();
        }
        protected override void DrawRect()
        {
            rect1.Width = Canvas.GetLeft(left1);
            rect2.Width = Canvas.GetLeft(left1);

            Canvas.SetLeft(rect2, Canvas.GetLeft(right1));
        }
        public override void DragFilterLeft1(DragDeltaEventArgs e)
        {
            double width = canvas.ActualWidth, bin = width / N, pos = Mouse.GetPosition(canvas).X;

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

            DrawRect();
        }
        public override void DragFilterLeft2(DragDeltaEventArgs e)
        {
            throw new NotImplementedException("Low Pass doesn't have second Thumb");
        }
        public override void DragFilterRight1(DragDeltaEventArgs e)
        {
            double width = canvas.ActualWidth, bin = width / N, pos = Mouse.GetPosition(canvas).X;

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

            DrawRect();
        }
        public override void DragFilterRight2(DragDeltaEventArgs e)
        {
            throw new NotImplementedException("Low Pass doesn't have second Thumb");
        }
        public override void DropFilterLeft1()
        {
            double width = canvas.ActualWidth, bin = width / N, binNumber = 1;

            for (double i = binNumber, distance = width; i <= N / 2; ++i)
            {
                if (Math.Abs(i * bin - Canvas.GetLeft(left1)) < distance)
                {
                    distance = Math.Abs(i * bin - Canvas.GetLeft(left1));
                    binNumber = i;
                }
            }

            Canvas.SetLeft(left1, binNumber * bin);
            Canvas.SetLeft(right1, width - Canvas.GetLeft(left1) + bin);

            DrawRect();

            fl1 = (long)binNumber - 1;
            fr1 = N - (long)binNumber;

            CreateFilter();
        }
        public override void DropFilterLeft2()
        {
            throw new NotImplementedException("Low Pass doesn't have second Thumb");
        }
        public override void DropFilterRight1()
        {
            double width = canvas.ActualWidth, bin = width / N, binNumber = N / 2 + 1;

            for (double i = binNumber, distance = width; i <= N; ++i)
            {
                if (Math.Abs(i * bin - Canvas.GetLeft(right1)) < distance)
                {
                    distance = Math.Abs(i * bin - Canvas.GetLeft(right1));
                    binNumber = i;
                }
            }

            Canvas.SetLeft(right1, binNumber * bin);
            Canvas.SetLeft(left1, width - Canvas.GetLeft(right1) + bin);

            this.DrawRect();
        }
        public override void DropFilterRight2()
        {
            throw new NotImplementedException("Low Pass doesn't have second Thumb");
        }
    }
}

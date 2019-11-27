using System;
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

                                                      Nyquist
            [0, 0, 0, 0, 0, 0,            1, 1, 1, 1,    1,    1, 1, 1, 1, 0, 0, 0, 0, 0]
                            freq(cutoff) ---------------> <---------------
        */
        protected override void CreateFilter()
        {
            A filter = new A();

            long i;

            //beginning of filter
            for (i = 0; i < fl1; i++)
            {
                filter.Add(0, 0);
            }

            //middle of filter
            for (; i <= fr1 && i < N; i++)
            {
                filter.Add(1, 1);
            }

            //end of filter
            for (; i < N; i++)
            {
                filter.Add(0, 0);
            }

            //return filter;
            WEIGHTS = Algorithms.ReverseDFT(filter);
        }

        //drawing and dragging
        public override void DrawFilter(ref Thumb left1, ref Thumb left2, ref Thumb right1, ref Thumb right2, ref Rectangle rect1, ref Rectangle rect2, ref Canvas canvas, ref long N)
        {
            this.N = N;
            this.left1 = left1;
            this.left2 = left2;
            this.right1 = right1;
            this.right2 = right2;
            this.rect1 = rect1;
            this.rect2 = rect2;
            this.canvas = canvas;

            double width = canvas.ActualWidth;

            left1.Visibility = Visibility.Visible;
            left2.Visibility = Visibility.Collapsed;
            right1.Visibility = Visibility.Visible;
            right2.Visibility = Visibility.Collapsed;

            Canvas.SetLeft(left1, 0);
            Canvas.SetLeft(right1, width);

            rect1.Visibility = Visibility.Visible;
            rect2.Visibility = Visibility.Collapsed;

            DrawRect();

            fl1 = 0;
            fr1 = N;

            CreateFilter();
        }
        protected override void DrawRect()
        {
            rect1.Width = Canvas.GetLeft(right1) - Canvas.GetLeft(left1);

            Canvas.SetLeft(rect1, Canvas.GetLeft(left1));
        }
        public override void DragFilterLeft1(ref DragDeltaEventArgs e)
        {
            double width = canvas.ActualWidth, bin = width / N, pos = Mouse.GetPosition(canvas).X;

            if (pos >= bin && pos <= width / 2 + bin / 2)
            {
                Canvas.SetLeft(left1, Canvas.GetLeft(left1) + e.HorizontalChange);
                Canvas.SetLeft(right1, width - Canvas.GetLeft(left1) + bin);
            }
            else if (pos >= width / 2 + bin / 2)
            {
                Canvas.SetLeft(left1, width / 2 + bin / 2);
                Canvas.SetLeft(right1, width / 2 + bin / 2);
            }
            else if (pos >= 0 && pos <= bin)
            {
                Canvas.SetLeft(left1, Canvas.GetLeft(left1) + e.HorizontalChange);
                Canvas.SetLeft(right1, width);
            }
            else
            {
                Canvas.SetLeft(left1, 0);
                Canvas.SetLeft(right1, width);
            }

            DrawRect();
        }
        public override void DragFilterLeft2(ref DragDeltaEventArgs e)
        {
            throw new NotImplementedException("High Pass doesn't have second Thumb");
        }
        public override void DragFilterRight1(ref DragDeltaEventArgs e)
        {
            double width = canvas.ActualWidth, bin = width / N, pos = Mouse.GetPosition(canvas).X;

            if (pos >= width / 2 + bin / 2 && pos <= width)
            {
                Canvas.SetLeft(right1, Canvas.GetLeft(right1) + e.HorizontalChange);
                Canvas.SetLeft(left1, width - Canvas.GetLeft(right1) + bin);
            }
            else if (pos <= width / 2 + bin / 2)
            {
                Canvas.SetLeft(right1, width / 2 + bin / 2);
                Canvas.SetLeft(left1, width / 2 + bin / 2);
            }
            else
            {
                Canvas.SetLeft(right1, width);
                Canvas.SetLeft(left1, bin);
            }

            DrawRect();
        }
        public override void DragFilterRight2(ref DragDeltaEventArgs e)
        {
            throw new NotImplementedException("High Pass doesn't have second Thumb");
        }
    }
}

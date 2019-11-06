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
    public class FilterBandPass : Filter
    {
        public override A CreateFilter(long frequencyBin1, long frequencyBin2, long N)
        {
            long nyquistLimit = N / 2;
            //error checking
            if (frequencyBin1 > nyquistLimit && frequencyBin2 > nyquistLimit && frequencyBin1 > frequencyBin2)
            {
                frequencyBin1 = nyquistLimit - (frequencyBin1 - nyquistLimit);
            }
            if (frequencyBin2 > nyquistLimit)
            {
                frequencyBin2 = nyquistLimit - (frequencyBin2 - nyquistLimit);
            }
            if (frequencyBin1 > frequencyBin2)
            {
                long temp = frequencyBin1;
                frequencyBin1 = frequencyBin2;
                frequencyBin2 = temp;
            }

            long difference1 = ((nyquistLimit - frequencyBin2) * 2) + 1, difference2 = ((nyquistLimit - frequencyBin1) * 2) + 1;
            A filter = new A();

            long i;

            //beginning of filter
            for (i = 0; i <= frequencyBin1; i++)
            {
                filter.Add(0, 0);
            }

            //1st band
            for (i = 0; i <= frequencyBin2; i++)
            {
                filter.Add(1, 1);
            }

            //middle of filter
            for (; i <= frequencyBin2 + difference1; i++)
            {
                filter.Add(0, 0);
            }

            //2nd band
            for (; i <= frequencyBin1 + difference2; i++)
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

        // drawing and dragging
        public override void DrawFilter(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            double width = canvas.ActualWidth, bin = width / Filter.getSize();

            left1.Visibility = Visibility.Visible;
            left2.Visibility = Visibility.Visible;
            right1.Visibility = Visibility.Visible;
            right2.Visibility = Visibility.Visible;

            Canvas.SetLeft(left1, bin);
            Canvas.SetLeft(left2, width / 2);
            Canvas.SetLeft(right2, width / 2 + bin);
            Canvas.SetLeft(right1, width);

            rect1.Visibility = Visibility.Visible;
            rect2.Visibility = Visibility.Visible;

            this.DrawRect(left1, left2, right1, right2, rect1, rect2);
        }
        protected override void DrawRect(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2)
        {
            rect1.Width = Canvas.GetLeft(left2) - (int)Canvas.GetLeft(left1);
            rect2.Width = Canvas.GetLeft(right1) - (int)Canvas.GetLeft(right2);

            Canvas.SetLeft(rect1, Canvas.GetLeft(left1));
            Canvas.SetLeft(rect2, Canvas.GetLeft(right2));
        }
        public override void DragFilterLeft1(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            double width = canvas.ActualWidth, bin = width / Filter.getSize(), pos = Mouse.GetPosition(canvas).X;

            if (pos >= bin && pos <= Canvas.GetLeft(left2))
            {
                Canvas.SetLeft(left1, Canvas.GetLeft(left1) + e.HorizontalChange);
                Canvas.SetLeft(right1, width - Canvas.GetLeft(left1) + bin);
            }
            else if (pos >= Canvas.GetLeft(left2))
            {
                Canvas.SetLeft(left1, Canvas.GetLeft(left2));
                Canvas.SetLeft(right1, Canvas.GetLeft(right2));
                
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
            double width = canvas.ActualWidth, bin = width / Filter.getSize(), pos = Mouse.GetPosition(canvas).X;

            if (pos >= Canvas.GetLeft(left1) && pos <= width / 2)
            {
                Canvas.SetLeft(left2, Canvas.GetLeft(left2) + e.HorizontalChange);
                Canvas.SetLeft(right2, width - Canvas.GetLeft(left2) + bin);
            }
            else if (pos <= Canvas.GetLeft(left1))
            {
                Canvas.SetLeft(left2, Canvas.GetLeft(left1));
                Canvas.SetLeft(right2, Canvas.GetLeft(right1));
            }
            else
            {
                Canvas.SetLeft(left2, width / 2);
                Canvas.SetLeft(right2, width / 2 + bin);
            }

            this.DrawRect(left1, left2, right1, right2, rect1, rect2);
        }
        public override void DragFilterRight1(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            double width = canvas.ActualWidth, bin = width / Filter.getSize(), pos = Mouse.GetPosition(canvas).X;

            if (pos >= Canvas.GetLeft(right2) && pos <= width)
            {
                Canvas.SetLeft(right1, Canvas.GetLeft(right1) + e.HorizontalChange);
                Canvas.SetLeft(left1, width - Canvas.GetLeft(right1) + bin);
            }
            else if (pos <= Canvas.GetLeft(right2))
            {
                Canvas.SetLeft(right1, Canvas.GetLeft(right2));
                Canvas.SetLeft(left1, Canvas.GetLeft(left2));
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
            double width = canvas.ActualWidth, bin = width / Filter.getSize(), pos = Mouse.GetPosition(canvas).X;

            if (pos >= width / 2 + bin && pos <= Canvas.GetLeft(right1))
            {
                Canvas.SetLeft(right2, Canvas.GetLeft(right2) + e.HorizontalChange);
                Canvas.SetLeft(left2, width - Canvas.GetLeft(right2) + bin);
            }
            else if (pos >= Canvas.GetLeft(right1))
            {
                Canvas.SetLeft(right2, Canvas.GetLeft(right1));
                Canvas.SetLeft(left2, Canvas.GetLeft(left1));
            }
            else
            {
                Canvas.SetLeft(right2, width / 2 + bin);
                Canvas.SetLeft(left2, width / 2);
            }

            this.DrawRect(left1, left2, right1, right2, rect1, rect2);
        }
        public override void DropFilterLeft1(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException("TODO");
        }
        public override void DropFilterLeft2(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException("TODO");
        }
        public override void DropFilterRight1(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException("TODO");
        }
        public override void DropFilterRight2(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException("TODO");
        }
    }
}

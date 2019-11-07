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
        public override void CreateFilter()
        {
            A filter = new A();

            long i;

            //beginning of filter
            for (i = 0; i <= fl1; i++)
            {
                filter.Add(0, 0);
            }

            //1st band
            for (i = 0; i <= fl2; i++)
            {
                filter.Add(1, 1);
            }

            //middle of filter
            for (; i <= fr1; i++)
            {
                filter.Add(0, 0);
            }

            //2nd band
            for (; i <= fr2; i++)
            {
                filter.Add(1, 1);
            }

            //end of filter
            for (; i < A.getN(); i++)
            {
                filter.Add(0, 0);
            }

            //return filter;
        }

        // drawing and dragging
        public override void DrawFilter(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            this.left1 = left1;
            this.left2 = left2;
            this.right1 = right1;
            this.right2 = right2;
            this.rect1 = rect1;
            this.rect2 = rect2;
            this.canvas = canvas;

            double width = canvas.ActualWidth, bin = width / A.getN();

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

            this.DrawRect();
        }
        protected override void DrawRect()
        {
            rect1.Width = Canvas.GetLeft(left2) - (int)Canvas.GetLeft(left1);
            rect2.Width = Canvas.GetLeft(right1) - (int)Canvas.GetLeft(right2);

            Canvas.SetLeft(rect1, Canvas.GetLeft(left1));
            Canvas.SetLeft(rect2, Canvas.GetLeft(right2));
        }
        public override void DragFilterLeft1(DragDeltaEventArgs e)
        {
            double width = canvas.ActualWidth, bin = width / A.getN(), pos = Mouse.GetPosition(canvas).X;

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

            this.DrawRect();
        }
        public override void DragFilterLeft2(DragDeltaEventArgs e)
        {
            double width = canvas.ActualWidth, bin = width / A.getN(), pos = Mouse.GetPosition(canvas).X;

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

            this.DrawRect();
        }
        public override void DragFilterRight1(DragDeltaEventArgs e)
        {
            double width = canvas.ActualWidth, bin = width / A.getN(), pos = Mouse.GetPosition(canvas).X;

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

            this.DrawRect();
        }
        public override void DragFilterRight2(DragDeltaEventArgs e)
        {
            double width = canvas.ActualWidth, bin = width / A.getN(), pos = Mouse.GetPosition(canvas).X;

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

            this.DrawRect();
        }
        public override void DropFilterLeft1()
        {
            throw new NotImplementedException("TODO");
        }
        public override void DropFilterLeft2()
        {
            throw new NotImplementedException("TODO");
        }
        public override void DropFilterRight1()
        {
            throw new NotImplementedException("TODO");
        }
        public override void DropFilterRight2()
        {
            throw new NotImplementedException("TODO");
        }
    }
}

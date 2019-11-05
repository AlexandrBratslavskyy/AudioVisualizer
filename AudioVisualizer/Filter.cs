using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AudioVisualizer
{
    //Helper class
    //Creation of filters
    public abstract class Filter
    {
        public abstract A CreateFilter(long frequencyBin1, long frequencyBin2, long N);
        public abstract void DrawFilter(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas);
        public abstract void DragFilterLeft1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e);
        public abstract void DragFilterLeft2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e);
        public abstract void DragFilterRight1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e);
        public abstract void DragFilterRight2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e);
        public abstract void DropFilterLeft1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas);
        public abstract void DropFilterLeft2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas);
        public abstract void DropFilterRight1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas);
        public abstract void DropFilterRight2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas);
        //statics
        static public Filter FILTER = new LowPassFilter();
        static public void ChangeFilter(int newFilter)
        {
            switch (newFilter)
            {
                case 2:
                    FILTER = new BandPassFilter();
                    break;
                case 1:
                    FILTER = new HighPassFilter();
                    break;
                case 0:
                default:
                    FILTER = new LowPassFilter();
                    break;
            }
        }
    }
    public class LowPassFilter : Filter
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
            left1.Visibility = Visibility.Visible;
            left2.Visibility = Visibility.Collapsed;
            right1.Visibility = Visibility.Visible;
            right2.Visibility = Visibility.Collapsed;

            Canvas.SetLeft(left1, canvas.ActualWidth / 2);
            Canvas.SetLeft(right1, canvas.ActualWidth / 2);

            rect1.Visibility = Visibility.Visible;
            rect2.Visibility = Visibility.Visible;

            rect1.Width = canvas.ActualWidth / 2;
            rect2.Width = canvas.ActualWidth / 2;

            Canvas.SetLeft(rect1, 0);
            Canvas.SetLeft(rect2, canvas.ActualWidth / 2);
        }
        public override void DragFilterLeft1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            Point m = Mouse.GetPosition(canvas);
            if (m.X >= 0 && m.X <= canvas.ActualWidth / 2)
            {
                Canvas.SetLeft(left1, Canvas.GetLeft(left1) + e.HorizontalChange);
                Canvas.SetLeft(right1, canvas.ActualWidth - Canvas.GetLeft(left1) + e.HorizontalChange);

                //TODO rectangle
            }
            else if (m.X >= canvas.ActualWidth / 2)
            {
                Canvas.SetLeft(left1, canvas.ActualWidth / 2);
                Canvas.SetLeft(right1, canvas.ActualWidth / 2);
            }
            else
            {
                Canvas.SetLeft(left1, 0);
                Canvas.SetLeft(right1, canvas.ActualWidth);
            }
                
        }
        public override void DragFilterLeft2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            throw new NotImplementedException("Low Pass does't have second Thumb");
        }
        public override void DragFilterRight1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            Point m = Mouse.GetPosition(canvas);
            if (m.X >= canvas.ActualWidth / 2 && m.X <= canvas.ActualWidth)
            {
                Canvas.SetLeft(left1, canvas.ActualWidth - Canvas.GetLeft(right1) + e.HorizontalChange);
                Canvas.SetLeft(right1, Canvas.GetLeft(right1) + e.HorizontalChange);
            }
            else if (m.X >= canvas.ActualWidth)
            {
                Canvas.SetLeft(left1, 0);
                Canvas.SetLeft(right1, canvas.ActualWidth);
            }
            else
            {
                Canvas.SetLeft(left1, canvas.ActualWidth / 2);
                Canvas.SetLeft(right1, canvas.ActualWidth / 2);
            }
        }
        public override void DragFilterRight2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            throw new NotImplementedException();
        }
        public override void DropFilterLeft1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException();
        }
        public override void DropFilterLeft2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException();
        }
        public override void DropFilterRight1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException();
        }
        public override void DropFilterRight2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException();
        }
    }
    public class HighPassFilter : Filter
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
        }
        public override void DragFilterLeft1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            Point m = Mouse.GetPosition(canvas);
            if (m.X >= 0 && m.X <= canvas.ActualWidth / 2)
            {
                Canvas.SetLeft(left1, Canvas.GetLeft(left1) + e.HorizontalChange);
                Canvas.SetLeft(right1, canvas.ActualWidth - Canvas.GetLeft(left1) + e.HorizontalChange);
            }
            else if (m.X >= canvas.ActualWidth / 2)
            {
                Canvas.SetLeft(left1, canvas.ActualWidth / 2);
                Canvas.SetLeft(right1, canvas.ActualWidth / 2);
            }
            else
            {
                Canvas.SetLeft(left1, 0);
                Canvas.SetLeft(right1, canvas.ActualWidth);
            }
        }
        public override void DragFilterLeft2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            throw new NotImplementedException();
        }
        public override void DragFilterRight1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            Point m = Mouse.GetPosition(canvas);
            if (m.X >= canvas.ActualWidth / 2 && m.X <= canvas.ActualWidth)
            {
                Canvas.SetLeft(left1, canvas.ActualWidth - Canvas.GetLeft(right1) + e.HorizontalChange);
                Canvas.SetLeft(right1, Canvas.GetLeft(right1) + e.HorizontalChange);
            }
            else if (m.X >= canvas.ActualWidth)
            {
                Canvas.SetLeft(left1, 0);
                Canvas.SetLeft(right1, canvas.ActualWidth);
            }
            else
            {
                Canvas.SetLeft(left1, canvas.ActualWidth / 2);
                Canvas.SetLeft(right1, canvas.ActualWidth / 2);
            }
        }
        public override void DragFilterRight2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            throw new NotImplementedException();
        }
        public override void DropFilterLeft1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException();
        }
        public override void DropFilterLeft2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException();
        }
        public override void DropFilterRight1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException();
        }
        public override void DropFilterRight2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException();
        }
    }
    public class BandPassFilter : Filter
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
            left1.Visibility = Visibility.Visible;
            left2.Visibility = Visibility.Visible;
            right1.Visibility = Visibility.Visible;
            right2.Visibility = Visibility.Visible;

            Canvas.SetLeft(left1, 0);
            Canvas.SetLeft(left2, canvas.ActualWidth / 2);
            Canvas.SetLeft(right2, canvas.ActualWidth / 2);
            Canvas.SetLeft(right1, canvas.ActualWidth);
        }
        public override void DragFilterLeft1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            Point m = Mouse.GetPosition(canvas);
            if (m.X >= 0 && m.X <= canvas.ActualWidth / 2)
            {
                Canvas.SetLeft(left1, Canvas.GetLeft(left1) + e.HorizontalChange);
                Canvas.SetLeft(right1, canvas.ActualWidth - Canvas.GetLeft(left1) + e.HorizontalChange);
            }
            else if (m.X >= canvas.ActualWidth / 2)
            {
                Canvas.SetLeft(left1, canvas.ActualWidth / 2);
                Canvas.SetLeft(right1, canvas.ActualWidth / 2);
            }
            else
            {
                Canvas.SetLeft(left1, 0);
                Canvas.SetLeft(right1, canvas.ActualWidth);
            }
        }
        public override void DragFilterLeft2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            Point m = Mouse.GetPosition(canvas);
            if (m.X >= 0 && m.X <= canvas.ActualWidth / 2)
            {
                Canvas.SetLeft(left2, Canvas.GetLeft(left2) + e.HorizontalChange);
                Canvas.SetLeft(right2, canvas.ActualWidth - Canvas.GetLeft(left2) + e.HorizontalChange);
            }
            else if (m.X >= canvas.ActualWidth / 2)
            {
                Canvas.SetLeft(left2, canvas.ActualWidth / 2);
                Canvas.SetLeft(right2, canvas.ActualWidth / 2);
            }
            else
            {
                Canvas.SetLeft(left2, 0);
                Canvas.SetLeft(right2, canvas.ActualWidth);
            }
        }
        public override void DragFilterRight1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            Point m = Mouse.GetPosition(canvas);
            if (m.X >= canvas.ActualWidth / 2 && m.X <= canvas.ActualWidth)
            {
                Canvas.SetLeft(left1, canvas.ActualWidth - Canvas.GetLeft(right1) + e.HorizontalChange);
                Canvas.SetLeft(right1, Canvas.GetLeft(right1) + e.HorizontalChange);
            }
            else if (m.X >= canvas.ActualWidth)
            {
                Canvas.SetLeft(left1, 0);
                Canvas.SetLeft(right1, canvas.ActualWidth);
            }
            else
            {
                Canvas.SetLeft(left1, canvas.ActualWidth / 2);
                Canvas.SetLeft(right1, canvas.ActualWidth / 2);
            }
        }
        public override void DragFilterRight2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e)
        {
            Point m = Mouse.GetPosition(canvas);
            if (m.X >= canvas.ActualWidth / 2 && m.X <= canvas.ActualWidth)
            {
                Canvas.SetLeft(left2, canvas.ActualWidth - Canvas.GetLeft(right2) + e.HorizontalChange);
                Canvas.SetLeft(right2, Canvas.GetLeft(right2) + e.HorizontalChange);
            }
            else if (m.X >= canvas.ActualWidth)
            {
                Canvas.SetLeft(left2, 0);
                Canvas.SetLeft(right2, canvas.ActualWidth);
            }
            else
            {
                Canvas.SetLeft(left2, canvas.ActualWidth / 2);
                Canvas.SetLeft(right2, canvas.ActualWidth / 2);
            }
        }
        public override void DropFilterLeft1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException();
        }
        public override void DropFilterLeft2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException();
        }
        public override void DropFilterRight1(Thumb left1, Thumb right1, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException();
        }
        public override void DropFilterRight2(Thumb left2, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas)
        {
            throw new NotImplementedException();
        }
    }
}

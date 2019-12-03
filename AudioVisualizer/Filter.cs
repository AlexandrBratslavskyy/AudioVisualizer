using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;

namespace AudioVisualizer
{
    //Helper class
    //Creation of filters
    public abstract class Filter
    {
        // calculations for filter
        protected long fl1, fl2, fr1, fr2, N;
        protected abstract void CreateFilter();
        //Filtering using convolution to the time domain
        //Convolution algorithm to create new samples
        public S Convolution(ref S OGs)
        {
            S NEWs = new S(OGs);
            NEWs.Convolution(WEIGHTS.Size());

            for (long t = 0; t < OGs.Size(); t++)
            {
                double sum = 0;
                for (long j = 0; j < WEIGHTS.Size(); j++)
                {
                    sum += WEIGHTS.Get(j) * NEWs.Get(t + j);
                }
                NEWs.Set(t, sum);
            }

            NEWs.DeConvolution(WEIGHTS.Size());

            return NEWs;
        }
        // visual filter
        protected Thumb left1, left2, right1, right2;
        protected Rectangle rect1, rect2;
        protected Canvas canvas;
        public abstract void DrawFilter(ref Thumb left1, ref Thumb left2, ref Thumb right1, ref Thumb right2, ref Rectangle rect1, ref Rectangle rect2, ref Canvas canvas, ref long N);
        protected abstract void DrawRect();
        public abstract void DragFilterLeft1(ref DragDeltaEventArgs e);
        public abstract void DragFilterLeft2(ref DragDeltaEventArgs e);
        public abstract void DragFilterRight1(ref DragDeltaEventArgs e);
        public abstract void DragFilterRight2(ref DragDeltaEventArgs e);
        public void DropFilterLeft1()
        {
            double width = canvas.ActualWidth, bin = width / N, pos = Mouse.GetPosition(canvas).X;

            if (pos >= width / 2 + bin / 2)
            {
                fl1 = N / 2 + 1;
                fr1 = N / 2 + 1;
            }
            else if (pos <= 0)
            {
                fl1 = 0;
                fr1 = N;
            }
            else
            {
                long binNumber = 1;

                for (double i = binNumber, distance = width; i <= N / 2; ++i)
                {
                    if (Math.Abs(i * bin - Canvas.GetLeft(left1)) < distance)
                    {
                        distance = Math.Abs(i * bin - Canvas.GetLeft(left1));
                        binNumber = (long)i;
                    }
                }

                Canvas.SetLeft(left1, binNumber * bin);
                Canvas.SetLeft(right1, width - Canvas.GetLeft(left1) + bin);

                DrawRect();

                fl1 = binNumber - 1;
                fr1 = N - binNumber;
            }
            CreateFilter();
        }
        public void DropFilterLeft2()
        {
            double width = canvas.ActualWidth, bin = width / N, pos = Mouse.GetPosition(canvas).X;

            if (pos >= width / 2 + bin / 2)
            {
                fl2 = N / 2 + 1;
                fr2 = N / 2 + 1;
            }
            else if (pos <= 0)
            {
                fl2 = 0;
                fr2 = N;
            }
            else
            {
                long binNumber = 1;

                for (double i = binNumber, distance = width; i <= N / 2; ++i)
                {
                    if (Math.Abs(i * bin - Canvas.GetLeft(left2)) < distance)
                    {
                        distance = Math.Abs(i * bin - Canvas.GetLeft(left2));
                        binNumber = (long)i;
                    }
                }

                Canvas.SetLeft(left2, binNumber * bin);
                Canvas.SetLeft(right2, width - Canvas.GetLeft(left2) + bin);

                DrawRect();

                fl2 = binNumber - 1;
                fr2 = N - binNumber;
            }
            CreateFilter();
        }
        public void DropFilterRight1()
        {
            double width = canvas.ActualWidth, bin = width / N, pos = Mouse.GetPosition(canvas).X;

            if (pos <= width / 2 + bin / 2)
            {
                fl1 = N / 2 + 1;
                fr1 = N / 2 + 1;
            }
            else
            {
                long binNumber = N / 2 + 1;

                for (double i = binNumber, distance = width; i <= N; ++i)
                {
                    if (Math.Abs(i * bin - Canvas.GetLeft(right1)) < distance)
                    {
                        distance = Math.Abs(i * bin - Canvas.GetLeft(right1));
                        binNumber = (long)i;
                    }
                }

                Canvas.SetLeft(right1, binNumber * bin);
                Canvas.SetLeft(left1, width - Canvas.GetLeft(right1) + bin);

                DrawRect();

                fl1 = N - binNumber;
                fr1 = binNumber - 1;
            }
            CreateFilter();
        }
        public void DropFilterRight2()
        {
            double width = canvas.ActualWidth, bin = width / N, pos = Mouse.GetPosition(canvas).X;

            if (pos <= width / 2 + bin / 2)
            {
                fl2 = N / 2 + 1;
                fr2 = N / 2 + 1;
            }
            else
            {
                long binNumber = N / 2 + 1;

                for (double i = binNumber, distance = width; i <= N; ++i)
                {
                    if (Math.Abs(i * bin - Canvas.GetLeft(right2)) < distance)
                    {
                        distance = Math.Abs(i * bin - Canvas.GetLeft(right2));
                        binNumber = (long)i;
                    }
                }

                Canvas.SetLeft(right2, binNumber * bin);
                Canvas.SetLeft(left2, width - Canvas.GetLeft(right2) + bin);

                DrawRect();

                fl2 = N - binNumber;
                fr2 = binNumber - 1;
            }
            CreateFilter();
        }

        //statics
        static public S WEIGHTS;
    }
}

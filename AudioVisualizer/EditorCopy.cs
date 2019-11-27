using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;

namespace AudioVisualizer
{
    public class EditorCopy : Editor
    {
        public override void Edit(ref S s, ref ScrollViewer scroll) 
        {
            CP = s.Copy((int)((Canvas.GetLeft(left) + scroll.HorizontalOffset) * Display.Get()), (int)((Canvas.GetLeft(right) + scroll.HorizontalOffset) * Display.Get()));
        }

        public override void DrawEditor(ref Thumb left, ref Thumb right, ref Rectangle rect, ref Canvas canvas, ref Button btn)
        {
            this.left = left;
            this.right = right;
            this.rect = rect;
            this.canvas = canvas;
            this.btn = btn;

            left.Visibility = Visibility.Visible;
            right.Visibility = Visibility.Visible;

            Canvas.SetLeft(left, 0);
            Canvas.SetLeft(right, 0);

            rect.Visibility = Visibility.Visible;

            DrawRect();

            btn.Visibility = Visibility.Visible;

            btn.Content = "copy";
        }

        public override void DragEditorLeft(ref DragDeltaEventArgs e)
        {
            double pos = Mouse.GetPosition(canvas).X;

            if (pos >= 0 && pos <= Canvas.GetLeft(right))
            {
                Canvas.SetLeft(left, Canvas.GetLeft(left) + e.HorizontalChange);
            }
            else if (pos >= Canvas.GetLeft(right))
            {
                Canvas.SetLeft(left, Canvas.GetLeft(right));
            }
            else
            {
                Canvas.SetLeft(left, 0);
            }

            DrawRect();
        }

        public override void DragEditorRight(ref DragDeltaEventArgs e)
        {
            double width = canvas.ActualWidth, pos = Mouse.GetPosition(canvas).X;

            if (pos >= Canvas.GetLeft(left) && pos <= width)
            {
                Canvas.SetLeft(right, Canvas.GetLeft(right) + e.HorizontalChange);
            }
            else if (pos <= Canvas.GetLeft(left))
            {
                Canvas.SetLeft(right, Canvas.GetLeft(left));
            }
            else
            {
                Canvas.SetLeft(right, width);
            }

            DrawRect();
        }
    }
}

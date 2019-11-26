using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;

namespace AudioVisualizer
{
    public class EditorCut : Editor
    {
        public override void Edit(S s, ScrollViewer scroll)
        {
            s.Cut((int)((Canvas.GetLeft(left) + scroll.HorizontalOffset) * Display.Get()), (int)((Canvas.GetLeft(right) + scroll.HorizontalOffset) * Display.Get()));
        }

        public override void DrawEditor(Thumb left, Thumb right, Rectangle rect, Canvas canvas, Button btn)
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

            btn.Content = "cut";
        }

        public override void DragEditorLeft(DragDeltaEventArgs e)
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

        public override void DragEditorRight(DragDeltaEventArgs e)
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

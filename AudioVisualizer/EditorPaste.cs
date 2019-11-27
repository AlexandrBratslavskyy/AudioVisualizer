using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;

namespace AudioVisualizer
{
    public class EditorPaste : Editor
    {
        public override void Edit(ref S s, ref ScrollViewer scroll)
        {
            s.Paste((int)((Canvas.GetLeft(left) + scroll.HorizontalOffset) * Display.Get()), CP);
        }

        public override void DrawEditor(ref Thumb left, ref Thumb right, ref Rectangle rect, ref Canvas canvas, ref Button btn)
        {
            this.left = left;
            this.right = right;
            this.rect = rect;
            this.canvas = canvas;
            this.btn = btn;

            left.Visibility = Visibility.Visible;
            right.Visibility = Visibility.Collapsed;

            Canvas.SetLeft(left, 0);

            rect.Visibility = Visibility.Collapsed;

            btn.Visibility = Visibility.Visible;

            btn.Content = "paste";
        }

        public override void DragEditorLeft(ref DragDeltaEventArgs e)
        {
            double width = canvas.ActualWidth, pos = Mouse.GetPosition(canvas).X;

            if (pos >= 0 && pos <= width)
            {
                Canvas.SetLeft(left, Canvas.GetLeft(left) + e.HorizontalChange);
            }
            else if (pos >= Canvas.GetLeft(right))
            {
                Canvas.SetLeft(left, width);
            }
            else
            {
                Canvas.SetLeft(left, 0);
            }
        }

        public override void DragEditorRight(ref DragDeltaEventArgs e)
        {
            throw new NotImplementedException("Paste doesn't have second Thumb");
        }
    }
}

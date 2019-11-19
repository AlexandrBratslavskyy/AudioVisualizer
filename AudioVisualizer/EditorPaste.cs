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
    public class EditorPaste : Editor
    {
        public override void Edit(S s, ScrollViewer scroll)
        {
            s.Paste((int)((Canvas.GetLeft(left) + scroll.HorizontalOffset) * Display.Get()), CP);
        }

        public override void DrawEditor(Thumb left, Thumb right, Rectangle rect, Canvas canvas, Button btn)
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

        public override void DragEditorLeft(DragDeltaEventArgs e)
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

        public override void DragEditorRight(DragDeltaEventArgs e)
        {
            throw new NotImplementedException("Paste doesn't have second Thumb");
        }
    }
}

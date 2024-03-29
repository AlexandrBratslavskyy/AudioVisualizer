﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;

namespace AudioVisualizer
{
    // to display cut copy paste options
    // real cut copy pasting in s.cs
    public abstract class Editor
    {
        public abstract void Edit(ref S s, ref ScrollViewer scroll);

        protected Thumb left, right;
        protected Rectangle rect;
        protected Canvas canvas;
        protected Button btn;
        public abstract void DrawEditor(ref Thumb left, ref Thumb right, ref Rectangle rect, ref Canvas canvas, ref Button btn);
        public void EraseEditor()
        {
            left.Visibility = Visibility.Collapsed;
            right.Visibility = Visibility.Collapsed;

            rect.Visibility = Visibility.Collapsed;

            btn.Visibility = Visibility.Collapsed;
        }
        protected void DrawRect()
        {
            rect.Width = Canvas.GetLeft(right) - Canvas.GetLeft(left);

            Canvas.SetLeft(rect, Canvas.GetLeft(left));
        }
        public abstract void DragEditorLeft(ref DragDeltaEventArgs e);
        public abstract void DragEditorRight(ref DragDeltaEventArgs e);
        //statics
        static protected S CP;
        static public bool isCP()
        {
            return CP != null;
        }
    }
    
}

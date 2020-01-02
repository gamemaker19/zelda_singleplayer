using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFRichTextBox;

namespace GameEditor.Editor
{
    public class GlobalInput
    {
        HashSet<Key> keysHeld = new HashSet<Key>();
        public Action<Key, bool> onKeyDown;
        public Action<Key> onKeyUp;
        public DependencyObject window;

        public GlobalInput(Canvas canvas, DependencyObject window)
        {
            this.window = window;
            canvas.KeyDown += keyDownEvent;
            canvas.KeyUp += keyUpEvent;
        }

        public void keyDownEvent(object sender, KeyEventArgs e)
        {
            if (!canProcessKeyEvent()) return;
            onKeyDown?.Invoke(e.Key, !keysHeld.Contains(e.Key));
            keysHeld.Add(e.Key);
        }

        public void keyUpEvent(object sender, KeyEventArgs e)
        {
            if (!canProcessKeyEvent()) return;
            keysHeld.Remove(e.Key);
            onKeyUp?.Invoke(e.Key);
        }

        private bool canProcessKeyEvent()
        {
            var focusedControl = FocusManager.GetFocusedElement(window);
            if (focusedControl == null || focusedControl is ScrollViewerWindowsFormsHost || focusedControl is ScrollViewer || focusedControl is Canvas)
            {
                return true;
            }
            return false;
        }

    }
}

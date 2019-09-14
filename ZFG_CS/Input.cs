using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class Input
    {
        public Dictionary<Key, bool> keyHeld = new Dictionary<Key, bool>();
        public Dictionary<Key, bool> keyPressed = new Dictionary<Key, bool>();
        
        public bool isHeld(Key keyCode)
        {
            if (!keyHeld.ContainsKey(keyCode))
            {
                return false;
            }
            return keyHeld[keyCode];
        }

        public bool isPressed(Key keyCode)
        {
            if (!keyPressed.ContainsKey(keyCode))
            {
                return false;
            }
            return keyPressed[keyCode];
        }

    }
}

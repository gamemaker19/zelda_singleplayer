using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class Control
    {
        public Key Up = Key.Up;
        public Key Down = Key.Down;
        public Key Left = Key.Left;
        public Key Right = Key.Right;
        public Key Action = Key.X;
        public Key Sword = Key.C;
        public Key Map = Key.Z;
        public Key Item1 = Key.Num1;
        public Key Item2 = Key.Num2;
        public Key Item3 = Key.Num3;
        public Key Item4 = Key.Num4;
        public Key Item5 = Key.Num5;
        public Key DropItem1 = Key.Q;
        public Key DropItem2 = Key.W;
        public Key DropItem3 = Key.E;
        public Key DropItem4 = Key.R;
        public Key DropItem5 = Key.T;

        private static Control _main;
        public static Control main
        {
            get
            {
                if(_main == null)
                {
                    string text = "";
                    if (File.Exists("controls.txt"))
                    {
                        text = File.ReadAllText("controls.txt");
                    }
                    if (string.IsNullOrEmpty(text))
                    {
                        _main = new Control();
                    }
                    else
                    {
                        _main = JsonConvert.DeserializeObject<Control>(text);
                    }
                }
                return _main;
            }
        }

        public void saveToFile()
        {
            string text = JsonConvert.SerializeObject(_main);
            File.WriteAllText("controls.txt", text);
        }
    }
}

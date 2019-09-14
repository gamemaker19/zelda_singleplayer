using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class ControlMenu : IMainMenu
    {
        public Point selectArrowPos = new Point(0, 0);
        public MainMenu previous;
        public bool listenForKey = false;
        public int bindFrames = 0;

        public ControlMenu(MainMenu mainMenu)
        {
            previous = mainMenu;
        }

        public void update()
        {
            if (listenForKey)
            {
                if(bindFrames > 0)
                {
                    bindFrames--;
                    if(bindFrames <= 0)
                    {
                        bindFrames = 0;
                        listenForKey = false;
                    }
                }
                return;
            }

            if (Global.input.isPressed(Key.Up))
            {
                selectArrowPos.y--;
                if (selectArrowPos.y < 0)
                {
                    selectArrowPos.y = 0;
                }
                else
                {
                    Global.playSound("cursor");
                }
            }
            else if (Global.input.isPressed(Key.Down))
            {
                selectArrowPos.y++;
                if (selectArrowPos.y > 9)
                {
                    selectArrowPos.y = 9;
                }
                else
                {
                    Global.playSound("cursor");
                }
            }
            if (Global.input.isPressed(Key.Left))
            {
                selectArrowPos.x--;
                if (selectArrowPos.x < 0)
                {
                    selectArrowPos.x = 0;
                }
                else
                {
                    Global.playSound("cursor");
                }
            }
            else if (Global.input.isPressed(Key.Right))
            {
                selectArrowPos.x++;
                if (selectArrowPos.x > 2)
                {
                    selectArrowPos.x = 2;
                }
                else
                {
                    Global.playSound("cursor");
                }
            }
            else if (Global.input.isPressed(Key.Z))
            {
                Control.main.saveToFile();
                Global.mainMenu = previous;
            }
            else if (Global.input.isPressed(Key.X))
            {
                listenForKey = true;
            }
        }

        public void bindKey(Key key)
        {
            if (selectArrowPos.x == 0 && selectArrowPos.y == 0) Control.main.Up = key;
            if (selectArrowPos.x == 0 && selectArrowPos.y == 1) Control.main.Down = key;
            if (selectArrowPos.x == 0 && selectArrowPos.y == 2) Control.main.Left = key;
            if (selectArrowPos.x == 0 && selectArrowPos.y == 3) Control.main.Right = key;
            if (selectArrowPos.x == 0 && selectArrowPos.y == 4) Control.main.Action = key;
            if (selectArrowPos.x == 0 && selectArrowPos.y == 5) Control.main.Sword = key;
            if (selectArrowPos.x == 0 && selectArrowPos.y == 6) Control.main.Map = key;

            if (selectArrowPos.x == 1 && selectArrowPos.y == 0) Control.main.Item1 = key;
            if (selectArrowPos.x == 1 && selectArrowPos.y == 1) Control.main.Item2 = key;
            if (selectArrowPos.x == 1 && selectArrowPos.y == 2) Control.main.Item3 = key;
            if (selectArrowPos.x == 1 && selectArrowPos.y == 3) Control.main.Item4 = key;
            if (selectArrowPos.x == 1 && selectArrowPos.y == 4) Control.main.Item5 = key;
            if (selectArrowPos.x == 1 && selectArrowPos.y == 5) Control.main.DropItem1 = key;
            if (selectArrowPos.x == 1 && selectArrowPos.y == 6) Control.main.DropItem2 = key;
            if (selectArrowPos.x == 1 && selectArrowPos.y == 7) Control.main.DropItem3 = key;
            if (selectArrowPos.x == 1 && selectArrowPos.y == 8) Control.main.DropItem4 = key;
            if (selectArrowPos.x == 1 && selectArrowPos.y == 9) Control.main.DropItem5 = key;

            bindFrames = 3;
        }

        public void render()
        {
            var topLeft = new Point(40, 80);
            float ySpace = 11;

            Global.animations["MainMenu"].draw(0, 0, 1, 1, 0, 1, null, ZIndex.HUD, false);
            Global.animations["HUDSelectArrow"].draw(35 + (selectArrowPos.x * 100), topLeft.y + 17 + (selectArrowPos.y * ySpace), 1, 1, 0, 1, null, ZIndex.HUD, false);
            //Global.animations["HUDRupee"].draw(skinPos.x - 2, skinPos.y + 25, 1, 1, 0, 1, null, ZIndex.HUD, false);

            //Draw delayed
            List<float> keys = new List<float>();
            foreach (var key in DrawWrappers.walDrawObjects)
            {
                keys.Add(key.Key);
            }
            keys.Sort();

            foreach (var key in keys)
            {
                var drawLayer = DrawWrappers.walDrawObjects[key];
                Global.window.Draw(drawLayer);
            }
            DrawWrappers.walDrawObjects.Clear();

            if(!listenForKey) Helpers.drawTextStd("X = Set Control, Z = Back", topLeft.x, 65);
            else Helpers.drawTextStd("Press desired key to bind", topLeft.x, 65);

            Helpers.drawTextStd("Up: " + Control.main.Up.ToString(), topLeft.x, topLeft.y + ySpace);
            Helpers.drawTextStd("Down: " + Control.main.Down.ToString(), topLeft.x, topLeft.y + ySpace * 2);
            Helpers.drawTextStd("Left: " + Control.main.Left.ToString(), topLeft.x, topLeft.y + ySpace * 3);
            Helpers.drawTextStd("Right: " + Control.main.Right.ToString(), topLeft.x, topLeft.y + ySpace * 4);
            Helpers.drawTextStd("Lift,Action: " + Control.main.Action.ToString(), topLeft.x, topLeft.y + ySpace * 5);
            Helpers.drawTextStd("Sword: " + Control.main.Sword.ToString(), topLeft.x, topLeft.y + ySpace * 6);
            Helpers.drawTextStd("Map: " + Control.main.Map.ToString(), topLeft.x, topLeft.y + ySpace * 7);

            Helpers.drawTextStd("Item 1: " + Control.main.Item1.ToString(), topLeft.x + 100, topLeft.y + ySpace);
            Helpers.drawTextStd("Item 2: " + Control.main.Item2.ToString(), topLeft.x + 100, topLeft.y + ySpace * 2);
            Helpers.drawTextStd("Item 3: " + Control.main.Item3.ToString(), topLeft.x + 100, topLeft.y + ySpace * 3);
            Helpers.drawTextStd("Item 4: " + Control.main.Item4.ToString(), topLeft.x + 100, topLeft.y + ySpace * 4);
            Helpers.drawTextStd("Item 5: " + Control.main.Item5.ToString(), topLeft.x + 100, topLeft.y + ySpace * 5);
            Helpers.drawTextStd("Drop Item 1: " + Control.main.DropItem1.ToString(), topLeft.x + 100, topLeft.y + ySpace * 6);
            Helpers.drawTextStd("Drop Item 2: " + Control.main.DropItem2.ToString(), topLeft.x + 100, topLeft.y + ySpace * 7);
            Helpers.drawTextStd("Drop Item 3: " + Control.main.DropItem3.ToString(), topLeft.x + 100, topLeft.y + ySpace * 8);
            Helpers.drawTextStd("Drop Item 4: " + Control.main.DropItem4.ToString(), topLeft.x + 100, topLeft.y + ySpace * 9);
            Helpers.drawTextStd("Drop Item 5: " + Control.main.DropItem5.ToString(), topLeft.x + 100, topLeft.y + ySpace * 10);
        }
    }
}

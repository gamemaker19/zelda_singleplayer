using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class OptionsMenu : IMainMenu
    {
        public Point selectArrowPos = new Point(0, 0);
        public MainMenu previous;

        public OptionsMenu(MainMenu mainMenu)
        {
            previous = mainMenu;
        }

        public void update()
        {
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
                if (selectArrowPos.y > 3)
                {
                    selectArrowPos.y = 3;
                }
                else
                {
                    Global.playSound("cursor");
                }
            }
            if (Global.input.isHeld(Key.Left))
            {
                if(selectArrowPos.y == 0)
                {
                    Options.main.numCPUs = Helpers.clampInt(Options.main.numCPUs - 1, 1, 99);
                }
                if (selectArrowPos.y == 2)
                {
                    Options.main.musicVolume = Helpers.clamp(Options.main.musicVolume - 0.01f, 0, 1);
                    Global.music.updateVolume();
                }
                if (selectArrowPos.y == 3)
                {
                    Options.main.soundVolume = Helpers.clamp(Options.main.soundVolume - 0.01f, 0, 1);
                }
            }
            else if (Global.input.isHeld(Key.Right))
            {
                if (selectArrowPos.y == 0)
                {
                    Options.main.numCPUs = Helpers.clampInt(Options.main.numCPUs + 1, 1, 99);
                }
                if (selectArrowPos.y == 2)
                {
                    Options.main.musicVolume = Helpers.clamp(Options.main.musicVolume + 0.01f, 0, 1);
                    Global.music.updateVolume();
                }
                if (selectArrowPos.y == 3)
                {
                    Options.main.soundVolume = Helpers.clamp(Options.main.soundVolume + 0.01f, 0, 1);
                }
            }

            if (Global.input.isPressed(Key.Left))
            {
                if (selectArrowPos.y == 1)
                {
                    Options.main.enableMasterSword = false;
                }
            }
            else if (Global.input.isPressed(Key.Right))
            {
                if (selectArrowPos.y == 1)
                {
                    Options.main.enableMasterSword = true;
                }
            }

            if (Global.input.isPressed(Key.Z))
            {
                Options.main.saveToFile();
                Global.mainMenu = previous;
            }
        }

        public void render()
        {
            var topLeft = new Point(40, 80);
            float ySpace = 15;

            Global.animations["MainMenu"].draw(0, 0, 1, 1, 0, 1, null, ZIndex.HUD, false);
            Global.animations["HUDSelectArrow"].draw(35 + (selectArrowPos.x * 100), topLeft.y + 22 + (selectArrowPos.y * ySpace), 1, 1, 0, 1, null, ZIndex.HUD, false);
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

            Helpers.drawTextStd("Left, Right: change option, Z: Back", topLeft.x, 65);

            Helpers.drawTextStd("Number of CPUs: " + Options.main.numCPUs.ToString(), topLeft.x, topLeft.y + ySpace);
            Helpers.drawTextStd("Enable Master Sword: " + (Options.main.enableMasterSword ? "Yes" : "No"), topLeft.x, topLeft.y + ySpace * 2);
            Helpers.drawTextStd("Music Volume: " + (Mathf.RoundInt(100f * Options.main.musicVolume).ToString()), topLeft.x, topLeft.y + ySpace * 3);
            Helpers.drawTextStd("Sound Volume: " + (Mathf.RoundInt(100f * Options.main.soundVolume).ToString()), topLeft.x, topLeft.y + ySpace * 4);
        }
    }
}

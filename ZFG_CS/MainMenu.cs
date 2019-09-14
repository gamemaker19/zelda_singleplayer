using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class MainMenu : IMainMenu
    {
        public Point selectArrowPos = new Point(0, 0);
        public Point skinPos = new Point(128, 70);
        public Point optionPos1 = new Point(50, 110);
        public Point optionPos2 = new Point(50, 130);
        public Point optionPos3 = new Point(50, 150);
        public Point optionPos4 = new Point(50, 170);
        public Point optionPos5 = new Point(50, 190);
        public bool done = false;
        public float doneTime = 0;

        public MainMenu()
        {
        }

        public void update()
        {
            if (done)
            {
                doneTime += Global.spf;
                if(doneTime >= 0)
                {
                    if(selectArrowPos.y == 0)
                    {
                        initBattleRoyale();
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
                if (selectArrowPos.y > 4)
                {
                    selectArrowPos.y = 4;
                }
                else
                {
                    Global.playSound("cursor");
                }
            }
            else if (Global.input.isPressed(Key.X))
            {
                if (selectArrowPos.y == 0)
                {
                    done = true;
                    Global.playSound("sword shine 1");
                    Global.music.music.Stop();
                }
                else if (selectArrowPos.y == 2)
                {
                    Global.playSound("cursor");
                    Global.mainMenu = new SkinMenu(this);
                }
                else if (selectArrowPos.y == 3)
                {
                    Global.playSound("cursor");
                    Global.mainMenu = new ControlMenu(this);
                }
                else if (selectArrowPos.y == 4)
                {
                    Global.playSound("cursor");
                    Global.mainMenu = new OptionsMenu(this);
                }
            }
        }

        public void render()
        {
            if(done)
            {
                DrawWrappers.DrawRect(0, 0, Global.screenW, Global.screenH, true, Color.Black, 1, ZIndex.HUD + 10, false);
                return;
            }
            
            Global.animations["MainMenu"].draw(0, 0, 1, 1, 0, 1, null, ZIndex.HUD, false);

            var tagsToHide = new HashSet<string>();
            tagsToHide.Add("shield");
            tagsToHide.Add("shield2");
            tagsToHide.Add("shield3");

            var clone = Global.animations["LinkIdleDown"].clone();
            clone.bitmap = Global.textures[SaveData.saveData.skin];
            clone.draw(skinPos.x, skinPos.y, 1, 1, 0, 1, null, ZIndex.HUD, false, childFrameTagsToHide: tagsToHide);
            Global.animations["HUDSelectArrow"].draw(40 + selectArrowPos.x, 117 + (selectArrowPos.y * 20), 1, 1, 0,1, null, ZIndex.HUD, false);
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

            Helpers.drawTextUI("Wins: " + SaveData.saveData.wins.ToString(), skinPos.x, skinPos.y + 25);
            Helpers.drawTextStd("Battle Royale (vs. 99 CPUs)", optionPos1.x, optionPos1.y);
            Helpers.drawTextStd("1v1 (Split-screen)", optionPos2.x, optionPos2.y);
            Helpers.drawTextStd("Skins", optionPos3.x, optionPos3.y);
            Helpers.drawTextStd("Controls", optionPos4.x, optionPos4.y);
            Helpers.drawTextStd("Options", optionPos5.x, optionPos5.y);
        }

        public void initBattleRoyale()
        {
            Global.mainMenu = null;
            Global.game = new Game();
            Global.game.loadLevels();

            Global.game.overworld = Global.levels["lttp_overworld"];
            Global.game.overworld.startMusic();

            Global.game.menu = new BRDropMenu();
            
            Global.game.initStorm();

        }
    }
}

using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class SkinMenu : IMainMenu
    {
        public Point selectArrowPos;
        public MainMenu previous;
        public List<string[,]> skinPages = new List<string[,]>();
        int skinsPerRow = 10;
        int rowsPerPage = 6;
        int page = 0;
        public Point text1 = new Point(25, 65);
        public Point text2 = new Point(224, 65);

        public SkinMenu(MainMenu mainMenu)
        {
            var skinsWithLink = new List<string>(Global.skins);
            skinsWithLink.Insert(0, "link2");
            int skinCount = skinsWithLink.Count;
            int numPerPage = skinsPerRow * rowsPerPage;

            string[][] chunks = skinsWithLink
                    .Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / numPerPage)
                    .Select(grp => grp.Select(x => x.Value).ToArray())
                    .ToArray();

            foreach (var skinGroup in chunks)
            {
                int skinCount2 = skinGroup.Length;
                var skins2 = new string[(int)Mathf.Ceil(skinCount2 / (float)skinsPerRow), skinsPerRow];
                int currentRow = 0;
                for (int i = 0; i < skinCount2; i++)
                {
                    if (i > 0 && i % skinsPerRow == 0) currentRow++;
                    skins2[currentRow, i % skinsPerRow] = skinGroup[i];
                }
                skinPages.Add(skins2);
            }

            previous = mainMenu;
        }

        string[,] currentSkinpage()
        {
            return skinPages[page];
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
                if (selectArrowPos.y >= skinPages[page].GetLength(0))
                {
                    selectArrowPos.y = skinPages[page].GetLength(0) - 1;
                }
                else
                {
                    Global.playSound("cursor");
                }
            }
            else if (Global.input.isPressed(Key.Left))
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
                if (selectArrowPos.x >= skinPages[page].GetLength(1))
                {
                    selectArrowPos.x = skinPages[page].GetLength(1) - 1;
                }
                else
                {
                    Global.playSound("cursor");
                }
            }
            else if (Global.input.isPressed(Key.X))
            {
                if (getSkin() != null)
                {
                    SaveData.saveData.setSkin(getSkin());
                    Global.playSound("sword shine 1");
                    Global.mainMenu = previous;
                }
            }
            else if (Global.input.isPressed(Key.Z))
            {
                Global.mainMenu = previous;
            }
            else if (Global.input.isPressed(Key.A))
            {
                page--;
                if (page < 0)
                {
                    page = 0;
                }
                else
                {
                    Global.playSound("cursor");
                    selectArrowPos = Point.Zero;
                }
            }
            else if (Global.input.isPressed(Key.S))
            {
                page++;
                if (page >= skinPages.Count)
                {
                    page = skinPages.Count - 1;
                }
                else
                {
                    Global.playSound("cursor");
                    selectArrowPos = Point.Zero;
                }
            }
        }

        public void render()
        {
            Global.animations["MainMenu"].draw(0, 0, 1, 1, 0, 1, null, ZIndex.HUD, false);

            var tagsToHide = new HashSet<string>();
            tagsToHide.Add("shield");
            tagsToHide.Add("shield2");
            tagsToHide.Add("shield3");

            float skinBoxLength = (20);
            Point skinBoxTopLeft = new Point(30, 80);

            int offsetY = (int)selectArrowPos.y - 6;
            if (offsetY < 0) offsetY = 0;

            for (int i = 0; i + offsetY < skinPages[page].GetLength(0); i++)
            {
                for(int j = 0; j < skinPages[page].GetLength(1); j++)
                {
                    var animation = Global.animations["LinkIdleDown"].clone();
                    string skin = skinPages[page][i + offsetY, j];
                    if (skin != null)
                    {
                        animation.bitmap = Global.textures[skin];
                        animation.draw(skinBoxTopLeft.x + j * skinBoxLength, skinBoxTopLeft.y + i * skinBoxLength, 1, 1, 0, 1, null, ZIndex.HUD, false, childFrameTagsToHide: tagsToHide);
                    }
                }
            }

            DrawWrappers.DrawRectWH(skinBoxTopLeft.x + selectArrowPos.x * skinBoxLength, skinBoxTopLeft.y + selectArrowPos.y * skinBoxLength, skinBoxLength, skinBoxLength, false, Color.Green, 2, ZIndex.HUD, false);

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

            Helpers.drawTextStd("X = Select, Z = Back", text2.x, text2.y, Alignment.Right);
            Helpers.drawTextStd("<A", 18, 124);
            Helpers.drawTextStd("S>", 241, 124, Alignment.Right);
            string displaySkin = getSkin();
            if (displaySkin != null)
            {
                if (displaySkin == "link2") displaySkin = "link";
                if (displaySkin.Contains(".2")) displaySkin = displaySkin.Replace(".2", "");
                Helpers.drawTextStd(displaySkin, text1.x, text1.y);
            }
        }

        string getSkin()
        {
            return skinPages[page][(int)selectArrowPos.y, (int)selectArrowPos.x];
        }
    }
}

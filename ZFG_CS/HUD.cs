using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class HUD
    {
        public Character character;
        public float msFlashTime = 0;
        public int msFlashIndex = 0;
        public float msBlinkTime = 0;
        public bool msBlink = false;

        public HUD()
        {
        }

        public HUD(Character character)
        {
            this.character = character;
        }

        public void drawOnMap(Texture texture, float sx, float sy, float sw, float sh, float dx, float dy)
        {
            Sprite sprite = new Sprite(texture, new IntRect((int)sx, (int)sy, (int)sw, (int)sh));
            sprite.Position = new Vector2f(dx, dy);
            Global.mapCircleBuffer.Draw(sprite);
        }

        public void draw()
        {
            Point topLeft = new Point(0, 0);

            Point heartTopLeft = topLeft + new Point(2, 2);
            Point magicTopLeft = topLeft + new Point(2, 11);
            Point itemTopLeft = topLeft + new Point(130, 2);

            Point stormTimerTopLeft = topLeft + new Point(130, 27);
            Point killsTopLeft = topLeft + new Point(159, 27);
            Point remainingTopLeft = topLeft + new Point(177, 27);

            Point rupeeTopLeft = topLeft + new Point(205, 27);
            Point arrowTopLeft = topLeft + new Point(230, 27);

            Point killFeedTopLeft = topLeft + new Point(5, 145);
            Point messageCenter = topLeft + new Point(128, 55);

            Point mapTopLeft = new Point(256 - 2, 224 - 2);

            Point heartSize = new Point(7, 7);
            Point itemSize = new Point(22, 22);
            Point magicOffset = new Point(3, 3);
            Point itemOffset = new Point(3, 3);
            Point magicRect = new Point(96, 7);

            for (int i = 0; i < character.health.maxValue; i++)
            {
                int oneIndex = i + 1;
                int frameIndex = 0;
                if (character.health.value >= oneIndex)
                {
                    frameIndex = 0;
                }
                else if (character.health.value < oneIndex - 1)
                {
                    frameIndex = 2;
                }
                else
                {
                    float dec = character.health.value - (float)Math.Floor(character.health.value);
                    if (dec >= 0.8f) frameIndex = 1;
                    else if (dec < 0.8f && dec >= 0.6f) frameIndex = 1;
                    else if (dec < 0.6f && dec >= 0.4f) frameIndex = 1;
                    else if (dec < 0.4f && dec >= 0.2f) frameIndex = 1;
                    else frameIndex = 2;
                }
                float offsetX = i * (heartSize.x + 2);
                Global.animations["HUDHeart"].frameIndex = frameIndex;
                Global.animations["HUDHeart"].draw(heartTopLeft.x + offsetX, heartTopLeft.y, 1, 1, 0, 1, null, ZIndex.HUD, false);
            }

            Global.animations["HUDMagicMeterBig"].draw(magicTopLeft.x, magicTopLeft.y, 1, 1, 0, 1, null, ZIndex.HUD, false);

            float w = (character.magic.value / character.magic.maxValue) * magicRect.x;
            DrawWrappers.DrawRect(magicTopLeft.x + magicOffset.x, magicTopLeft.y + magicOffset.y, magicTopLeft.x + magicOffset.x + w, magicTopLeft.y + magicOffset.y + magicRect.y, true, new Color(32, 214, 50), 1, ZIndex.HUD, false);

            for (int i = 0; i < 5; i++)
            {
                float offsetX = i * (itemSize.x + itemOffset.x);
                Global.animations["HUDItemBox"].draw(itemTopLeft.x + offsetX, itemTopLeft.y, 1, 1, 0, 1, null, ZIndex.HUD, false);

                var item = character.items[i];
                if (item != null)
                {
                    Global.animations["HUDItem"].frameIndex = item.item.spriteIndex;

                    if (item.item == Item.bow && character.arrows.value == 0)
                    {
                        Global.animations["HUDItem"].frameIndex = 15;
                    }

                    Global.animations["HUDItem"].draw(itemTopLeft.x + offsetX + 3, itemTopLeft.y + 3, 1, 1, 0, 1, null, ZIndex.HUD, false);
                    if (item.item.usesQuantity)
                    {
                        int count = (int)item.count;
                        Helpers.drawTextUI(count.ToString(), itemTopLeft.x + offsetX + 14, itemTopLeft.y + 10);
                    }
                }
            }

            for (int i = 0; i < Global.game.killFeed.Count; i++)
            {
                //Helpers.drawTextStd(Global.killFeed[i].message, killFeedTopLeft.x, killFeedTopLeft.y + (i * 12));
                Global.game.killFeed[i].draw(killFeedTopLeft.x, killFeedTopLeft.y + (i * 16));
            }

            if (!Global.hideMap)
            {
                //Map section
                Global.window.SetView(Global.hudView);

                Global.mapCircleBuffer.Clear(Color.Transparent);
                RenderStates states = new RenderStates(Global.mapCircleBuffer.Texture);
                states.BlendMode = new BlendMode(BlendMode.Factor.One, BlendMode.Factor.One, BlendMode.Equation.Subtract);

                float x = (float)Math.Floor(Global.game.getCurrentLevel().getTopLeftCamPos().x);
                float y = (float)Math.Floor(Global.game.getCurrentLevel().getTopLeftCamPos().y);

                byte a = (int)(0.33f * 255);
                Color stormColor = new Color(252, 186, 3, a);

                RectangleShape rect = new RectangleShape(new Vector2f(75, 75));
                rect.FillColor = stormColor;
                Global.mapCircleBuffer.Draw(rect, states);

                float edgeOffset = 5;
                float actualSize = 65;

                CircleShape circle1 = new CircleShape(Mathf.Ceil(Global.game.currentStormRadius / actualSize));
                circle1.FillColor = stormColor;
                circle1.Position = new Vector2f(
                    edgeOffset + Mathf.Ceil(Global.game.currentStormCenter.x / actualSize) - circle1.Radius,
                    edgeOffset + Mathf.Ceil(Global.game.currentStormCenter.y / actualSize) - circle1.Radius);

                Global.mapCircleBuffer.Draw(circle1, states);

                CircleShape circle2 = new CircleShape(Mathf.Ceil(Global.game.nextStormRadius / actualSize));
                circle2.OutlineThickness = 1;
                circle2.OutlineColor = Color.Black;
                circle2.FillColor = Color.Transparent;
                circle2.Position = new Vector2f(
                    edgeOffset + Mathf.Ceil(Global.game.nextStormCenter.x / actualSize) - circle2.Radius,
                    edgeOffset + Mathf.Ceil(Global.game.nextStormCenter.y / actualSize) - circle2.Radius);
                Global.mapCircleBuffer.Draw(circle2);

                CircleShape circle3 = new CircleShape(Mathf.Ceil(Global.game.currentStormRadius / actualSize));
                circle3.OutlineThickness = 1;
                circle3.OutlineColor = Color.Red;
                circle3.FillColor = Color.Transparent;
                circle3.Position = new Vector2f(
                    edgeOffset + Mathf.Ceil(Global.game.currentStormCenter.x / actualSize) - circle3.Radius,
                    edgeOffset + Mathf.Ceil(Global.game.currentStormCenter.y / actualSize) - circle3.Radius);
                Global.mapCircleBuffer.Draw(circle3);

                msBlinkTime += Global.spf;
                if (msBlinkTime > 0.5)
                {
                    msBlinkTime = 0;
                    msBlink = !msBlink;
                }

                var ms = Global.animations["MapSword"];

                Point msPos;
                int msFrameIndex = 0;
                if (Global.game.masterSwordChar != null)
                {
                    msFlashTime += Global.spf;
                    if (msFlashTime > 0.1)
                    {
                        msFlashTime = 0;
                        msFlashIndex = (msFlashIndex == 1 ? 0 : 1);
                    }
                    msFrameIndex = msFlashIndex;
                    msPos = new Point(edgeOffset + Mathf.Ceil(Global.game.masterSwordChar.overworldPos.x / actualSize), edgeOffset + Mathf.Ceil(Global.game.masterSwordChar.overworldPos.y / actualSize));
                    if (Global.game.masterSwordChar == Global.game.camCharacter)
                    {
                        msPos.x++;
                        msPos.y--;
                    }

                    drawOnMap(ms.bitmap, ms.frames[msFrameIndex].rect.x1, ms.frames[msFrameIndex].rect.y1, ms.frames[msFrameIndex].rect.w(), ms.frames[msFrameIndex].rect.h(), msPos.x, msPos.y - 2);
                }
                else if(Options.main.enableMasterSword)
                {
                    if (msBlink)
                    {
                        if (Global.game.masterSword != null)
                        {
                            msPos = new Point(edgeOffset + Mathf.Ceil(Global.game.masterSword.overworldPos.x / actualSize), edgeOffset + Mathf.Ceil(Global.game.masterSword.overworldPos.y / actualSize));
                        }
                        else
                        {
                            msPos = new Point(edgeOffset + Mathf.Ceil(30 / actualSize), edgeOffset + Mathf.Ceil(30 / actualSize));
                        }
                        drawOnMap(ms.bitmap, ms.frames[msFrameIndex].rect.x1, ms.frames[msFrameIndex].rect.y1, ms.frames[msFrameIndex].rect.w(), ms.frames[msFrameIndex].rect.h(), msPos.x - 1, msPos.y - 3);
                    }
                }

                var mp = Global.animations["MapPlayer"];
                if (Global.game.camCharacter != null)
                {
                    drawOnMap(mp.bitmap, mp.frames[0].rect.x1, mp.frames[0].rect.y1, mp.frames[0].rect.w(), mp.frames[0].rect.h(), edgeOffset + Mathf.Ceil(Global.game.camCharacter.overworldPos.x / actualSize), edgeOffset + Mathf.Ceil(Global.game.camCharacter.overworldPos.y / actualSize));
                }

                foreach (Character character in Global.game.characters)
                {
                    //if(character.aiStateManager && character.aiStateManager.alwaysActive)
                    //al_draw_bitmap_region(mp.bitmap, mp.frames[0].rect.x1, mp.frames[0].rect.y1, mp.frames[0].rect.w(), mp.frames[0].rect.h(), edgeOffset + Mathf.Ceil(character.overworldPos.x / actualSize), edgeOffset + Mathf.Ceil(character.overworldPos.y / actualSize), 0);
                }
                
                //al_set_target_bitmap(Global.buffer);

                //end inside map section

                Global.animations["MapSmall"].drawImmediate(mapTopLeft.x, mapTopLeft.y);

                Global.mapCircleBuffer.Display();
                Sprite sprite = new Sprite(Global.mapCircleBuffer.Texture);
                sprite.Position = new Vector2f((mapTopLeft.x - 80), (mapTopLeft.y - 80));
                Global.window.Draw(sprite);

                //wal_draw_bitmap_region(Global.mapCircleBuffer, 0, 0, 75, 75, (mapTopLeft.x - 80), (mapTopLeft.y - 80), 0, ZIndex.HUD, 1, false, { });
                Helpers.drawTextUI("z: hide map", mapTopLeft.x - 4, mapTopLeft.y - 94, Alignment.Right);

                //end map section
            }
            else
            {
                Helpers.drawTextUI("z: show map", 256 - 4, 224 - 14, Alignment.Right);
            }

            if (Global.game.currentMessage != "")
            {
                Helpers.drawTextStd(Global.game.currentMessage, messageCenter.x, messageCenter.y, Alignment.Center);
            }
            //character.rupees.value = 999;
            //character.arrows.value = 99;

            int stormTimeMinutes = (int)Global.game.stormTime / 60;
            int stormTimeSeconds = (int)Global.game.stormTime - (stormTimeMinutes * 60);
            string minutesStr = stormTimeMinutes.ToString();
            string secondsStr = stormTimeSeconds.ToString();
            if (secondsStr.Length == 1) secondsStr = "0" + secondsStr;

            if (Global.game.isStormWait) Global.animations["HUDClock"].frameIndex = 0;
            else Global.animations["HUDClock"].frameIndex = 1;
            Global.animations["HUDClock"].draw(stormTimerTopLeft.x, stormTimerTopLeft.y - 2, 1, 1, 0, 1, null, ZIndex.HUD, false);
            Helpers.drawTextUI(minutesStr + ":" + secondsStr, stormTimerTopLeft.x + 13, stormTimerTopLeft.y - 1);

            Global.animations["HUDRemaining"].draw(remainingTopLeft.x, remainingTopLeft.y - 3, 1, 1, 0, 1, null, ZIndex.HUD, false);
            Helpers.drawTextUI(Global.game.remainingCharacters.ToString(), remainingTopLeft.x + 17, remainingTopLeft.y - 1);

            Global.animations["HUDKill"].draw(killsTopLeft.x, killsTopLeft.y, 1, 1, 0, 1, null, ZIndex.HUD, false);
            Helpers.drawTextUI(((int)character.kills).ToString(), killsTopLeft.x + 10, killsTopLeft.y - 1);

            Global.animations["HUDRupee"].draw(rupeeTopLeft.x, rupeeTopLeft.y, 1, 1, 0, 1, null, ZIndex.HUD, false);
            Helpers.drawTextUI(((int)character.rupees.value).ToString(), rupeeTopLeft.x + 10, rupeeTopLeft.y - 1);

            Global.animations["HUDArrows"].draw(arrowTopLeft.x, arrowTopLeft.y, 1, 1, 0, 1, null, ZIndex.HUD, false);
            Helpers.drawTextUI(((int)character.arrows.value).ToString(), arrowTopLeft.x + 15, arrowTopLeft.y - 1);

            float rx = Global.screenW / 2;
            float ry = Global.screenH / 2;
            //wal_draw_rectangle(rx - 1, ry - 1, rx + 1, ry + 1, true, al_color_name("red"), 1, (int)ZIndices.HUD, false);
        }

    }
}

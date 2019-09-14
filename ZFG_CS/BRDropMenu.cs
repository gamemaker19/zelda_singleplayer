using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class BRDropMenu
    {
        public int x = 0;
        public int y = 0;
        public int howManyToDropNearMasterSword = 5;
        public float offsetX = 0;
        public float offsetY = 0;
        public bool disabled = false;
        public bool playerDropped = false;
        public float playerDroppedTime = 0;
        public float remainingTime = 30;
        public int cpusDropped = 0;
        public bool transitionIn = false;
        public bool transitionOut = false;
        public float blinkTime = 0;
        public bool blink = false;
        public List<DropCharacter> dropChars = new List<DropCharacter>();
        public List<List<Widget>> widgets;
        public Animation sprite;

        public BRDropMenu()
        {
            sprite = Global.animations["Map"];
            sprite.xScale = 0.75f;
            sprite.yScale = 0.75f;
            offsetX = 0;
            offsetY = 0;
            x = 256;
            y = 256;
            //x = 114;
            //y = 136;
            widgets = Helpers.RepeatedDefault<List<Widget>>(512);
            
            for (int i = 0; i < widgets.Count; i++)
            {
                widgets[i] = Helpers.RepeatedDefault<Widget>(512);
                for (int j = 0; j < widgets[i].Count; j++)
                {
                    widgets[i][j] = new Widget("");
                }
            }
        }

        public void dropCPU(bool playSound = true)
        {
            if (!disabled)
            {
                if (playSound)
                {
                    Global.playSound("link falls");
                }
            }
            DropCharacter dropChar = new DropCharacter("");
            int dropX = 0;
            int dropY = 0;
            do
            {
                dropX = Helpers.randomRange(0, widgets[0].Count - 1);
                dropY = Helpers.randomRange(0, widgets.Count - 1);
            }
            while (!Global.game.overworld.tileSlots[dropY][dropX].canLand || !droppedNearMasterSword(dropX, dropY));
            howManyToDropNearMasterSword--;
            if (Global.debugDrop)
            {
                dropX = x;
                dropY = y;
            }
            dropChar.dropCoords = new GridCoords(dropY, dropX);

            dropChars.Add(dropChar);
        }

        public bool droppedNearMasterSword(int dropX, int dropY)
        {
            if (howManyToDropNearMasterSword <= 0) return true;
            return dropX < 192 && dropY < 192;
        }

        public void update()
        {
            for (int i = 0; i < dropChars.Count; i++)
            {
                DropCharacter dropChar = dropChars[i];

                if (!disabled)
                {
                    dropChar.fallSprite.update();
                    Point dropPos = getDropPos(dropChar.dropCoords.i, dropChar.dropCoords.j);
                    dropChar.fallSprite.draw(dropPos.x - 6, dropPos.y - 6, 1, 1, 0, 1, null, (int)ZIndex.HUD + 2, false);
                }

                dropChar.dropTime += Global.spf;
                if (dropChar.dropTime >= 1.5 && !dropChar.dropped)
                {
                    dropChar.dropped = true;
                    string name = dropChar.isPlayer ? "Player1" : "CPU" + i.ToString();
                    Point landPos = new Point(4 + dropChar.dropCoords.j * 8, 4 + dropChar.dropCoords.i * 8);
                    
                    string skin = "";
                    if (dropChar.isPlayer) skin = SaveData.saveData.skin;
                    else skin = dropChar.skin;
                    Character character = new Character(Global.game.overworld, landPos, Direction.Down, dropChar.isPlayer, skin, name, dropChar.isPlayer ? Global.input : new Input());
                    character.pos -= character.linkColliderOffset;

                    if (dropChar.isPlayer)
                    {
                        disabled = true;
                        Global.game.character = character;
                        Global.game.camCharacter = character;
                        Global.game.hud = new HUD(character);
                        Global.game.character.updateCamera();
                        character.checkMusicChange();
                    }
                    else
                    {
                        character.aiStateManager = new AIStateManager(character);
                    }

                    Global.game.characters.Add(character);
                    character.stateManager.changeState(new LinkLand(), true);

                }
            }

            remainingTime -= Global.spf;

            if (remainingTime <= 0 && cpusDropped < Options.main.numCPUs)
            {
                int remaining = Options.main.numCPUs - cpusDropped;
                while (remaining > 0)
                {
                    cpusDropped++;
                    dropCPU(false);
                    remaining--;
                }
            }
            else if (cpusDropped < Options.main.numCPUs)
            {
                if (Global.debugDrop)
                {
                    cpusDropped++;
                    dropCPU();
                }
                else
                {
                    int randDrop = Helpers.randomRange(0, 1800 / Options.main.numCPUs);
                    if (randDrop == 0)
                    {
                        cpusDropped++;
                        dropCPU();
                    }
                }
            }

            if (playerDropped)
            {
                return;
            }

            if (Global.input.isHeld(Control.main.Left))
            {
                x -= 2;
                if (x < 0) x = 0;
            }
            else if (Global.input.isHeld(Control.main.Right))
            {
                x += 2;
                if (x >= widgets[0].Count) x = widgets[0].Count - 1;
            }
            if (Global.input.isHeld(Control.main.Up))
            {
                y -= 2;
                if (y < 0) y = 0;
            }
            else if (Global.input.isHeld(Control.main.Down))
            {
                y += 2;
                if (y >= widgets.Count) y = widgets.Count - 1;
            }

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                playerDropped = true;
                Global.playSound("link falls");
                DropCharacter dropChar = new DropCharacter(SaveData.saveData.skin);
                dropChar.isPlayer = true;
                while (!Global.game.overworld.tileSlots[y][x].canLand)
                {
                    x = Helpers.randomRange(0, widgets[0].Count - 1);
                    y = Helpers.randomRange(0, widgets.Count - 1);
                }
                dropChar.dropCoords = new GridCoords(y, x);
                dropChars.Add(dropChar);
            }
            else if (Global.input.isPressed(Control.main.Action) && Global.game.overworld.tileSlots[y][x].canLand)
            {
                playerDropped = true;
                Global.playSound("link falls");
                DropCharacter dropChar = new DropCharacter(SaveData.saveData.skin);
                dropChar.dropCoords = new GridCoords(y, x);
                dropChar.isPlayer = true;
                dropChars.Add(dropChar);
                Global.playSound("link falls");
            }
        }

        public Point getDropPos(int i, int j)
        {
            float offsetX = 26;
            float offsetY = 12;
            float ratio = 0.0458984375f;
            float selX = offsetX + (j * 8) * ratio;
            float selY = offsetY + (i * 8) * ratio;
            return new Point(selX, selY);
        }

        public void render()
        {
            if (disabled) return;

            sprite.draw(offsetX, offsetY, 1, 1, 0, 1, null, (int)ZIndex.HUD, false);

            if (playerDropped) return;
            for (int i = 0; i < widgets.Count; i++)
            {
                for (int j = 0; j < widgets[i].Count; j++)
                {
                    if (widgets[i][j] == null) continue;
                    Point sel = getDropPos(i, j);
                    widgets[i][j].render(i, j, (int)sel.x, (int)sel.y, x == j && y == i);
                }
            }
            Helpers.drawTextStd("Select Landing Zone", 128, 2, Alignment.Center);
            Helpers.drawTextStd("Arrow Keys: Move, " + Control.main.Action.ToString() + ": Select", 128, 15, Alignment.Center);
            Helpers.drawTextStd("Seconds before auto-drop: " + ((int)remainingTime).ToString(), 128, 25, Alignment.Center);
        }

        public bool isTransitionedOut()
        {
            return (offsetY >= Global.screenH);
        }
    }


    public class Widget
    {
        public Animation sprite = null;

        public Widget(string spriteName)
        {
            if (spriteName != "")
            {
                sprite = Global.animations[spriteName];
            }
        }

        public void render(int i, int j, int x, int y, bool isSelected)
        {
            if (sprite != null)
            {
                sprite.draw(x, y, 1, 1, 0, 1, null, 0, false);
            }

            if (isSelected)
            {
                var clone = Global.animations["LinkMapDungeon"].clone();
                clone.bitmap = Global.textures[SaveData.saveData.skin];
                clone.draw(x, y, 1, 1, 0, 1, null, (int)ZIndex.HUD + 1, false);
                if (Global.game.overworld.tileSlots[i][j].canLand)
                {
                    Global.animations["MapGood"].draw(x + 8, y + 8, 1, 1, 0, 1, null, (int)ZIndex.HUD + 2, false);
                }
                else
                {
                    Global.animations["MapBad"].draw(x + 8, y + 8, 1, 1, 0, 1, null, (int)ZIndex.HUD + 2, false);
                }
                //DrawWrappers.DrawRect(x, y, x + 8, y + 8, false, al_color_name("red"), 2, ZHUD + 1, false);
            }
        }
    }

    public class DropCharacter
    {
        public Animation fallSprite;
        public GridCoords dropCoords;
        public float dropTime = 0;
        public bool dropped = false;
        public bool isPlayer = false;
        public string skin = "";

        public DropCharacter(string optSkin)
        {
            if(string.IsNullOrEmpty(optSkin))
            {
                //skin = "Link";
                if (Helpers.randomRange(0, 1) == 0) skin = Global.skins[Helpers.randomRange(0, Global.skins.Count - 1)];
                else skin = "Link";
            }
            else
            {
                skin = optSkin;
            }

            this.fallSprite = Global.animations["LinkFall"].clone();
            if (skin != "Link") this.fallSprite.bitmap = Global.textures[skin];
        }
    }

}
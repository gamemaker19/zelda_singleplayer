using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace ZFG_CS
{
    public class KillFeedEntry
    {
        public string message = "";
        public float time = 0;
        public Item killItem;
        public Actor killer;
        public Character victim;
        
        public KillFeedEntry(string message)
        {
	        this.message = message;
        }

        public KillFeedEntry(Actor killer, Character victim, Item killItem)
        {
            this.killer = killer;
            this.victim = victim;
            this.killItem = killItem;
        }

        public void draw(float x, float y)
        {
            if (message != "")
            {
                Helpers.drawTextStd(message, x, y);
                return;
            }

            if (killer == Global.game.stormKiller)
            {
                string message = victim.playerName + " was engulfed in Twilight";
                Helpers.drawTextStd(message, x, y);
                return;
            }
            else if (killer == null)
            {
                Helpers.drawTextStd(victim.playerName + " died", x, y);
                return;
            }

            Character killerChar = killer.getChar();
            if (killerChar == null)
            {
                Helpers.drawTextStd(victim.playerName + " died", x, y);
                return;
            }
            if (killItem != null)
            {
                if (killerChar != victim)
                {
                    Helpers.drawTextStd(killerChar.playerName, x, y);

                    Text text = new Text(killerChar.playerName, Global.font, 48);

                    float width = text.GetLocalBounds().Width / Global.windowScale;
                    Global.animations["HUDItem"].frameIndex = killItem.spriteIndex;
                    Global.animations["HUDItem"].draw(x + width + 2, y - 2, 1, 1, 0, 1, null, (int)ZIndex.HUD, false);
                    Helpers.drawTextStd(victim.playerName, x + width + 20, y);
                }
                else
                {
                    Global.animations["HUDItem"].frameIndex = killItem.spriteIndex;
                    Global.animations["HUDItem"].draw(x, y, 1, 1, 0, 1, null, (int)ZIndex.HUD, false);
                    Helpers.drawTextStd(victim.playerName, x + 20, y);
                }
            }
            else
            {
                Helpers.drawTextStd(killerChar.playerName + " defeated " + victim.playerName, x, y);
            }
        }
    }
}

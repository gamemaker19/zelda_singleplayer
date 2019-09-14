using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class LinkWin : ActorState
    {
        public LinkWin() : base("LinkWin", "LinkBombos")
        {
            isInvincible = true;
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
            actor.sprite.frameIndex = 10;
            actor.sprite.frameSpeed = 0;
            if (actor.getChar() == Global.game.character)
            {
                actor.level.startMusic("victory");
                SaveData.saveData.incWins();
            }
            actor.childFrameTagsToHide.Add("shield");
            actor.changeDir(Direction.Right);
        }

        public override void update()
        {
            base.update();
            if (actor.getChar() == Global.game.character)
            {
                Global.animations["HUDVictory"].draw(128, 180, 1, 1, 0, 1, null, (int)ZIndex.AboveFont + 1, false);
            }
            else
            {
                Global.game.setCurrentMessage(actor.getChar().playerName + " wins!", 100);
            }
            if (stateTime > 8)
            {
                Global.game.setCurrentMessage("Press Enter to continue", 999);
                Global.game.enterGoesToMainMenu = true;
            }
        }
    }
}

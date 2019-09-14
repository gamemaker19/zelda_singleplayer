using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkFall : ActorState
    {
        Entrance entrance = null;

        public LinkFall(Entrance entrance) : base("LinkFall")
        {
            this.entrance = entrance;
            drawWadable = false;
            aiTarget = false;
            isInvincible = true;
        }

        public override void onEnter(ActorState oldState)
        {
            actor.changePos(entrance.pos, false);
            actor.changeDir(Direction.Right);
            actor.drawShadow = false;
            actor.playSound("link falls");
        }

        public override void update()
        {
            base.update();
            if (actor.sprite.isAnimOver())
            {
                actor.drawShadow = true;
                stateManager.changeState(new LinkLand(), false);
                Global.game.changeLevel(entrance, actor as Character);
            }
        }

    }
}

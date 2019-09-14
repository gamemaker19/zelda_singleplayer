using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkCaneOfBryana : ActorState
    {
        Actor bryanaStart = null;
        public LinkCaneOfBryana() : base("LinkCaneBryana")
        {
        }

        public override void update()
        {
            base.update();
            actor.sprite.useBluePalette = true;

            if (bryanaStart == null)
            {
                bryanaStart = new Anim(actor.level, actor.pos, "BryanaStart");
            }
            else if (actor.sprite.frames.Count > 0 && actor.sprite.getCurrentFrame().POIs.Count > 0)
            {
                Point offset = actor.sprite.getCurrentFrame().POIs[0];
                if (actor.getXDir() == -1)
                {
                    offset.x *= -1;
                    offset.x += 14;
                }
                bryanaStart.pos = this.actor.getScreenPos() + this.actor.getOffset(true) + offset;
                if (bryanaStart.sprite.frameIndex == 3)
                {
                    bryanaStart.sprite.frameIndex = 1;
                }
            }

            if (actor.sprite.isAnimOver())
            {
                BryanaRing bryanaRing = new BryanaRing(actor.level, actor, actor.pos);
                actor.stateManager.bryanaRing = bryanaRing;
                LinkIdle linkIdle = new LinkIdle();
                stateManager.changeState(linkIdle, false);
            }
        }

    }

}

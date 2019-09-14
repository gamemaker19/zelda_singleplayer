using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkBow : ActorState
    {
        bool isSilver = false;

        public LinkBow(bool isSilver) : base("LinkBow")
        {
            this.isSilver = isSilver;
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
        }

        public override void update()
        {
            base.update();
            if (actor.sprite.frameIndex == 3 && !once)
            {
                once = true;
                Point poiOffset = this.actor.sprite.getCurrentFrame().POIs[0];
                if (actor.getXDir() == -1)
                {
                    poiOffset.x *= -1;
                    poiOffset.x += 10;
                }
                Arrow arrow = new Arrow(actor.level, actor.getScreenPos() + this.actor.getOffset(true) + poiOffset, actor.dir, actor, isSilver);
                arrow.elevation = actor.elevation;
                Character character = actor as Character;
            }
            if (actor.sprite.isAnimOver())
            {
                stateManager.changeState(new LinkIdle(), false);
            }
        }

    }

}

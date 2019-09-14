using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkMagicPowder : ActorState
    {
        public LinkMagicPowder() : base("LinkPowder")
        {

        }

        public override Actor getProj(Point pos, Direction dir)
        {
            return new MagicPowderProj(actor.level, actor, pos);
        }

        public override void update()
        {
            base.update();
            if (projectileCode() != null)
            {

            }
            if (actor.sprite.isAnimOver())
            {
                stateManager.changeState(new LinkIdle(), false);
            }
        }

    }

}

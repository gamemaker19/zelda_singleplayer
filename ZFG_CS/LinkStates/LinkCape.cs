using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkCape : ActorState
    {
        public LinkCape() : base("LinkIdle")
        {
        }

        public override Actor getProj(Point pos, Direction dir)
        {
            return new Anim(actor.level, pos, "CapePoof");
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

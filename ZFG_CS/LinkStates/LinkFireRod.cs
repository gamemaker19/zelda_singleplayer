using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkFireRod : ActorState
    {
        public LinkFireRod() : base("LinkRod")
        {
        }

        public override Actor getProj(Point pos, Direction dir)
        {
            return new FireRodProj(actor.level, pos, dir, actor);
        }

        public override void update()
        {
            base.update();
            if (projectileCode() != null)
            {
                //character.arrows--;
            }
            if (actor.sprite.isAnimOver())
            {
                stateManager.changeState(new LinkIdle(), false);
            }
        }

    }

}

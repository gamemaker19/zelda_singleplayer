using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkIceRod : ActorState
    {
        public LinkIceRod() : base("LinkRod")
        {
        }

        public override Actor getProj(Point pos, Direction dir)
        {
            return new IceRodProj(actor.level, pos, dir, actor);
        }

        public override void update()
        {
            base.update();
            actor.sprite.useBluePalette = true;
            if (projectileCode() != null)
            {
                //character.magic--;
            }
            if (actor.sprite.isAnimOver())
            {
                stateManager.changeState(new LinkIdle(), false);
            }
        }

    }

}

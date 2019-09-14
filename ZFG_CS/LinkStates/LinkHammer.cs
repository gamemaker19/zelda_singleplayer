using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkHammer : ActorState
    {
        public LinkHammer() : base("LinkHammer")
        {
        }

        public override Actor getProj(Point pos, Direction dir)
        {
            if (actor.level.isActorInTileWithTag(actor, "swater,water"))
            {
                actor.playSound("item in water");
                return new Anim(actor.level, pos, "SplashObject");
            }
            else
            {
                actor.playSound("hammer");
                return new Anim(actor.level, pos, "HammerHit");
            }
        }

        public override void update()
        {
            base.update();
            projectileCode();
            if (actor.sprite.isAnimOver())
            {
                stateManager.changeState(new LinkIdle(), false);
            }
        }

    }
}

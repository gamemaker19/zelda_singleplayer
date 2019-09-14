using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkSwimJump : ActorState
    {
        public LinkSwimJump() : base("LinkSwimJump", "LinkHurt")
        {
            isInvincible = true;
        }

        public override void update()
        {
            if (actor.z <= 0)
            {
                actor.playSound("water");
                new Anim(actor.level, actor.pos, "SplashSwim");
                stateManager.changeState(new LinkSwim(), false);
            }

        }
        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
            actor.vel = Helpers.dirToVec(actor.dir);
            actor.zVel = 0.5f;
            actor.z = 0.01f;
            stateManager.lastLandPos = actor.pos - (Helpers.dirToVec(actor.dir) * 4);
        }

    }
}

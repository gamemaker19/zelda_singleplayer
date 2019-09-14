using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkDash : ActorState
    {
        float particleTime = 0;

        public LinkDash() : base("LinkDash", "LinkDash")
        {
            canCut = true;
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
            //actor.vel = Helpers.dirToVec(actor.dir) * 3;
        }

        public override void update()
        {
            base.update();

            particleTime += Global.spf;
            if (particleTime > 0.1)
            {
                particleTime = 0;
                float yPos = 10;
                if (actor.dir == Direction.Down) yPos = -10;
                Anim dust = new Anim(actor.level, actor.pos.addxy(0, yPos), "Dust");
            }

            actor.playLoopingSound("pegasus boots");

            commonLinkGround();
        }

    }

}

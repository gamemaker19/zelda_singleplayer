using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkHurt : ActorState
    {
        Point recoilVel;

        public LinkHurt(Point recoilVel) : base("LinkHurt")
        {
            this.recoilVel = recoilVel * 2;
            this.isInvincible = true;
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
            stateManager.hurtTime = 1;
            if (actor.getChar().skin == "homer")
            {
                actor.playSound("doh");
            }
            else
            {
                actor.playSound("link hurt");
            }
        }

        public override void update()
        {
            base.update();
            actor.move(recoilVel, true, false);
            distTravelled += recoilVel.magnitude;

            if (distTravelled > 30 || stateTime > 0.25)
            {
                if (actor.level.isActorInTileWithTag(actor, "water"))
                {
                    stateManager.changeState(new LinkSwim(), false);
                }
                else
                {
                    stateManager.changeState(new LinkIdle(), false);
                }
            }
        }

    }

}

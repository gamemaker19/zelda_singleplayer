using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkRecoil : ActorState
    {
        Point recoilVel;

        public LinkRecoil(Point recoilVel, string baseSpriteName) : base("LinkRecoil", baseSpriteName)
        {
            this.recoilVel = recoilVel * 2;
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
        }

        public override void update()
        {
            base.update();
            commonLinkGround();
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

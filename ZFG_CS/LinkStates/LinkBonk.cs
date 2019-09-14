using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkBonk : ActorState
    {
        float chargeTime = 0;

        public LinkBonk() : base("LinkBonk", "LinkHurt")
        {
            drawWadable = false;
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
            actor.vel = Helpers.dirToVec(actor.dir) * -1;
            actor.z = 0.1f;
            actor.zVel = 1.5f;
            actor.playSound("ram");
            float shakeX = Math.Abs(actor.vel.x) > 0 ? 0.25f : 0;
            float shakeY = Math.Abs(actor.vel.y) > 0 ? 0.25f : 0;
            if (actor == Global.game.camCharacter) actor.level.shake(shakeX, shakeY, true, 0);
        }

        public override void update()
        {
            base.update();
            if (actor.z <= 0)
            {
                actor.vel = new Point(0, 0);
                stateManager.changeState(new LinkIdle(), false);
            }
        }

    }

}

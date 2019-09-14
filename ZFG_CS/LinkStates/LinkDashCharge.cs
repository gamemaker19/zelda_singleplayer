using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class LinkDashCharge : ActorState
    {
        float particleTime = 0;
        float chargeTime = 0;

        public LinkDashCharge() : base("LinkDashCharge", "LinkDashCharge")
        {
        }

        public override void update()
        {
            base.update();

            playFootsteps(true);

            particleTime += Global.spf;
            if (particleTime > 0.08)
            {
                particleTime = 0;
                once = !once;
                float xOnceOff = once ? 8 : 0;
                float xOff = 0;
                if (actor.dir == Direction.Up) xOff = -4;
                else if (actor.dir == Direction.Down) xOff = -4;
                else if (actor.dir == Direction.Right) xOff = -9;
                else if (actor.dir == Direction.Left) xOff = 0;
                Point origin = actor.pos.addxy(xOnceOff + xOff, 10);
                Anim dust = new Anim(actor.level, origin, "Dust");
            }

            actor.playLoopingSound("pegasus boots");

            if (!stateManager.input.isHeld(Control.main.Action))
            {
                stateManager.changeState(new LinkIdle(), false);
            }
            else if (actor.sprite.isAnimOver())
            {
                stateManager.changeState(new LinkDash(), false);
            }
        }

    }

}

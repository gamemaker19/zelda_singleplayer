using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkLamp : ActorState
    {
        public LinkLamp() : base("LinkLamp", "LinkIdle")
        {

        }

        public override void update()
        {
            base.update();
            if (!once)
            {
                once = true;
                Point poiOffset = Point.Zero;
                if (actor.dir == Direction.Left) poiOffset = new Point(-15, 0);
                else if (actor.dir == Direction.Right) poiOffset = new Point(15, 0);
                else if (actor.dir == Direction.Up) poiOffset = new Point(0, -15);
                else if (actor.dir == Direction.Down) poiOffset = new Point(0, 15);
                LampFlame proj = new LampFlame(actor.level, actor, actor.getScreenPos() + poiOffset);
                proj.elevation = actor.elevation;
            }
            if (stateTime > 0.25f)
            {
                stateManager.changeState(new LinkIdle(), false);
            }
        }

    }
}

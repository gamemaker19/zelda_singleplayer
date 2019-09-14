using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkStun : ActorState
    {
        public Point recoilVel;
        public float distTravelled = 0;

        public LinkStun(Point recoilDir) : base("LinkStun", "LinkIdle")
        {
            this.recoilVel = recoilDir * 2;
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
            actor.vel = recoilVel;
            actor.playSound("enemy hit");
        }

        public override void update()
        {
            base.update();
            distTravelled += actor.vel.magnitude;
            if (distTravelled > 30)
            {
                actor.vel = Point.Zero;
            }
            actor.shake = new Point(Helpers.randomRange(-1, 1), Helpers.randomRange(-1, 1));
            if (stateTime > 1)
            {
                actor.vel = Point.Zero;
                actor.shake = Point.Zero;
                stateManager.changeState(new LinkIdle(), false);
            }
        }

        public override void onExit(ActorState newState)
        {
            base.onExit(newState);
            actor.shake = Point.Zero;
            actor.vel = Point.Zero;
        }

    }
}

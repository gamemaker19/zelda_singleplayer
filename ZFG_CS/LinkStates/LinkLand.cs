using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkLand : ActorState
    {
        float turnTime = 0;

        public LinkLand() : base("LinkLand", "LinkIdle")
        {
            isInvincible = true;
            aiTarget = false;
        }

        public override void onEnter(ActorState oldState)
        {
            actor.z = 130;
            actor.useGravity = false;
            drawWadable = false;
        }

        public override void update()
        {
            base.update();
            turnTime += Global.spf;
            actor.z -= Global.spf * 200;
            if (turnTime > 0.1)
            {
                turnTime = 0;
                if (actor.dir == Direction.Down) actor.changeDir(Direction.Left);
                else if (actor.dir == Direction.Left) actor.changeDir(Direction.Up);
                else if (actor.dir == Direction.Up) actor.changeDir(Direction.Right);
                else if (actor.dir == Direction.Right) actor.changeDir(Direction.Down);
            }
            if (actor.z <= 0)
            {
                actor.z = 0;
                actor.useGravity = true;
                actor.playSound("land");
                stateManager.changeState(new LinkIdle(), false);
            }
        }

    }

}

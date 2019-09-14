using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkLift : ActorState
    {
        public LinkLift(Actor throwable) : base("LinkLift")
        {
            this.throwable = throwable;
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
            this.throwable.throwable.lifter = actor;
        }

        public override void update()
        {
            base.update();
            if (this.throwable == null)
            {
                return;
            }
            bool isHeavy = throwable.throwable.isHeavy;

            if (!once)
            {
                if (!isHeavy || actor.sprite.frameIndex >= 2)
                {
                    actor.playSound("lift");
                    throwable.throwable.lift();
                    once = true;
                }
            }

            if (isHeavy) actor.sprite.frameSpeed = 0.25f;

            Point poiOffset = this.actor.sprite.getCurrentFrame().POIs[0];

            if (!isHeavy || actor.sprite.frameIndex >= 2)
            {
                poiOffset.x *= actor.getXDir();
                this.throwable.changePos(this.actor.getOffsetPos() + poiOffset, false);
            }

            if (actor.sprite.isAnimOver())
            {
                stateManager.changeState(new LinkCarry(throwable), false);
            }
        }

        public override void onExit(ActorState newState)
        {
            base.onExit(newState);
            if (this.throwable != null && newState.name != "LinkCarry")
            {
                this.throwable.throwable.doThrow(actor.dir);
                this.throwable = null;
            }
        }

    }
}

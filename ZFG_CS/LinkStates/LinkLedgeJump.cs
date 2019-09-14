using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkLedgeJump : ActorState
    {
        CollideData ledgeCollideData;
        LedgeTile ledgeTile;
        bool teleportDown = false;
        float maxDist = 0;
        Point destPoint;

        public LinkLedgeJump(CollideData ledgeCollideData, float maxDist, Point destPoint, bool teleportDown, string baseSpriteName) : base("LinkJump", baseSpriteName)
        {
            this.ledgeCollideData = ledgeCollideData;
            this.ledgeTile = ledgeCollideData.collidedTile as LedgeTile;
            this.maxDist = maxDist;
            this.destPoint = destPoint;
            this.teleportDown = teleportDown;
            this.teleportShadow = teleportDown;
            drawWadable = false;
            isSolid = false;
            isInvincible = true;
            aiTarget = false;
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
            this.throwable = oldState.throwable;
            actor.playSound("fall");
        }

        public override void onExit(ActorState newState)
        {
            base.onExit(newState);
            actor.sprite.frameSpeed = 1;
            if (newState.name != "LinkCarry" && this.throwable != null)
            {
                this.throwable.throwable.doThrow(actor.dir);
                this.throwable = null;
            }
        }

        public override void update()
        {
            base.update();
            actor.sprite.frameSpeed = 0;
            actor.sprite.frameIndex = 1;
            float inc = actor.vel.magnitude;
            distTravelled += inc;
            if (!once && (distTravelled > maxDist - inc || maxDist == 0))
            {
                once = true;
                actor.vel.x = 0;
                actor.vel.y = 0;
                actor.changePos(destPoint, false);
                //stateManager.changeState(new LinkIdle(), false);
            }
            if (once && actor.z == 0)
            {
                actor.z = 0;
                if (actor.level.isActorInTileWithTag(actor, "water"))
                {
                    actor.playSound("water");
                    stateManager.changeState(new LinkSwim(), false);
                }
                else
                {
                    actor.playSound("land");
                    if (prevState != null)
                    {
                        stateManager.changeState(prevState, false);
                    }
                    else
                    {
                        stateManager.changeState(new LinkIdle(), false);
                    }
                }

            }
            /*
            if (actor.moveToPos(destPoint, runSpeed * 2, false))
            {
                actor.changePos(destPoint, false);
                actor.z = 0;
                stateManager.changeState(new LinkIdle(), false);
            }
            */
        }

        public override void lateUpdate()
        {
            if (throwable != null)
            {
                Point poiOffset = this.actor.sprite.getCurrentFrame().POIs[0];
                poiOffset.x *= actor.getXDir();
                this.throwable.changePos(this.actor.getScreenPos() + this.actor.getOffset(true) + poiOffset, false);
            }
        }

    }

}

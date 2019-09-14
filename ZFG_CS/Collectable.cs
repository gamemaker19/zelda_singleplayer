using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Collectable
    {
        public Actor actor;
        public int magicGain = 0;
        public int healthGain = 0;
        public int rupeeGain = 0;
        public int bombGain = 0;
        public int arrowGain = 0;
        public float hideTime = 0;
        public bool shouldFade = true;
        public string origSprite = "";
        public string driftSprite = "";
        public InventoryItem inventoryItem;
        public bool drift = false;
        public bool collectOnTouch = true;
        public float delay = 0;
        public bool collected = false;
        public int driftState = 0;
        public float xVel = 0;

        public Collectable(Actor actor, bool bounceUp = false, bool drift = false)
        {
	        this.actor = actor;
	        this.drift = drift;
	        if (bounceUp)
	        {
		        actor.z = 0.1f;
		        actor.zVel = 1;
	        }
	        if (drift)
	        {
		        actor.z = 0.1f;
		        actor.zVel = 0.8f;
		        actor.dir = Direction.Right;
		        actor.useGravity = false;
	        }
            actor.drawShadow = true;
	        actor.smallShadow = true;
	        actor.fetchable = true;
	        actor.checkWadables = true;
	        actor.checkTriggers = false;
	        actor.isStatic = false;
        }

        public void collect(Character character)
        {
            if (actor.time < delay) return;
            if (collected) return;
            character.collect(this);
            actor.level.removeActor(actor);
        }

        public void update()
        {
            if (shouldFade)
            {
                if (actor.time > 10)
                {
                    hideTime += Global.spf;
                    if (hideTime > 0.03)
                    {
                        actor.visible = !actor.visible;
                        hideTime = 0;
                    }
                }
                if (actor.time > 13)
                {
                    actor.level.removeActor(actor);
                }
            }
            if (drift)
            {
                const float driftVel = -0.25f;
                const float xVelStart = 6;
                if (driftState == 0)
                {
                    actor.z += actor.zVel;
                    actor.zVel -= Global.spf * 3;
                    if (actor.zVel <= 0)
                    {
                        actor.zVel = driftVel;
                        xVel = xVelStart;
                        driftState = 1;
                        actor.useGravity = false;
                        if (driftSprite != "")
                        {
                            origSprite = actor.baseSpriteName;
                            actor.changeSprite(driftSprite);
                        }
                    }
                }
                if (driftState == 1)
                {
                    actor.z += actor.zVel;
                    actor.move(new Point(0.1f * actor.getXDir() * xVel, 0), false, false);
                    actor.zVel += 0.01f;
                    xVel -= 0.175f;
                    if (xVel < 0) xVel = 0;
                    if (actor.zVel >= 0)
                    {
                        actor.zVel = driftVel;
                        xVel = xVelStart;
                        actor.dir = (actor.dir == Direction.Left ? Direction.Right : Direction.Left);
                    }
                    if (actor.z <= 0)
                    {
                        actor.z = 0;
                        drift = false;
                        driftState = 2;
                        actor.changeSprite(origSprite);
                    }
                }
            }
            if (!drift && actor.zVel == 0)
            {
                actor.vel = new Point(0, 0);
            }
        }

    }
}

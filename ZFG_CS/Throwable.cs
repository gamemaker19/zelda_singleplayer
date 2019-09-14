using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Throwable
    {
        public string liftSpriteName;
        public string breakSpriteName;
        public bool thrown = false;
        public bool generatePickup;
        public bool bounce = false;
        public bool breakOnSword = false;
        public bool drawShadowOnThrow = true;
        public bool isHeavy = false;
        public float zVel = 0;
        public float speed = 3;
        public float throwTime = 0;
        public bool bounceBack = false;
        public bool revealSound = false;
        public bool lifted = false;
        public Actor actor;
        public Actor lifter;
        public Item itemRequired;
        public float damage = 0;
        public string breakSound = "";

        public Throwable(Actor actor, string liftSpriteName, string breakSpriteName, bool generatePickup, float damage, string breakSound)
        {
	        this.actor = actor;
	        this.liftSpriteName = liftSpriteName;
	        this.breakSpriteName = breakSpriteName;
	        this.generatePickup = generatePickup;
	        this.damage = damage;
	        this.breakSound = breakSound;
	        isHeavy = actor.sprite.name.Contains("Big");
	        actor.checkTriggers = false;
	        actor.isStatic = true;
        }

        public void update()
        {
            if (thrown)
            {
                throwTime += Global.spf;
            }
            if (thrown && actor.z == 0)
            {
                land();
            }
        }

        public void onSwordHit()
        {
            if (lifted) return;
            if (!breakOnSword) return;
            if (liftSpriteName != "")
            {
                Actor baseActor = new Actor(actor.level, actor.pos, liftSpriteName);
                baseActor.shader = actor.shader;
                baseActor.autoIncId = -1;
            }
            if (generatePickup)
            {
                WorldObjectFactories.createRandomPickup(actor.level, actor.pos, false);
            }
            doBreak();
        }

        public void doBreak()
        {
            if (!isHeavy)
            {
                Anim anim = new Anim(actor.level, actor.pos, breakSpriteName);
                anim.shader = actor.shader;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    Point offset = Point.Zero;
                    if (i == 0) offset = new Point(-4, -4);
                    if (i == 1) offset = new Point(-4, 4);
                    if (i == 2) offset = new Point(4, -4);
                    if (i == 3) offset = new Point(4, 4);
                    Anim anim = new Anim(actor.level, actor.pos + offset, breakSpriteName);
                    anim.shader = actor.shader;
                }
            }
            if (breakSound != "")
            {
                actor.playSound(breakSound);
            }
            actor.remove();
        }

        public void land()
        {
            if (actor.name != "CaneBlock")
            {
                actor.getMainCollider(true).damager = null;
            }
            if (actor.level.isActorInTileWithTag(actor, "water"))
            {
                actor.playSound("item in water");
                actor.remove();
                Anim splash = new Anim(actor.level, actor.pos, "SplashObject");
                return;
            }
            if (breakSpriteName != "")
            {
                doBreak();
            }
            else
            {
                actor.playSound("land");
                actor.drawWadeSprite = true;
                actor.vel = new Point(0, 0);
                thrown = false;
                bounceBack = false;
                throwTime = 0;
                actor.projectile = null;
                lifted = false;
            }
        }

        public void lift()
        {
            if (lifted) return;
            lifted = true;
            actor.zIndex = (int)ZIndex.Link + 1;
            actor.isSolid = false;
            actor.drawShadow = false;
            actor.drawWadeSprite = false;
            if (actor.sprite.frames.Count == 2)
            {
                actor.sprite.frameIndex = 1;
            }
            if (liftSpriteName != "")
            {
                Actor baseActor = new Actor(actor.level, actor.pos, liftSpriteName);
                baseActor.shader = actor.shader;
            }
            if (generatePickup)
            {
                Actor pickup = WorldObjectFactories.createRandomPickup(actor.level, actor.pos, false);
            }
            if (revealSound)
            {
                actor.playSound("secret");
            }
        }

        public void doThrow(Direction dir)
        {
            actor.isSolid = true;
            actor.drawShadow = drawShadowOnThrow;
            actor.checkTriggers = true;
            actor.isStatic = false;
            Collider c = actor.getMainCollider(true);
            c.isTrigger = true;
            actor.vel = Helpers.dirToVec(dir) * speed;
            actor.zVel = zVel;
            float yDist = 10;
            actor.z = yDist;
            actor.bounce = bounce;
            actor.changePos(actor.pos + new Point(0, yDist), false);
            thrown = true;
            actor.projectile = new Projectile(actor, lifter, null);
            if (damage > 0) actor.getMainCollider(true).damager = new Damager(lifter, null, damage);
            if (lifter != null)
            {
                lifter = null;
            }
        }

        public void onCollision(CollideData collideData)
        {
            if (!thrown) return;
            var damager = actor.getMainCollider(true).damager;
            if (collideData.collidedTile != null && throwTime > 0.1f)
            {
                if (bounce)
                {
                    if (!bounceBack)
                    {
                        bounceBack = true;
                        actor.vel *= -0.25f;
                    }
                }
                else
                {
                    land();
                }
            }
            else if (collideData.collidedActor != null && throwTime > 0.1f && damager != null && damager.damage > 0 && collideData.collidedActor != lifter && collideData.collidedActor.isLink())
            {
                Character otherChar = (collideData.collidedActor as Character);
                Damager damager2 = collideData.myCollider.damager;
                if (otherChar.canCollisionDamage(collideData.myCollider, collideData.collider))
                {
                    otherChar.applyDamage(damager2, collideData.normal * -1);
                    land();
                }
            }
        }
    }
}

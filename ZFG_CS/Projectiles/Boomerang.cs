using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Boomerang : Actor
    {
        public Point returnDir;
        public bool returnToThrower;
        public bool reversedDir = false;
        public bool isMagical = false;
        public Actor fetchedActor;
        public Character thrower;
        
        public Boomerang(Level level, Point pos, Direction dir, Actor owner, bool isMagical) : base(level, pos, "BoomerangThrow")
        {
            name = "Boomerang";
            this.isMagical = isMagical;
            float speed = 3;
            if (isMagical)
            {
                speed = 4;
            }
            else
            {
                shader = Global.shaders["replaceRedBlue"];
            }
            projectile = new Projectile(this, owner, isMagical ? Item.magicBoomerang : Item.boomerang, dir, speed, "", false, false, 0, "");
            getDamager().stun = true;
            returnDir = (vel * -1).normalized;
        }

        public override void update()
        {
            base.update();
            if (fetchedActor != null)
            {
                fetchedActor.changePos(pos, false);
            }
            if (reversedDir)
            {
                //elevation = 10;
            }
            if (reversedDir && pos.distTo(projectile.owner.pos) < 5)
            {
                onReturn();
                return;
            }
            vel += returnDir * 0.075f;
            if (vel.magnitude < 0.5)
            {
                reversedDir = true;
            }
            if (reversedDir)
            {
                vel = pos.dirTo(thrower.pos) * vel.magnitude;
            }
            playLoopingSound("boomerang");
        }

        public override void onCollision(CollideData collideData)
        {
            base.onCollision(collideData);
            if (collideData.collidedActor != null && collideData.collidedActor.name == "Link" && collideData.collidedActor == projectile.owner && reversedDir)
            {
                onReturn();
            }
            else if (!reversedDir && collideData.collidedActor != null && collideData.collidedActor.fetchable)
            {
                reversedDir = true;
                fetchedActor = collideData.collidedActor;
            }
            else if (!reversedDir && collideData.collidedActor != null && projectile.actorHit(collideData))
            {
                reversedDir = true;
            }
            else if (collideData.collidedActor != null)
            {
                projectile.actorHit(collideData);
            }
            else if (!reversedDir && collideData.collidedTile != null)
            {
                reversedDir = true;
                new Anim(level, pos, "ParticleHookshot");
                playSound("tink");
            }
        }

        void onReturn()
        {
            thrower.boomerang = null;
            if (fetchedActor != null)
            {
                fetchedActor.changePos(projectile.owner.pos, false);
            }
            level.removeActor(this);
        }
    }
}

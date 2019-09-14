using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Arrow : Actor
    {
        public bool stopped = false;
        public bool isSilver = false;
        public bool bounceOff = false;
        public float timeToLive = 15;

        public Arrow(Level level, Point pos, Direction dir, Actor owner, bool isSilver) : base(level, pos, "Arrow")
        {
            projectile = new Projectile(this, owner, isSilver ? Item.silverBow : Item.bow, dir, 3, "", false, false, 1, "");
            projectile.raycastActors = true;
            sprite.frameSpeed = 0;
            this.isSilver = isSilver;
            name = "Arrow";
            if (!this.isSilver)
            {
                shader = Global.shaders["replaceSilverBrown"];
            }
            else
            {
                getDamager().damage *= 3;
            }
        }

        public override void update()
        {
            base.update();
            if (stopped)
            {
                timeToLive -= Global.spf;
                if (timeToLive <= 0)
                {
                    level.removeActor(this);
                }
            }
            else
            {
                if (this.isSilver)
                {
                    Point randPos = new Point(Helpers.randomRange(-2, 2), Helpers.randomRange(-2, 2));
                    new Anim(level, pos + randPos, "SwordSparkle");
                }
            }
        }

        public void stop()
        {
            if (stopped) return;
            stopped = true;
            if (!bounceOff)
            {
                vel = Point.Zero;
                sprite.frameSpeed = 1;
                removeColliders();
                playSound("arrow_hit_wall");
            }
            else
            {
                Anim anim = new Anim(this.level, this.pos, "ArrowBounce");
                anim.vel = vel * -0.25f;
                anim.fade = true;
                remove();
            }
        }

        public override void onCollision(CollideData other)
        {
            base.onCollision(other);
            if (stopped) return;
            if (other.collidedTile != null)
            {
                stop();
            }
        }

        public override void onProjectileHit(Character character)
        {
            stop();
            level.removeActor(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class HookshotHook : Actor
    {
        public bool reverseDir = false;
        public bool returnedToUser = false;
        public Point origin;
        public Point origVel;
        public Actor character;
        public Actor fetchedActor;
        public bool hooked = false;
        public float distTravelled = 0;
        public float maxDistToTravel;
        public float MAX_DIST;
        public int frames = 0;
        public HookshotHook(Level level, Point pos, Direction dir, Actor owner) : base(level, pos, "HookshotHook")
        {
            projectile = new Projectile(this, owner, Item.hookshot, dir, 4, "", true, false, 0, "");
            MAX_DIST = 108;
            maxDistToTravel = MAX_DIST;
            origVel = vel;
            origin = pos;
            getDamager().stun = true;
        }

        public override void update()
        {
            base.update();
            frames++;
            if (fetchedActor != null)
            {
                fetchedActor.changePos(pos, false);
            }
            distTravelled += origVel.magnitude;
            if (!reverseDir)
            {
                if (distTravelled > maxDistToTravel)
                {
                    doReverseDir();
                }
            }
            else
            {
                if (hooked)
                {
                    origin += origVel;
                }
                if (distTravelled > maxDistToTravel)
                {
                    returnedToUser = true;
                }
            }
        }

        public void moveHost(Actor host)
        {
            if (distTravelled + 16 >= maxDistToTravel)
            {
                host.isSolid = true;
                var collisions = host.level.getActorCollisions(host, origVel);
                foreach (var collision in collisions)
                {
                    if (!collision.isTrigger)
                    {
                        return;
                    }
                }
            }
            host.move(origVel, false, false);
        }

        public override void render()
        {
            base.render();

            int maxChains = 10; //Max number of chains at max length

            float xDist = Math.Abs(origin.x - pos.x);
            float yDist = Math.Abs(origin.y - pos.y);
            int numChainsX = (int)Math.Round(maxChains * (xDist / MAX_DIST));
            int numChainsY = (int)Math.Round(maxChains * (yDist / MAX_DIST));
            float xIncDist = numChainsX == 0 ? 0 : Math.Sign(origVel.x) * xDist / numChainsX;
            float yIncDist = numChainsY == 0 ? 0 : Math.Sign(origVel.y) * yDist / numChainsY;
            int numChains = Math.Max(numChainsX, numChainsY);

            for (int i = 0; i < numChains; i++)
            {
                float chainX = xIncDist * i;
                float chainY = yIncDist * i;
                Global.animations["HookshotChain"].draw(origin.x + chainX, origin.y + chainY, 1, 1, 0, 1, null, (int)ZIndex.Link - 1, true);
            }
        }

        public override void onCollision(CollideData other)
        {
            base.onCollision(other);
            if (reverseDir) return;
            if (other.collidedTile != null)
            {
                if (!other.collidedTile.hasTag("hookable"))
                {
                    Anim fade = new Anim(level, pos, "ParticleHookshot");
                    playSound("tink");
                    doReverseDir();
                    return;
                }
                hook();
            }
            else if (other.collidedActor != null && other.collidedActor != projectile.owner)
            {
                if (other.collidedActor.fetchable)
                {
                    fetchedActor = other.collidedActor;
                    doReverseDir();
                    return;
                }
                else if (other.collidedActor.hookable)
                {
                    hook();
                }
                else if (projectile.actorHit(other))
                {
                    doReverseDir();
                }
                else
                {
                    Anim fade = new Anim(level, pos, "ParticleHookshot");
                    doReverseDir();
                }
            }
        }

        public void doReverseDir()
        {
            maxDistToTravel = distTravelled;
            distTravelled = 0;
            reverseDir = true;
            vel *= -1;
        }

        public void hook()
        {
            doReverseDir();
            hooked = true;
            character.isSolid = false;
            Character c = character.getChar();
            c.stateManager.actorState.isInvincible = true;
            vel = new Point(0, 0);
            maxDistToTravel += 12;
        }

        public void onRemove()
        {
            if (fetchedActor != null)
            {
                fetchedActor.changePos(character.pos, false);
            }
        }
    }
}

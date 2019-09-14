using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Projectile
    {
        public Actor actor;
        public Actor owner;
        public Item item;
        public bool reflectable = false;
        public bool raycastTiles = false;
        public bool raycastActors = false;
        public bool firstFrameEnded = false;
        public bool useProjCollision = false;
        public string breakAnim = "";
        public string breakSound = "";
        public HashSet<TileData> ledgeTilesUsed = new HashSet<TileData>();
        //public bool hitLedge(int i, int j);
        public bool hit = false;
        
        public Projectile(Actor actor, Actor owner, Item item)
        {
	        this.actor = actor;
	        this.owner = owner;
	        this.item = item;
	        this.actor.checkTriggers = true;
	        this.actor.isStatic = false;
        }

        public Projectile(Actor actor, Actor owner, Item item, Direction dir, float vel, string breakAnim, bool rotateToDir, bool useProjCollision, float damage, string breakSound) : this(actor, owner, item)
        {
            this.breakAnim = breakAnim;
            this.useProjCollision = useProjCollision;
            actor.changeDir(dir);
            if (rotateToDir)
            {
                actor.dirRotation = true;
            }
            this.actor.vel = Helpers.dirToVec(dir).normalized * vel;
            this.breakSound = breakSound;
            actor.getMainCollider(true).damager = new Damager(owner, item, damage);
        }

        public void update()
        {
            GridCoords coords = new GridCoords(actor.pos);
            Point dirVec = actor.vel.normalized; //Helpers.dirToVec(actor.dir);
            dirVec.x = Math.Sign(dirVec.x);
            dirVec.y = Math.Sign(dirVec.y);

            if (actor.level.gridCoordsInBounds(coords))
            {
                TileData tile1 = null;
                if (!firstFrameEnded)
                {
                    TileData prevTile = actor.level.tileSlots[coords.i - (int)dirVec.y][coords.j - (int)dirVec.x].tileInstances[0];
                    if (prevTile.hasTag("ledge"))
                    {
                        tile1 = prevTile;
                    }
                }
                if (tile1 == null) tile1 = actor.level.tileSlots[coords.i][coords.j].tileInstances[0];

                if (actor.elevation == 0 && tile1.isLedge() && !ledgeTilesUsed.Contains(tile1))
                {
                    ledgeTilesUsed.Add(tile1);
                    coords.i += (int)dirVec.y;
                    coords.j += (int)dirVec.x;
                    int loop = 0;
                    while (actor.level.gridCoordsInBounds(coords))
                    {
                        loop++; if (loop > 1000000) { throw new Exception("INFINITE LOOP IN PROJECTILE UPDATE"); }
                        TileData nextTile = actor.level.tileSlots[coords.i][coords.j].tileInstances[0];
                        if (nextTile.hasTag("ledgewall") && !nextTile.isLedge())
                        {
                            actor.elevation++;
                            //cout << "ELEVATION INC" << endl;
                            ledgeTilesUsed.Add(nextTile);
                        }
                        else
                        {
                            break;
                        }
                        coords.i += (int)dirVec.y;
                        coords.j += (int)dirVec.x;
                    }
                }
                else if (actor.elevation > 0 && !ledgeTilesUsed.Contains(tile1))
                {
                    ledgeTilesUsed.Add(tile1);
                    if (tile1.hasTag("ledgewall"))
                    {
                        actor.elevation--;
                        if (dirVec.y == 1) actor.elevation--;
                        //cout << "ELEVATION DEC" << endl;
                    }
                    else if (tile1.hasTag("ledge"))
                    {
                        actor.elevation = 0;
                        //cout << "ELEVATION ZERO" << endl;
                    }
                }
            }

            if (raycastTiles || raycastActors)
            {
                var collideDatas = actor.level.raycastAll(actor, actor.pos, actor.vel);
                foreach (var collideData in collideDatas)
                {
                    if (raycastTiles && collideData.collidedTile != null)
                    {
                        onHit(collideData);
                    }
                    if (raycastActors)
                    {
                        actorHit(collideData);
                    }
                }
            }

            firstFrameEnded = true;
        }

        public bool actorHit(CollideData collideData)
        {
            if (hit) return false;

            if (collideData.collidedActor != null && collideData.collidedActor != owner && (collideData.collidedActor is Character))
            {
                Character otherChar = (collideData.collidedActor as Character);
                Damager damager = collideData.myCollider.damager;

                if (otherChar.getState() == "LinkFreeze")
                {
                    Arrow arrow = (actor as Arrow);
                    if (arrow != null)
                    {
                        arrow.bounceOff = true;
                    }
                    actor.onProjectileHit(otherChar);
                    if (otherChar.timerFreeAndSet("freeze_cling", 0.5f))
                    {
                        new Anim(actor.level, collideData.intersectionPoints[0], "Cling");
                        actor.playSound("tink");
                    }
                    return true;
                }

                //Check if a shield was hit
                bool shieldHit = false;
                CollideData shieldHitData = null;
                if (raycastActors)
                {
                    Point raycastDir = actor.pos.rayTo(otherChar.pos);
                    if (actor.vel.x == 0) raycastDir.x = 0;
                    if (actor.vel.y == 0) raycastDir.y = 0;
                    shieldHitData = actor.level.raycast(actor, actor.pos, raycastDir);
                    shieldHit = shieldHitData != null && shieldHitData.collider.tags.Contains("shield") && shieldHitData.collidedActor == collideData.collidedActor;
                }
                else
                {
                    var tempCollisions = actor.level.getActorCollisions(actor, new Point());
                    foreach (var tempCollideData in tempCollisions)
                    {
                        if (tempCollideData.collidedActor != null && tempCollideData.collider.tags.Contains("shield") && tempCollideData.collidedActor == collideData.collidedActor)
                        {
                            shieldHit = true;
                            shieldHitData = tempCollideData;
                            break;
                        }
                    }
                }

                //Fighter's shield should not block reflectables
                if (reflectable && shieldHitData != null && shieldHitData.collider.tags == "shield")
                {
                    shieldHit = false;
                }

                if (shieldHit && otherChar.canShieldProj(actor))
                {
                    new Anim(actor.level, shieldHitData.intersectionPoints[0], "Cling");
                    actor.playSound("bombable wall");
                    Arrow arrow = (actor as Arrow);
                    if (arrow != null)
                    {
                        arrow.bounceOff = true;
                    }
                    if (reflectable && shieldHitData.collider.tags.Contains("shield3"))
                    {
                        actor.vel *= -1;
                        owner = collideData.collidedActor;
                        actor.getDamager().owner = collideData.collidedActor;
                        return false;
                    }
                    else
                    {
                        onHit(collideData);
                        actor.onProjectileHit(otherChar);
                        return true;
                    }
                }
                else if (otherChar.canCollisionDamage(collideData.myCollider, collideData.collider))
                {
                    otherChar.applyDamage(damager, collideData.normal * -1);
                    onHit(collideData);
                    actor.onProjectileHit(otherChar);
                    return true;
                }
            }
            else if (collideData.collidedActor != null)
            {
                CaneBlock caneBlock = (collideData.collidedActor as CaneBlock);
                if (caneBlock != null)
                {
                    int damage = (int)actor.getDamager().damage;
                    if (damage == 0) damage = 1;
                    caneBlock.deductHealth(damage);
                    onHit(collideData);
                    actor.onProjectileHit(null);
                    return true;
                }
            }
            return false;
        }

        public void onHit(CollideData collideData)
        {
            if (!hit) hit = true;
            else return;
            if (!useProjCollision) return;

            if (collideData != null && collideData.intersectionPoints.Count > 0)
            {
                Point rayTo = actor.pos.rayTo(collideData.intersectionPoints[0]);
                Point rayToComponent = rayTo.project(Helpers.dirToVec(actor.dir));
                actor.pos += rayToComponent;
            }

            if (breakSound != "")
            {
                actor.playSound(breakSound);
            }
            if (breakAnim != "")
            {
                Anim swordBeamBreak = new Anim(actor.level, actor.pos, breakAnim);
                actor.level.removeActor(actor);
            }

        }

        public void onCollision(CollideData other)
        {
            if (!useProjCollision) return;
            if (hit) return;
            if (!raycastTiles && other.collidedTile != null)
            {
                onHit(other);
            }
            if (!raycastActors && other.collidedActor != null)
            {
                actorHit(other);
            }
        }
    }
}

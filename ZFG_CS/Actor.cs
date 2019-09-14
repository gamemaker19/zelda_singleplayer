using SFML.Audio;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFG_CS
{
    public class Actor
    {
        public string baseSpriteName = "";
        public Animation sprite;
        public Point pos;
        public float alpha = 1;
        Color tint;
        public float z = 0;
        public float w = 0;
        public float zVel = 0;
        public float time = 0;
        public float globalColliderW = 0;
        public float globalColliderH = 0;
        public bool drawShadow = false;
        public bool drawWadeSprite = false;
        public bool smallShadow = false;
        public bool dirRotation = false;
        public bool useGravity = true;
        public float angle = 0;
        public bool visible = true;
        public bool startedInCollision = false;
        public Dictionary<string, float> timers = new Dictionary<string, float>();
        public HashSet<string> childFrameTagsToHide = new HashSet<string>();
        public Dictionary<string, int> projTileToElevInc = new Dictionary<string, int>();
        public Point shadowOffset;
        public Point vel;
        public Point accelToVel;
        public Direction dir = Direction.Down;
        public Level level;
        public int loopCount = 0;
        public string wrapMode = "";
        public Collider hitCollider;
        public Collider globalCollider;
        public Shader shader;
        public string name = "";
        public Point overworldPos;
        public int elevation = 0;
        public bool canCatchInNet = false;
        public int alliance = Alliance.AllianceNeutral;
        public HashSet<Actor> collidedInFrame = new HashSet<Actor>();
        public bool isSolid = true;
        public int zIndex = 0;
        public int varIncId = 0;
        public bool bounce = false;
        public HashSet<HashSet<Actor>> gridActorSets = new HashSet<HashSet<Actor>>();    //A set of actor sets in the grid collision matrix this actor is part of.
        public bool hookable = false;
        public bool fetchable = false; //Can it be fetched with a boomerang or hookshot?
        public Point shake;
        public int autoIncId;
        public Point linkColliderOffset;
        public TempVel tempVel;

        //Optimization variables
        public bool checkTriggers = false;
        public bool checkWadables = false;
        public bool isStatic = true;

        //Components

        public StateManager stateManager;
        public AIStateManager aiStateManager;
        public Throwable throwable;
        public Collectable collectable;
        public Animation wadeSprite;
        public TextGen textGen;
        public Projectile projectile;
        public Animation burnSprite;
        
        public Actor()
        {
        }

        public Actor(Level level)
        {
            this.level = level;
            level.addActor(this);
        }

        public Actor(Level level, Point pos)
        {
            this.level = level;
            this.pos = pos;
            level.addActor(this);
        }

        public Actor(Level level, Point pos, string baseSpriteName)
        {
            this.level = level;
            this.pos = pos;
            changeSprite(baseSpriteName);
            level.addActor(this);
        }

        public List<CollideData> move(Point amount, bool checkCollision, bool slideDiagonal)
        {
            if (amount.magnitude < 0.01) return new List<CollideData>();

            level.removeActorGrid(this);
            if (stateManager != null && !stateManager.actorState.isSolid)
            {
                checkCollision = false;
            }
            if (!isSolid)
            {
                checkCollision = false;
            }
            if (!checkCollision)
            {
                pos += amount;
            }
            else
            {
                float angle = amount.angle();
                float mag = amount.magnitude;

                int loop = 0;
                while (mag > 0)
                {
                    loop++; if (loop > 1000000) { throw new Exception("INFINITE LOOP IN ACTOR MOVE"); }

                    float incAmount = 1;
                    if (mag < 1) incAmount = mag;
                    float deltaX = incAmount * (float)Math.Cos(angle);
                    float deltaY = incAmount * (float)Math.Sin(angle);
                    pos.x += deltaX;
                    pos.y += deltaY;
                    mag -= incAmount;

                    List<CollideData> collisions = level.getActorCollisions(this, new Point(0, 0));
                    for (int i = collisions.Count - 1; i >= 0; i--)
                    {
                        if (collisions[i].isTrigger)
                        {
                            //The cane block's owner collides with their cane block
                            if (collisions[i].collidedActor != null)
                            {
                                CaneBlock caneBlock = collisions[i].collidedActor as CaneBlock;
                                if (caneBlock != null && caneBlock.owner == this) continue;
                            }
                            collisions.RemoveAt(i);
                        }
                    }
                    if (collisions.Count > 0)
                    {
                        pos.x -= deltaX;
                        pos.y -= deltaY;
                        foreach (CollideData collideData in collisions)
                        {
                            if (!collidedInFrame.Contains(collideData.collidedActor))
                            {
                                onCollision(collideData);
                            }

                            if (collideData.collidedActor != null)
                            {
                                collidedInFrame.Add(collideData.collidedActor);
                            }
                            else if (collideData.collidedTile != null && !collideData.collidedTile.hasTag("ledge") && slideDiagonal && !(amount.x != 0 && amount.y != 0))
                            {
                                Point diagonalDir = new Point(0, 0);
                                if (collideData.diagDir == 1)
                                {
                                    if (amount.x < 0) diagonalDir = new Point(-1, 1);
                                    else if (amount.y < 0)
                                    {
                                        diagonalDir = new Point(1, -1);
                                    }
                                }
                                else if (collideData.diagDir == 2)
                                {
                                    if (amount.x > 0) diagonalDir = new Point(1, 1);
                                    else if (amount.y < 0) diagonalDir = new Point(-1, -1);
                                }
                                else if (collideData.diagDir == 3)
                                {
                                    if (amount.x < 0) diagonalDir = new Point(-1, -1);
                                    else if (amount.y > 0) diagonalDir = new Point(1, 1);
                                }
                                else if (collideData.diagDir == 4)
                                {
                                    if (amount.x > 0) diagonalDir = new Point(1, -1);
                                    else if (amount.y > 0) diagonalDir = new Point(-1, 1);
                                }
                                if (diagonalDir.isNonZero())
                                {
                                    diagonalDir.normalize();
                                    return move(diagonalDir * amount.magnitude, true, false);
                                }
                            }
                            if (collisions.Count == 1 && slideDiagonal && !(amount.x != 0 && amount.y != 0) &&
                                ((
                                    (collideData.collidedTile != null && !collideData.collidedTile.hasTag("ledge")) &&
                                    (collideData.collidedTile.hitboxMode == HitboxMode.Tile || collideData.collidedTile.hitboxMode == HitboxMode.BoxBot || collideData.collidedTile.hitboxMode == HitboxMode.BoxLeft || collideData.collidedTile.hitboxMode == HitboxMode.BoxRight || collideData.collidedTile.hitboxMode == HitboxMode.BoxTop ||
                                    collideData.collidedTile.hitboxMode == HitboxMode.BoxBotLeft || collideData.collidedTile.hitboxMode == HitboxMode.BoxTopRight || collideData.collidedTile.hitboxMode == HitboxMode.BoxBotLeft || collideData.collidedTile.hitboxMode == HitboxMode.BoxBotRight)
                                ) ||
                                collideData.collidedActor != null))
                            {
                                Point diagonalDir = new Point(0, 0);
                                Rect actorRect = collideData.myCollider.getShape(this).toRect();
                                Rect tileRect;
                                if (collideData.collidedActor != null)
                                    tileRect = collideData.collider.getShape(collideData.collidedActor).toRect();
                                else
                                    tileRect = collideData.collider.getShape(collideData.collidedTile).toRect();

                                if (Math.Abs(amount.y) > 0)
                                {
                                    if (actorRect.x2 < tileRect.x2)
                                    {
                                        float dist = actorRect.x2 - tileRect.x1;
                                        if (dist < 5)
                                        {
                                            diagonalDir.x = -dist;
                                        }
                                    }
                                    else if (actorRect.x1 > tileRect.x1)
                                    {
                                        float dist = tileRect.x2 - actorRect.x1;
                                        if (dist < 5)
                                        {
                                            diagonalDir.x = dist;
                                        }
                                    }
                                }
                                if (Math.Abs(amount.x) > 0)
                                {
                                    if (actorRect.y2 < tileRect.y2)
                                    {
                                        float dist = actorRect.y2 - tileRect.y1;
                                        if (dist < 5)
                                        {
                                            diagonalDir.y = -dist;
                                        }
                                    }
                                    else if (actorRect.y1 > tileRect.y1)
                                    {
                                        float dist = tileRect.y2 - actorRect.y1;
                                        if (dist < 5)
                                        {
                                            diagonalDir.y = dist;
                                        }
                                    }
                                }
                                if (diagonalDir.isNonZero())
                                {
                                    diagonalDir.normalize();
                                    return move(diagonalDir * amount.magnitude, true, false);
                                }
                            }
                        }
                        level.updateActorGrid(this);
                        return collisions;
                    }
                }

            }
            level.updateActorGrid(this);
            return new List<CollideData>();
        }

        public void changePos(Point destPoint, bool checkCollision)
        {
            move(destPoint - pos, checkCollision, false);
        }

        public bool moveToPos(Point destPoint, float speed, bool checkCollision)
        {
            Point amount = pos.dirTo(destPoint).normalized * speed;
            move(amount, checkCollision, false);
            if (pos.distTo(destPoint) < speed)
            {
                return true;
            }
            return false;
        }

        public bool actorInSight(Actor otherActor)
        {
            return pos.distTo(otherActor.pos) < 200 && otherActor.elevation == elevation;
        }

        public virtual void preUpdate()
        {
            if (stateManager != null) stateManager.preUpdate();
        }

        /*
        public void checkStartedInCollision()
        {
            foreach (Collider collider in getColliders())
            {
                Shape shape = collider.getShape(this);
                List<GridCoords> gridCoords = level.getGridCoords(shape);

                foreach (GridCoords gridCoord : gridCoords)
                {
                    int i = gridCoord.i;
                    int j = gridCoord.j;

                    for (var otherTile : level.tileSlots[i][j].tileInstances)
                    {
                        if (otherTile.hitboxMode != HMNone && !level.tileSlots[i][j].noCollision)
                        {
                            startedInCollision = true;
                            goto breakNest;
                        }
                    }
                }
            }
            breakNest: { }
        }
        */

        public virtual void update()
        {
            time += Global.spf;
            
            if (aiStateManager != null) aiStateManager.update();
            if (stateManager != null) stateManager.update();
            if (throwable != null) throwable.update();
            if (textGen != null) textGen.update();
            if (collectable != null) collectable.update();
            if (projectile != null) projectile.update();
            
            /*
            if (startedInCollision)
            {
                foreach (Collider collider in getColliders())
                {
                    Shape shape = collider.getShape(this);
                    List<GridCoords> gridCoords = level.getGridCoords(shape);

                    for (GridCoords gridCoord : gridCoords)
                    {
                        int i = gridCoord.i;
                        int j = gridCoord.j;

                        for (var otherTile : level.tileSlots[i][j].tileInstances)
                        {
                            if (otherTile.hitboxMode != HMNone && !level.tileSlots[i][j].noCollision)
                            {
                                goto breakNest;
                            }
                        }
                    }
                }
                startedInCollision = false;
                breakNest: { }
            }
            */

            if (sprite.update() && !isStatic)
            {
                //KZTODO optimize this?
                level.updateActorGrid(this);
            }

            if (wadeSprite != null) wadeSprite.update();
            if (z > 0 && useGravity)
            {
                z += zVel;
                zVel -= Global.gravity;
                if (z < 0)
                {
                    if (bounce)
                    {
                        if (level.isActorInTileWithTag(this, "swater"))
                        {
                            z = 0;
                            zVel = 0;
                            playSound("walk water");
                        }
                        else if (level.isActorInTileWithTag(this, "water"))
                        {
                            playSound("item in water");
                            level.removeActor(this);
                            Anim splash = new Anim(level, this.pos, "SplashObject");
                            return;
                        }
                        else
                        {
                            playSound("land");
                            bounce = false;
                            z = 1;
                            zVel *= -0.25f;
                        }
                    }
                    else
                    {
                        z = 0;
                        zVel = 0;
                    }
                }
            }
            if (accelToVel.isNonZero())
            {
                vel += accelToVel.normalized * 0.1f;
                if (vel.magnitude >= accelToVel.magnitude)
                {
                    accelToVel = new Point(0, 0);
                }
            }
            if (vel.isNonZero())
            {
                move(vel, true, false);
            }
            if (tempVel != null)
            {
                move(tempVel.vel, true, false);
                tempVel.time -= Global.spf;
                if(tempVel.time <= 0)
                {
                    tempVel = null;
                }
            }

            if (checkTriggers)
            {
                //Trigger check
                List<CollideData> collisions = level.getActorCollisions(this, Point.Zero);
                for (int i = collisions.Count - 1; i >= 0; i--)
                {
                    if (!collisions[i].isTrigger)
                    {
                        collisions.RemoveAt(i);
                    }
                }
                if (collisions.Count > 0)
                {
                    foreach (CollideData collideData in collisions)
                    {
                        onCollision(collideData);
                    }
                }
            }

            if (checkWadables)
            {
                //Wadable check
                bool isNonWadableState = (stateManager != null && !stateManager.actorState.drawWadable);
                //Check wadables
                if (!isNonWadableState && level.isActorInTileWithTag(this, "swater"))
                {
                    if (wadeSprite == null || wadeSprite.name != "WadeWater")
                    {
                        wadeSprite = Global.animations["WadeWater"].clone();
                    }
                }
                else if (!isNonWadableState && level.isActorInTileWithTag(this, "tallgrass"))
                {
                    if (wadeSprite == null || wadeSprite.name != "WadeGrass")
                    {
                        wadeSprite = Global.animations["WadeGrass"].clone();
                        wadeSprite.frameSpeed = 0;
                    }
                }
                else
                {
                    wadeSprite = null;
                }
            }

            if (stateManager != null) stateManager.lateUpdate();

            if (pos.x < -100 || pos.x > level.pixelWidth() + 100 || pos.y < -100 || pos.y > level.pixelHeight() + 100)
            {
                Console.WriteLine("DELETE ACTOR OUT OF MAP");
                level.removeActor(this);
            }

            if (burnSprite != null) burnSprite.update();

            List<string> keysToErase = new List<string>();
            foreach (var key in timers.Keys.ToList())
            {
                timers[key] -= Global.spf;
                if (timers[key] <= 0)
                {
                    keysToErase.Add(key);
                }
            }
            foreach (var key in keysToErase)
            {
                timers.Remove(key);
            }

            if (level.name == "lttp_overworld")
            {
                overworldPos = pos;
            }
        }

        public Point getCamPos()
        {
            if (drawShadow)
            {
                if ((stateManager != null && stateManager.actorState.teleportShadow))
                {
                    return getScreenPos();
                }
            }
            return pos;
        }

        public Point getColliderPos()
        {
            return pos + linkColliderOffset;
        }

        //Location of character on screen when w and z offsets are accounted for.
        public Point getScreenPos()
        {
            float offsetX = 0;//getCurrentFrame().offset.x + offsetX;
            float offsetY = 0;//getCurrentFrame().offset.y + offsetY;

            float drawX = pos.x + shake.x + offsetX - w;
            float drawY = pos.y + shake.y + offsetY - z;

            return new Point(drawX, drawY);
        }

        //Gets the LTTP-specific sprite offset forced upon us because it has everything set as top-left
        //This will be false most of the time. Only time true when you need the constant factor, such as for collision boxes so that you don't get stuck
        public virtual Point getOffset(bool constantOffset)
        {
            return new Point(0, 0);
        }

        //Helper function that increments the real pos by the offset above.
        public Point getOffsetPos()
        {
            return new Point(pos.x + getOffset(false).x * getXDir(), pos.y + getOffset(false).y);
        }

        public virtual void render()
        {
            if (baseSpriteName == "" || !visible)
            {
                if (Global.showHitboxes)
                {
                    var colliders = getColliders();
                    foreach (Collider collider in colliders)
                    {
                        Shape shape = collider.getShape(this);
                        //DrawWrappers.DrawTexture(shape.points[0].x, shape.points[0].y, shape.points[2].x, shape.points[2].y, false, al_map_rgba(0, 0, 255, 128), 1.0, ZHUD, true);
                    }
                }
                return;
            }

            bool isInvisible = stateManager != null && stateManager.invisible && stateManager.hurtTime == 0;

            Point screenPos = getScreenPos();

            if (drawShadow && (!isInvisible || getChar() == Global.game.camCharacter))
            {
                float halfHeight = sprite.frames[0].rect.h() / 2;
                if (isLink()) halfHeight = 8;
                string shadowSprite = smallShadow ? "ShadowSmall" : "Shadow";
                Global.animations[shadowSprite].draw(pos.x + shadowOffset.x, pos.y + shadowOffset.y + halfHeight, 1, 1, 0, 1, null, zIndex, true);
            }

            if (isInvisible) return;

            if (dirRotation)
            {
                angle = Helpers.dirToAngle(dir);
            }

            int overrideZ = 0;
            if (getState() == "LinkLand")
            {
                overrideZ = (int)ZIndex.Foreground + 100;
            }

            sprite.draw(screenPos.x, screenPos.y, getXDir(true), 1, angle, alpha, shader, zIndex + overrideZ, true, getOffset(false).x, getOffset(false).y, childFrameTagsToHide);

            if (Global.showHitboxes)
            {
                var colliders = getColliders();
                foreach (Collider collider in colliders)
                {
                    Shape shape = collider.getShape(this);
                    DrawWrappers.DrawRect(shape.points[0].x, shape.points[0].y, shape.points[2].x, shape.points[2].y, true, new Color(0, 0, 255, 128), 1, (int)ZIndex.HUD);
                }
            }

            if (wadeSprite != null && drawWadeSprite)
            {
                float halfHeight = sprite.frames[0].rect.h() / 2;
                float wadeOffsetY = 0;
                float wadableZ = zIndex + 0.25f;
                wadableZ = zIndex + 0.5f;
                wadeSprite.draw(pos.x + shadowOffset.x, pos.y + shadowOffset.y + halfHeight - wadeOffsetY, 1, 1, 0, 1, null, wadableZ, true, 0, 0);
            }
            //DrawWrappers.DrawRect(pos.x, pos.y, pos.x + 1, pos.y + 1, true, Color.Red, 1, (int)ZIndices.HUD);

            int collectableCount = 0;
            if (collectable != null && collectable.arrowGain > 0)
            {
                collectableCount = collectable.arrowGain;
            }
            else if (collectable != null && collectable.bombGain > 0)
            {
                collectableCount = collectable.bombGain;
            }
            else if (collectable != null && collectable.actor.baseSpriteName == "PickupRupeeGreen" && collectable.rupeeGain > 1)
            {
                collectableCount = collectable.rupeeGain;
            }
            else if (collectable != null && collectable.inventoryItem != null && collectable.inventoryItem.item.usesQuantity)
            {
                collectableCount = collectable.inventoryItem.count;
            }
            if (collectableCount > 0)
            {
                Point camPos = level.getTopLeftCamPos();
                Point drawPos = getScreenPos();
                Point offset = new Point(4, 2);
                Helpers.drawTextUI(collectableCount.ToString(), drawPos.x - camPos.x + offset.x, drawPos.y - camPos.y + offset.y);
            }

            if (stateManager != null && stateManager.burnTime > 0)
            {
                burnSprite.draw(pos.x, pos.y + 4, 1, 1, 0, 1, null, zIndex + 1, true, 0, 0);
            }
        }

        public virtual void changeDir(Direction newDir)
        {
            if (dir == newDir) return;
            dir = newDir;
            changeSprite(baseSpriteName);
        }

        public int getXDir(bool factorLeftSprite = false)
        {
            if (dir == Direction.Left)
            {
                if (factorLeftSprite && sprite.name.EndsWith("Left")) return 1;
                return -1;
            }
            else if (dir == Direction.Right) return 1;
            return 1;
        }

        public int getYDir()
        {
            if (dir == Direction.Up) return -1;
            else if (dir == Direction.Down) return 1;
            return 1;
        }

        public virtual void onCollision(CollideData collideData)
        {
            if (throwable != null)
            {
                throwable.onCollision(collideData);
            }
            if (projectile != null)
            {
                projectile.onCollision(collideData);
            }
        }

        public List<Collider> getColliders()
        {
            List<Collider> colliders = new List<Collider>();
            if (sprite.frames.Count == 0) return colliders;
            Frame currentFrame = getCurrentFrame();
            if (globalCollider != null)
            {
                colliders.Add(globalCollider);
            }
            if (hitCollider != null)
            {
                colliders.Add(hitCollider);
            }
            foreach (Collider hitbox in sprite.hitboxes)
            {
                colliders.Add(hitbox);
            }
            foreach (Collider hitbox in currentFrame.hitboxes)
            {
                colliders.Add(hitbox);
            }

            foreach (Collider collider in colliders)
            {
                if (collider.tags == "hammer")
                {
                    Damager hammerDamager = new Damager(this, Item.hammer, 2);
                    hammerDamager.isConcussive = true;
                    collider.damager = hammerDamager;
                }
		        if (collider.tags == "spinattack")
		        {
			        Item sword = null;
                    int swordDamage = 0;
                    Character character = getChar();
			        if (character.hasItem(Item.sword2))
			        {
				        sword = Item.sword2;
				        swordDamage = 4;
			        }
			        else if (character.hasItem(Item.sword3))
			        {
				        sword = Item.sword3;
				        swordDamage = 3;
			        }
			        else if (character.hasItem(Item.sword4))
			        {
				        sword = Item.sword4;
				        swordDamage = 2;
			        }
			        else if (character.hasItem(Item.sword1))
			        {
				        sword = Item.sword1;
				        swordDamage = 1;
			        }

			        Damager spinAttackDamager = new Damager(this, sword, swordDamage);
                    collider.damager = spinAttackDamager;
		        }
	        }

	        if (currentFrame.childFrames.Count > 0)
	        {
		        int xDir = getXDir(true);
                Point screenPos = getScreenPos();
                float x = screenPos.x;
                float y = screenPos.y;

                float cx = 0;
                float cy = 0;

		        if (sprite.alignment == "topleft") {
			        cx = 0; cy = 0;
		        }
		        else if (sprite.alignment == "topmid") {
			        cx = 0.5f; cy = 0;
		        }
		        else if (sprite.alignment == "topright") {
			        cx = 1; cy = 0;
		        }
		        else if (sprite.alignment == "midleft") {
			        cx = 0; cy = 0.5f;
		        }
		        else if (sprite.alignment == "center") {
			        cx = 0.5f; cy = 0.5f;
		        }
		        else if (sprite.alignment == "midright") {
			        cx = 1; cy = 0.5f;
		        }
		        else if (sprite.alignment == "botleft") {
			        cx = 0; cy = 1;
		        }
		        else if (sprite.alignment == "botmid") {
			        cx = 0.5f; cy = 1;
		        }
		        else if (sprite.alignment == "botright") {
			        cx = 1; cy = 1;
		        }

		        cx = cx* currentFrame.rect.w();
                cy = cy* currentFrame.rect.h();

                Point offset = getOffset(false);

                float frameOffsetX = (currentFrame.offset.x + offset.x) * xDir * currentFrame.xDir;
                float frameOffsetY = currentFrame.offset.y + offset.y;

		        foreach (Frame childFrame in currentFrame.childFrames)
		        {
			        if (!childFrame.tags.Contains("sword") && !childFrame.tags.Contains("shield")) continue;
			        if (childFrameTagsToHide.Contains(childFrame.tags)) continue;
			        //if (!childFrame.enabled) continue;

			        float childZ = zIndex + ((childFrame.zIndex + 0.5f) * 0.1f);

                    int childDirX = childFrame.xDir * xDir;

                    float flipOffsetX = 0;
                    float flipOffsetY = (childFrame.yDir == -1 ? childFrame.rect.h() : 0);

                    float childOffsetX = (currentFrame.offset.x + childFrame.offset.x + offset.x) * xDir;
                    float childOffsetY = currentFrame.offset.y + childFrame.offset.y + offset.y + flipOffsetY;

			        if (xDir == 1 && childFrame.xDir == -1)
			        {
				        flipOffsetX = childFrame.rect.w();
				        childOffsetX = (currentFrame.offset.x + childFrame.offset.x + offset.x) + flipOffsetX;
			        }
			        else if (xDir == -1 && childFrame.xDir == -1)
			        {
				        flipOffsetX = childFrame.rect.w();
				        childOffsetX = (currentFrame.offset.x + childFrame.offset.x + offset.x) + flipOffsetX;
				        childOffsetX *= -1;
			        }
			        float w = childFrame.rect.w() * childDirX;
                    float h = childFrame.rect.h() * childFrame.yDir;
                    Rect rect = new Rect(new Point(x + childOffsetX, y + childOffsetY), new Point(w, h));
                    bool flipped = xDir == -1;
                    rect.x1 += (!flipped? childFrame.topLeftOffset.x : childFrame.rect.w() - childFrame.topLeftOffset.x);
                    rect.y1 += childFrame.topLeftOffset.y;
			        rect.x2 += (!flipped? childFrame.botRightOffset.x : -(childFrame.rect.w() + childFrame.botRightOffset.x));
			        rect.y2 += childFrame.botRightOffset.y;

			        Collider hitbox = new Collider(rect);
                    hitbox.syncedToActor = true;
			        hitbox.isTrigger = true;
			        if(hitbox.tags == "") hitbox.tags = childFrame.tags;
			        //SWORD COLLIDERS
			        //Swords are an exception to the "hitbox declared at creation" rule, because they don't have hitboxes assigned in the JSON editor, it's an var conversion process
			        if (canStateDamageSword())
			        {
				        Character character = getChar();
				        if (character.hasItem(Item.sword2)) hitbox.damager = new Damager(this, Item.sword2, 4);
				        else if (character.hasItem(Item.sword3)) hitbox.damager = new Damager(this, Item.sword3, 1);
				        else if (character.hasItem(Item.sword4)) hitbox.damager = new Damager(this, Item.sword4, 2);
				        else if (character.hasItem(Item.sword1)) hitbox.damager = new Damager(this, Item.sword1, 0.5f);
			        }
			        colliders.Add(hitbox);
		        }
	        }
	        return colliders;
        }

        public void removeColliders()
        {
            level.removeActorGrid(this);
            globalCollider = null;
            sprite.hitboxes.Clear();
            foreach (var frame in sprite.frames)
            {
                frame.hitboxes.Clear();
            }
        }

        public Rect getBoundingRect()
        {
            List<Collider> colliders = getColliders();
            List<Shape> shapes = new List<Shape>();
            foreach (Collider collider in colliders)
            {
                Shape shape = collider.getShape(this);
                shapes.Add(shape);
            }
            return Helpers.getBoundingBox(shapes);
        }

        public Collider getMainCollider(bool includeTrigger)
        {
            if (globalCollider != null)
            {
                return globalCollider;
            }
            foreach (Collider collider in sprite.hitboxes)
            {
                if (includeTrigger) return collider;
                else if (!collider.isTrigger) return collider;
            }
            Frame currentFrame = sprite.getCurrentFrame();
            foreach (Collider collider in currentFrame.hitboxes)
            {
                if (includeTrigger) return collider;
                else if (!collider.isTrigger) return collider;
            }
            return null;
        }

        public Frame getCurrentFrame()
        {
	        return sprite.getCurrentFrame();
        }

        public void changeSprite(string newSpriteName)
        {
            string newSpriteNameWithDir = newSpriteName;
            if (dir == Direction.Left)
            {
                newSpriteNameWithDir += "Left";
                if (!Global.animations.ContainsKey(newSpriteNameWithDir))
                {
                    newSpriteNameWithDir = newSpriteName + "Right";
                }
            }
            else if (dir == Direction.Right) newSpriteNameWithDir += "Right";
            else if (dir == Direction.Up) newSpriteNameWithDir += "Up";
            else if (dir == Direction.Down) newSpriteNameWithDir += "Down";
            string newSpriteNameFinal;
            if (!Global.animations.ContainsKey(newSpriteNameWithDir))
            {
                newSpriteNameFinal = newSpriteName;
            }
            else
            {
                newSpriteNameFinal = newSpriteNameWithDir;
            }

            if (stateManager != null && stateManager.bunnyTime > 0)
            {
                string backup = newSpriteNameFinal;
                newSpriteNameFinal = newSpriteNameFinal.Replace("Link", "LinkBunny");
                //cout << "Attempt switch to " << newSpriteNameFinal << endl;
                if (!Global.animations.ContainsKey(newSpriteNameFinal))
                {
                    //cout << "Could not find it" << endl;
                    string dirStr = "";
                    if (dir == Direction.Left || dir == Direction.Right) dirStr = "Right";
                    else if (dir == Direction.Up) dirStr = "Up";
                    else if (dir == Direction.Down) dirStr = "Down";
                    newSpriteNameFinal = "LinkBunnyIdle" + dirStr;
                }
                else
                {
                    //cout << "Found it" << endl;
                }
            }

            if (sprite != null && newSpriteNameFinal == sprite.name) return;
            baseSpriteName = newSpriteName;

            bool firstInit = false;
            if (sprite != null)
            {
                level.removeActorGrid(this);
            }
            else
            {
                //First time setting sprite
                firstInit = true;
            }
            if (!Global.animations.ContainsKey(newSpriteNameFinal))
            {
                throw new Exception("Could not find sprite \"" + newSpriteNameFinal + "\"!");
            }
            sprite = Global.animations[newSpriteNameFinal].clone();

            //If character, change base sprite to skin
            Character character = this as Character;
            if (character != null && character.skin != "Link" && character.skin.ToLower() != "link2")
            {
                sprite.bitmap = Global.textures[character.skin];
            }

            if (firstInit)
            {
                globalColliderW = sprite.frames[0].rect.w();
                globalColliderH = sprite.frames[0].rect.h();
            }
            else
            {
                level.updateActorGrid(this);
            }
        }
        
        public bool isLink()
        {
            return this is Character;
        }

        public string getState()
        {
            if (stateManager == null) return "";
            return stateManager.actorState.name;
        }

        public bool canStateCut()
        {
            if (stateManager == null) return false;
            return stateManager.actorState.canCut;
        }

        public bool canStateDamageSword()
        {
            return canStateCut() || getState() == "LinkSpinAttackCharge" || getState() == "LinkPoke";
        }

        public bool checkProjectileCollision(CollideData collideData)
        {
            return collideData.collidedActor != null && collideData.collidedActor.alliance != alliance && collideData.collidedActor != projectile.owner && collideData.collidedActor.isLink();
        }

        public virtual void onProjectileHit(Character character)
        {
        }

        public bool isDeleted()
        {
            return level.deletedActors.Contains(this);
        }

        public void remove()
        {
            level.removeActor(this);
        }

        public Character getChar()
        {
            return this as Character;
        }

        public Damager getDamager()
        {
	        return getMainCollider(true).damager;
        }

        public void setDamager(Damager damager)
        {
            getMainCollider(true).damager = damager;
        }

        public void playSound(string key)
        {
            level.playSound(key, pos);
        }

        public void playLoopingSound(string key, float lengthAdjust = 0)
        {
            string timerKey = "sound_" + key;
            if (!timers.ContainsKey(timerKey))
            {
                SoundBuffer sound = Global.soundBuffers[key];
                timers[timerKey] = sound.Duration.AsSeconds() + lengthAdjust;
                level.playSound(key, pos);
            }
        }

        public bool timerFreeAndSet(string key, float time)
        {
            if (!timers.ContainsKey(key))
            {
                timers[key] = time;
                return true;
            }
            return false;
        }

        public Direction getFaceDir(Point facePos)
        {
            Direction dir = Direction.Down;

            float xMag = Math.Abs(pos.x - facePos.x);
            float yMag = Math.Abs(pos.y - facePos.y);

            if (xMag >= yMag)
            {
                if (pos.x > facePos.x) dir = Direction.Left;
                else if (pos.x <= facePos.x) dir = Direction.Right;
            }
            else
            {
                if (pos.y > facePos.y) dir = Direction.Up;
                if (pos.y <= facePos.y) dir = Direction.Down;
            }
            return dir;
        }

        public void facePos(Point facePos)
        {
            Direction dir = getFaceDir(facePos);
            changeDir(dir);
        }

        public bool canSee(Actor otherActor)
        {
            var collisions = level.raycastAll(this, getColliderPos(), Helpers.dirToVec(dir) * 64);
            foreach (var collision in collisions)
            {
                if (collision.collidedTile != null)
                {
                    return false;
                }
                if (collision.collidedActor == otherActor)
                {
                    return true;
                }
            }
            return false;
        }

        public Point getLookDir()
        {
            return Helpers.dirToVec(dir);
        }

        public bool inStorm()
        {
            return overworldPos.distTo(Global.game.currentStormCenter) > Global.game.currentStormRadius;
        }
        
    }
}

using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZFG_CS
{
    public class Character : Actor
    {
        public Stat health;
        public Stat magic;
        public Stat rupees;
        public Stat arrows;
        public Animation itemGet;
        public int kills = 0;
        public string playerName = "";
        public float itemGetTime = 0;
        public bool isMainCharacter = false;
        public Boomerang boomerang = null;
        public CaneBlock caneBlock = null;
        public string skin = "Link";
        public List<InventoryItem> items;
        public HashSet<Actor> actorsTouchingInFrame = new HashSet<Actor>();
        public bool won = false;
        public bool hasEverything = false;
        public bool moveCollisionLastFrame = false;
        public float distMovedInFrame = 0;
        public Entrance entrance = null;
        public Shader paletteShader;

        public Character(Level level, Point pos, Direction dir, bool isMainCharacter, string skin, string playerName, Input input) : base()
        {
            this.skin = skin;
            this.playerName = playerName;
            this.name = "Link";
            this.level = level;
            this.pos = pos;
            this.dir = dir;
            alliance = ++Global.game.autoIncId;

            paletteShader = Helpers.cloneShader("palette");
            shader = paletteShader;

            //if you change this you must also change linkColliderOffset
            globalCollider = new Collider(new Rect(4, 14, 12, 22));
            linkColliderOffset = new Point(0, 6);
            if (isMainCharacter)
            {
                globalCollider = new Collider(new Rect(3, 12, 13, 22));
                linkColliderOffset = new Point(0, 6);
            }

            hitCollider = new Collider(new Rect(0, 6, 16, 22));
            hitCollider.isTrigger = true;
            hitCollider.isDamageHitbox = true;

            stateManager = new StateManager(this, input);
            stateManager.runSpeed = 1.25f;
            stateManager.changeState(new LinkIdle(), false);

            this.drawShadow = true;
            drawWadeSprite = true;
            this.zIndex = (int)ZIndex.Link;

            health = new Stat(StatType.Health, 3, 3, 6, this);
            health.maxValueCap = 10;

            magic = new Stat(StatType.Magic, 128, 128, 64, this);
            rupees = new Stat(StatType.Rupee, 0, 999, 40, this);
            arrows = new Stat(StatType.Arrow, 0, 99, 1, this);

            childFrameTagsToHide.Add("sword");
            childFrameTagsToHide.Add("sword2");
            childFrameTagsToHide.Add("sword3");
            childFrameTagsToHide.Add("sword4");

            childFrameTagsToHide.Add("shield");
            childFrameTagsToHide.Add("shield2");
            childFrameTagsToHide.Add("shield3");

            items = new List<InventoryItem>(new InventoryItem[5]);

            burnSprite = Global.animations["FlameBurn"].clone();
            checkWadables = true;
            checkTriggers = true;
            isStatic = false;

            /*
            InventoryItem bombs = new InventoryItem(Item.bombs);
            bombs.count = 5;
            addItem(bombs, 0);
            addItem(new InventoryItem(Item.bow), 1);
            addItem(new InventoryItem(Item.sword1), 2);
            addItem(new InventoryItem(Item.emptyBottle), 3);
            addItem(new InventoryItem(Item.emptyBottle), 4);
            */

            if (isMainCharacter) updateCamera();

            level.addActor(this);
        }

        public void updateCamera()
        {
            float newViewX = Helpers.clamp(pos.x, Global.screenW / 2, level.pixelWidth() - Global.screenW / 2);
            float newViewY = Helpers.clamp(pos.y, Global.screenH / 2, level.pixelHeight() - Global.screenH / 2);

            float halfW = Global.screenW / 2;
            float halfH = Global.screenH / 2;

            //Global.debugString1 = numToStr(level.getCamPos().x);

            //Did we cross a scroll line?
            foreach (Line line in level.scrollLines)
            {
                if (line.isVertical() && newViewY + halfH >= line.topY() && newViewY - halfH <= line.botY())
                {
                    float lineX = line.leftX();
                    if (lineX > newViewX - halfW && lineX < newViewX + halfW)
                    {
                        float dist = lineX - newViewX;
                        newViewX = lineX - (Math.Sign(dist) * halfW);
                    }
                }
                if (line.isHorizontal() && newViewX + halfW >= line.leftX() && newViewX - halfW <= line.rightX())
                {
                    float lineY = line.topY();
                    if (lineY > newViewY - halfH && lineY < newViewY + halfH)
                    {
                        float dist = lineY - newViewY;
                        newViewY = lineY - (Math.Sign(dist) * halfH);
                    }
                }
            }

            level.setCamPos(newViewX, newViewY);
        }

        public void setActorsTouched()
        {
            actorsTouchingInFrame.Clear();
            foreach (Collider collider in getColliders())
            {
                Shape shape = collider.getShape(this);
                List<GridCoords> gridCoords = level.getGridCoords(shape);

                HashSet<Actor> usedActors = new HashSet<Actor>();
                foreach (GridCoords gridCoord in gridCoords)
                {
                    int i = gridCoord.i;
                    int j = gridCoord.j;

                    HashSet<Actor> actors = level.tileSlots[i][j].actors;
                    foreach (Actor otherActor in actors)
                    {
                        if (!level.canCollide(this, otherActor)) continue;
                        if (usedActors.Contains(otherActor)) continue;
                        usedActors.Add(otherActor);

                        List<Collider> colliders = otherActor.getColliders();
                        foreach (Collider actorCollider in colliders)
                        {
                            Shape otherShape = actorCollider.getShape(otherActor);
                            CollideData collideData = Helpers.shapesIntersect(otherShape, shape);
                            if (collideData != null && /*otherActor.isLink() &&*/ !collider.isTrigger && !actorCollider.isTrigger)
                            {
                                actorsTouchingInFrame.Add(otherActor);
                            }
                        }
                    }
                }
            }
        }

        public override void update()
        {
            moveCollisionLastFrame = false;

            //Get any actors this character is colliding with
            setActorsTouched();

            Point prevPos = getCamPos();
            base.update();

            if(aiStateManager == null && isSolid && stateManager.actorState.isSolid)
            {
                var collisions = level.getActorCollisions(this, Point.Zero);
                if (collisions.Any(col => !col.isTrigger && col.collidedTile != null))
                {
                    level.freeActor(this);
                }
            }
            
            health.update();
            magic.update();
            rupees.update();

            if (inStorm())
            {
                if (Global.music.name != "bunny" && this == Global.game.camCharacter) Global.game.getCurrentLevel().startMusic("bunny");
                stateManager.applyBunny(0.2f);
                applyDamage(Global.game.getStormDamage(), new Point(0, 0), Global.game.stormKiller, null, false);
            }
            else
            {
                if (Global.music.name == "bunny" && this == Global.game.camCharacter) Global.game.getCurrentLevel().startMusic();
            }

            var hookshotState = (stateManager.actorState as LinkHookshot);
            if (hookshotState != null && hookshotState.hook != null && hookshotState.hook.hooked)
            {
                hookshotState.hook.moveHost(this);
            }

            if (stateManager.actorState.name != "LinkScroll" && stateManager.actorState.name != "LinkLand" && Global.game.camCharacter == this)
            {
                //Check if Link crossed a scroll line, if so scroll
                //Did we cross a scroll line?
                float linkLeftX = pos.x;
                float linkRightX = pos.x;
                float linkTopY = pos.y;
                float linkBotY = pos.y;
                float indoorOffset = 0;
                float incDist = 16;

                if (level.isIndoor)
                {
                    GridCoords gridCoords = new GridCoords(pos);
                    if (level.isCover(gridCoords.i, gridCoords.j))
                    {
                        if (level.isCover(gridCoords.i - 3, gridCoords.j))
                        {
                            int scrollLineY = (int)Math.Floor(pos.y / 256) * 32;
                            int startY = 0;
                            int endY = 0;
                            while (level.isCover(gridCoords.i + startY, gridCoords.j)) startY++;
                            while (level.isCover(gridCoords.i + endY, gridCoords.j)) endY--;
                            startY = gridCoords.i + startY;
                            endY = gridCoords.i + endY;
                            indoorOffset = (startY - scrollLineY) * 8;
                            incDist = (scrollLineY - endY) * 8;
                        }
                        else if (level.isCover(gridCoords.i + 3, gridCoords.j))
                        {
                            int scrollLineY = (int)Math.Ceiling(pos.y / 256) * 32;
                            int startY = 0;
                            int endY = 0;
                            while (level.isCover(gridCoords.i + startY, gridCoords.j)) startY--;
                            while (level.isCover(gridCoords.i + endY, gridCoords.j)) endY++;
                            startY = gridCoords.i + startY;
                            endY = gridCoords.i + endY;
                            indoorOffset = -16 + (scrollLineY - startY) * 8;
                            incDist = 16 + (endY - scrollLineY) * 8;
                        }
                        else if (level.isCover(gridCoords.i, gridCoords.j - 3))
                        {
                            int scrollLineX = (int)Math.Floor(pos.x / 256) * 32;
                            int startX = 0;
                            int endX = 0;
                            while (level.isCover(gridCoords.i, gridCoords.j + startX)) startX++;
                            while (level.isCover(gridCoords.i, gridCoords.j + endX)) endX--;
                            startX = gridCoords.j + startX;
                            endX = gridCoords.j + endX;
                            indoorOffset = -16 + (startX - scrollLineX) * 8;
                            incDist = (scrollLineX - endX) * 8;
                        }
                        else if (level.isCover(gridCoords.i, gridCoords.j + 3))
                        {
                            int scrollLineX = (int)Math.Ceiling(pos.x / 256) * 32;
                            int startX = 0;
                            int endX = 0;
                            while (level.isCover(gridCoords.i, gridCoords.j + startX)) startX--;
                            while (level.isCover(gridCoords.i, gridCoords.j + endX)) endX++;
                            startX = gridCoords.j + startX;
                            endX = gridCoords.j + endX;
                            indoorOffset = -16 + (scrollLineX - startX) * 8;
                            incDist = 8 + (endX - scrollLineX) * 8;
                        }
                    }
                }

                foreach (Line line in level.scrollLines)
                {
                    if (line.isVertical() && pos.y >= line.topY() && pos.y <= line.botY())
                    {
                        //Left to right
                        if (prevPos.x <= line.point1.x - indoorOffset && pos.x >= line.point1.x - indoorOffset)
                        {
                            stateManager.changeState(new LinkScroll(new Point(line.point1.x + incDist, pos.y), baseSpriteName), true);
                            return;
                        }
                        //Right to left
                        else if (prevPos.x >= line.point1.x + indoorOffset && pos.x <= line.point1.x + indoorOffset)
                        {
                            stateManager.changeState(new LinkScroll(new Point(line.point1.x - incDist, pos.y), baseSpriteName), true);
                            return;
                        }
                    }
                    if (line.isHorizontal() && pos.x >= line.leftX() && pos.x <= line.rightX())
                    {
                        //Top to bot
                        if (indoorOffset == 56 && Global.testCounter < 1 && line.point1.y == 96 * 8)
                        {
                            Global.testCounter++;
                        }
                        if (prevPos.y <= line.point1.y - indoorOffset && pos.y >= line.point1.y - indoorOffset)
                        {
                            stateManager.changeState(new LinkScroll(new Point(pos.x, line.point1.y + incDist), baseSpriteName), true);
                            return;
                        }
                        //Bot to top
                        else if (prevPos.y >= line.point1.y + indoorOffset && pos.y <= line.point1.y + indoorOffset)
                        {
                            stateManager.changeState(new LinkScroll(new Point(pos.x, line.point1.y - incDist), baseSpriteName), true);
                            return;
                        }
                    }
                }

                if (this == Global.game.camCharacter)
                {
                    Point delta = getCamPos() - prevPos;
                    float viewX = level.getCamPos().x;
                    float viewY = level.getCamPos().y;
                    float newViewX = Helpers.clamp(viewX + delta.x, Global.screenW / 2, level.pixelWidth() - Global.screenW / 2);
                    float newViewY = Helpers.clamp(viewY + delta.y, Global.screenH / 2, level.pixelHeight() - Global.screenH / 2);

                    float halfW = Global.screenW / 2;
                    float halfH = Global.screenH / 2;

                    //Global.debugString1 = numToStr(level.getCamPos().x);

                    //Did we cross a scroll line?
                    foreach (Line line in level.scrollLines)
                    {
                        if (line.isVertical() && viewY + halfH >= line.topY() && viewY - halfH <= line.botY())
                        {
                            if (viewX <= line.point1.x - halfW && newViewX >= line.point1.x - halfW)
                            {
                                newViewX = line.point1.x - halfW;
                            }
                            else if (viewX >= line.point1.x + halfW && newViewX <= line.point1.x + halfW)
                            {
                                newViewX = line.point1.x + halfW;
                            }
                        }
                        if (line.isHorizontal() && viewX + halfW >= line.leftX() && viewX - halfW <= line.rightX())
                        {
                            if (viewY <= line.point1.y - halfH && newViewY >= line.point1.y - halfH)
                            {
                                newViewY = line.point1.y - halfH;
                            }
                            else if (viewY >= line.point1.y + halfH && newViewY <= line.point1.y + halfH)
                            {
                                newViewY = line.point1.y + halfH;
                            }
                        }
                    }

                    bool dontMoveX = false;
                    bool dontMoveY = false;

                    if (pos.x > viewX && delta.x < 0)
                    {
                        dontMoveX = true;
                    }
                    if (pos.x < viewX && delta.x > 0)
                    {
                        dontMoveX = true;
                    }
                    if (pos.y > viewY && delta.y < 0)
                    {
                        dontMoveY = true;
                    }
                    if (pos.y < viewY && delta.y > 0)
                    {
                        dontMoveY = true;
                    }

                    if (dontMoveX)
                    {
                        newViewX = viewX;
                    }
                    if (dontMoveY)
                    {
                        newViewY = viewY;
                    }

                    if (Global.game.masterSwordPulled && Global.game.getCurrentLevel().inLostWoodsNotSwordArea())
                    {
                        Global.game.woodsBackdropOffset.x += (newViewX - viewX) * 0.75f;
                        Global.game.woodsBackdropOffset.y += (newViewY - viewY) * 0.75f;
                    }

                    level.setCamPos(newViewX, newViewY);
                }
            }

            if (entrance != null)
            {
                entrance.enter(this);
                entrance = null;
            }
        }

        public override void preUpdate()
        {
            base.preUpdate();
        }

        public override void render()
        {
            base.render();
            if (itemGetTime > 0)
            {
                itemGetTime += Global.spf;
                float offsetY = itemGetTime > 0.5f ? 0.5f : itemGetTime;
                itemGet.draw(getScreenPos().x, getScreenPos().y - 5 - offsetY * 24, 1, 1, 0, 1, null, zIndex + 1, true);
                if (itemGetTime > 1)
                {
                    itemGetTime = 0;
                }
            }
            Point colPos = getColliderPos();
            //wal_draw_circle(colPos, 2, al_color_name("red"), ZHUD, true);
        }

        public bool canCollisionDamage(Collider damagingCollider, Collider charColliderHit)
        {
            Collider collider = damagingCollider;
            return collider.damager != null && (collider.damager.canHurtOwner || collider.damager.alliance() != alliance) && (!charColliderHit.isTrigger || charColliderHit.isDamageHitbox) && !isInvincible();
        }

        public bool isInvincible()
        {
            return stateManager.actorState.isInvincible || stateManager.hurtTime > 0 || stateManager.bryanaRing != null || won;
        }

        public void applyDamage(Damager damager, Point recoilDir)
        {
            if (isInvincible()) return;

            if (hasItem(Item.redMail))
            {
                damager.damage -= 1;
                if (damager.damage <= 0) damager.damage = 0.25f;
            }
            else if (hasItem(Item.blueMail))
            {
                damager.damage -= 0.5f;
                if (damager.damage <= 0) damager.damage = 0.25f;
            }

            if (getState() == "LinkFreeze")
            {
                if (damager.burn)
                {
                    stateManager.actorState.stateTime = 10;
                    return;
                }
                if (damager.isConcussive)
                {
                    stateManager.actorState.stateTime = 10;
                    (stateManager.actorState as LinkFreeze).shatter = true;
                    damager.damage *= 2;
                }
                else
                {
                    return;
                }
            }

            if (damager.damage > 0)
            {
                health.deduct(damager.damage);
            }
            if (health.value <= 0)
            {
                Character killer = damager.owner as Character;
                if (killer != null)
                {
                    killer.kills++;
                }
                Global.game.killFeed.Add(new KillFeedEntry(damager.owner, this, damager.item));
                stateManager.changeState(new LinkDie(killer), true);
            }
            else
            {
                if (damager.damage > 0)
                {
                    bool hurtState = false;
                    if (damager.flinch && !stateManager.actorState.superArmor)
                    {
                        hurtState = true;
                        stateManager.changeState(new LinkHurt(recoilDir), true);
                    }
                    else if (damager.invulnFrames)
                    {
                        stateManager.hurtTime = 1;
                    }
                    if (damager.actor != null && damager.actor.name == "CaneBlock")
                    {
                        (damager.actor as CaneBlock).deductHealth(1);
                    }
                    if (!hurtState)
                    {
                        if (getChar().skin == "homer")
                        {
                            playSound("doh");
                        }
                        else
                        {
                            playSound("link hurt");
                        }
                    }
                }
                if (damager.burn)
                {
                    stateManager.applyBurn(damager.owner, damager.item);
                }
                if (damager.freeze)
                {
                    stateManager.changeState(new LinkFreeze(), true);
                }
                if (damager.stun)
                {
                    stateManager.changeState(new LinkStun(recoilDir), true);
                }
                if (damager.bunnify)
                {
                    stateManager.applyBunny(15);
                }
            }
        }

        public void applyDamage(float damage, Point recoilDir, Actor owner, Item item, bool flinch, bool invulnFrames = true)
        {
            Damager tempDamager = new Damager(owner, item, damage);
            tempDamager.flinch = flinch;
            tempDamager.invulnFrames = invulnFrames;
            applyDamage(tempDamager, recoilDir);
        }

        public override void onCollision(CollideData other)
        {
            base.onCollision(other);
            Collider collider = other.collider;
            ActorState currentState = stateManager.actorState;
            if (canCollisionDamage(other.collider, other.myCollider) && other.collidedActor.projectile == null)
            {
                Damager damager = other.collider.damager;
                applyDamage(collider.damager, other.normal);
            }
            if (!other.myCollider.isTrigger && other.isTrigger && other.collidedActor != null && other.collidedActor.collectable != null && other.collidedActor.collectable.collectOnTouch)
            {
                Collectable collectable = other.collidedActor.collectable;
                collectable.collect(this);
            }
            if (other.myCollider.isTrigger && other.collidedActor != null && other.myCollider.tags.Contains("sword") && (canStateCut() || getState() == "LinkPoke"))
            {
                if (other.collidedActor.throwable != null)
                {
                    Throwable throwable = other.collidedActor.throwable;
                    throwable.onSwordHit();
                }
            }

            //Sword clang
            if (other.myCollider.isTrigger && other.myCollider.tags.Contains("sword") && canStateDamageSword() && tempVel == null &&
                other.collider.isTrigger && other.collider.tags.Contains("sword") && other.collidedActor.canStateDamageSword())
            {
                Point recoilVel = other.normal;
                new Anim(level, other.intersectionPoints[0], "Cling");
                playSound("tink");
                tempVel = new TempVel(recoilVel * 2, 0.1f);
                other.collidedActor.tempVel = new TempVel(recoilVel * -2, 0.1f);
                //stateManager.changeState(new LinkRecoil(recoilVel, stateManager.actorState.baseSpriteName), true);
                //other.collidedActor.stateManager.changeState(new LinkRecoil(recoilVel * -1, other.collidedActor.stateManager.actorState.baseSpriteName), true);
            }

            if (other.myCollider.isTrigger && other.collidedActor != null && other.myCollider.tags.Contains("net") && other.collidedActor.canCatchInNet)
            {
                int itemSlot = getItemSlot(Item.emptyBottle);
                if (itemSlot != -1)
                {
                    playSound("heart");
                    items[itemSlot] = new InventoryItem(Item.bottledFairy);
                    other.collidedActor.remove();
                }
            }

            Door door = (other.collidedActor as Door);
            if (door != null && dir == Direction.Up && !other.myCollider.isTrigger && getState() != "LinkLand")
            {
                door.open();
            }

            Entrance otherEntrance = (other.collidedActor as Entrance);
            if (otherEntrance != null && getState() != "LinkJump" && getState() != "LinkLand" && !other.myCollider.isTrigger)
            {
                entrance = otherEntrance;
            }

            BigFairy bigFairy = (other.collidedActor as BigFairy);
            if (bigFairy != null && !other.myCollider.isTrigger)
            {
                bigFairy.heal(this);
            }

            Stake stake = (other.collidedActor as Stake);
            if (stake != null && !stake.pounded && getState() == "LinkHammer" && other.myCollider.tags == "hammer")
            {
                stake.pound();
            }
        }

        public int getItemSlot(Item item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null && items[i].item == item) return i;
            }
            return -1;
        }

        public override void changeDir(Direction newDir)
        {
            base.changeDir(newDir);
        }

        public override Point getOffset(bool constantOffset)
        {
            Point offset = new Point(-8, -12);
            if (!constantOffset)
            {
                if (dir == Direction.Left || dir == Direction.Right)
                {
                    offset.y = -11;
                }
            }
            return offset;
        }

        public void collect(Collectable collectable)
        {
            if (collectable.healthGain == -1) health.fillMax();
            else if (collectable.healthGain > 0)
            {
                if (collectable.actor.name == "Fairy") playSound("fairy");
                else playSound("heart");
                health.add(collectable.healthGain);
            }

            if (collectable.magicGain == -1) magic.fillMax();
            else if (collectable.magicGain > 0) magic.add(collectable.magicGain);

            if (collectable.rupeeGain == -1) rupees.fillMax();
            else if (collectable.rupeeGain > 0) rupees.add(collectable.rupeeGain);

            if (collectable.arrowGain > 0)
            {
                playSound("heart");
                arrows.addImmediate(collectable.arrowGain);
            }

            if (collectable.bombGain > 0)
            {
            }

            if (collectable.inventoryItem != null)
            {
                InventoryItem inventoryItem = collectable.inventoryItem;
                collectable.collected = collectItem(inventoryItem);
            }
        }

        public int getEmptySlot()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool hasEmptySlot()
        {
            return getEmptySlot() != -1;
        }

        public bool canShieldProj(Actor proj)
        {
            if (getState() != "LinkSpinAttackCharge")
            {
                if (proj.vel.y < 0 && dir == Direction.Down) return true;
                if (proj.vel.y > 0 && dir == Direction.Up) return true;
                if (proj.vel.x > 0 && dir == Direction.Left) return true;
                if (proj.vel.x < 0 && dir == Direction.Right) return true;
            }
            else
            {
                if (proj.vel.x > 0 && dir == Direction.Down) return true;
                if (proj.vel.x < 0 && dir == Direction.Up) return true;
            }
            return false;
        }

        public void addItem(InventoryItem inventoryItem, int slot)
        {
            if (inventoryItem.item.immediate)
            {
                if (inventoryItem.item == Item.heartContainer)
                {
                    health.increaseMaxValue(1);
                    health.add(1);
                }
                if (inventoryItem.item == Item.arrows10) arrows.addImmediate(5);
                if (inventoryItem.item == Item.arrows30) arrows.addImmediate(30);
                if (inventoryItem.item == Item.blueRupee) rupees.add(5);
                if (inventoryItem.item == Item.redRupee) rupees.add(20);
                if (inventoryItem.item == Item.purpleRupee) rupees.add(50);
                if (inventoryItem.item == Item.rupees100) rupees.add(100);
                if (inventoryItem.item == Item.rupees300) rupees.add(300);
                return;
            }

            items[slot] = inventoryItem;
            if (inventoryItem.item == Item.shield1) childFrameTagsToHide.Remove("shield");
            if (inventoryItem.item == Item.shield2) childFrameTagsToHide.Remove("shield2");
            if (inventoryItem.item == Item.shield3) childFrameTagsToHide.Remove("shield3");

            if (inventoryItem.item == Item.sword1) childFrameTagsToHide.Remove("sword");
            if (inventoryItem.item == Item.sword2) childFrameTagsToHide.Remove("sword2");
            if (inventoryItem.item == Item.sword3) childFrameTagsToHide.Remove("sword3");
            if (inventoryItem.item == Item.sword4) childFrameTagsToHide.Remove("sword4");

            if (hasItem(Item.redMail) && hasItem(Item.blueMail))
            {
                paletteShader.SetUniform("palette", 2);
            }
            else if (hasItem(Item.blueMail))
            {
                paletteShader.SetUniform("palette", 1);
            }
            else if (hasItem(Item.redMail))
            {
                paletteShader.SetUniform("palette", 2);
            }
            else paletteShader.SetUniform("palette", 0);
        }

        public void dropItem(int slot)
        {
            if (items[slot] == null) return;
            InventoryItem inventoryItem = items[slot];

            if (inventoryItem.item == Item.caneOfBryana)
            {
                if (stateManager.bryanaRing != null)
                {
                    stateManager.bryanaRing.remove();
                    stateManager.bryanaRing = null;
                }
            }
            if (inventoryItem.item == Item.caneOfSomaria)
            {
                if (caneBlock != null)
                {
                    caneBlock.split();
                    caneBlock = null;
                }
            }
            if (inventoryItem.item == Item.cape)
            {
                if (stateManager.invisible)
                {
                    stateManager.invisible = false;
                    new Anim(level, pos, "CapePoof");
                    playSound("cape off");
                }
            }

            FieldItem fieldItem = new FieldItem(level, pos, inventoryItem, true);

            if (inventoryItem.item == Item.sword2)
            {
                Global.game.masterSwordChar = null;
                Global.game.masterSword = fieldItem;
            }

            fieldItem.vel = Helpers.dirToVec(dir);
            items[slot] = null;

            if (!hasItem(Item.shield1)) childFrameTagsToHide.Add("shield");
            if (!hasItem(Item.shield2)) childFrameTagsToHide.Add("shield2");
            if (!hasItem(Item.shield3)) childFrameTagsToHide.Add("shield3");

            if (!hasItem(Item.sword1)) childFrameTagsToHide.Add("sword");
            if (!hasItem(Item.sword2)) childFrameTagsToHide.Add("sword2");
            if (!hasItem(Item.sword3)) childFrameTagsToHide.Add("sword3");
            if (!hasItem(Item.sword4)) childFrameTagsToHide.Add("sword4");

            if (hasItem(Item.redMail) && hasItem(Item.blueMail))
            {
                paletteShader.SetUniform("palette", 2);
            }
            else if (hasItem(Item.blueMail)) paletteShader.SetUniform("palette", 1);
            else if (hasItem(Item.redMail)) paletteShader.SetUniform("palette", 2);
            else paletteShader.SetUniform("palette", 0);
            playSound("throw");
        }

        public bool hasItem(Item item)
        {
            if (hasEverything) return true;
            foreach (var inventoryItem in items)
            {
                if (inventoryItem != null && inventoryItem.item == item) return true;
            }
            return false;
        }

        public bool hasSword()
        {
            return hasItem(Item.sword1) || hasItem(Item.sword2) || hasItem(Item.sword3) || hasItem(Item.sword4);
        }

        public void setItemGetAnimation(Animation sprite)
        {
            itemGet = sprite;
            itemGetTime = 0.01f;
        }

        public bool collectItem(InventoryItem inventoryItem, int overwriteSlot = -1)
        {
            bool collected = false;
            bool alreadyAnimed = false;
            if (inventoryItem.item.usesQuantity)
            {
                int firstIndex = -1;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] != null && items[i].item == inventoryItem.item && !items[i].maxed())
                    {
                        int loop = 0;
                        while (!items[i].maxed() && inventoryItem.count > 0)
                        {
                            loop++; if (loop > 10000) { throw new Exception("INFINITE LOOP IN CHARACTER COLLECT"); }
                            if (firstIndex == -1) firstIndex = i;
                            items[i].count++;
                            inventoryItem.count--;
                        }
                    }
                }
                if (inventoryItem.item == Item.heartPiece && firstIndex != -1 && items[firstIndex].count >= 4)
                {
                    items[firstIndex].count -= 4;
                    if (items[firstIndex].count == 0) items[firstIndex] = null;
                    inventoryItem.item = Item.heartContainer;
                    inventoryItem.count = 1;
                    collected = true;
                    addItem(inventoryItem, -1);
                }
                else if (inventoryItem.count == 0)
                {
                    collected = true;
                }

                if (firstIndex != -1)
                {
                    Animation sprite = Global.animations["FieldItem"].clone();
                    sprite.frameIndex = inventoryItem.item.spriteIndex;
                    sprite.frameSpeed = 0;
                    setItemGetAnimation(sprite);
                    if (this == Global.game.camCharacter) playSound("item get 1");
                    alreadyAnimed = true;
                }

            }

            int slot = getEmptySlot();
            if (overwriteSlot >= 0)
            {
                slot = overwriteSlot;
                items[slot] = null;
            }
            if (slot == -1 && !inventoryItem.item.immediate)
            {
                return collected;
            }

            if (inventoryItem.item.immediate || items[slot] == null)
            {
                if (!alreadyAnimed)
                {
                    addItem(inventoryItem, slot);
                    Animation sprite = Global.animations["FieldItem"].clone();
                    sprite.frameIndex = inventoryItem.item.spriteIndex;
                    sprite.frameSpeed = 0;
                    setItemGetAnimation(sprite);
                    collected = true;
                    if (this == Global.game.camCharacter) playSound("item get 1");
                }
            }

            if (inventoryItem.item == Item.sword2)
            {
                Global.game.masterSword = null;
                Global.game.masterSwordChar = this;
            }

            return collected;
        }

        public bool canLift(Item item)
        {
            if (hasItem(item)) return true;
            if (item == Item.powerGlove && hasItem(Item.titansMitt)) return true;
            return false;
        }

        public bool canSwim()
        {
            return hasItem(Item.flippers) && stateManager.bunnyTime == 0;
        }

        public void checkMusicChange()
        {
            if (Global.game.camCharacter != this) return;
            if (level.inLostWoods())
            {
                if (!Global.game.masterSwordPulled)
                {
                    level.startMusic("lost_woods");
                }
            }
            else if (level.inKakarikoVillage())
            {
                if (Global.music != null && Global.music.name != "kakariko")
                {
                    level.startMusic("kakariko");
                }
            }
            else if (Global.music.name == "lost_woods" || Global.music.name == "kakariko")
            {
                level.startMusic();
            }
        }

        public bool shouldAIGetItem(Item item)
        {
            int weaponCount = 0;
            for (int i = 0; i < 5; i++)
            {
                if (items[i] != null && (items[i].item.isWeapon))
                {
                    weaponCount++;
                }
            }
            weaponCount -= (item.isWeapon ? 0 : 1);
            if (weaponCount == 0)
            {
                return false;
            }

            for (int i = 0; i < 5; i++)
            {
                if (items[i] == null) continue;
                if (item.spawnOddsWeight > items[i].item.spawnOddsWeight)
                {
                    return true;
                }
            }
            return false;
        }

        public void aiDropWorstSlot()
        {
            if (hasEmptySlot()) return;
            int worstWeight = -1;
            int worstI = 0;
            for (int i = 0; i < 5; i++)
            {
                if (items[i] == null) continue;
                if (items[i].item.spawnOddsWeight > worstWeight)
                {
                    worstWeight = items[i].item.spawnOddsWeight;
                    worstI = i;
                }
            }
            dropItem(worstI);
        }

    }
}

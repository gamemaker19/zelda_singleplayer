using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class ActorState
    {
	    public string name = "";
        public float stateTime = 0;
        public float runSpeed = 1;
        public string baseSpriteName = "";
        public Actor actor;
        public ActorState prevState = null;
        public StateManager stateManager;
        public Actor throwable = null;
        public string enterSound = "";
        public float distTravelled = 0;
        public bool superArmor = false;
        public bool matchTileElevation = true;
        public bool once = false;  //Generic flag that can be used for various purposes, typically for "one-shot" actions in a state
        public int state = 0;  //Generic state var that can be used for various purposes
        public bool isInvincible = false;
        public bool isSolid = true;
        public bool aiTarget = true;
        public bool teleportShadow = false;
        public bool strafe = false;
        public int ledgeFrames = 0;
        public float swimStrokeTime = 0;
        public bool drawWadable = true;
        public bool canCut = false;
        public int poiFrame = -1;

        public ActorState(string name)
        {
            this.name = name;
            this.baseSpriteName = name;
        }

        public ActorState(string name, string baseSpriteName)
        {
            this.name = name;
            this.baseSpriteName = baseSpriteName;
        }

        public virtual Actor getProj(Point pos, Direction dir)
        {
            return null;
        }

        public void getPOIFrame()
        {
            if (poiFrame == -1)
            {
                for (int i = 0; i < this.actor.sprite.frames.Count; i++)
                {
                    Frame frame = this.actor.sprite.frames[i];
                    if (frame.POIs.Count > 0)
                    {
                        poiFrame = i;
                        return;
                    }
                }
                poiFrame = 0;
            }
        }

        public Character getChar()
        {
            return actor as Character;
        }

        public Actor projectileCode()
        {
            getPOIFrame();
            if (actor.sprite.frameIndex == poiFrame && !once)
            {
                once = true;
                List<Point> pois = this.actor.sprite.getCurrentFrame().POIs;
                Point poiOffset = Point.Zero;
                if (pois.Count > 0)
                {
                    poiOffset = pois[0];
                    if (actor.getXDir() == -1)
                    {
                        poiOffset.x *= -1;
                        poiOffset.x += 10;
                    }
                }
                Actor proj = getProj(this.actor.getScreenPos() + this.actor.getOffset(true) + poiOffset, actor.dir);
                proj.elevation = actor.elevation;
                Character character = actor as Character;
                return proj;
            }
            return null;
        }

        public virtual void update()
        {
            stateTime += Global.spf;
        }

        public virtual void lateUpdate()
        {
        }

        public virtual void onExit(ActorState newState)
        {

        }

        public virtual void onEnter(ActorState oldState)
        {
            Character character = actor as Character;
            if (!string.IsNullOrEmpty(enterSound)) character.playSound(enterSound);
        }

        public void playFootsteps(bool isFast)
        {
            if (actor.wadeSprite == null) return;
            float offset = isFast ? -0.1f : 0f;
            if (actor.wadeSprite.name == "WadeGrass") actor.playLoopingSound("walk grass", offset);
            else if (actor.wadeSprite.name == "WadeWater") actor.playLoopingSound("walk water", offset);
        }
        
        public bool commonLinkGround()
        {
            Character character = actor as Character;
            character.distMovedInFrame = 0;

            Point move = Point.Zero;
            float boost = 1;

#if DEBUG
            if (stateManager.input.isHeld(Key.LShift)) boost = 3;
#endif

            if (stateManager.input.isHeld(Control.main.Left))
            {
                move.x = -1;
            }
            else if (stateManager.input.isHeld(Control.main.Right))
            {
                move.x = 1;
            }
            if (stateManager.input.isHeld(Control.main.Up))
            {
                move.y = -1 * (stateManager.invertUp ? 0 : 1);
            }
            else if (stateManager.input.isHeld(Control.main.Down))
            {
                move.y = 1;
            }

            if (!stateManager.input.isHeld(Control.main.Up)) stateManager.invertUp = false;

            bool zStrafe = false;
            if (stateManager.input.isHeld(Key.Z) && actor.aiStateManager != null)
            {
                zStrafe = true;
            }
            else
            {
                zStrafe = false;
            }

            if (actor.dir == Direction.Up || actor.dir == Direction.Down)
            {
                if (!strafe && !zStrafe)
                {
                    if (move.y > 0) actor.changeDir(Direction.Down);
                    else if (move.y < 0) actor.changeDir(Direction.Up);
                    else if (move.x > 0) actor.changeDir(Direction.Right);
                    else if (move.x < 0) actor.changeDir(Direction.Left);
                }
            }
            else
            {
                if (!strafe && !zStrafe)
                {
                    if (move.x > 0) actor.changeDir(Direction.Right);
                    else if (move.x < 0) actor.changeDir(Direction.Left);
                    else if (move.y > 0) actor.changeDir(Direction.Down);
                    else if (move.y < 0) actor.changeDir(Direction.Up);
                }
            }
            if (move.x != 0 && move.y != 0)
            {
                //move = move.normalized();
            }

            if (move.x != 0 || move.y != 0)
            {
                if (name == "LinkDash")
                {
                    stateManager.changeState(new LinkIdle(), true);
                    return true;
                }
            }

            move.x *= stateManager.runSpeed * boost;
            move.y *= stateManager.runSpeed * boost;

            bool fastFootsteps = false;
            if (name == "LinkDash")
            {
                move = Helpers.dirToVec(actor.dir) * 4;
                fastFootsteps = true;
            }

            if (move.isNonZero())
            {
                playFootsteps(fastFootsteps);
            }

            if (name != "LinkSwim" && actor.level.isActorInTileWithTag(actor, "ladder"))
            {
                move *= 0.5f;
            }

            stateManager.lastLandPos = actor.pos;

            if (name == "LinkSwim")
            {
                if (move.magnitude == 0)
                {
                    actor.vel *= 0.95f;
                    if (actor.vel.magnitude < 0.01) actor.vel = Point.Zero;
                    move = actor.vel;
                }
                else
                {
                    Point with = actor.vel.project(move);
                    Point without = actor.vel.withoutComponent(move);
                    without *= 0.95f;
                    actor.vel = with + without;

                    float maxSwimSpeed = 0.5f;
                    if (swimStrokeTime > 0) maxSwimSpeed = 1;

                    if (with.magnitude < maxSwimSpeed)
                    {
                        actor.vel += move * 0.01f;
                    }
                    else
                    {
                        actor.vel *= 0.95f;
                        if (actor.vel.magnitude < 0.01) actor.vel = Point.Zero;
                    }
                    move = actor.vel;
                }
            }

            if (name == "LinkCarry")
            {
                if (throwable.throwable.isHeavy)
                {
                    move = Point.Zero;
                }
            }

            if (move.magnitude > 0)
            {
                if (actor.wadeSprite != null && actor.wadeSprite.name == "WadeGrass")
                {
                    actor.wadeSprite.frameIndex = actor.sprite.frameIndex % 2;
                }
                Point before = actor.pos;
                List<CollideData> collisions = actor.move(move, true, true);
                if (collisions.Count > 0 && move.x != 0 && move.y != 0)
                {
                    Point move2 = move;
                    move2.x = 0;
                    List<CollideData> collisions2 = actor.move(move2, true, true);
                    if (collisions2.Count > 0)
                    {
                        move2 = move;
                        move2.y = 0;
                        collisions2 = actor.move(move2, true, true);
                        if (collisions2.Count == 0)
                        {
                            collisions = collisions2;
                            move = move2;
                        }
                    }
                    else
                    {
                        collisions = collisions2;
                        move = move2;
                    }
                }
                if (collisions.Count > 0) character.moveCollisionLastFrame = true;
                Point after = actor.pos;

                character.distMovedInFrame = (after - before).magnitude;

                /*
                CaneBlock* caneBlockPushed = checkCaneBlock(move);
                if (caneBlockPushed)
                {
                  stateManager.changeState(new LinkPush(caneBlockPushed), false);
                  return true;
                }
                */

                bool hitLedge = checkLedgeJump(move, collisions);

                bool noBonk = false;
                foreach (var collision in collisions)
                {
                    if (collision.collidedTile == null) continue;
                    if (collision.collidedTile.hasTag("ledge"))
                    {
                        if (ledgeFrames > 0)
                        {
                            noBonk = true;
                            break;
                        }
                    }
                    else if (collision.diagDir != 0 && after != before)
                    {
                        noBonk = true;
                        break;
                    }
                }

                if (name == "LinkDash")
                {
                    if (!noBonk && collisions.Count > 0)
                    {
                        stateManager.changeState(new LinkBonk(), true);
                        foreach (CollideData collideData in collisions)
                        {
                            RockPile rockPile = collideData.collidedActor as RockPile;
                            if (rockPile != null)
                            {
                                rockPile.onDash();
                            }
                        }
                        return true;
                    }
                }

                if (name == "LinkSpinAttackCharge")
                {
                    Point dir = Helpers.dirToVec(actor.dir);
                    if (!noBonk && collisions.Count > 0 && ((move.x != 0 && Math.Sign(move.x) == Math.Sign(dir.x)) || (move.y != 0 && Math.Sign(move.y) == Math.Sign(dir.y))))
                    {
                        bool playClingSound = true;
                        if (collisions[0].collidedActor != null) playClingSound = false;
                        stateManager.changeState(new LinkPoke(playClingSound), true);
                        return true;
                    }
                }

                if (collisions.Count > 0 && name == "LinkSwim")
                {
                    actor.vel = Point.Zero;
                }

                if (hitLedge) return true;
                if (checkSwim(move)) return true;
                if (name != "LinkDash") character.changeSprite(baseSpriteName + "Move");
            }
            else
            {
                character.changeSprite(baseSpriteName);
            }
            return false;
        }

        public bool checkSwim(Point move)
        {
            if (actor.level.isActorInTileWithTag(actor, "water"))
            {
                if (name != "LinkSwim")
                {
                    if (name != "LinkJump" && !getChar().canSwim())
                    {
                        stateManager.changeState(new LinkSwimJump(), false);
                    }
                    else
                    {
                        stateManager.changeState(new LinkSwim(), false);
                    }
                    return true;
                }
            }
            else
            {
                if (name == "LinkSwim" || name == "LinkJump")
                {
                    stateManager.changeState(new LinkIdle(), false);
                    return true;
                }
            }
            return false;
        }

        public CaneBlock checkCaneBlock(Point move)
        {
            List<CollideData> collisions = actor.level.getActorCollisions(actor, move);
            foreach (CollideData collideData in collisions)
            {
                if (collideData.collidedActor == null) continue;
                CaneBlock caneBlock = collideData.collidedActor as CaneBlock;
                if (caneBlock != null && caneBlock.owner == actor)
                {
                    return caneBlock;
                }
            }
            return null;
        }

        public bool checkLedgeJump(Point move, List<CollideData> collisions)
        {
            bool ledgeFound = false;
            foreach (CollideData collideData in collisions)
            {
                //Check swim ledges
                if (collideData.collidedTile != null && collideData.collidedTile.hasTag("swimledge"))
                {
                    if (actor.dir == Direction.Down)
                    {

                    }
                }

                LedgeTile ledgeTile = collideData.collidedTile as LedgeTile;
                if (ledgeTile != null)
                {
                    ledgeFound = true;
                    ledgeFrames++;
                    if ((name == "LinkDash" && ledgeFrames > 15) || ledgeFrames > 30)
                    {
                        float xDir = ledgeTile.xDir;
                        float yDir = ledgeTile.yDir;
                        if (yDir == 0 && xDir == 0)
                        {
                            //Helpers.exception("left and down 0!");
                            ledgeFrames = 0;
                            return false;
                        }

                        float tilesLeftOrRight = xDir;
                        float tilesUpOrDown = yDir;

                        int i = collideData.tileI + (int)tilesUpOrDown;
                        int j = collideData.tileJ + (int)tilesLeftOrRight;

                        bool atLeastOne = false;
                        while (i >= 0 && i < actor.level.gridHeight && j >= 0 && j < actor.level.gridWidth)
                        {
                            TileSlot tileSlot = actor.level.tileSlots[i][j];

                            bool ledgeNotFound = true;
                            foreach (TileData tileData in tileSlot.tileInstances)
                            {
                                //If we can't find a contributer to ledge jump distance, break out of nested loop
                                //if (Helpers.strContains(tileData.tag, "ledgewall"))
                                if (tileData.hitboxMode != HitboxMode.None)
                                {
                                    ledgeNotFound = false;
                                    break;
                                }
                            }
                            if (ledgeNotFound)
                            {
                                break;
                            }
                            atLeastOne = true;

                            i += (int)yDir;
                            tilesUpOrDown += yDir;
                            j += (int)xDir;
                            tilesLeftOrRight += xDir;
                        }

                        if (!atLeastOne)
                        {
                            ledgeFrames = 0;
                            return false;
                        }

                        //cout << "SIDES: " << tilesLeftOrRight << endl;
                        //cout << "Direction.Up/Direction.Down: " << tilesUpOrDown << endl;

                        tilesLeftOrRight += Math.Sign(tilesLeftOrRight);
                        tilesUpOrDown += Math.Sign(tilesUpOrDown);

                        bool teleportDown = false;
                        float maxDist = 0;
                        Point destPoint;
                        bool upLeft = false;
                        bool leftRight = false;

                        //Up left/right
                        if ((xDir == -1 && yDir == -1) || (xDir == 1 && yDir == -1))
                        {
                            upLeft = true;
                        }
                        //Up
                        else if (xDir == 0 && yDir == -1)
                        {
                        }
                        //down
                        else if (xDir == 0 && yDir == 1)
                        {
                            teleportDown = true;
                            actor.zVel = 1;
                        }
                        //Sides
                        else if ((xDir == -1 && yDir == 0) || (xDir == 1 && yDir == 0))
                        {
                            teleportDown = true;
                            leftRight = true;
                            actor.zVel = 1;
                            tilesLeftOrRight *= 1.25f;
                            tilesUpOrDown = (Math.Abs(tilesLeftOrRight) * 0.625f);
                        }
                        //down left / right
                        else
                        {
                            teleportDown = true;
                            actor.zVel = 1;
                            tilesLeftOrRight *= 1.125f;
                            tilesUpOrDown = Math.Abs(tilesLeftOrRight);
                        }

                        Point offset = new Point(tilesLeftOrRight * 8, tilesUpOrDown * 8);
                        destPoint = actor.pos + offset.incMag(8);
                        matchTileElevation = false;

                        if (teleportDown)
                        {
                            actor.z = destPoint.y - actor.pos.y;
                            actor.pos.y = destPoint.y;
                            //float t = sqrt((2 * actor.z) / global.gravity);
                            float v = actor.zVel;
                            float g = Global.gravity;
                            float d = actor.z;
                            float t = (v + (float)Math.Sqrt((v * v + 2 * g * d))) / g;
                            float velX = Math.Abs(actor.pos.x - destPoint.x) / t;
                            actor.vel.x = velX * Math.Sign(tilesLeftOrRight);
                        }
                        else
                        {
                            float speed = 1;
                            if (upLeft) speed = 1.5f;
                            float dist = actor.pos.distTo(destPoint);
                            float t = dist / speed;
                            actor.zVel = 0.5f * Global.gravity * t;
                            actor.z = 0.1f;
                            actor.vel = actor.pos.dirTo(destPoint) * speed;
                        }

                        maxDist = actor.pos.distTo(destPoint);
                        string jumpSprite = baseSpriteName + "Move";
                        if (name == "LinkDash") jumpSprite = "LinkDash";
                        var ledgeJumpState = new LinkLedgeJump(collideData, maxDist, destPoint, teleportDown, jumpSprite);
                        var linkCarry = stateManager.actorState as LinkCarry;
                        stateManager.changeState(ledgeJumpState, false);
                        return true;
                    }
                }
            }
            if (!ledgeFound)
            {
                ledgeFrames = 0;
            }
            return false;
        }

    }
}

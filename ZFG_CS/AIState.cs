using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class AIState
    {
	    public Actor actor;
        public Character character;
        public AIStateManager ai;
        public string name = "";
        public bool findTargets = true;
        public Point dirToNext;
        public AIState(AIStateManager ai, string name)
        {
	        this.name = name;
	        this.ai = ai;
	        this.actor = ai.actor;
	        this.character = actor.getChar();
        }
        public virtual void update() { }
    }

    public class DecideState : AIState
    {
        public DecideState(AIStateManager ai) : base(ai, "Decide")
        {
        }

        public override void update()
        {
            ai.decided = false;
        }
    }

    public class MoveToPos : AIState
    {
	    public Point dest;
        public List<TileSlot> path;
        public int pathIndex = 0;
        public float distMoved = 0;
        public int iterations = 0;
        public string tag = "";
        public Point freeDir;
        public AIState onMovedToPos = null;
        public MoveToPos(AIStateManager ai, Point dest, bool findTargets, AIState onMovedToPos = null) : base(ai, "MoveToPos")
        {
            this.dest = dest;
            this.findTargets = findTargets;
            this.onMovedToPos = onMovedToPos;
            //if (Global.logAI) cout << "Finding path from to " << (int)(character.pos.x / 8) << "," << (int)(character.pos.y / 8) << " to " << (int)(dest.x / 8) << "," << (int)(dest.y / 8) << endl;
            path = actor.level.getShortestPath(actor.getChar(), actor.getColliderPos(), dest, ai.target);
            if (path.Count == 0)
            {
                //if (Global.logAI) cout << "Finding path failed!" << endl;
            }
            Global.game.shortestPath = path;
        }

        public override void update()
        {
            if (tag == "mastersword" && Global.game.masterSwordBeingPulled)
            {
                ai.decided = false;
                return;
            }

            if (path.Count == 0 || pathIndex >= path.Count - 1)
            {
                //if (Global.logAI && path.Count == 0) cout << "Empty path! Moving immediately..." << endl;
                //if (Global.logAI && path.Count > 0) cout << "Moved to position" << endl;

                if (onMovedToPos != null)
                {
                    ai.aiState = onMovedToPos;
                }
                else
                {
                    ai.decided = false;
                }
                return;
            }

            TileSlot nextSlot = path[pathIndex + 1];
            Point nextPoint = new Point(4 + nextSlot.gridCoords.j * 8, 4 + nextSlot.gridCoords.i * 8);

            int nextI = (int)(nextPoint.y / 8);
            int nextJ = (int)(nextPoint.x / 8);

            int i = path[pathIndex].gridCoords.i;
            int j = path[pathIndex].gridCoords.j;

            Point move = new Point(nextJ - j, nextI - i);

            Point actorPos = actor.getColliderPos();
            float dist = 0;
            if (move.x != 0)
            {
                dist = Mathf.Abs(nextPoint.x - actorPos.x);
            }
            else if (move.y != 0)
            {
                dist = Mathf.Abs(nextPoint.y - actorPos.y);
            }
            float lenience = 1.1f;
            //if (iterations > 12) lenience = 8;

            if (dist <= lenience)
            {
                iterations = 0;
                distMoved = 0;
                freeDir = new Point();
                dirToNext = actorPos.dirTo(nextPoint);
                float dist2 = actorPos.distTo(nextPoint);
                if (dist2 >= 20)
                {
                    ai.aiState = new MoveToPos(ai, dest, findTargets, onMovedToPos);
                    return;
                }
                Point offset = actor.getColliderPos() - character.pos;
                if (offset.magnitude > 4)
                {
                    //offset = Point();
                }
                actor.changePos(nextPoint - offset, false);
                pathIndex++;
            }
            else
            {
                iterations++;
                distMoved += character.distMovedInFrame;
                if (iterations > 12)
                {
                    move = (nextPoint - actorPos).normalized;
                }

                if (actor.getState() == "LinkCarry")
                {
                    ai.stateManager.input.keyPressed[Control.main.Action] = true;
                    return;
                }

                /*
                //Stuck, free self
                if (iterations > 12)
                {
                    if (freeDir.isZero())
                    {
                        Point forwardDir = actor.getLookDir();
                        Point rightDir = forwardDir.right();
                        Point leftDir = rightDir * -1;
                        for (int i1 = 0; i1 < 16; i1++)
                        {
                            int i2 = (i1 >= 8 ? i1 - 8 : i1);
                            Point dirToUse = (i1 >= 8 ? leftDir : rightDir);
                            Point offset = forwardDir + (dirToUse * i2);
                            var collisions2 = actor.level.getActorCollisions(actor, offset);
                            if (collisions2.Count == 0)
                            {
                                freeDir = offset;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    freeDir = new Point();
                }
                */

                move += freeDir;

                if (move.x > 0) ai.stateManager.input.keyHeld[Control.main.Right] = true;
                else if (move.x < 0) ai.stateManager.input.keyHeld[Control.main.Left] = true;
                if (move.y > 0) ai.stateManager.input.keyHeld[Control.main.Down] = true;
                else if (move.y < 0) ai.stateManager.input.keyHeld[Control.main.Up] = true;

                var collisions = actor.level.getActorCollisions(actor, move);
                foreach (var collision in collisions)
                {
                    if (collision.collidedActor != null && collision.collidedActor.throwable != null && collision.collidedActor.isSolid && !collision.collidedActor.throwable.thrown)
                    {
                        Direction dir = Helpers.vecToDir(move);
                        if (dir == Direction.Left || dir == Direction.Right || dir == Direction.Up || dir == Direction.Down)
                        {
                            if (actor.getState() == "LinkIdle") actor.changeDir(dir);
                        }
                        if (ai.liftCooldown == 0)
                        {
                            ai.stateManager.input.keyPressed[Control.main.Action] = true;
                            ai.liftCooldown = Global.spf;
                        }
                    }
                    else if (collision.collidedActor != null && collision.collidedActor.name == "Stake")
                    {
                        Direction dir = Helpers.vecToDir(move);
                        if (dir == Direction.Left || dir == Direction.Right || dir == Direction.Up || dir == Direction.Down)
                        {
                            if (actor.getState() == "LinkIdle") actor.changeDir(dir);
                        }
                        int hammerIndex = character.getItemSlot(Item.hammer);
                        if (hammerIndex == 0) ai.stateManager.input.keyPressed[Control.main.Item1] = true;
                        if (hammerIndex == 1) ai.stateManager.input.keyPressed[Control.main.Item2] = true;
                        if (hammerIndex == 2) ai.stateManager.input.keyPressed[Control.main.Item3] = true;
                        if (hammerIndex == 3) ai.stateManager.input.keyPressed[Control.main.Item4] = true;
                        if (hammerIndex == 4) ai.stateManager.input.keyPressed[Control.main.Item5] = true;
                        //return;
                    }
                }

                if (iterations > 180)
                {
                    //cout << "IDLE, DECIDE" << endl;
                    ai.decided = false;
                }

            }
        }

    }

    public class OpenChest : AIState
    {
	    Chest targetChest;
        public OpenChest(AIStateManager ai, Chest targetChest) : base(ai, "OpenChest")
        {
            this.targetChest = targetChest;
        }

        public override void update()
        {
            actor.stateManager.input.keyHeld[Control.main.Up] = true;
            actor.stateManager.input.keyPressed[Control.main.Action] = true;
            if (targetChest.opened)
            {
                ai.decided = false;
            }
        }

    }

    public class EnterEntrance : AIState
    {
	    Entrance targetEntrance;
	    bool startedIndoors = false;

        public EnterEntrance(AIStateManager ai, Entrance targetEntrance) : base(ai, "EnterEntrance")
        {
            this.targetEntrance = targetEntrance;
            this.startedIndoors = actor.level.isIndoor;
        }

        public override void update()
        {
            if (!targetEntrance.level.isIndoor) actor.stateManager.input.keyHeld[Control.main.Up] = true;
            else actor.stateManager.input.keyHeld[Control.main.Down] = true;
            if (actor.level.isIndoor && !startedIndoors)
            {
                ai.decided = false;
            }
            else if (!actor.level.isIndoor && startedIndoors)
            {
                ai.decided = false;
            }
        }

    }

    public class GetItem : AIState
    {
	    float stateTime = 0;
	    FieldItem targetItem;

        public GetItem(AIStateManager ai, FieldItem targetItem) : base(ai, "GetItem")
        {
            this.targetItem = targetItem;
        }

        public override void update()
        {
            if (!character.hasEmptySlot())
            {
                character.aiDropWorstSlot();
            }
            stateTime += Global.spf;
            if (stateTime > 0.5)
            {
                actor.stateManager.input.keyPressed[Control.main.Action] = true;
                ai.decided = false;
            }
        }

    }

    public class PullMasterSword : AIState
    {
	    float stateTime = 0;

        public PullMasterSword(AIStateManager ai) : base(ai, "PullMasterSword")
        {
            if (!character.hasEmptySlot())
            {
                character.aiDropWorstSlot();
            }
        }

        public override void update()
        {
            if (Global.game.masterSwordBeingPulled && character.getState() != "LinkMasterSwordPull")
            {
                ai.decided = false;
                return;
            }
            if (Global.game.masterSwordPulled && character.getState() != "LinkMasterSwordPull")
            {
                ai.decided = false;
                return;
            }
            stateTime += Global.spf;
            if (stateTime > 30)
            {
                ai.decided = false;
                return;
            }
            actor.stateManager.input.keyHeld[Control.main.Down] = true;
            actor.stateManager.input.keyPressed[Control.main.Action] = true;
        }

    }
}

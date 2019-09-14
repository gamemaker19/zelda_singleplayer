using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkMasterSwordPull : ActorState
    {
        Actor origSword = null;
        Actor spark1 = null;
        Actor spark2 = null;
        Actor sword = null;
        int iterCount = 0;
        bool frameAlt = false;
        float iterTime = 0;

        public LinkMasterSwordPull(Actor origSword) : base("LinkMasterSwordPull", "LinkGrab")
        {
            this.origSword = origSword;
            superArmor = true;
            Global.game.masterSwordBeingPulled = true;
        }

        public override void update()
        {
            base.update();
            Character character = actor.getChar();
            if (state == 0)
            {
                if (stateTime > 3.5)
                {
                    state = 1;
                    stateTime = 0;
                    spark1.remove();
                    spark1 = null;
                    spark2 = new Anim(actor.level, actor.pos.addxy(0, 15), "ParticlePullSword2", false);
                }
            }
            else if (state == 1)
            {
                Point origin = actor.pos.addxy(0, 15);
                Color lineCol = Color.White;
                iterTime += Global.spf;
                if (iterTime > 0.1)
                {
                    iterCount++;
                    if (iterCount > 2) iterCount = 0;
                    frameAlt = !frameAlt;
                    iterTime = 0;
                }
                for (int i = 0; i < 8; i++)
                {
                    if (i % 2 != (frameAlt ? 1 : 0)) continue;
                    float mag = 5;
                    float xDist = mag * Mathf.Cos(i * 45);
                    float yDist = mag * Mathf.Sin(i * 45);
                    int iters = iterCount + (i % 2);
                    Point start = new Point(origin.x +xDist * iters, origin.y + yDist * iters);
                    Point end = new Point(origin.x +xDist * (iters + 1), origin.y + yDist * (iters + 1));
                    DrawWrappers.DrawLine(origin.x, origin.y, end.x, end.y, lineCol, 1, ZIndex.Link + 1, true);
                    //wal_draw_line(origin.x, origin.y, end.x, end.y, lineCol, 3, ZLink + 1, true);
                }
                if (stateTime > 3.5)
                {
                    state = 2;
                    stateTime = 0;
                    actor.sprite.frameSpeed = 1;
                    actor.sprite.wrapMode = "once";
                }
            }
            else if (state == 2)
            {
                float alpha = -1;
                if (stateTime< 0.5)
                {
                    alpha = stateTime* 2;
                }
                else if (stateTime >= 0.5 && stateTime< 1)
                {
                    alpha = (1 - stateTime) * 2;
                }
                if (alpha != -1)
                {
                    if (Global.game.camCharacter.level.inLostWoods())
                    {
                        DrawWrappers.DrawRect(0, 0, Global.screenW, Global.screenH, true, new Color(255, 255, 255, (byte)(int)(alpha * 255)), 1, ZIndex.HUD - 4, false);
                    }
                }

                if (stateTime > 0.5 && sword == null && !once)
                {
                    once = true;
                    var character2 = actor.getChar();
                    origSword.remove();
                    origSword = null;
                    spark2.remove();
                    spark2 = null;
                    actor.playSound("sword shine 1");
                    sword = new Actor(actor.level, actor.pos.addxy(5, -12), "MasterSwordFlash");
                    actor.changeSprite("LinkItemGet");
                    Global.game.masterSwordChar = character2;
                    Global.game.masterSwordPulled = true;
                    int slot = character2.getEmptySlot();
                    character2.addItem(new InventoryItem(Item.sword2), slot);
                    Global.game.killFeed.Add(new KillFeedEntry(character2.playerName + " claimed the Master Sword"));
                }
                if (stateTime > 1)
                {
                    state = 3;
                    stateTime = 0;
                }
            }
            else if (state == 3)
            {
                if (stateTime > 4)
                {
                    state = 4;
                    stateTime = 0;
                    //actor.getChar()
                    if (Global.game.camCharacter.level.inLostWoods())
                    {
                        Global.game.getCurrentLevel().startMusic("overworld");
                    }
                    stateManager.changeState(new LinkIdle(), false);
                }
            }
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
            spark1 = new Anim(actor.level, actor.pos.addxy(0, 15), "ParticlePullSword1", false);
            actor.sprite.frameSpeed = 0;

            if (Global.game.camCharacter.level.inLostWoods())
            {
                Global.music.music.Stop();
                Global.game.getCurrentLevel().startMusic("master_sword");
            }
        }

        public override void onExit(ActorState newState)
        {
            base.onExit(newState);
            if (spark1 != null) spark1.remove();
            if (spark2 != null) spark2.remove();
            if (sword != null) sword.remove();
            Global.game.masterSwordBeingPulled = false;
            if (Global.game.camCharacter.level.inLostWoods())
            {
                Global.music.music.Stop();
                if (!Global.game.masterSwordPulled) Global.game.getCurrentLevel().startMusic("lost_woods");
                else Global.game.getCurrentLevel().startMusic("overworld");
            }
        }

    }

}

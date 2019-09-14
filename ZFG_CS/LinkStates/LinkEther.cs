using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkEther : ActorState
    {
        Actor etherLightning = null;
        Actor etherLightning2 = null;
        float ballAngle = 0;
        float ballRadius = 48;
        List<Actor> etherBalls = new List<Actor>();
        int etherState = 0;

        public LinkEther() : base("LinkEther")
        {
            superArmor = true;
        }

        public override void update()
        {
            base.update();
            if (actor.sprite.frameIndex >= 12 && etherState == 0)
            {
                etherState = 1;
                actor.playSound("ether");
                etherLightning = new Anim(actor.level, actor.pos.addxy(0, 12), "EtherLightning", false);
            }
            if (etherState > 0)
            {
                if (stateTime > 0.04)
                {
                    stateTime = 0;
                    if(actor.shader == null)
                    {
                        actor.shader = Global.shaders["etherFlash"];
                    }
                    else
                    {
                        actor.shader = null;
                    }
                }
            }
            if (etherState == 1 && etherLightning != null && etherLightning.sprite.isAnimOver())
            {
                Damager etherDamager = new Damager(actor, Item.ether, 1);
                etherDamager.freeze = true;
                actor.level.removeActor(etherLightning);
                etherLightning = null;
                etherLightning2 = new Anim(actor.level, actor.pos, "EtherLightning2", etherDamager, false);
                etherState = 2;
            }
            if (etherState == 2 && etherLightning2.sprite.isAnimOver())
            {
                actor.level.removeActor(etherLightning2);
                etherLightning2 = null;
                etherState = 3;
            }
            if (etherState == 3)
            {
                for (int i = 0; i < 8; i++)
                {
                    Damager etherDamager = new Damager(actor, Item.ether, 1);
                    etherDamager.freeze = true;
                    Point ballPos = new Point(Mathf.Cos(i * 45) * ballRadius, Mathf.Sin(i * 45) * ballRadius);
                    Actor ball = new Anim(actor.level, actor.pos + ballPos, "EtherBall", etherDamager, false);
                    etherBalls.Add(ball);
                }
                etherState = 4;
            }
            if (etherState == 4)
            {
                ballAngle += Global.spf * 450;
                for (int i = 0; i < 8; i++)
                {
                    etherBalls[i].pos = actor.pos + new Point(Mathf.Cos(ballAngle + i * 45) * ballRadius, Mathf.Sin(ballAngle + i * 45) * ballRadius);
                }
                if (ballAngle > 450) etherState = 5;
            }
            if (etherState == 5)
            {
                ballRadius += Global.spf * 300;
                for (int i = 0; i < 8; i++)
                {
                    etherBalls[i].pos = actor.pos + new Point(Mathf.Cos(ballAngle + i * 45) * ballRadius, Mathf.Sin(ballAngle + i * 45) * ballRadius);
                }
                if (ballRadius > 200)
                {
                    etherState = 6;
                }
            }
            if (etherState == 6)
            {
                stateManager.changeState(new LinkIdle(), false);
            }
        }

        public override void onExit(ActorState newState)
        {
            base.onExit(newState);
            actor.shader = null;
            foreach (var etherBall in etherBalls)
            {
                etherBall.remove();
            }
            if (etherLightning != null) etherLightning.remove();
            if (etherLightning2 != null) etherLightning2.remove();
        }

    }
}

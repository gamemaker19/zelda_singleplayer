using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkBombos : ActorState
    {
        float flameAngle = 0;
        float flameRadius = 0;
        int bombosState = 0;
        int bombsSpawned = 0;

        public LinkBombos() : base("LinkBombos")
        {

        }
        
        public override void update()
        {
            base.update();

            if (bombosState == 0 && actor.sprite.isAnimOver())
            {
                stateTime = 0;
                bombosState = 1;
            }
            if (bombosState == 1)
            {
                actor.playLoopingSound("fire");
                if (stateTime > 0.1)
                {
                    Point basePos = actor.pos.addxy(0, 25);
                    stateTime = 0;
                    Point flamePos = new Point(Mathf.Cos(flameAngle) * flameRadius, Mathf.Sin(flameAngle) * flameRadius);
                    Anim bombosFlame = new Anim(actor.level, basePos + flamePos, "BombosFlame");
                    bombosFlame.isSolid = true;
                    Damager damager = new Damager(actor, Item.bombos, 1);
                    damager.burn = true;
                    bombosFlame.setDamager(damager);
                    flameRadius += 5;
                    flameAngle += 40;
                }
                if (flameRadius >= 80)
                {
                    bombosState = 2;
                    stateTime = 0;
                }
            }
            else if (bombosState == 2)
            {
                if (stateTime > 1)
                {
                    bombosState = 3;
                    stateTime = 0;
                }
            }
            else if (bombosState == 3)
            {
                if (stateTime > 0.05)
                {
                    int randX = Helpers.randomRange(-128, 128);
                    int randY = Helpers.randomRange(-112, 112);
                    Point randPos = new Point(randX, randY);
                    Damager damager = new Damager(actor, Item.bombos, 3);
                    //damager.burn = true;
                    Anim bombosBomb = new Anim(actor.level, actor.pos + randPos, "BombosBomb", damager);
                    randX = Helpers.randomRange(-128, 128);
                    randY = Helpers.randomRange(-112, 112);
                    bombosBomb = new Anim(actor.level, actor.pos + randPos, "BombosBomb", damager);
                    actor.playLoopingSound("bomb explode", -1);
                    stateTime = 0;
                    bombsSpawned++;
                }
                if (bombsSpawned > 25)
                {
                    stateManager.changeState(new LinkIdle(), false);
                }
            }
        }

        public override void onExit(ActorState newState)
        {
            base.onExit(newState);
        }

    }

}

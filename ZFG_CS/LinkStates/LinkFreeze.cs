using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkFreeze : ActorState
    {
        public bool shatter = false;
        public LinkFreeze() : base("LinkFreeze", "LinkIdle")
        {
            //isInvincible = true;
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
            actor.z = 0.01f;
            actor.zVel = 1;
            actor.shader = Global.shaders["frozen"];
            actor.playSound("ice rod");
            //actor.throwable = new Throwable(actor, "", "", false, 1, "");
            //actor.throwable.itemRequired = Item.powerGlove;
            //actor.throwable.bounce = true;
        }

        public override void update()
        {
            base.update();
            if (Global.frameCount % 120 == 0)
            {
                Point randOffset = new Point(Helpers.randomRange(-8, 8), Helpers.randomRange(-8, 8));
                new Anim(actor.level, actor.pos + randOffset, "ParticleFrozenSparkle");
            }
            if (stateTime > 10)
            {
                stateManager.changeState(new LinkIdle(), false);
            }
        }

        public override void onExit(ActorState newState)
        {
            base.onExit(newState);
            actor.shader = null;
            if (shatter)
            {
                new Anim(actor.level, actor.pos.addxy(0, actor.sprite.frames[0].rect.h() / 2), "ParticleIceBreak");
            }
            else
            {
                new Anim(actor.level, actor.pos.addxy(0, actor.sprite.frames[0].rect.h() / 2), "ParticleMelt");
            }
            //actor.throwable = nullopt;
            //actor.vel = Point();
            //actor.z = 0;
        }

    }

}

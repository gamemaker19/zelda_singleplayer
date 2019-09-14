using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkSpinAttack : ActorState
    {
        float particleTime = 0;
        bool spinParticleEnd = false;

        public LinkSpinAttack() : base("LinkSpinAttack", "LinkSpin")
        {
            canCut = true;
        }

        public override void update()
        {
            base.update();
            float startAngle = 0;
            if (actor.dir == Direction.Up)
            {
                startAngle = 90;
            }
            else if (actor.dir == Direction.Down)
            {
                startAngle = 270;
            }
            else if (actor.dir == Direction.Left)
            {
                startAngle = 0;
            }
            else if (actor.dir == Direction.Right)
            {
                startAngle = 180;
            }
            Point offset;
            float animTime = Helpers.clampMin(actor.sprite.animTime - 0.1f, 0);
            float totalAnimTime = actor.sprite.getAnimLength() - 0.1f;
            offset.x = 20 * Mathf.Cos(startAngle + 420 * (animTime / totalAnimTime));
            offset.y = 20 * Mathf.Sin(startAngle + 420 * (animTime / totalAnimTime));

            particleTime += Global.spf;
            if (particleTime > 0.05)
            {
                Point origin = actor.pos;
                particleTime = 0;
                Anim sparkle = new Anim(actor.level, origin + offset, "SpinParticle");
                if (!once)
                {
                    once = true;
                    new Anim(actor.level, origin + offset, "SpinParticleStart");
                    actor.playSound("spin attack");
                }
                if (!spinParticleEnd && animTime / totalAnimTime >= 1)
                {
                    spinParticleEnd = true;
                    new Anim(actor.level, origin + offset, "SpinParticleEnd");
                }
            }

            if (actor.sprite.isAnimOver())
            {
                stateManager.changeState(new LinkIdle(), false);
            }
        }

    }

}

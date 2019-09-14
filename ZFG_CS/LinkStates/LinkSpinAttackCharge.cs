using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkSpinAttackCharge : ActorState
    {
        float chargeTime = 0;
        float particleTime = 0;
        bool particleAlt = false;

        public LinkSpinAttackCharge() : base("LinkSpinAttackCharge", "LinkSpinCharge")
        {
            strafe = true;
        }

        public override void update()
        {
            base.update();
            commonLinkGround();
            chargeTime += Global.spf;

            particleTime += Global.spf;
            if (particleTime > 0.1)
            {
                particleTime = 0;
                particleAlt = !particleAlt;
                Point offset = Point.Zero;
                Point origin = Point.Zero;
                float clampedChargeTime = Helpers.clampMax(chargeTime, 1);
                float chargeDist = 20;
                float shakeOffset = particleAlt ? 0 : 4;
                if (actor.dir == Direction.Up)
                {
                    origin = actor.pos;
                    offset.x = shakeOffset;
                    offset.y = -clampedChargeTime * chargeDist;
                }
                else if (actor.dir == Direction.Down)
                {
                    origin = actor.pos;
                    offset.x = shakeOffset;
                    offset.y = clampedChargeTime * chargeDist;
                }
                else if (actor.dir == Direction.Left)
                {
                    origin = actor.pos;
                    offset.x = -clampedChargeTime * chargeDist;
                    offset.y = shakeOffset;
                }
                else if (actor.dir == Direction.Right)
                {
                    origin = actor.pos;
                    offset.x = clampedChargeTime * chargeDist;
                    offset.y = shakeOffset;
                }
                Anim sparkle = new Anim(actor.level, origin + offset, "SwordSparkle");
                if (chargeTime > 1 && !once)
                {
                    once = true;
                    actor.playSound("spin attack charge");
                    sparkle = new Anim(actor.level, origin + offset, "SpinParticleReady");
                }
            }

            if (!stateManager.input.isHeld(Control.main.Sword))
            {
                if (chargeTime > 1)
                {
                    stateManager.changeState(new LinkSpinAttack(), false);
                }
                else
                {
                    stateManager.changeState(new LinkIdle(), false);
                }
            }
        }

    }

}

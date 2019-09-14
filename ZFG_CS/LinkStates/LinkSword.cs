using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class LinkSword : ActorState
    {
        public LinkSword() : base("LinkSword")
        {
            canCut = true;
        }

        public override void update()
        {
            base.update();
            Character character = getChar();
            if (actor.sprite.frameIndex == 4 && !once)
            {
                once = true;
                Point offset = Point.Zero;
                Point origin = Point.Zero;
                float chargeDist = 30;
                string sparkleSprite = "SwordSwingSparkle";
                if (actor.dir == Direction.Up)
                {
                    origin = actor.pos;
                    offset.y = -chargeDist;
                }
                else if (actor.dir == Direction.Down)
                {
                    origin = actor.pos;
                    offset.y = chargeDist;
                }
                else if (actor.dir == Direction.Left)
                {
                    origin = actor.pos;
                    offset.x = -chargeDist;
                }
                else if (actor.dir == Direction.Right)
                {
                    origin = actor.pos;
                    offset.x = chargeDist;
                }

                if (character.hasItem(Item.sword2))
                {
                    Anim sparkle = new Anim(actor.level, origin + offset, sparkleSprite);
                    sparkle.dirRotation = true;
                    sparkle.dir = actor.dir;
                    if (character.health.isMax())
                    {
                        actor.level.playSound("sword beam", actor.pos);
                        SwordBeam beam = new SwordBeam(actor.level, origin, actor.dir, actor);
                    }
                }
            }

            if (actor.sprite.isAnimOver())
            {
                if (character.hasSword() && stateManager.input.isHeld(Control.main.Sword))
                {
                    stateManager.changeState(new LinkSpinAttackCharge(), false);
                }
                else
                {
                    stateManager.changeState(new LinkIdle(), false);
                }
            }
        }

    }

}

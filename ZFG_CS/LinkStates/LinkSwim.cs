using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class LinkSwim : ActorState
    {
        float swimCooldown = 0;

        public LinkSwim() : base("LinkSwim")
        {
            drawWadable = false;
        }

        public override void update()
        {
            base.update();
            Character character = getChar();
            if (!character.canSwim())
            {
                character.visible = false;
                if (stateTime > 0.125)
                {
                    character.visible = true;
                    character.changePos(stateManager.lastLandPos, false);
                    character.applyDamage(0.5f, Point.Zero, actor, null, false);
                    stateManager.changeState(new LinkIdle(), false);
                }
                return;
            }

            if (swimStrokeTime == 0 && swimCooldown == 0 && stateManager.input.isPressed(Control.main.Action))
            {
                actor.playSound("swim");
                swimStrokeTime = 0.01f;
                actor.sprite.frameIndex = 3;
                actor.sprite.frameTime = 0;
                actor.childFrameTagsToHide.Remove("swimstroke");
            }
            if (swimStrokeTime > 0)
            {
                swimStrokeTime += Global.spf;
                if (swimStrokeTime >= 0.52)
                {
                    swimCooldown = 0.01f;
                    swimStrokeTime = 0;
                    actor.childFrameTagsToHide.Add("swimstroke");
                }
            }
            if (swimCooldown > 0)
            {
                swimCooldown += Global.spf;
                if (swimCooldown > 0.25)
                {
                    swimCooldown = 0;
                }
            }
            commonLinkGround();
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
            actor.vel = Point.Zero;
            Character character = getChar();
            character.stateManager.burnTime = 0;
            if (!character.canSwim())
            {
                //cout << stateManager.lastLandPos.toString() << endl;
                new Anim(actor.level, actor.pos, "SplashSwim");
                return;
            }

            actor.childFrameTagsToHide.Add("swimstroke");
            new Anim(actor.level, actor.pos, "SplashSwim");
            actor.drawShadow = false;
        }

        public override void onExit(ActorState newState)
        {
            base.onExit(newState);
            actor.childFrameTagsToHide.Remove("swimstroke");
            actor.drawShadow = true;
            actor.vel = Point.Zero;
        }

    }

}

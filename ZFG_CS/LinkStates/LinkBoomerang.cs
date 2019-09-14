using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{

    public class LinkBoomerang : ActorState
    {
        bool isMagical = false;

        public LinkBoomerang(bool isMagical) : base("LinkBoomerang")
        {
            this.isMagical = isMagical;
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
        }

        public override void update()
        {
            base.update();
            if (actor.sprite.frameIndex == 2 && !once)
            {
                once = true;
                Character character = actor as Character;
                if (character.boomerang == null)
                {
                    Point move = Point.Zero;
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
                        move.y = -1;
                    }
                    else if (stateManager.input.isHeld(Control.main.Down))
                    {
                        move.y = 1;
                    }
                    if (move.isZero())
                    {
                        move = Helpers.dirToVec(actor.dir);
                    }
                    move.normalize();
                    Boomerang boomerang = new Boomerang(actor.level, actor.pos, Helpers.vecToDir(move), actor, isMagical);
                    boomerang.thrower = character;
                    character.boomerang = boomerang;
                }
            }
            if (actor.sprite.isAnimOver())
            {
                stateManager.changeState(new LinkIdle(), false);
            }
        }

    }
}

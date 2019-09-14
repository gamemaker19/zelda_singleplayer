using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{

    public class LinkPull : ActorState
    {
        public LinkPull() : base("LinkPull", "LinkGrab")
        {
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
            actor.sprite.frameSpeed = 1;
            actor.sprite.wrapMode = "once";
            actor.sprite.frameIndex = 0;
            actor.sprite.frameTime = 0;
        }

        public override void update()
        {
            base.update();
            if (!stateManager.input.isHeld(Control.main.Action))
            {
                stateManager.changeState(new LinkIdle(), false);
            }

            Point move = Point.Zero;
            if (stateManager.input.isHeld(Control.main.Left)) move.x = -1;
            else if (stateManager.input.isHeld(Control.main.Right)) move.x = 1;
            else if (stateManager.input.isHeld(Control.main.Up)) move.y = -1;
            else if (stateManager.input.isHeld(Control.main.Down)) move.y = 1;

            Point dirVec = Helpers.dirToVec(actor.dir);
            if (dirVec != move * -1)
            {
                stateManager.changeState(new LinkGrab(), false);
            }
        }

    }

}

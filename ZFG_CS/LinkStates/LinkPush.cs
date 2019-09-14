using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class LinkPush : ActorState
    {
        CaneBlock caneBlock;
        public LinkPush(CaneBlock caneBlock) : base("LinkPush")
        {
            this.caneBlock = caneBlock;
        }

        public override void update()
        {
            base.update();

            Point move = Point.Zero;
            if (stateManager.input.isHeld(Control.main.Left)) move.x = -1;
            else if (stateManager.input.isHeld(Control.main.Right)) move.x = 1;
            else if (stateManager.input.isHeld(Control.main.Up)) move.y = -1;
            else if (stateManager.input.isHeld(Control.main.Down)) move.y = 1;

            Point dirVec = Helpers.dirToVec(actor.dir);
            if (dirVec != move)
            {
                stateManager.changeState(new LinkGrab(), false);
                return;
            }

            if (caneBlock != null)
            {
                if (checkCaneBlock(move) == null)
                {
                    stateManager.changeState(new LinkIdle(), false);
                    return;
                }
                actor.move(move * 0.5f, true, false);
                caneBlock.move(move * 0.5f, false, false);
            }
        }
    }

}

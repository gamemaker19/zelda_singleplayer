using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkDialog : ActorState
    {
        public LinkDialog() : base("LinkDialog", "LinkIdle")
        {
        }

        public override void update()
        {
            if (Global.game.dialogBox == null)
            {
                stateManager.changeState(new LinkIdle(), false);
            }
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
        }

        public override void onExit(ActorState newState)
        {
            base.onExit(newState);
            Global.game.dialogBox = null;
            Global.game.dialogFrameDelay = 1;
        }

    }

}

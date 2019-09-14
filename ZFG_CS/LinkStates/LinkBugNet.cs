using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkBugNet : ActorState
    {
        public LinkBugNet() : base("LinkBugNet")
        {
        }

        public override void update()
        {
            base.update();
            if (actor.sprite.isAnimOver())
            {
                stateManager.changeState(new LinkIdle(), false);
            }
        }

    }

}

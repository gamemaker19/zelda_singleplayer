using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkThrow : ActorState
    {
        public LinkThrow(Actor throwable) : base("LinkThrow")
        {
            this.throwable = throwable;
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
            if (this.throwable != null)
            {
                this.throwable.throwable.doThrow(actor.dir);
                actor.playSound("throw");
            }
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

using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class LinkCarry : ActorState
    { 
        public LinkCarry(Actor throwable) : base("LinkCarry")
        {
            this.throwable = throwable;
        }

        public override void update()
        {
            base.update();
            if (throwable == null)
            {
                return;
            }

            if (stateManager.input.isPressed(Control.main.Action))
            {
                stateManager.changeState(new LinkThrow(throwable), false);
                return;
            }
            commonLinkGround();
        }

        public override void lateUpdate()
        {
            Point poiOffset = this.actor.sprite.getCurrentFrame().POIs[0];
            poiOffset.x *= actor.getXDir();
            this.throwable.changePos(this.actor.getOffsetPos() + poiOffset, false);
        }

        public override void onExit(ActorState newState)
        {
            base.onExit(newState);
            if (this.throwable != null && newState.name != "LinkThrow" && newState.name != "LinkJump" && newState.name != "LinkScroll")
            {
                this.throwable.throwable.doThrow(actor.dir);
                this.throwable = null;
            }
        }

    }

}

using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkHookshot : ActorState
    {
        public HookshotHook hook = null;
        public LinkHookshot() : base("LinkHookshot")
        {

        }

        public override Actor getProj(Point pos, Direction dir)
        {
            return new HookshotHook(actor.level, pos, dir, actor);
        }

        public override void update()
        {
            base.update();
            Actor proj = projectileCode();
            actor.playLoopingSound("hookshot");
            if (proj != null)
            {
                hook = (proj as HookshotHook);
                hook.character = actor;
            }
            if (hook != null && !hook.reverseDir)
            {
                /*
                if (stateManager.input.isPressed(ALLEGRO_KEY_4))
                {
                    hook.doReverseDir();
                }
                */
            }
            if (hook != null && hook.returnedToUser)
            {
                if (actor.level.isActorInTileWithTag(actor, "water"))
                {
                    stateManager.changeState(new LinkSwim(), false);
                }
                else
                {
                    stateManager.changeState(new LinkIdle(), false);
                }

            }
        }

        public override void onExit(ActorState newState)
        {
            base.onExit(newState);
            Character character = actor.getChar();
            hook.onRemove();
            actor.level.removeActor(hook);
            hook = null;
            actor.isSolid = true;
        }

    }
}

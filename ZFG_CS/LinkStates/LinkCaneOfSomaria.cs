using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkCaneOfSomaria : ActorState
    {
        bool destroyedCaneBlock = false;
        bool createdCaneBlock = false;

        public LinkCaneOfSomaria() : base("LinkCane")
        {

        }

        public override Actor getProj(Point pos, Direction dir)
        {
            CaneBlock caneBlock = new CaneBlock(actor.level, pos, actor);
            Character character = (actor as Character);
            character.caneBlock = caneBlock;
            return caneBlock;
        }

        public override void update()
        {
            base.update();
            Character character = actor as Character;
            if (!destroyedCaneBlock && character.caneBlock == null)
            {
                if (projectileCode() != null)
                {
                    createdCaneBlock = true;
                }
            }
            else if (!createdCaneBlock && !destroyedCaneBlock)
            {
                character.caneBlock.split();
                character.caneBlock = null;
                destroyedCaneBlock = true;
            }

            if (actor.sprite.isAnimOver())
            {
                stateManager.changeState(new LinkIdle(), false);
            }
        }

    }

}

using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class LinkPoke : ActorState
    {
        bool playClingSound = false;

        public LinkPoke(bool playClingSound) : base("LinkPoke")
        {
            this.playClingSound = playClingSound;
        }

        public override Actor getProj(Point pos, Direction dir)
        {
            if (playClingSound) actor.playSound("tink");
            return new Anim(actor.level, pos, "Cling");
        }

        public override void update()
        {
            projectileCode();
            if (actor.sprite.frameIndex == actor.sprite.frames.Count - 1)
            {
                once = false;
            }

            Point move = Point.Zero;
            if (stateManager.input.isHeld(Control.main.Left))
            {
                move.x = -1;
            }
            else if (stateManager.input.isHeld(Control.main.Right))
            {
                move.x = 1;
            }
            else if (stateManager.input.isHeld(Control.main.Up))
            {
                move.y = -1;
            }
            else if (stateManager.input.isHeld(Control.main.Down))
            {
                move.y = 1;
            }
            Point dir = Helpers.dirToVec(actor.dir);
            if (actor.sprite.loopCount < 1) return;

            if (!stateManager.input.isHeld(Control.main.Sword))
            {
                stateManager.changeState(new LinkIdle(), false);
            }
            else if (dir.x != move.x || dir.y != move.y)
            {
                stateManager.changeState(prevState, false);
            }
            else
            {
                var collideDatas = actor.level.getActorCollisions(actor, dir);
                for (int i = collideDatas.Count - 1; i >= 0; i--)
                {
                    if (collideDatas[i].isTrigger)
                    {
                        collideDatas.RemoveAt(i);
                    }
                }
                if (collideDatas.Count == 0)
                {
                    stateManager.changeState(prevState, false);
                }
            }
        }

    }

}

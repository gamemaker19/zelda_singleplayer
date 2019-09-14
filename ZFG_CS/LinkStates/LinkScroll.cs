using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkScroll : ActorState
    {
        Point destPoint;
        Point destViewPoint;
        Point scrollDir;
        float totalScrollDist;
        float totalLinkDist;

        public LinkScroll(Point destPoint, string baseSpriteName) : base("LinkScroll", baseSpriteName)
        {
            this.destPoint = destPoint;
            isInvincible = true;
        }

        public override void onEnter(ActorState oldState)
        {
            base.onEnter(oldState);
            this.throwable = oldState.throwable;
            scrollDir = this.actor.pos.dirTo(destPoint);
            totalScrollDist = (scrollDir.x != 0 ? 256 : 224);
            totalLinkDist = this.actor.pos.distTo(destPoint);
            destViewPoint = actor.level.getCamPos() + (scrollDir * totalScrollDist);
        }

        public override void update()
        {
            float percent = 0.025f;
            Point camPos = actor.level.getCamPos();
            camPos += new Point(scrollDir.x * totalScrollDist * percent, scrollDir.y * totalScrollDist * percent);
            if (actor == Global.game.camCharacter) actor.level.setCamPos(camPos.x, camPos.y);
            if (actor.moveToPos(destPoint, totalLinkDist * percent, false))
            {
                Character character = actor as Character;
                character.checkMusicChange();
                if (character == Global.game.camCharacter) actor.level.setCamPos(destViewPoint.x, destViewPoint.y);
                if (prevState != null)
                {
                    stateManager.changeState(prevState, false);
                }
                else
                {
                    stateManager.changeState(new LinkIdle(), false);
                }
            }
        }

        public override void lateUpdate()
        {
            if (throwable != null)
            {
                Point poiOffset = this.actor.sprite.getCurrentFrame().POIs[0];
                poiOffset.x *= actor.getXDir();
                this.throwable.changePos(this.actor.getOffsetPos() + poiOffset, false);
            }
        }

    }

}

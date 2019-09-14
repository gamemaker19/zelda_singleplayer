using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Bomb : Actor
    {
        float timeBeforeFlash = 1.5f;
        Actor owner = null;
        
        public Bomb(Level level, Point pos, Actor owner) : base(level, pos, "BombFlash")
        {
            name = "Bomb";
            sprite.frameSpeed = 0;
            drawShadow = true;
            zIndex = (int)ZIndex.Link + 1;
            throwable = new Throwable(this, "", "", false, 0, "");
            throwable.bounce = true;
            throwable.zVel = 1;
            throwable.speed = 1.5f;
            drawWadeSprite = true;
            checkWadables = true;
            checkTriggers = true;
            checkWadables = true;
            this.owner = owner;
        }

        public override void update()
        {
            base.update();
            if (time >= timeBeforeFlash)
            {
                sprite.frameSpeed = 1;
            }
            if (sprite.isAnimOver())
            {
                playSound("bomb explode");
                Anim explosion = new Anim(level, pos, "BombExplosion");
                explosion.name = "BombExplosion";
                explosion.isSolid = true;
                explosion.checkTriggers = true;
                explosion.isStatic = false;
                Damager damager = new Damager(owner, Item.bombs, 1);
                damager.canHurtOwner = true;
                damager.isConcussive = true;
                explosion.getMainCollider(true).damager = damager;
                if (throwable.lifter != null)
                {
                    throwable.lifter.stateManager.actorState.throwable = null;
                }
                level.removeActor(this);
            }
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Anim : Actor
    {
        public bool fade = false;
        public bool destroyOnAnimDone = false;

        public Anim(Level level, Point pos, string spriteName, bool destroyOnAnimDone = true) : base(level, pos, spriteName)
        {
            zIndex = (int)ZIndex.Link + 1;
            this.isSolid = false;
            this.checkTriggers = false;
            this.isStatic = true;
            this.destroyOnAnimDone = destroyOnAnimDone;
        }

        public Anim(Level level, Point pos, string spriteName, Damager damager, bool destroyOnAnimDone = true) : this(level, pos, spriteName, destroyOnAnimDone)
        {
            setDamager(damager);
            isSolid = true;
            checkTriggers = true;
            isStatic = false;
        }

        public override void update()
        {
            base.update();
            if (fade)
            {
                alpha = 1 - (sprite.animTime / sprite.getAnimLength());
            }
            if (sprite.isAnimOver() && destroyOnAnimDone)
            {
                level.removeActor(this);
            }
        }
    }

    public class ParticleTwilight : Actor
    {
        public ParticleTwilight(Level level, Point pos) : base(level, pos, "ParticleTwilight")
        {
            vel.y = -0.25f;
        }

        public override void update()
        {
            base.update();
            alpha = 1 - time;
            if (alpha <= 0)
            {
                remove();
            }
        }
    }
}

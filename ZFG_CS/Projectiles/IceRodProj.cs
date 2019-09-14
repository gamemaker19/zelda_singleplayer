using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    class IceRodProj : Actor
    {
        public IceRodProj(Level level, Point pos, Direction dir, Actor owner) : base(level, pos, "IceRodProj")
        {
            projectile = new Projectile(this, owner, Item.icerod, dir, 1.5f, "IceRodHit", false, true, 0, "");
            getDamager().freeze = true;
            Anim trail = new Anim(level, pos, "IceRodProj");
            projectile.reflectable = true;
        }

        public override void update()
        {
            base.update();
            if (time > 0.2)
            {
                time = 0;
                Anim trail = new Anim(level, pos, "IceRodProj");
            }
        }
    }
}

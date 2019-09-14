using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class FireRodProj : Actor
    {
        public FireRodProj(Level level, Point pos, Direction dir, Actor owner) : base(level, pos, "FireRodProj")
        {
            projectile = new Projectile(this, owner, Item.firerod, dir, 3, "FireRodFlame", false, true, 1, "fire");
            getDamager().burn = true;
            projectile.reflectable = true;
        }
    }
}

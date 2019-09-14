using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class CaneProj : Actor
    {
        public CaneProj(Level level, Point pos, Direction dir, Actor owner) : base(level, pos, "SomariaProj")
        {
            projectile = new Projectile(this, owner, Item.caneOfSomaria, dir, 3, "SwordBeamBreak", true, true, 0.5f, "");
            projectile.reflectable = true;
        }
    }
}

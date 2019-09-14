using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    class MagicPowderProj : Anim
    {
        public MagicPowderProj(Level level, Actor owner, Point pos) : base(level, pos, "MagicPowderSpray")
        {
            isSolid = true;
            Damager damager = new Damager(owner, Item.powder, 0);
            damager.bunnify = true;
            setDamager(damager);
        }
    }
}

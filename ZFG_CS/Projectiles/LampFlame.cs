using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    class LampFlame : Anim
    {
        public LampFlame(Level level, Actor owner, Point pos) : base(level, pos, "LampFlame")
        {
            Damager damager = new Damager(owner, Item.lamp, 0.25f);
            damager.burn = true;
            setDamager(damager);
            isSolid = true;
        }
    }
}

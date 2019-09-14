using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class RockPile : Actor
    {
        public bool dashed = false;
        public bool hasBase = false;
        public bool revealSound = false;
        public RockPile(Level level, Point pos, string baseSpriteName, bool hasBase) : base(level, pos, baseSpriteName)
        {
            this.hasBase = hasBase;
        }

        public override void onCollision(CollideData other)
        {
            base.onCollision(other);
        }

        public void onDash()
        {
            level.removeActor(this);
            if (hasBase)
            {
                Actor baseActor = new Actor(level, pos, "RockBigBase");
            }
            for (int i = 0; i < 4; i++)
            {
                Point offset = Point.Zero;
                if (i == 0) offset = new Point(-4, -4);
                if (i == 1) offset = new Point(-4, 4);
                if (i == 2) offset = new Point(4, -4);
                if (i == 3) offset = new Point(4, 4);
                Anim anim = new Anim(level, pos + offset, "PotBreak");
            }
            if (revealSound)
            {
                playSound("secret");
            }
        }
    }
}

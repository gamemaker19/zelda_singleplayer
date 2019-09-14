using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Stake : Actor
    {
        public bool pounded = false;

        public Stake(Level level, Point pos) : base(level, pos, "HammerStake")
        {
            name = "Stake";
            sprite.frameSpeed = 0;
        }

        public void pound()
        {
            pounded = true;
            sprite.frameIndex = 1;
            playSound("hammer pound");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Chest : Actor
    {
        public bool opened = false;
        public bool isBig = false;
        public Item itemRequired;
        public bool outOfReach = false;
        
        public Chest(Level level, Point pos, bool isBig) : base(level, pos, isBig? "ChestBig" : "ChestSmall")
        {
            sprite.frameSpeed = 0;
            this.isBig = isBig;
            hookable = true;
            name = "Chest";
            isStatic = true;
            checkTriggers = false;
        }

        public void open()
        {
            opened = true;
            playSound("chest open");
            sprite.frameIndex = 1;
        }

    }
}

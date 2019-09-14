using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Fairy : Actor
    {
        Point dest;
        Point origin;

        public Fairy(Level level, Point pos) : base(level, pos, "Fairy")
        {
            name = "Fairy";
            collectable = new Collectable(this, false);
            collectable.healthGain = 7;
            collectable.shouldFade = false;
            z = 8;
            useGravity = false;
            origin = pos;
            canCatchInNet = true;
            getNextMovePos();
            checkTriggers = false;
            checkWadables = false;
        }

        public override void update()
        {
            base.update();
            if (moveToPos(dest, 1, false))
            {
                getNextMovePos();
            }
        }

        public void getNextMovePos()
        {
            int startX = -20;
            int endX = 20;
            int startY = -20;
            int endY = 20;

            if (pos.x - origin.x > 20) endX = 0;
            else if (pos.x - origin.x < -20) startX = 0;
            if (pos.y - origin.y > 20) endY = 0;
            else if (pos.y - origin.y < -20) startY = 0;

            //int randX = (startX + endX) / 2;
            //int randY = (startY + endY) / 2;

            int randX = Helpers.randomRange(startX, endX);
            int randY = Helpers.randomRange(startY, endY);

            dest.x = pos.x + randX;
            dest.y = pos.y + randY;

            if (dest.x < pos.x) changeDir(Direction.Left);
            else if (dest.x >= pos.x) changeDir(Direction.Right);

        }
    }
}

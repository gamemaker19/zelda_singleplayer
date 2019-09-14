using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class HealSparkle : Actor
    {
        public float stateTime = 0;
        public Character targetChar;
        public HealSparkle(Level level, Point pos) : base(level, pos, "Empty")
        {
            this.level = level;
            this.pos = pos;
        }

        public override void update()
        {
            base.update();

            if (targetChar != null)
            {
                if (targetChar.getState() == "LinkDie" || targetChar.isDeleted())
                {
                    remove();
                    return;
                }
                if (moveToPos(targetChar.pos, 1.5f, false) && stateTime == 0)
                {
                    var character = targetChar.getChar();
                    character.health.fillMax();
                    character.magic.fillMax();
                    stateTime = 0.01f;
                }
            }

            if (stateTime > 0)
            {
                stateTime += Global.spf;
                if (stateTime > 2)
                {
                    remove();
                    return;
                }
            }

            Point randPos = new Point(Helpers.randomRange(-15, 15), Helpers.randomRange(-15, 15));
            new Anim(level, pos + randPos, "SwordSparkle");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Door : Actor
    {
        bool opened = false;
        public Door(Level level, Point pos, string baseSpriteName) : base(level, pos, baseSpriteName)
        {
            sprite.frameSpeed = 0;
        }

        public override void update()
        {
            base.update();
            if (opened && sprite.isAnimOver())
            {
                level.removeActor(this);
            }
        }

        public override void onCollision(CollideData other)
        {
            base.onCollision(other);
            if (!opened && other.collidedActor.isLink() && other.collidedActor.dir == Direction.Up)
            {
                open();
            }
        }

        public void open()
        {
            if (opened) return;
            opened = true;
            sprite.frameSpeed = 1;
            playSound("door open");
        }

    }
}

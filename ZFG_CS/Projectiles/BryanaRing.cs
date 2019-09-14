using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class BryanaRing : Actor
    {
        public Actor owner;
        public float shaderSwapTime = 0;
        public BryanaRing(Level level, Actor owner, Point pos) : base(level, pos, "BryanaRing")
        {
            isSolid = true;
            zIndex = (int)ZIndex.Link + 1;
            angle = 270;
            setDamager(new Damager(owner, Item.caneOfBryana, 0.5f));
        }

        public override void update()
        {
            base.update();
            angle += Global.spf * 1000;
            //if (time > 0.1) isSolid = true;
            shaderSwapTime += Global.spf;
            if (shaderSwapTime > 0.1)
            {
                shaderSwapTime = 0;
                if (shader == null) shader = Global.shaders["replaceColorSpin"];
                else shader = null; ;
            }
        }

        public override void onCollision(CollideData other)
        {
            base.onCollision(other);
            /*
            if (other.collidedTile)
            {
                onHit();
            }
            */
        }
    }
}

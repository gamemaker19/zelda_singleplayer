using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    class SwordBeam : Actor
    {
        float shaderSwapTime = 0;
        public SwordBeam(Level level, Point pos, Direction dir, Actor owner) : base(level, pos, "SwordBeam")
        {
            projectile = new Projectile(this, owner, Item.sword2, dir, 4, "SwordBeamBreak", false, true, 3, "");
            projectile.raycastTiles = true;
            projectile.raycastActors = false;
            projectile.reflectable = true;
        }

        public override void update()
        {
            base.update();
            angle += Global.spf * 2600;
            if (time > 0.1) isSolid = true;
            if (time > 0.1)
            {
                getMainCollider(true).damager.damage = 0.5f;
            }
            shaderSwapTime += Global.spf;
            if (shaderSwapTime > 0.1)
            {
                shaderSwapTime = 0;
                if (shader == null) shader = Global.shaders["replaceColorSpin"];
                shader = null;
            }
        }
    }
}

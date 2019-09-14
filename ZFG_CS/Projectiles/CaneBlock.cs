using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class CaneBlock : Actor
    {
        public Actor owner;
        public int health = 4;
        public CaneBlock(Level level, Point pos, Actor actor) : base(level, pos, "SomariaBlock")
        {
            name = "CaneBlock";
            owner = actor;
            throwable = new Throwable(this, "", "", false, 0.5f, "");
            throwable.bounce = true;
            throwable.zVel = 1;
            throwable.speed = 1.5f;
            throwable.drawShadowOnThrow = false;
            throwable.itemRequired = Item.powerGlove;
            Damager damager = new Damager(owner, null, 0.5f);
            damager.actor = this;
            checkWadables = true;
            isStatic = false;
            setDamager(damager);
        }

        public override void update()
        {
            base.update();
            if (health <= 0)
            {
                onBreak();
            }
        }

        public void split()
        {
            Anim caneBreak = new Anim(level, pos, "SomariaSpawn");

            CaneProj proj1 = new CaneProj(level, pos, Direction.Left, owner);
            CaneProj proj2 = new CaneProj(level, pos, Direction.Right, owner);
            CaneProj proj3 = new CaneProj(level, pos, Direction.Up, owner);
            CaneProj proj4 = new CaneProj(level, pos, Direction.Down, owner);

            level.removeActor(this);
        }

        public void onBreak()
        {
            Anim caneBreak = new Anim(level, pos, "SomariaBreak");
            owner.getChar().caneBlock = null;
            level.removeActor(this);
        }

        public void deductHealth(int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                onBreak();
            }
        }

        public override void onCollision(CollideData other)
        {
            base.onCollision(other);
            if (other.collider.damager != null && other.collider.damager.owner != owner)
            {
                deductHealth((int)other.collider.damager.damage);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class LinkQuake : ActorState
    {
        Actor quakeLightning = null;

        public LinkQuake() : base("LinkQuake")
        {
            superArmor = true;
        }

        public override void update()
        {
            base.update();
            if (actor.sprite.isAnimOver())
            {
                if (quakeLightning == null)
                {
                    foreach (var character in Global.game.characters)
                    {
                        if (character != actor && character.level == actor.level && character.pos.distTo(actor.pos) < 128)
                        {
                            Damager damager = new Damager(actor, Item.quake, 0);
                            damager.bunnify = true;
                            character.applyDamage(damager, Point.Zero);
                        }
                    }
                    actor.playSound("ram");
                    actor.playSound("quake 1");
                    quakeLightning = new Anim(actor.level, actor.getScreenPos().addxy(-13, -6), "QuakeLightning", false);
                    if (actor == Global.game.camCharacter) actor.level.shake(0, 0.25f, false, 100);
                }
            }
            if (quakeLightning != null && quakeLightning.sprite.isAnimOver())
            {
                stateManager.changeState(new LinkIdle(), false);
            }
        }

        public override void onExit(ActorState newState)
        {
            base.onExit(newState);
            if (actor == Global.game.camCharacter) actor.level.shakeTime = 0.1f;
            if (quakeLightning != null) quakeLightning.remove();
        }
    }
}

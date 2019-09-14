using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class StateManager
    {
        public Actor actor;
        public Actor bryanaRing = null;
        public float hurtTime = 0;
        public float hurtFlashTime = 0;
        public bool changedStateInFrame;
        public float runSpeed = 1;
        public float bunnyTime = 0;
        public float burnTime = 0;
        public bool invisible = false;
        public Actor burner;
        public Item burnItem;
        public float burnDamageCooldown = 0;
        public Point lastLandPos;
        public ActorState actorState;
        public Input input;
        public bool invertUp = false;

        public StateManager(Actor actor, Input input)
        {
            this.actor = actor;
            this.input = input;
        }

        public void preUpdate()
        {
            changedStateInFrame = false;
        }

        public void update()
        {
            actorState.update();

            Character character = actor as Character;
            if (bryanaRing != null)
            {
                bryanaRing.changePos(actor.getScreenPos(), false);
                character.magic.deduct(Global.spf * 8);
                if (character.magic.value <= 0)
                {
                    bryanaRing.level.removeActor(bryanaRing);
                    bryanaRing = null;
                }
                character.playLoopingSound("zol");
            }

            if (invisible)
            {
                character.magic.deduct(Global.spf * 8);
                if (character.magic.value <= 0)
                {
                    invisible = false;
                    new Anim(actor.level, actor.pos, "CapePoof");
                    actor.playSound("cape off");
                }
            }

            if (hurtTime > 0)
            {
                hurtTime -= Global.spf;
                hurtFlashTime += Global.spf;
                if (hurtFlashTime >= 0.04)
                {
                    hurtFlashTime = 0;
                    actor.visible = !actor.visible;
                }
                if (hurtTime <= 0)
                {
                    hurtTime = 0;
                    actor.visible = true;
                }
            }

            if (burnTime > 0)
            {
                burnTime -= Global.spf;
                burnDamageCooldown += Global.spf;
                if (burnDamageCooldown > 2)
                {
                    burnDamageCooldown = 0;
                    actor.getChar().applyDamage(0.25f, Point.Zero, burner, burnItem, false, false);
                }
                if (burnTime <= 0)
                {
                    burnDamageCooldown = 0;
                    burnTime = 0;
                }
            }

            if (bunnyTime > 0)
            {
                bunnyTime -= Global.spf;
                if (bunnyTime <= 0)
                {
                    new Anim(actor.level, actor.pos, "CapePoof");
                    actor.playSound("cape off");
                    bunnyTime = 0;
                }
            }
        }

        public void applyBurn(Actor burner, Item burnItem)
        {
            if (burnTime > 0) return;

            this.burner = burner;
            this.burnItem = burnItem;
            burnTime = 8;
        }

        public void applyBunny(float time)
        {
            if (actor.getChar().hasItem(Item.moonPearl)) return;
            if (actor.getState() == "LinkSwim") return;
            if (actor.getState() == "LinkLift" || actor.getState() == "LinkCarry")
            {
                changeState(new LinkIdle(), true);
            }

            if (bunnyTime == 0)
            {
                new Anim(actor.level, actor.pos, "CapePoof");
                actor.playSound("cape on");
            }
            bunnyTime = time;
        }

        public void lateUpdate()
        {
            actorState.lateUpdate();
        }

        public void changeState(ActorState newState, bool forceChange)
        {
            if (actorState != null && actorState.name == newState.name) return;
            if (changedStateInFrame && !forceChange) return;
            changedStateInFrame = true;
            newState.actor = this.actor;
            newState.stateManager = this;

            actor.changeSprite(newState.baseSpriteName);

            ActorState oldState = actorState;
            actorState = newState;

            if (oldState != null)
            {
                oldState.prevState = null;
                newState.prevState = oldState;
                oldState.onExit(newState);
                newState.onEnter(oldState);
            }
        }
    }
}

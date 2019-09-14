using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{

    public class LinkDie : ActorState
    {
        float zDist = 0;
        int zDir = 1;
        float fairySoundTime = 0;
        Direction lastDir;
        Actor fairy;
        Point fairyDestPos1;
        Point fairyDestPos2;
        Actor enemyExplosion;
        Character killer;
        bool fairyRevive = false;

        public LinkDie(Character killer) : base("LinkDie")
        {
            this.isInvincible = true;
            this.killer = killer;
            aiTarget = false;
        }

        public override void onEnter(ActorState oldState)
        {
            Character character = getChar();

            stateManager.burnTime = 0;
            stateManager.bunnyTime = 0;

            if (stateManager.bryanaRing != null)
            {
                stateManager.bryanaRing.remove();
                stateManager.bryanaRing = null;
            }

            if (oldState.name == "LinkSwim" || !character.hasItem(Item.bottledFairy))
            {
                if (character.skin == "homer")
                {
                    actor.playSound("homer scream");
                }
                enemyExplosion = new Anim(actor.level, actor.pos, "EnemyExplosion", false);
                actor.changeSprite("LinkHurt");
                foreach (var item in character.items)
                {
                    if (item != null)
                    {
                        FieldItem fieldItem = new FieldItem(actor.level, actor.pos, item, true);
                        float angle = Helpers.randomRange(0, 360);
                        fieldItem.vel.x = Mathf.Cos(angle);
                        fieldItem.vel.y = Mathf.Sin(angle);
                        if (item.item == Item.sword2)
                        {
                            Global.game.masterSwordChar = null;
                            Global.game.masterSword = fieldItem;
                        }
                    }
                }
                if (character.rupees.value > 0)
                {
                    Actor pickup = WorldObjectFactories.createRupeeGreen(actor.level, actor.pos, true, (int)character.rupees.value);
                    pickup.collectable.delay = 1;
                    float angle = Helpers.randomRange(0, 360);
                    pickup.vel.x = Mathf.Cos(angle);
                    pickup.vel.y = Mathf.Sin(angle);
                    pickup.sprite.frameSpeed = 0;
                    pickup.collectable.shouldFade = false;
                }
                if (character.arrows.value > 0)
                {
                    Actor pickup = WorldObjectFactories.createArrowPickup(actor.level, actor.pos, (int)character.arrows.value, true);
                    float angle = Helpers.randomRange(0, 360);
                    pickup.vel.x = Mathf.Cos(angle);
                    pickup.vel.y = Mathf.Sin(angle);
                    pickup.collectable.delay = 1;
                    pickup.collectable.shouldFade = false;
                }
                actor.playSound("enemy dies");

                Global.game.remainingCharacters--;
                Global.game.characters.Remove(character);
                if (Global.game.remainingCharacters == 1)
                {
                    Character finalChar = Global.game.characters.First();
                    finalChar.won = true;
                }

                for (int i = 0; i < 5; i++)
                {
                    character.items[i] = null;
                }
                character.rupees.value = 0;
                character.arrows.value = 0;
                if (Global.game.character == character)
                {
                    int place = Global.game.remainingCharacters;
                    string placeStr = place.ToString();
                    if (place > 3) placeStr += "th";
                    if (place == 3) placeStr += "rd";
                    if (place == 2) placeStr += "nd";
                    Global.game.setCurrentMessage("You placed " + placeStr + "\nPress Enter to leave match", 10);
                    Global.game.enterGoesToMainMenu = true;
                }
            }
            else
            {
                lastDir = actor.dir;
                actor.dir = Direction.Right;
                stateManager.hurtTime = 1;
                fairyRevive = true;
                actor.playSound("link dies");
            }
        }

        public override void update()
        {
            base.update();
            Character character = getChar();
            if (!fairyRevive)
            {
                if (state == 0)
                {
                    if (enemyExplosion.sprite.frameIndex >= 6)
                    {
                        //Set another AI as always active
                        if (character.aiStateManager != null && character.aiStateManager.alwaysActive)
                        {
                            foreach (var c in Global.game.characters)
                            {
                                if (c.aiStateManager == null || c.aiStateManager.alwaysActive) continue;
                                c.aiStateManager.alwaysActive = true;
                                break;
                            }
                        }
                        enemyExplosion.level.removeActor(enemyExplosion);
                        character.level.removeActor(character);

                        if (Global.game.remainingCharacters == 1)
                        {
                            Character finalChar = Global.game.characters.First();
                            finalChar.stateManager.changeState(new LinkWin(), true);
                        }

                        stateTime = 0;
                        state = 1;
                    }
                }
                else if (state == 1)
                {
                    if (stateTime > 5)
                    {
                        state = 2;
                        if (character != Global.game.character) return;
                        if (killer != null)
                        {
                            Global.game.camCharacter = killer;
                        }
                        else
                        {
                            int randIndex = Helpers.randomRange(0, Global.game.characters.Count - 1);
                            int i = 0;
                            foreach (var c in Global.game.characters)
                            {
                                if (randIndex == i)
                                {
                                    Global.game.camCharacter = c;
                                    break;
                                }
                                i++;
                            }
                        }
                    }
                }
                else if(state == 2)
                {
                    if (character == Global.game.character && Global.input.isPressed(Key.Enter))
                    {
                        Global.goToMainMenu(false);
                    }
                }
            }
            else
            {
                if (state == 0)
                {
                    if (stateTime > 2.5)
                    {
                        state = 1;
                        fairy = new Actor(actor.level, actor.pos, "Fairy");
                        fairyDestPos1 = actor.pos.addxy(0, -20);
                        for (int i = 0; i < character.items.Count; i++)
                        {
                            if (character.items[i] != null && character.items[i].item == Item.bottledFairy)
                            {
                                character.items[i].item = Item.emptyBottle;
                                break;
                            }
                        }
                    }
                }
                if (state == 1)
                {
                    if (fairy.moveToPos(fairyDestPos1, 0.5f, false))
                    {
                        state = 2;
                        fairy.changeSprite("FairyHeal");
                    }
                }
                else if (state == 2)
                {
                    fairy.pos.y = fairyDestPos1.y + Mathf.Sin(stateTime * 100) * 1;
                    if (fairySoundTime == 0)
                    {
                        actor.playSound("fairy");
                        fairySoundTime += Global.spf;
                    }
                    else
                    {
                        fairySoundTime += Global.spf;
                        if (fairySoundTime > 1.25)
                        {
                            fairySoundTime = 0;
                        }
                    }
                    if (fairy.sprite.loopCount >= 2)
                    {
                        state = 3;
                        fairy.changeSprite("Fairy");
                        fairy.accelToVel = new Point(1.25f, -1.25f);
                        character.health.add(7);
                    }
                }
                else if (state == 3)
                {
                    fairy.alpha -= Global.spf;
                    character.useGravity = false;
                    float zInc = 0.2f * zDir;
                    character.z += zInc;
                    if (zInc > 12)
                    {
                        zInc = 0;
                        zDir = -1;
                    }
                    if (!character.health.isChanging())
                    {
                        fairy.level.removeActor(fairy);
                        actor.dir = lastDir;
                        character.useGravity = true;
                        character.z = 0;
                        stateManager.hurtTime = 1;
                        Global.game.killFeed.Add(new KillFeedEntry(character.playerName + " was revived"));
                        stateManager.changeState(new LinkIdle(), false);
                    }
                }
            }
        }

    }

}

using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class AIStateManager
    {
        public float attackCooldown = 0;
        public StateManager stateManager;
        public AIState aiState;
        public AIState decideState;
        public bool decided = false;
        public bool couldNotDecide = false;
        public bool goToMasterSword = false;
        public bool masterSwordChosen = false; //The "chosen" will always go to the master sword immediately upon drop
        public bool alwaysActive = false;
        public float liftCooldown = 0;
        public float fakeItemGetTime = 0;
        public float fakeKillTime = 0;
        public Character character;
        public Character target;
        public HashSet<Entrance> entrancesUsed = new HashSet<Entrance>();
        public Entrance currentExit;
        public Actor actor;
        public bool once = false;
        public bool isAI = false;
        public float decideTimer = 0;
        public float aiTimer = 0;

        public AIStateManager(Actor actor)
        {
	        this.actor = actor;
	        this.stateManager = actor.stateManager;
	        this.aiState = new AIState(this, "AIState");
	        this.character = actor.getChar();
	        if (Global.game.numCPUsActive > 0)
	        {
		        alwaysActive = true;
		        Global.game.numCPUsActive--;
		        masterSwordChosen = true;
	        }
            fakeItemGetTime = Helpers.randomRange(15, 50);
	        fakeKillTime = Helpers.randomRange(30, 80);
        }

        public bool hasWeapon()
        {
            return bestWeapon() != null;
        }

        public Item bestWeapon()
        {
            if (Global.debugCharMovement) return null;
            if (character.hasItem(Item.silverBow) && character.arrows.value > 0) return Item.silverBow;
            if (character.hasItem(Item.bombos) && character.magic.value >= 32) return Item.bombos;
            if (character.hasItem(Item.quake) && character.magic.value >= 32) return Item.quake;
            if (character.hasItem(Item.ether) && character.magic.value >= 32) return Item.ether;
            if (character.hasItem(Item.sword2)) return Item.sword2;
            if (character.hasItem(Item.sword4)) return Item.sword4;
            if (character.hasItem(Item.caneOfBryana) && character.magic.value >= 8) return Item.caneOfBryana;
            if (character.hasItem(Item.caneOfSomaria) && character.magic.value >= 8) return Item.caneOfSomaria;
            if (character.hasItem(Item.firerod) && character.magic.value >= 16) return Item.firerod;
            if (character.hasItem(Item.icerod) && character.magic.value >= 16) return Item.icerod;
            if (character.hasItem(Item.bow) && character.arrows.value > 0) return Item.bow;
            if (character.hasItem(Item.sword3)) return Item.sword3;
            if (character.hasItem(Item.hammer)) return Item.hammer;
            if (character.hasItem(Item.sword1)) return Item.sword1;
            if (character.hasItem(Item.lamp) && character.magic.value >= 4) return Item.lamp;
            return null;
        }

        public bool canTarget(Character target)
        {
            return !target.isDeleted() && target.level == actor.level && target.stateManager.actorState.aiTarget && hasWeapon();
        }

        public void doFakeItemGet()
        {
            if (!character.hasEmptySlot()) return;

            float closestDist = 1000000;
            Chest closest = null;
            foreach (var chest in Global.game.chests)
            {
                if (chest.level != actor.level) continue;
                if (chest.opened) continue;
                if (chest.outOfReach) continue;
                if (chest.itemRequired != null && !character.hasItem(chest.itemRequired)) continue;
                if (actor.level.isIndoor) continue;
                if (Global.game.camCharacter != null && chest.pos.distTo(Global.game.camCharacter.pos) < 512) continue;

                float dist = chest.pos.distTo(actor.pos);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = chest;
                }
            }
            if (closest != null)
            {
                closest.open();
                Item randomItem = Item.getRandomItem();
                InventoryItem inventoryItem = new InventoryItem(randomItem);
                character.addItem(inventoryItem, character.getEmptySlot());
            }
            //cout << "FAKE ITEM GET" << endl;
        }

        public void doFakeKill()
        {
            foreach (Character enemy in Global.game.characters)
            {
                if (enemy == character) continue;
                if (enemy.aiStateManager != null && enemy.aiStateManager.alwaysActive) continue;
                if (enemy.health.value <= 0) continue;
                if (enemy.isInvincible()) continue;
                if (enemy == Global.game.camCharacter) continue;
                if (enemy.level.isIndoor) continue;
                if (Global.game.camCharacter != null && enemy.overworldPos.distTo(Global.game.camCharacter.overworldPos) < 512) continue;
                Item randomItem = null;
                for (int i = 0; i < 5; i++)
                {
                    if (character.items[i] != null)
                    {
                        if (!character.items[i].item.isWeapon)
                        {
                            continue;
                        }
                        randomItem = character.items[i].item;
                        break;
                    }
                }
                if (randomItem == null) return;
                Damager fakeDamager = new Damager(character, randomItem, 1000);
                enemy.applyDamage(fakeDamager, new Point());
                break;
            }
	        //cout << "FAKE KILL" << endl;
        }

        public void update()
        {
            if (character.getState() == "LinkFreeze" || character.getState() == "LinkStun" || character.getState() == "LinkHurt" || character.getState() == "LinkDie")
            {
                return;
            }

            //stateManager.input.keyHeld[Key.Down] = true;
            //return;

            if (liftCooldown > 0)
            {
                liftCooldown += Global.spf;
                if (liftCooldown > 1) liftCooldown = 0;
            }

            bool isOutOfScreen = Global.game.camCharacter == null || actor.overworldPos.distTo(Global.game.camCharacter.overworldPos) > 200;
            if (!alwaysActive && isOutOfScreen)
            {
                fakeItemGetTime -= Global.spf;
                fakeKillTime -= Global.spf;
                if (fakeItemGetTime <= 0)
                {
                    doFakeItemGet();
                    fakeItemGetTime = Helpers.randomRange(5, 30);
                }
                if (fakeKillTime <= 0)
                {
                    doFakeKill();
                    fakeKillTime = Helpers.randomRange(60, 300);
                }
                return;
            }

            /*
	        if (!alwaysActive)
	        {
		        if (!Global.game.camCharacter || actor.overworldPos.distTo(Global.game.camCharacter.overworldPos) > 200)
		        {
			        //aiTimer += Global.spf;
			        //if (aiTimer < 1) return;
		        }
		        aiTimer = 0;
	        }
	        */

            if (character.getState() == "LinkHurt" || character.getState() == "LinkDie")
            {
                decided = false;
                return;
            }

            if (aiState.findTargets)
            {
                findTarget();
            }
            else
            {
                target = null;
            }
            if (target == null || !decided)
            {
                decide();
            }

            bool ignoreUpdate = false;

            if (target != null)
            {
                Item weapon = bestWeapon();
                if (!weapon.isMelee)
                {
                    Direction dir = actor.getFaceDir(target.pos);
                    if (dir == Direction.Left || dir == Direction.Right)
                    {
                        float yDist = actor.getColliderPos().y - target.getColliderPos().y;
                        if (yDist < -1)
                        {
                            stateManager.input.keyHeld[Control.main.Down] = true;
                        }
                        if (yDist > 1)
                        {
                            stateManager.input.keyHeld[Control.main.Up] = true;
                        }
                    }
                    else if (dir == Direction.Up || dir == Direction.Down)
                    {
                        float xDist = actor.getColliderPos().x - target.getColliderPos().x;
                        if (xDist < -1)
                        {
                            stateManager.input.keyHeld[Control.main.Right] = true;
                        }
                        if (xDist > 1)
                        {
                            stateManager.input.keyHeld[Control.main.Left] = true;
                        }
                    }
                    if (actor.pos.distTo(target.pos) < 64)
                    {
                        ignoreUpdate = true;
                    }
                    attack();
                }

                if (weapon.isMelee)
                {
                    float dist = actor.pos.distTo(target.pos);
                    if (dist < 24)
                    {
                        ignoreUpdate = true;
                        attack();
                    }
                }

                if (attackCooldown > 0)
                {
                    attackCooldown += Global.spf;
                    if (attackCooldown > 1)
                    {
                        attackCooldown = 0;
                    }
                }
            }
            if (!ignoreUpdate)
            {
                aiState.update();
            }

        }

        void attack()
        {
            if (attackCooldown > 0) return;
            attackCooldown = 0.01f;
            Item weapon = bestWeapon();
            actor.facePos(target.pos);
            if (weapon.isSword) stateManager.input.keyPressed[Control.main.Sword] = true;
            else
            {
                int slot = character.getItemSlot(weapon);
                if (slot == 0) stateManager.input.keyPressed[Control.main.Item1] = true;
                if (slot == 1) stateManager.input.keyPressed[Control.main.Item2] = true;
                if (slot == 2) stateManager.input.keyPressed[Control.main.Item3] = true;
                if (slot == 3) stateManager.input.keyPressed[Control.main.Item4] = true;
                if (slot == 4) stateManager.input.keyPressed[Control.main.Item5] = true;
            }
        }

        void findTarget()
        {
            if (target != null)
            {
                if (!canTarget(target))
                {
                    target = null;
                }
            }
            if (target == null)
            {
                float closestDist = 1000000;
                Character closest = null;
                foreach (var character in Global.game.characters)
                {
                    if (!canTarget(character)) continue;
                    if (actor == character) continue;
                    float dist = character.pos.distTo(actor.pos);
                    if (dist > 128) continue;
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closest = character;
                    }

                }
                target = closest;
            }
            if (target != null && Global.frameCount % 15 == 0)
            {
                var prospectiveAIState = new MoveToPos(this, target.getColliderPos(), true);
                if (prospectiveAIState.path.Count == 0)
                {
                    target = null;
                }
                else
                {
                    aiState = prospectiveAIState;
                }
            }
        }

        //#pragma optimize( "", off )
        void decide()
        {
            if (decided)
            {
                return;
            }

            if (!alwaysActive)
            {
                if (Global.game.camCharacter == null || actor.overworldPos.distTo(Global.game.camCharacter.overworldPos) > 150)
                {
                    decideTimer += Global.spf;
                    if (decideTimer < 5) return;
                }
            }

            if (!decided)
            {
                decided = true;
                decideTimer = 0;
            }

            if (!actor.level.isIndoor && character.pos.distTo(Global.game.currentStormCenter) > Global.game.currentStormRadius - 250 && Global.game.stormPhase >= 2)
            {
                //Try to find chest closest to center
                float closestDist2 = 1000000;
                Actor closest2 = null;
                foreach (var chest in Global.game.chests)
                {
                    if (chest.level != actor.level) continue;
                    if (chest.outOfReach) continue;
                    if (chest.itemRequired != null && !character.hasItem(chest.itemRequired)) continue;
                    if (chest.inStorm()) continue;

                    float dist = chest.pos.distTo(Global.game.currentStormCenter);

                    if (dist < closestDist2)
                    {
                        closestDist2 = dist;
                        closest2 = chest;
                    }
                }
                //cout << "TRIED TO DODGE STORM" << endl;
                if (closest2 != null)
                {
                    //cout << "DODGING STORM" << endl;
                    aiState = new MoveToPos(this, closest2.pos.addxy(0, 16), true);
                    return;
                }
            }

            //cout << "DECIDE" << endl;
            if (couldNotDecide)
            {
                couldNotDecide = false;
                Point dirToStormCenter = Global.game.nextStormCenter - actor.pos;

                int xSign = Mathf.Sign(dirToStormCenter.x);
                int ySign = Mathf.Sign(dirToStormCenter.y);

                int xOff = 0;
                int yOff = 0;

                if (xSign > 0) xOff = Helpers.randomRange(0, 128);
                if (xSign < 0) xOff = Helpers.randomRange(-128, 0);
                if (ySign > 0) yOff = Helpers.randomRange(0, 128);
                if (ySign > 0) yOff = Helpers.randomRange(-128, 0);

                Point nextPos = actor.pos + new Point(xSign, ySign);
                if (nextPos.x < 0 || nextPos.y < 0 || nextPos.x >= actor.level.pixelWidth() || nextPos.y >= actor.level.pixelHeight())
                {
                    decided = false;
                    //cout << "Could not decide!" << endl;
                    return;
                }

                var decideState = new DecideState(this);
                var prospectiveState = new MoveToPos(this, nextPos, true, decideState);
                if (prospectiveState.path.Count == 0)
                {
                    decided = false;
                    //cout << "Could not decide!" << endl;
                    return;
                }
                else
                {
                    couldNotDecide = false;
                    aiState = prospectiveState;
                    return;
                }
            }

            //Decide whether to go to the master sword or not
            if (!goToMasterSword && !Global.game.masterSwordPulled && Global.game.stormPhase < 2 &&
                (
                    (actor.level.name == "lttp_overworld" && actor.pos.distTo(character.level.entrances["43"].pos) < 1536) ||
                    (actor.level.name == "house" && actor.pos.x > 128 * 8 && actor.pos.y > 288 * 8)
                ) && masterSwordChosen)
            {
                goToMasterSword = true;
            }

            //If going to master sword:
            if (goToMasterSword && !Global.game.masterSwordPulled && !Global.game.masterSwordBeingPulled)
            {
                //Outdoors: head towards the entrance
                if (!character.level.isIndoor)
                {
                    Entrance masterSwordEntrance = character.level.entrances["43"];
                    var nextState = new EnterEntrance(this, masterSwordEntrance);
                    var prospectiveState = new MoveToPos(this, masterSwordEntrance.pos.addxy(0, 24), true, nextState);
                    prospectiveState.tag = "mastersword";
                    if (prospectiveState.path.Count > 0)
                    {
                        aiState = prospectiveState;
                        return;
                    }
                    else
                    {
                        goToMasterSword = false;
                    }
                }
                //Indoors: move to the master sword pedastel
                else
                {
                    Actor masterSword = Global.game.unpulledMasterSword;
                    var nextState = new PullMasterSword(this);
                    var prospectiveState = new MoveToPos(this, masterSword.pos.addxy(0, -8), true, nextState);
                    prospectiveState.tag = "mastersword";
                    if (prospectiveState.path.Count > 0)
                    {
                        aiState = prospectiveState;
                        return;
                    }
                    else
                    {
                        goToMasterSword = false;
                    }
                }
            }

            //See if loot is nearby
            float closestDist = 1000000;

            if (character.hasEmptySlot())
            {
                FieldItem closestItem = null;
                foreach (var fieldItem in Global.game.fieldItems)
                {
                    if (fieldItem.isDeleted() || fieldItem == null || fieldItem.inventoryItem.item == null) continue;
                    if (!fieldItem.inventoryItem.item.immediate)
                    {
                        if (character.getEmptySlot() == -1 && !character.shouldAIGetItem(fieldItem.inventoryItem.item))
                        {
                            continue;
                        }
                    }
                    float dist = fieldItem.pos.distTo(actor.pos);
                    if (dist < closestDist && dist < 128)
                    {
                        closestDist = dist;
                        closestItem = fieldItem;
                    }
                }

                if (closestItem != null)
                {
                    //cout << closest.pos.toString() << endl;
                    var getItem = new GetItem(this, closestItem);
                    var prospectiveState = new MoveToPos(this, closestItem.pos, true, getItem);
                    if (prospectiveState.path.Count == 0)
                    {
                    }
                    else
                    {
                        aiState = prospectiveState;
                        return;
                    }
                }
            }

            //Try to find chest
            closestDist = 1000000;
            Actor closest = null;
            foreach (var chest in Global.game.chests)
            {
                if (chest.level != actor.level) continue;
                if (chest.opened) continue;
                if (chest.outOfReach) continue;
                if (chest.itemRequired != null && !character.hasItem(chest.itemRequired)) continue;
                if (chest.inStorm()) continue;

                float dist = chest.pos.distTo(actor.pos);

                if (actor.level.isIndoor)
                {
                    if (dist > 1024) continue;
                    var path = actor.level.getShortestPath(character, character.getColliderPos(), chest.pos, chest);
                    if (path.Count == 0)
                    {
                        continue;
                    }
                }

                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = chest;
                }
            }

            if (!actor.level.isIndoor)
            {
                foreach (var pair in actor.level.entrances)
                {
                    Entrance entrance = pair.Value;
                    if (entrancesUsed.Contains(entrance)) continue;
                    if (entrance.inStorm()) continue;
                    if (entrance.fall) continue;
                    if (entrance.entranceId == "46") continue;
                    if (entrance.entranceId == "59") continue;
                    if (entrance.entranceId == "60") continue;
                    if (entrance.entranceId == "62") continue;
                    if (entrance.entranceId == "63") continue;
                    if (entrance.entranceId == "37a") continue;
                    if (entrance.entranceId == "2") continue;
                    if (entrance.entranceId == "48") continue;
                    if (entrance.entranceId == "49") continue;
                    if (entrance.entranceId == "50") continue;
                    if (entrance.entranceId == "37") continue;
                    if (entrance.entranceId == "52") continue;
                    if (entrance.entranceId == "5") continue;
                    if (entrance.entranceId == "39") continue;
                    if (entrance.entranceId == "63a") continue;
                    if (entrance.entranceId == "29") continue;
                    if (entrance.entranceId == "27") continue;
                    if (entrance.entranceId == "28") continue;
                    if (entrance.entranceId == "55") continue;
                    if (entrance.entranceId == "41") continue;
                    if (entrance.entranceId == "44") continue;
                    if (entrance.entranceId == "7") continue;
                    if (entrance.entranceId == "47") continue;
                    if (entrance.entranceId == "21") continue;
                    if (entrance.entranceId == "19") continue;

                    float dist = entrance.pos.distTo(actor.pos);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closest = entrance;
                    }
                }
            }
            if (actor.level.isIndoor)
            {
                if (closest == null)
                {
                    closest = currentExit;
                }
            }

            if (closest != null)
            {
                //cout << closest.pos.toString() << endl;
                Chest chest = (closest as Chest);
                Entrance entrance = (closest as Entrance);
                AIState nextState = null;
                Point movePos = Point.Zero;
                if (chest != null)
                {
                    nextState = new OpenChest(this, chest);
                    movePos = closest.pos.addxy(0, 16);
                }
                else if (entrance != null)
                {
                    nextState = new EnterEntrance(this, entrance);
                    if (entrance.level.isIndoor) movePos = closest.pos.addxy(0, -16);
                    else movePos = closest.pos.addxy(0, 24);
                }

                var prospectiveState = new MoveToPos(this, movePos, true, nextState);
                if (prospectiveState.path.Count == 0)
                {
                    decided = false;
                    couldNotDecide = true;
                    //cout << "Could not decide!" << endl;
                }
                else
                {
                    aiState = prospectiveState;
                }
            }
            else
            {
                decided = false;
                couldNotDecide = true;
                //cout << "Could not decide!" << endl;
            }
        }
    }
}

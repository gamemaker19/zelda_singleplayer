using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class LinkIdle : ActorState
    {
        int dashFrames = 0;
        public LinkIdle() : base("LinkIdle")
        {
        }

        public override void update()
        {
            base.update();
            if (stateManager.bunnyTime == 0)
            {
                Character character = actor as Character;
                int slot = -1;
                Item itemUsed = null;
                InventoryItem inventoryItem = null;
                if (stateManager.input.isPressed(Control.main.Item1) && character.items[0] != null)
                {
                    itemUsed = character.items[0].item;
                    inventoryItem = character.items[0];
                    slot = 0;
                }
                if (stateManager.input.isPressed(Control.main.Item2) && character.items[1] != null)
                {
                    itemUsed = character.items[1].item;
                    inventoryItem = character.items[1];
                    slot = 1;
                }
                if (stateManager.input.isPressed(Control.main.Item3) && character.items[2] != null)
                {
                    itemUsed = character.items[2].item;
                    inventoryItem = character.items[2];
                    slot = 2;
                }
                if (stateManager.input.isPressed(Control.main.Item4) && character.items[3] != null)
                {
                    itemUsed = character.items[3].item;
                    inventoryItem = character.items[3];
                    slot = 3;
                }
                if (stateManager.input.isPressed(Control.main.Item5) && character.items[4] != null)
                {
                    itemUsed = character.items[4].item;
                    inventoryItem = character.items[4];
                    slot = 4;
                }

                if (stateManager.input.isPressed(Control.main.DropItem1) && character.items[0] != null)
                {
                    slot = 0;
                    character.dropItem(slot);
                    itemUsed = null;
                }
                if (stateManager.input.isPressed(Control.main.DropItem2) && character.items[1] != null)
                {
                    slot = 1;
                    character.dropItem(slot);
                    itemUsed = null;
                }
                if (stateManager.input.isPressed(Control.main.DropItem3) && character.items[2] != null)
                {
                    slot = 2;
                    character.dropItem(slot);
                    itemUsed = null;
                }
                if (stateManager.input.isPressed(Control.main.DropItem4) && character.items[3] != null)
                {
                    slot = 3;
                    character.dropItem(slot);
                    itemUsed = null;
                }
                if (stateManager.input.isPressed(Control.main.DropItem5) && character.items[4] != null)
                {
                    slot = 4;
                    character.dropItem(slot);
                    itemUsed = null;
                }

#if DEBUG
                if (stateManager.input.isPressed(Key.B)) itemUsed = Item.bombs;
                else if (stateManager.input.isPressed(Key.W)) itemUsed = Item.bow;
                else if (stateManager.input.isPressed(Key.R)) itemUsed = Item.boomerang;
                else if (stateManager.input.isPressed(Key.H)) itemUsed = Item.hammer;
                else if (stateManager.input.isPressed(Key.L)) itemUsed = Item.lamp;
                else if (stateManager.input.isPressed(Key.I)) itemUsed = Item.icerod;
                else if (stateManager.input.isPressed(Key.F)) itemUsed = Item.firerod;
                else if (stateManager.input.isPressed(Key.E)) itemUsed = Item.hookshot;
                else if (stateManager.input.isPressed(Key.Comma)) itemUsed = Item.bombos;
                else if (stateManager.input.isPressed(Key.Period)) itemUsed = Item.ether;
                else if (stateManager.input.isPressed(Key.Slash)) itemUsed = Item.quake;
                else if (stateManager.input.isPressed(Key.P)) itemUsed = Item.powder;
                else if (stateManager.input.isPressed(Key.Y)) itemUsed = Item.caneOfBryana;
                else if (stateManager.input.isPressed(Key.S)) itemUsed = Item.caneOfSomaria;
                else if (stateManager.input.isPressed(Key.A)) itemUsed = Item.cape;
                else if (stateManager.input.isPressed(Key.N)) itemUsed = Item.net;

                else if (stateManager.input.isPressed(Key.Num7)) character.addItem(new InventoryItem(Item.bottledFairy), 0);
                else if (stateManager.input.isPressed(Key.Num8)) character.addItem(new InventoryItem(Item.greenPotion), 0);
                else if (stateManager.input.isPressed(Key.Num9)) character.addItem(new InventoryItem(Item.redPotion), 0);
                else if (stateManager.input.isPressed(Key.Num0)) character.addItem(new InventoryItem(Item.bluePotion), 0);
                else if (stateManager.input.isPressed(Key.LBracket)) character.addItem(new InventoryItem(Item.blueMail), 0);
                else if (stateManager.input.isPressed(Key.RBracket)) character.addItem(new InventoryItem(Item.redMail), 0);
                else if (stateManager.input.isPressed(Key.U)) character.addItem(new InventoryItem(Item.pegasusBoots), 0);
                else if (stateManager.input.isPressed(Key.K)) character.addItem(new InventoryItem(Item.flippers), 0);
                else if (stateManager.input.isPressed(Key.O)) character.addItem(new InventoryItem(Item.moonPearl), 0);
#endif

                if (itemUsed == Item.bottledFairy)
                {
                    character.items[slot] = new InventoryItem(Item.emptyBottle);
                    new Fairy(character.level, character.pos + Helpers.dirToVec(character.dir) * 15);
                }
                else if (itemUsed == Item.bombs)
                {
                    Point pos = character.pos + Helpers.dirToVec(character.dir) * 10;
                    Bomb bomb = new Bomb(actor.level, pos, actor);
                    actor.playSound("bomb place");
                    if (slot != -1)
                    {
                        character.items[slot].count--;
                        if (character.items[slot].count == 0)
                        {
                            character.items[slot] = null;
                        }
                    }
                }
                else if (itemUsed == Item.bow && (character.aiStateManager != null || character.arrows.tryDeduct(1)))
                {
                    actor.playSound("arrow 1");
                    stateManager.changeState(new LinkBow(false), false);
                    return;
                }
                else if (itemUsed == Item.silverBow && (character.aiStateManager != null || character.arrows.tryDeduct(1)))
                {
                    actor.playSound("arrow 2");
                    stateManager.changeState(new LinkBow(true), false);
                    return;
                }
                else if (itemUsed == Item.boomerang)
                {
                    actor.playSound("boomerang");
                    stateManager.changeState(new LinkBoomerang(false), false);
                    return;
                }
                else if (itemUsed == Item.magicBoomerang)
                {
                    actor.playSound("boomerang");
                    stateManager.changeState(new LinkBoomerang(true), false);
                    return;
                }
                else if (itemUsed == Item.hookshot)
                {
                    stateManager.changeState(new LinkHookshot(), false);
                    return;
                }
                else if (itemUsed == Item.hammer)
                {
                    stateManager.changeState(new LinkHammer(), false);
                    return;
                }
                else if (itemUsed == Item.powder)
                {
                    if (character.magic.tryDeduct(8))
                    {
                        actor.playSound("magic powder 1");
                        stateManager.changeState(new LinkMagicPowder(), false);
                    }
                    return;
                }
                else if (itemUsed == Item.caneOfBryana)
                {
                    if (stateManager.bryanaRing != null)
                    {
                        actor.level.removeActor(stateManager.bryanaRing);
                        stateManager.bryanaRing = null;
                        return;
                    }
                    actor.playSound("cane");
                    stateManager.changeState(new LinkCaneOfBryana(), false);
                    return;
                }
                else if (itemUsed == Item.caneOfSomaria)
                {
                    if (character.caneBlock != null || character.magic.tryDeduct(8))
                    {
                        if (character.caneBlock != null) actor.playSound("sword beam");
                        else actor.playSound("cane");
                        stateManager.changeState(new LinkCaneOfSomaria(), false);
                    }
                    return;
                }
                else if (itemUsed == Item.firerod)
                {
                    if (character.magic.tryDeduct(16))
                    {
                        actor.playSound("fire rod");
                        stateManager.changeState(new LinkFireRod(), false);
                    }
                    return;
                }
                else if (itemUsed == Item.icerod)
                {
                    if (character.magic.tryDeduct(16))
                    {
                        actor.playSound("ice rod");
                        stateManager.changeState(new LinkIceRod(), false);
                    }
                    return;
                }
                else if (itemUsed == Item.cape)
                {
                    stateManager.invisible = !stateManager.invisible;
                    if (stateManager.invisible) actor.playSound("cape on");
                    else actor.playSound("cape off");
                    new Anim(actor.level, actor.pos, "CapePoof");
                    //stateManager.changeState(new LinkCape(), false);
                    return;
                }
                else if (itemUsed == Item.lamp)
                {
                    if (character.magic.tryDeduct(8))
                    {
                        actor.playSound("fire");
                        stateManager.changeState(new LinkLamp(), false);
                    }
                    return;
                }
                else if (itemUsed == Item.quake)
                {
                    if (character.hasSword() && character.magic.tryDeduct(32))
                    {
                        actor.playSound("medallion");
                        stateManager.changeState(new LinkQuake(), false);
                    }
                    return;
                }
                else if (itemUsed == Item.ether)
                {
                    if (character.hasSword() && character.magic.tryDeduct(32))
                    {
                        actor.playSound("medallion");
                        stateManager.changeState(new LinkEther(), false);
                    }
                    return;
                }
                else if (itemUsed == Item.bombos)
                {
                    if (character.hasSword() && character.magic.tryDeduct(32))
                    {
                        actor.playSound("medallion");
                        stateManager.changeState(new LinkBombos(), false);
                    }
                    return;
                }
                else if (itemUsed == Item.net)
                {
                    actor.playSound("bug net");
                    stateManager.changeState(new LinkBugNet(), false);
                    return;
                }
                else if (itemUsed == Item.greenPotion)
                {
                    character.items[slot].item = Item.emptyBottle;
                    character.magic.fillMax();
                }
                else if (itemUsed == Item.redPotion)
                {
                    character.items[slot].item = Item.emptyBottle;
                    character.health.fillMax();
                }
                else if (itemUsed == Item.bluePotion)
                {
                    character.items[slot].item = Item.emptyBottle;
                    character.magic.fillMax();
                    character.health.fillMax();
                }
                else if (stateManager.input.isPressed(Control.main.Sword))
                {
                    string sound = "";
                    if (character.hasItem(Item.sword2)) sound = "fighter sword 2";
                    else if (character.hasItem(Item.sword4)) sound = "golden sword";
                    else if (character.hasItem(Item.sword3)) sound = "master sword";
                    else if (character.hasItem(Item.sword1)) sound = "fighter sword 1";
                    if (sound != "") actor.playSound(sound);
                    stateManager.changeState(new LinkSword(), false);
                    return;
                }
                else
                {
                    bool xPressed = stateManager.input.isPressed(Control.main.Action);
                    Point forwardDir = Helpers.dirToVec(character.dir) * 1;
                    List<CollideData> collisionsInFront = actor.level.getActorCollisions(character, forwardDir);
                    collisionsInFront.Sort((CollideData a, CollideData b) => { return a.rayTo.magnitude < b.rayTo.magnitude ? -1 : 1; });
                    foreach (CollideData collideData in collisionsInFront)
                    {
                        if (Math.Sign(forwardDir.y) != 0 && Math.Abs(collideData.rayTo.x) > 10) continue;
                        if (Math.Sign(forwardDir.x) != 0 && Math.Abs(collideData.rayTo.y) > 10) continue;
                        Chest chest = collideData.collidedActor as Chest;
                        if (actor.aiStateManager == null && collideData.collidedActor != null && collideData.collidedActor.textGen != null && character.dir == Direction.Up && Global.game.dialogBox == null && Global.game.dialogFrameDelay == 0)
                        {
                            //if (character == Global.game.character) Global.game.setCurrentMessage("Press X to talk", 0.1);
                            if (xPressed)
                            {
                                Global.game.dialogBox = new DialogBox(collideData.collidedActor.textGen.dialogs);
                                stateManager.changeState(new LinkDialog(), false);
                                return;
                            }
                        }
                        else if (collideData.collidedActor != null && collideData.collidedActor.throwable != null && Global.game.dialogFrameDelay == 0)
                        {
                            var throwable = collideData.collidedActor.throwable;
                            bool canLift = (throwable.itemRequired == null || character.canLift(throwable.itemRequired)) && !throwable.lifted;
                            if (character == Global.game.character)
                            {
                                if (canLift)
                                {
                                    //if (throwable.itemRequired) Global.game.setCurrentMessage("Lifting this requires " + throwable.itemRequired.name, 0.1);
                                    //else Global.game.setCurrentMessage("Press X to pick up", 0.1);
                                }

                            }
                            if (canLift && xPressed)
                            {
                                stateManager.changeState(new LinkLift(collideData.collidedActor), true);
                                return;
                            }
                        }
                        else if (collideData.collidedActor != null && chest != null && !chest.opened && actor.dir == Direction.Up)
                        {
                            //if (character == Global.game.character) Global.game.setCurrentMessage("Press X to open", 0.1);
                            if (xPressed)
                            {
                                chest.open();
                                if (!chest.isBig)
                                {
                                    Item randomItem = Item.getRandomItem();
                                    InventoryItem inventoryItem2 = new InventoryItem(randomItem);
                                    if (!character.collectItem(inventoryItem2))
                                    {
                                        FieldItem fieldItem = new FieldItem(actor.level, chest.pos, inventoryItem2, true);
                                    }
                                    else
                                    {
                                        if (inventoryItem2.item == Item.bow || inventoryItem2.item == Item.silverBow)
                                        {
                                            character.arrows.addImmediate(5);
                                        }
                                    }
                                }
                                else
                                {
                                    Item randomItem = Item.getRandomItem();
                                    InventoryItem inventoryItem2 = new InventoryItem(randomItem);
                                    FieldItem fieldItem = new FieldItem(actor.level, chest.pos, inventoryItem2, true);

                                    randomItem = Item.getRandomItem();
                                    inventoryItem2 = new InventoryItem(randomItem);
                                    FieldItem fieldItem2 = new FieldItem(actor.level, chest.pos, inventoryItem2, true);

                                    randomItem = Item.getRandomItem();
                                    inventoryItem2 = new InventoryItem(randomItem);
                                    FieldItem fieldItem3 = new FieldItem(actor.level, chest.pos, inventoryItem2, true);

                                    fieldItem.vel = new Point(-1, 1);
                                    fieldItem2.vel = new Point(0, 1);
                                    fieldItem3.vel = new Point(1, 1);
                                }
                                return;
                            }
                        }
                        else if (collideData.collidedActor != null && collideData.collidedActor.collectable != null && !collideData.collidedActor.collectable.collectOnTouch)
                        {
                            if (character == Global.game.character && character.hasEmptySlot())
                            {
                                Global.game.setCurrentMessage("Press X to pick up " + collideData.collidedActor.collectable.inventoryItem.item.name, 0.1f);
                            }
                            if (xPressed)
                            {
                                character.collect(collideData.collidedActor.collectable);
                                if (collideData.collidedActor.collectable.collected)
                                {
                                    collideData.collidedActor.level.removeActor(collideData.collidedActor);
                                }
                                return;
                            }
                        }
                        else if (collideData.collidedActor != null && collideData.collidedActor.name == "MasterSwordWoods" && actor.dir == Direction.Down && !Global.game.masterSwordBeingPulled && !Global.game.masterSwordPulled)
                        {
                            if (character == Global.game.character && character.hasEmptySlot())
                            {
                                Global.game.setCurrentMessage("Press X to claim Master Sword", 0.1f);
                            }
                            else if (character == Global.game.character)
                            {
                                Global.game.setCurrentMessage("Must have empty slot to claim Master Sword", 0.1f);
                            }
                            if (xPressed && character.hasEmptySlot())
                            {
                                Point movePos = character.pos;
                                movePos.x = collideData.collidedActor.pos.x;
                                character.changePos(movePos, false);
                                stateManager.changeState(new LinkMasterSwordPull(collideData.collidedActor), false);
                                return;
                            }
                        }
                        else if (collideData.collidedActor != null && collideData.collidedActor is SaleItem)
                        {
                            SaleItem item = collideData.collidedActor as SaleItem;
                            if (character.hasItem(Item.emptyBottle))
                            {
                                if (character.rupees.value >= item.price)
                                {
                                    if (Global.game.camCharacter == character) Global.game.setCurrentMessage("Press X to buy " + item.item.name, 0.1f);
                                    if (xPressed)
                                    {
                                        character.rupees.deduct(item.price);
                                        int slot2 = character.getItemSlot(Item.emptyBottle);
                                        InventoryItem inventoryItem2 = new InventoryItem(item.item);
                                        character.collectItem(inventoryItem2, slot2);
                                    }
                                }
                                else
                                  if (Global.game.camCharacter == character) Global.game.setCurrentMessage("Not enough rupees", 0.1f);
                            }
                            else
                            {
                                if (Global.game.camCharacter == character) Global.game.setCurrentMessage("Need empty bottle to buy this", 0.1f);
                            }
                        }

                        /*
                        if ((collideData.collidedActor || collideData.collidedTile) && !collideData.isTrigger)
                        {
                          if (xPressed)
                          {
                            stateManager.changeState(new LinkGrab(), false);
                            return;
                          }
                        }
                        */
                    }
                }
                if (character.hasItem(Item.pegasusBoots) && stateManager.input.isHeld(Control.main.Action))
                {
                    dashFrames++;
                    if (dashFrames > 8)
                    {
                        stateManager.changeState(new LinkDashCharge(), false);
                        return;
                    }
                }
                else
                {
                    dashFrames = 0;
                }

            }
            commonLinkGround();
        }

    }

}

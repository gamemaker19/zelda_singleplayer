using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Item
    {
        public int spriteIndex = 0;
        public bool usesQuantity = false;
        public int maxQuantity = 1;
        public int spawnOddsWeight = 100;
        public int spawnOddsOverride = 0;  //If not set to 0 will use this instead, but AI and rarity ranking still use the above
        public bool immediate = false;
        public bool isSword = false;
        public bool isMelee = false;
        public bool isWeapon = false;
        public string name = "";
        public static List<Item> items = new List<Item>();

        public Item() { }

        public Item(int spriteIndex, string name, int spawnOddsWeight, bool usesQuantity = false, int maxQuantity = 1, bool immediate = false)
        {
            this.spriteIndex = spriteIndex - 1;
            this.name = name;
            this.usesQuantity = usesQuantity;
            this.maxQuantity = maxQuantity;
            this.spawnOddsWeight = spawnOddsWeight;
            this.immediate = immediate;
            items.Add(this);
        }

        public static Item getRandomItem()
        {
            //Common = 100
            //Uncommon = 50
            //Rare = 25
            //Epic = 10
            //Legendary = 5

            if (Global.nextItem != null)
            {
                Item itemToReturn = Global.nextItem;
                Global.nextItem = null;
                return itemToReturn;
            }

            float totalWeight = 0;
            foreach (Item item in Item.items)
            {
                if (item.spawnOddsOverride == 0)
                {
                    totalWeight += item.spawnOddsWeight;
                }
                else
                {
                    totalWeight += item.spawnOddsOverride;
                }
            }
            int rand = Helpers.randomRange(0, (int)totalWeight);
            int previousOdds = 0;
            int currentOdds = 0;
            foreach (Item item in Item.items)
            {
                if (item.spawnOddsOverride == 0)
                {
                    currentOdds += item.spawnOddsWeight;
                }
                else
                {
                    currentOdds += item.spawnOddsOverride;
                }
                if (rand >= previousOdds && rand < currentOdds) return item;
                previousOdds = currentOdds;
            }
            return Item.items[Helpers.randomRange(0, Item.items.Count - 1)];
        }

        #region Item List
        public static Item sword1;
        public static Item sword2;
        public static Item sword3;
        public static Item sword4;
        public static Item shield1;
        public static Item shield2;
        public static Item shield3;
        public static Item greenMail;
        public static Item blueMail;
        public static Item redMail;
        public static Item greenPendant;
        public static Item bluePendant;
        public static Item redPendant;
        public static Item powerGlove;
        public static Item titansMitt;
        public static Item flippers;
        public static Item moonPearl;
        public static Item pegasusBoots;
        public static Item bombs;
        public static Item bow;
        public static Item silverBow;
        public static Item boomerang;
        public static Item magicBoomerang;
        public static Item hookshot;
        public static Item firerod;
        public static Item icerod;
        public static Item hammer;
        public static Item net;
        public static Item lamp;
        public static Item caneOfSomaria;
        public static Item caneOfBryana;
        public static Item cape;
        public static Item powder;
        public static Item ether;
        public static Item quake;
        public static Item bombos;
        public static Item emptyBottle;
        public static Item bottledFairy;
        public static Item greenPotion;
        public static Item redPotion;
        public static Item bluePotion;
        public static Item heartPiece;
        public static Item heartContainer;
        public static Item blueRupee;
        public static Item redRupee;
        public static Item purpleRupee;
        public static Item rupees100;
        public static Item rupees300;
        public static Item arrows10;
        public static Item arrows30;

        public static void initItemList()
        {
            sword1 = new Item(1, "Fighter's Sword", 100);;
            sword2 = new Item(2, "Master Sword", 0);;
            sword3 = new Item(3, "Tempered Sword", 10);;
            sword4 = new Item(4, "Golden Sword", 5);;
            shield1 = new Item(5, "Fighter's Shield", 100);;
            shield2 = new Item(6, "Magic Shield", 25);;
            shield3 = new Item(7, "Mirror Shield", 5);;
            greenMail = new Item(8, "Green Mail", 0);;
            blueMail = new Item(9, "Blue Main", 10);;
            redMail = new Item(10, "Red Mail", 5);;
            greenPendant = new Item(46, "Pendant of Courage", 0);;
            bluePendant = new Item(47, "Pendant of Wisdom", 0);;
            redPendant = new Item(48, "Pendant of Power", 0);;
            powerGlove = new Item(12, "Power Glove", 100);;
            titansMitt = new Item(13, "Titan's Mitt", 50);;
            flippers = new Item(14, "Flippers", 50);;
            moonPearl = new Item(15, "Moon Pearl", 50);;
            pegasusBoots = new Item(11, "Pegasus Boots", 25);;

            bombs = new Item(21, "Bombs", 100, true, 30);;
            bow = new Item(17, "Bow", 50);;
            silverBow = new Item(18, "Bow", 5);;
            boomerang = new Item(19, "Boomerang", 100);;
            magicBoomerang = new Item(20, "Magic Boomerang", 50);;
            hookshot = new Item(22, "Hookshot", 25);;
            firerod = new Item(24, "Fire Rod", 25);;
            icerod = new Item(25, "Ice Rod", 25);;
            hammer = new Item(30, "Hammer", 10);;
            net = new Item(33, "Net", 100);;
            lamp = new Item(29, "Lamp", 100);;
            caneOfSomaria = new Item(35, "Cane of Somaria", 25);;
            caneOfBryana = new Item(36, "Cane of Bryana", 5);;
            cape = new Item(37, "Cape", 10);;
            powder = new Item(39, "Magic Powder", 50);;
            ether = new Item(26, "Ether", 10);;
            quake = new Item(27, "Quake", 10);;
            bombos = new Item(28, "Bombos", 10);;
            emptyBottle = new Item(40, "Empty Bottle", 50);;
            bottledFairy = new Item(41, "Bottled Fairy", 10);;
            greenPotion = new Item(42, "Green Potion", 25);;
            redPotion = new Item(43, "Red Potion", 10);;
            bluePotion = new Item(44, "Blue Potion", 5);;
            //* items::flute = new Item(32, "Flute", 5);
            //* items::shovel = new Item(31, "Shovel", 50);
            heartPiece = new Item(49, "Heart Piece", 25, true, 400);;
            heartContainer = new Item(50, "Heart Container", 10, false, 1, true);;
            blueRupee = new Item(51, "Blue Rupee", 100, false, 1, true);;
            redRupee = new Item(52, "Red Rupee", 50, false, 1, true);;
            purpleRupee = new Item(53, "Purple Rupee", 25, false, 1, true);;
            rupees100 = new Item(54, "100 Rupees", 10, false, 1, true);;
            rupees300 = new Item(55, "300 Rupees", 5, false, 1, true);;
            arrows10 = new Item(56, "10 Arrows", 100, false, 1, true);;
            arrows30 = new Item(57, "30 Arrows", 25, false, 1, true);;

            sword1.isSword = true;
            sword2.isSword = true;
            sword3.isSword = true;
            sword4.isSword = true;

            sword1.isMelee = true;
            sword2.isMelee = true;
            sword3.isMelee = true;
            sword4.isMelee = true;
            hammer.isMelee = true;
            lamp.isMelee = true;

            sword1.isWeapon = true;
            sword2.isWeapon = true;
            sword3.isWeapon = true;
            sword4.isWeapon = true;

            sword1.isWeapon = true;
            sword2.isWeapon = true;
            sword3.isWeapon = true;
            sword4.isWeapon = true;
            hammer.isWeapon = true;
            lamp.isWeapon = true;
            silverBow.isWeapon = true;
            bow.isWeapon = true;
            firerod.isWeapon = true;
            icerod.isWeapon = true;

            caneOfBryana.isWeapon = true;
            caneOfBryana.isMelee = true;
            caneOfSomaria.isWeapon = true;

            //Items that should be more common than their rarity ranks, for balance's sake
            heartPiece.spawnOddsOverride = 1000;
            heartContainer.spawnOddsOverride = 100;
            sword1.spawnOddsOverride = 1000;
            bow.spawnOddsOverride = 1000;
            lamp.spawnOddsOverride = 1000;
        }
        #endregion
    }

    public class InventoryItem
    {
	    public Item item;
        public int count = 1;
        public InventoryItem() { }

        public InventoryItem(Item item)
        {
	        this.item = item;
	        this.count = 1;
	        if (item == Item.bombs)
	        {
		        int rand = Helpers.randomRange(0, 3);
		        if (rand == 0) this.count = 15;
		        else this.count = 5;
	        }
        }

        public bool maxed()
        {
            return count >= item.maxQuantity;
        }

    }

}

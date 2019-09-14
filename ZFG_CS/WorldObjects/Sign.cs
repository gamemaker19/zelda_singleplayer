using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Sign : Actor
    {
        public Sign(Level level, Point pos) : base(level, pos, "Sign")
        {
            throwable = new Throwable(this, "SignBase", "PotBreak", false, 0.5f, "rock break");
            var messages = new List<List<string>>();
            messages.Add(new List<string>() { "Tip: The mirror shield will reflect all energy-based projectiles back at the caster!", "The user must face their opponent for this to work." });
            messages.Add(new List<string>() { "Tip: A bottled fairy cannot revive you while swimming." });
            messages.Add(new List<string>() { "Tip: Use the Magic Powder to turn opponents into bunnies." });
            messages.Add(new List<string>() { "Tip: The Quake Medallion will turn nearby opponents into bunnies." });
            messages.Add(new List<string>() { "Tip: The Moon Pearl will protect you from turning into a bunny from the Twilight, Quake or Magic Powder." });
            messages.Add(new List<string>() { "Tip: The Magic Cape makes you invisible, but not invincible.", "The Cane of Bryana makes you invincible, but not invisible." });
            messages.Add(new List<string>() { "Tip: Unlike the Red and Mirror Shield, the Fighter's Shield cannot block energy-based projectiles." });
            //messages.Add(new List<string>() { "Tip: The Master Sword requires the three pendants in your inventory to claim from its pedestal." });
            messages.Add(new List<string>() { "Tip: The Master Sword, the Legendary Blade of Evil's Bane, is the strongest sword in the game.", "It deals 4 damage and can shoot energy beams at full health." });
            messages.Add(new List<string>() { "Tip: Flippers are required in your inventory to swim in bodies of water." });
            messages.Add(new List<string>() { "Tip: A frozen enemy cannot be damaged by swords.", "Use blunt-force weapons like the hammer or bombs." });
            messages.Add(new List<string>() { "Tip: Catch a fairy with the Bug Catching Net to revive on death.", "You will need an empty bottle and the net in your inventory." });
            messages.Add(new List<string>() { "Tip: Skins provide no advantage in gameplay and are purely cosmetic." });
            //messages.Add(new List<string>() { "Tip: Use rupees in-game to buy items from shops, or skins in the main menu." });
            messages.Add(new List<string>() { "Tip: The storm deals damage over time, in addition to turning you into a bunny.", "As a bunny, you cannot do anything but move around." });
            messages.Add(new List<string>() { "Tip: The lamp can burn enemies, but deals little damage." });
            messages.Add(new List<string>() { "Tip: You can set enemies on fire with the Lamp, Fire Rod and Bombos.", "Enemies on fire take damage over time." });
            messages.Add(new List<string>() { "Tip: When on fire, jump in the water to put it out!" });
            messages.Add(new List<string>() { "Tip: You can thaw a frozen enemy or ally with the Fire Rod, Lamp or Bombos." });
            messages.Add(new List<string>() { "Tip: Houses and caves contain the most chests." });
            messages.Add(new List<string>() { "Tip: Kakariko Village has the most chests of any landing zone.", "But beware, hero: many opponents will also land here, likely resulting in confrontation." });
            messages.Add(new List<string>() { "Tip: Even with the Moon Pearl you will still take damage in the Twilight." });
            //messages.Add(new List<string>() { "Tip: Carrying multiples of the same item makes its effect more potent." });
            messages.Add(new List<string>() { "Tip: Every 4 pieces of heart you collect in your inventory gives you an extra heart container." });
            //messages.Add(new List<string>() { "Tip: Every 500 rupees you collect gives you an extra inventory slot, up to 10.", "If you spend your rupees, you will lose your progress towards this goal." });
            textGen = new TextGen(this, messages[Helpers.randomRange(0, messages.Count - 1)]);
            hookable = true;
        }
    }
}

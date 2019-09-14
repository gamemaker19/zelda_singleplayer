using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Damager
    {
        public Actor owner;
        public Actor actor; //Optional for now
        public Item item;
        public float damage;
        public bool invulnFrames;
        public bool flinch;
        public bool burn;
        public bool freeze;
        public bool stun;
        public bool bunnify;
        public bool canHurtOwner;
        public bool isConcussive;

        public Damager(Actor owner, Item item, float damage)
        {
	        this.owner = owner;
	        this.item = item;
	        this.damage = damage;
            invulnFrames = true;
            flinch = true;

        }

        public int alliance()
        {
            if (owner == null) return 0;
            return owner.alliance;
        }
    }
}

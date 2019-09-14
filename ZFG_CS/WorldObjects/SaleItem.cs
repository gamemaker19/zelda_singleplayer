using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class SaleItem : Actor
    {
	    public Item item = null;
        public int price = 0;
        public SaleItem(Level level, Point pos, Item item, int price) : base(level, pos, "EmptyCollider")
        {
            this.item = item;
            this.price = price;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor_CS.Models
{
    public class Obj
    {
        public string name;
        public string spriteOrImage;
        public bool snapToTile;
        public Point snapOffset;

        public Obj(string name, string image, bool snapToTile, Point snapOffset)
        {
            this.name = name;
            this.spriteOrImage = image;
            this.snapToTile = snapToTile;
            this.snapOffset = snapOffset;
        }


    }
}

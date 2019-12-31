namespace GameEditor.Models
{
    public class Obj
    {
        public string name { get; set; }
        public string spriteOrImage { get; set; }
        public bool snapToTile { get; set; }
        public Point snapOffset { get; set; }

        public Obj(string name, string image, bool snapToTile, Point snapOffset)
        {
            this.name = name;
            this.spriteOrImage = image;
            this.snapToTile = snapToTile;
            this.snapOffset = snapOffset;
        }

        public override string ToString()
        {
            return name;
        }

    }
}

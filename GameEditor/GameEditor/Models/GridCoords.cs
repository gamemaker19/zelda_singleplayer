using GameEditor.Editor;

namespace GameEditor.Models
{
    public class GridCoords
    {
        public int i { get; set; }
        public int j { get; set; }
        public GridCoords(int i, int j)
        {
            this.i = i;
            this.j = j;
        }

        public Rect getRect()
        {
            return new Rect(this.j * Consts.TILE_WIDTH, this.i * Consts.TILE_WIDTH, (this.j + 1) * Consts.TILE_WIDTH, (this.i + 1) * Consts.TILE_WIDTH);
        }

        public Rect getRectCustomWidth(float width)
        {
            return new Rect(this.j * width, this.i * width, (this.j + 1) * width, (this.i + 1) * width);
        }

        public GridCoords clone()
        {
            return new GridCoords(this.i, this.j);
        }

        public bool equals(GridCoords other)
        {
            if (other == null) return false;
            return this.i == other.i && this.j == other.j;
        }
    }
}

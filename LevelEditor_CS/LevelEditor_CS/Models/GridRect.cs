using LevelEditor_CS.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor_CS.Models
{
    public class GridRect
    {
        public GridCoords topLeftGridCoords;
        public GridCoords botRightGridCoords;

        public GridRect(float i1, float j1, float i2, float j2)
        {
            this.topLeftGridCoords = new GridCoords(i1, j1);
            this.botRightGridCoords = new GridCoords(i2, j2);
        }
        
        public string toString()
        {
            return (this.topLeftGridCoords.i).ToString() + "_" + (this.topLeftGridCoords.j).ToString() + "_" + (this.botRightGridCoords.i).ToString() + "_" + (this.botRightGridCoords.j).ToString();
        }
        
        public bool equals(GridRect other)
        {
            return this.topLeftGridCoords.equals(other.topLeftGridCoords) && this.botRightGridCoords.equals(other.botRightGridCoords);
        }
        
        public Rect getRect()
        {
            return new Rect(this.j1 * Consts.TILE_WIDTH, this.i1 * Consts.TILE_WIDTH, (this.j2 + 1) * Consts.TILE_WIDTH, (this.i2 + 1) * Consts.TILE_WIDTH);
        }
        
        public float i1 { get { return this.topLeftGridCoords.i; } }
        public float j1 { get { return this.topLeftGridCoords.j; } }
        public float i2 { get { return this.botRightGridCoords.i; } }
        public float j2 { get { return this.botRightGridCoords.j; } }
    }
}

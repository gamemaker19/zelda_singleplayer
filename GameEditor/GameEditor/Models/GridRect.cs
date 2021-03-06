﻿using GameEditor.Editor;

namespace GameEditor.Models
{
    public class GridRect
    {
        public GridCoords topLeftGridCoords { get; set; }
        public GridCoords botRightGridCoords { get; set; }

        public GridRect(int i1, int j1, int i2, int j2)
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

        public int i1 { get { return this.topLeftGridCoords.i; } }
        public int j1 { get { return this.topLeftGridCoords.j; } }
        public int i2 { get { return this.botRightGridCoords.i; } }
        public int j2 { get { return this.botRightGridCoords.j; } }
    }
}

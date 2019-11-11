using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor_CS.Models
{
    public class TileInstance
    {
        public TileData tileData;
        public string tileDataId;
        public GridCoords destPoint;
        public TileInstance(TileData tileData, GridCoords destPoint)
        {
            this.tileData = tileData;
            this.destPoint = destPoint;
        }

        /*
        public void getNonSerializedKeys()
        {
            return ["tileData"];
        }
        */

        public void onSerialize()
        {
            this.tileDataId = this.tileData.getId();
        }
        public void onDeserialize()
        {
        }
        public void setTileData(List<TileData> tileDatas)
        {
            this.tileData = tileDatas.Where((TileData tileData) => { return tileData.getId() == this.tileDataId; }).FirstOrDefault();
        }
        /*
        public void draw(ctx: CanvasRenderingContext2D)
        {
            var rect = this.tileData.gridCoords.getRect();
            Helpers.drawImage(ctx, this.tileData.tileset.imgEl, rect.x1, rect.y1, rect.w, rect.h, this.destPoint.j * Consts.TILE_WIDTH, this.destPoint.i * Consts.TILE_WIDTH);
        }
        */
        public Rect getRect()
        {
            return this.destPoint.getRect();
        }

    }
}

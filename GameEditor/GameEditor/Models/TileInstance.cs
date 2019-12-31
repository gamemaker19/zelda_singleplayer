using GameEditor.Editor;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;

namespace GameEditor.Models
{
    public class TileInstance
    {
        [JsonIgnore]
        public TileData tileData { get; set; }
        public string tileDataId { get; set; }
        public GridCoords destPoint { get; set; }

        public TileInstance(TileData tileData, GridCoords destPoint)
        {
            this.tileData = tileData;
            this.destPoint = destPoint;
        }

        [OnSerialized]
        public void onSerialize(StreamingContext context)
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

        public void draw(Graphics canvas)
        {
            var rect = this.tileData.gridCoords.getRect();
            Helpers.drawImage(canvas, this.tileData.tileset.image, rect.x1, rect.y1, rect.w, rect.h, this.destPoint.j * Consts.TILE_WIDTH, this.destPoint.i * Consts.TILE_WIDTH);
        }

        public Rect getRect()
        {
            return this.destPoint.getRect();
        }

    }
}

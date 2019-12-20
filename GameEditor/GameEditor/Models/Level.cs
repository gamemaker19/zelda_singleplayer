using GameEditor.Editor;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace GameEditor.Models
{
    public class CoordProperties
    {
        public int i;
        public int j;
        public Dictionary<string, string> properties;
    }

    public class Level
    {
        public string name { get; set; }
        public List<List<string>> tileInstances = new List<List<string>>();
        public List<SpriteInstance> instances = new List<SpriteInstance>();
        public List<CoordProperties> coordProperties = new List<CoordProperties>();

        [JsonIgnore]
        public List<List<Dictionary<string, string>>> coordPropertiesGrid = new List<List<Dictionary<string, string>>>();

        [JsonIgnore]
        public List<Bitmap> layers = new List<Bitmap>();

        public List<Line> scrollLines = new List<Line>();

        public int width { get; set; } = 0;
        public int height { get; set; } = 0;
        public Level(string name, int width, int height)
        {
            this.name = name;
            this.width = width;
            this.height = height;
            this.init();
        }

        public override string ToString()
        {
            return name;
        }

        public void setLayers(List<Spritesheet> spritesheets)
        {
            foreach (var layer in this.layers)
            {
                layer.Dispose();
            }
            this.layers = new List<Bitmap>();
            foreach (var spritesheet in spritesheets)
            {
                var pieces = Path.GetFileNameWithoutExtension(spritesheet.path).Split('_').ToList();
                pieces.RemoveAt(pieces.Count - 1);
                if (string.Join("_", pieces) == this.name)
                {
                    spritesheet.init(false);
                    this.layers.Add(spritesheet.image);
                }
            }
            if (this.layers.Count == 0)
            {
                this.layers.Add(null);
            }
        }

        public void resize()
        {
            this.coordPropertiesGrid = Helpers.make2DArray<Dictionary<string, string>>(this.height, this.width, new Dictionary<string, string>());

            foreach(var layer in this.layers) 
            {
                //layer.Width = this.width * Consts.TILE_WIDTH;
                //layer.Height = this.height * Consts.TILE_WIDTH;
            }
        }

        public void init()
        {
            this.coordPropertiesGrid = Helpers.make2DArray<Dictionary<string, string>>(this.height, this.width, new Dictionary<string, string>());
            if (this.tileInstances.Count == 0) 
            {
                for (var i = 0; i < this.height; i++)
                {
                    var row = new List<string>();
                    for (var j = 0; j < this.width; j++)
                    {
                        row.Add(null);
                    }
                    this.tileInstances.Add(row);
                }
            }
        }

        [OnSerializing]
        public void onSerialize(StreamingContext context)
        {
            this.coordProperties = new List<CoordProperties>();
            for (var i = 0; i < this.coordPropertiesGrid.Count; i++)
            {
                for (var j = 0; j < this.coordPropertiesGrid[i].Count; j++)
                {
                    var props = this.coordPropertiesGrid[i][j];
                    if (props.Keys.Count > 0)
                    {
                        this.coordProperties.Add(new CoordProperties() {
                            i = i,
                            j = j,
                            properties = props
                        });
                    }
                }
            }
        }

        /*
        public void setTileInstancesOnSerialize() //(levelCtx: CanvasRenderingContext2D, tileHashes: { [tileHash: string]: GridCoords }[], tileGrids: TileData[][][]) 
        {
            this.tileInstances = new List<List<string>>();
            for(var i = 0; i < this.height; i++) {
                var row = new List<string>();
                this.tileInstances.Add(row);
                for (var j = 0; j < this.width; j++)
                {
                    var tileHashKey = Helpers.getTileHash(levelCtx, i, j, this.name);
                    if (!tileHashKey)
                    {
                        Console.WriteLine("Tile hash at " + i.ToString() + "," + j.ToString() + " not found!");
                        row.Add("");
                        continue;
                    }
                    var id = "";
                    GridCoords gridCoords = null;
                    for (var n = 0; n < tileHashes.Count; n++)
                    {
                        coords = tileHashes[n][tileHashKey];
                        if (coords != null)
                        {
                            var tileData = tileGrids[n][coords.i][coords.j];
                            id = tileData.getId();
                            break;
                        }
                    }
                    row.Add(id);
                }
            }
        }
        */

        [OnDeserialized]
        public void onDeserialize(StreamingContext context)
        {
            this.init();
            foreach (var coordProperty in this.coordProperties)
            {
                this.coordPropertiesGrid[coordProperty.i][coordProperty.j] = coordProperty.properties;
            }
        }

        public void destroy()
        {
            foreach (var layer in this.layers)
            {
                layer.Dispose();
            }
        }
    }
}

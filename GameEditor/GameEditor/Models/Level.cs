using GameEditor.Editor;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace GameEditor.Models
{
    public class CoordProperties
    {
        public int i { get; set; }
        public int j { get; set; }
        public Dictionary<string, string> properties { get; set; }
    }

    public class Level
    {
        public string name { get; set; }
        public List<List<List<string>>> tileInstances { get; set; } = new List<List<List<string>>>();
        public ObservableCollection<SpriteInstance> instances { get; set; } = new ObservableCollection<SpriteInstance>();
        public List<CoordProperties> coordProperties { get; set; } = new List<CoordProperties>();

        [JsonIgnore]
        public List<List<Dictionary<string, string>>> coordPropertiesGrid = new List<List<Dictionary<string, string>>>();

        [JsonIgnore]
        public List<Layer> layers = new List<Layer>();

        public List<Line> scrollLines { get; set; } = new List<Line>();

        public string defaultTileId { get; set; } = "";

        public int width { get; set; } = 0;
        public int height { get; set; } = 0;
        public Level(string name, int width, int height)
        {
            this.name = name;
            this.width = width;
            this.height = height;
            this.init();
            addLayer();
        }

        public override string ToString()
        {
            return name;
        }

        public void setLayers(List<Spritesheet> levelImages, bool reset = true)
        {
            foreach (var layer in this.layers)
            {
                layer.bitmap.Dispose();
            }
            this.layers = new List<Layer>();
            if (!reset)
            {
                foreach (var spritesheet in levelImages)
                {
                    var pieces = Path.GetFileNameWithoutExtension(spritesheet.path).Split('_').ToList();
                    pieces.RemoveAt(pieces.Count - 1);
                    if (string.Join("_", pieces) == this.name)
                    {
                        spritesheet.init(false);
                        this.layers.Add(new Layer(spritesheet.image));
                    }
                }
            }
            else
            {
                foreach(var grid in tileInstances)
                {
                    Bitmap bitmap = new Bitmap(width * Consts.TILE_WIDTH, height * Consts.TILE_WIDTH);
                    layers.Add(new Layer(bitmap));
                    redrawLayer(layers.Count - 1);
                }
            }
        }

        public void redrawLayer(int index)
        {
            Bitmap bitmap = layers[index].bitmap;
            var grid = tileInstances[index];
            Graphics canvas = Graphics.FromImage(bitmap);
            canvas.Clear(Color.Transparent);
            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Count; j++)
                {
                    string tileId = grid[i][j];
                    if (string.IsNullOrEmpty(tileId))
                    {
                        continue;
                    }
                    string tilesetName = tileId.Split('_')[0];
                    int ti = int.Parse(tileId.Split('_')[1]);
                    int tj = int.Parse(tileId.Split('_')[2]);

                    TileData tileData = MainWindow.tileDataGrids[tilesetName][ti][tj];
                    if (tileData == null)
                    {
                        continue;
                    }
                    else
                    {
                        Helpers.drawImage(canvas, tileData.tileset.image, j * Consts.TILE_WIDTH, i * Consts.TILE_WIDTH, tileData.gridCoords.j * Consts.TILE_WIDTH, tileData.gridCoords.i * Consts.TILE_WIDTH, Consts.TILE_WIDTH, Consts.TILE_WIDTH);
                    }
                }
            }
            canvas.Dispose();
        }

        public void addLayer()
        {
            this.layers.Add(new Layer(new Bitmap(width * Consts.TILE_WIDTH, height * Consts.TILE_WIDTH)));
            tileInstances.Add(Helpers.make2DArray<string>(height, width, defaultTileId));
        }

        public void removeLayer(int layerIndex)
        {
            this.layers[layerIndex].bitmap.Dispose();
            this.layers.RemoveAt(layerIndex);
            tileInstances.RemoveAt(layerIndex);
        }

        // Amounts are in tile coords not pixel coords
        public void resize(int leftAmount, int rightAmount, int topAmount, int bottomAmount)
        {
            int leftPixelAmount = leftAmount * Consts.TILE_WIDTH;
            int topPixelAmount = topAmount * Consts.TILE_WIDTH;

            width = width + leftAmount + rightAmount;
            height = height + topAmount + bottomAmount;

            var newCoordPropertiesGrid = Helpers.make2DArray<Dictionary<string, string>>(height, width, new Dictionary<string, string>());
            for(int i = 0; i < newCoordPropertiesGrid.Count; i++)
            {
                for(int j = 0; j < newCoordPropertiesGrid[i].Count; j++)
                {
                    if (i - topAmount < 0 || j - leftAmount < 0 || i - topAmount >= coordPropertiesGrid.Count || j - leftAmount >= coordPropertiesGrid[0].Count) continue;
                    newCoordPropertiesGrid[i][j] = coordPropertiesGrid[i - topAmount][j - leftAmount];       
                }
            }
            coordPropertiesGrid = newCoordPropertiesGrid;

            for (int index = 0; index < tileInstances.Count; index++)
            {
                var grid = tileInstances[index];
                var newGrid = Helpers.make2DArray<string>(height, width, defaultTileId);
                for (int i = 0; i < newGrid.Count; i++)
                {
                    for (int j = 0; j < newGrid[i].Count; j++)
                    {
                        if (i - topAmount < 0 || j - leftAmount < 0 || i - topAmount >= grid.Count || j - leftAmount >= grid[0].Count) continue;
                        newGrid[i][j] = grid[i - topAmount][j - leftAmount];
                    }
                }
                tileInstances[index] = newGrid;
            }

            for (int index = 0; index < layers.Count; index++)
            {
                Bitmap layer = layers[index].bitmap;
                Bitmap newLayer = new Bitmap(width * Consts.TILE_WIDTH, height * Consts.TILE_WIDTH);

                /*
                using (Graphics g = Graphics.FromImage(newLayer))
                {
                    g.DrawImage(layer, leftPixelAmount, topPixelAmount);
                }
                */

                layer.Dispose();
                layers[index].bitmap = newLayer;
            }

            for(int i = 0; i < layers.Count; i++)
            {
                redrawLayer(i);
            }
            
            foreach(var instance in instances)
            {
                instance.pos.x += leftPixelAmount;
                instance.pos.y += topPixelAmount;
            }

        }

        public void init()
        {
            this.coordPropertiesGrid = Helpers.make2DArray<Dictionary<string, string>>(this.height, this.width, new Dictionary<string, string>());
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
                        this.coordProperties.Add(new CoordProperties()
                        {
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
                layer.bitmap.Dispose();
            }
        }
    }
}

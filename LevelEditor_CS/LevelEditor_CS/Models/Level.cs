using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor_CS.Models
{
    class Level
    {
        public string name;
        public List<List<string>> tileInstances = new List<List<string>>();
        public List<SpriteInstance> instances = new List<SpriteInstance>();
        //public List<HTMLCanvasElement> layers = new List<HTMLCanvasElement>();
        //coordPropertiesGrid: any[][] = [];
        public List<Line> scrollLines = new List<Line>();
        //private coordProperties: any[] = [];
        public float width = 0;
        public float height = 0;
        Level(string name, float width, float height)
        {
            this.name = name;
            //this.coordPropertiesGrid = [];
            this.width = width;
            this.height = height;
            //this.init();
        }

        /*

        public void addCanvas(img: HTMLImageElement)
        {
            let newCanvas = document.createElement("canvas");
            newCanvas.width = this.width * consts.TILE_WIDTH;
            newCanvas.height = this.height * consts.TILE_WIDTH;
            $(newCanvas).addClass("layer-canvas");
            $(newCanvas).insertBefore("#level-canvas");
            if (img)
            {
                newCanvas.getContext("2d").drawImage(img, 0, 0);
            }
            else
            {
                //newCanvas.getContext("2d").clearRect(0, 0, this.width * consts.TILE_WIDTH, this.height * consts.TILE_WIDTH);
            }
            this.layers.push(newCanvas);
        }

        public void setLayers(spritesheets: Spritesheet[])
        {
            for (let layer of this.layers)
            {
                layer.remove();
            }
            this.layers = [];
            for (let spritesheet of spritesheets)
            {
                let pieces = Helpers.baseName(spritesheet.path).split("_");
                pieces.pop();
                if (pieces.join("_") === this.name)
                {
                    this.addCanvas(spritesheet.imgEl);
                }
            }
            if (this.layers.length === 0)
            {
                this.addCanvas(undefined);
            }
        }

        public void resize()
        {
            this.coordPropertiesGrid.length = this.height;
            for (let i = 0; i < this.coordPropertiesGrid.length; i++)
            {
                if (!this.coordPropertiesGrid[i])
                {
                    let row = [];
                    for (let counter = 0; counter < this.width; counter++)
                    {
                        row.push({ });
                    }
                    this.coordPropertiesGrid[i] = row;
                }
                else 
                {
                    this.coordPropertiesGrid[i].length = this.width;
                }
            }
            for(let row of this.coordPropertiesGrid) 
            {
                for (let i = 0; i < row.length; i++)
                {
                    if (!row[i])
                    {
                        row[i] = [];
                    }
                }
            }
            for(let layer of this.layers) 
            {
                let context = layer.getContext("2d");
                let imageData = context.getImageData(0, 0, this.width * consts.TILE_WIDTH, this.height * consts.TILE_WIDTH);
                layer.width = this.width * consts.TILE_WIDTH;
                layer.height = this.height * consts.TILE_WIDTH;
                context.putImageData(imageData, 0, 0);
            }
        }

        public void init()
        {
            this.coordPropertiesGrid = [];
            for (let i = 0; i < this.height; i++)
            {
                let row = [];
                for (let j = 0; j < this.width; j++)
                {
                    row.push({ });
                }
                this.coordPropertiesGrid.push(row);
            }
            if(this.tileInstances.length === 0) 
            {
                for (let i = 0; i < this.height; i++)
                {
                    let row = [];
                    for (let j = 0; j < this.width; j++)
                    {
                        row.push(undefined);
                    }
                    this.tileInstances.push(row);
                }
            }
        }
        public void getNonSerializedKeys()
        {
            return ["layers", "coordPropertiesGrid"];
        }
        
        public void onSerialize()
        {
            this.coordProperties = [];
            for (let i = 0; i < this.coordPropertiesGrid.length; i++)
            {
                for (let j = 0; j < this.coordPropertiesGrid[i].length; j++)
                {
                    let props = this.coordPropertiesGrid[i][j];
                    if (!_.isEmpty(props))
                    {
                        this.coordProperties.push({
                            i: i,
                            j: j,
                            properties: props
                        });
                    }
                }
            }
        }

        public void setTileInstancesOnSerialize(levelCtx: CanvasRenderingContext2D, tileHashes: { [tileHash: string]: GridCoords }[], tileGrids: TileData[][][]) 
        {
            this.tileInstances = [];
            for(let i = 0; i< this.height; i++) {
                let row: string[] = [];
                this.tileInstances.push(row)
                for (let j = 0; j < this.width; j++)
                {
                    //@ts-ignore
                    let tileHashKey = Helpers.getTileHash(levelCtx, i, j, this.name);
                    if (!tileHashKey)
                    {
                        console.warn("Tile hash at " + String(i) + "," + String(j) + " not found!");
                        row.push("");
                        continue;
                    }
                    let id = "";
                    let coords: GridCoords = undefined;
                    for (let n = 0; n < tileHashes.length; n++)
                    {
                        coords = tileHashes[n][tileHashKey];
                        if (coords)
                        {
                            let tileData = tileGrids[n][coords.i][coords.j];
                            id = tileData.getId();
                            break;
                        }
                    }
                    row.push(id);
                }
            }
        }
    
        onDeserialize()
        {
            this.init();
            for (let coordProperty of this.coordProperties)
            {
                this.coordPropertiesGrid[coordProperty.i][coordProperty.j] = coordProperty.properties;
            }
        }
        destroy()
        {
            for (let layer of this.layers)
            {
                layer.remove();
            }
        }
        */
    }
}

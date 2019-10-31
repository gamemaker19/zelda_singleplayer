import { TileInstance } from "./TileInstance";
import { TileData } from "./TileData";
import * as Helpers from "../helpers";
import { Rect } from "./Rect";
import { consts } from "../consts";
import * as _ from "lodash";
import { SpriteInstance } from "./SpriteInstance";
import { GridCoords } from "./GridCoords";
import { GridRect } from "./GridRect";
import { Spritesheet } from "./Spritesheet";
import { Line } from "./Line";

export class Level {
  name: string;
  tileInstances: string[][] = [];
  instances: SpriteInstance[] = [];
  layers: HTMLCanvasElement[] = [];
  coordPropertiesGrid: any[][] = [];
  scrollLines: Line[] = [];
  private coordProperties: any[] = [];
  width: number = 0;
  height: number = 0;
  constructor(name: string, width: number, height: number) {
    this.name = name;
    this.coordPropertiesGrid = [];
    this.width = width;
    this.height = height;
    this.init();
  }
  addCanvas(img: HTMLImageElement) {
    let newCanvas = document.createElement("canvas");
    newCanvas.width = this.width * consts.TILE_WIDTH;
    newCanvas.height = this.height * consts.TILE_WIDTH;
    $(newCanvas).addClass("layer-canvas");
    $(newCanvas).insertBefore("#level-canvas");
    if(img) {
      newCanvas.getContext("2d").drawImage(img, 0, 0);
    }
    else {
      //newCanvas.getContext("2d").clearRect(0, 0, this.width * consts.TILE_WIDTH, this.height * consts.TILE_WIDTH);
    }
    this.layers.push(newCanvas);
  }
  setLayers(spritesheets: Spritesheet[]) {
    for(let layer of this.layers) {
      layer.remove();
    }
    this.layers = [];
    for(let spritesheet of spritesheets) {
      let pieces = Helpers.baseName(spritesheet.path).split("_");
      pieces.pop();
      if(pieces.join("_") === this.name) {
        this.addCanvas(spritesheet.imgEl);
      }
    }
    if(this.layers.length === 0) {
      this.addCanvas(undefined);
    }
  }
  resize() {
    this.coordPropertiesGrid.length = this.height;
    for(let i = 0; i < this.coordPropertiesGrid.length; i++) {
      if(!this.coordPropertiesGrid[i]) {
        let row = [];
        for(let counter = 0; counter < this.width; counter++) {
          row.push({});
        }
        this.coordPropertiesGrid[i] = row;
      }
      else {
        this.coordPropertiesGrid[i].length = this.width;
      }
    }
    for(let row of this.coordPropertiesGrid) {
      for(let i = 0; i < row.length; i++) {
        if(!row[i]) {
          row[i] = [];
        }
      }
    }
    for(let layer of this.layers) {
      let context = layer.getContext("2d");
      let imageData = context.getImageData(0, 0, this.width * consts.TILE_WIDTH, this.height * consts.TILE_WIDTH);
      layer.width = this.width * consts.TILE_WIDTH;
      layer.height = this.height * consts.TILE_WIDTH;
      context.putImageData(imageData, 0, 0);
    }
  }
  init() {
    this.coordPropertiesGrid = [];
    for(let i = 0; i < this.height; i++) {
      let row = [];
      for(let j = 0; j < this.width; j++) {
        row.push({});
      }
      this.coordPropertiesGrid.push(row);
    }
    if(this.tileInstances.length === 0) {
      for(let i = 0; i < this.height; i++) {
        let row = [];
        for(let j = 0; j < this.width; j++) {
          row.push(undefined);
        }
        this.tileInstances.push(row);
      }
    }
  }
  getNonSerializedKeys() {
    return ["layers", "coordPropertiesGrid"];
  }
  onSerialize() {
    this.coordProperties = [];
    for(let i = 0; i < this.coordPropertiesGrid.length; i++) {
      for(let j = 0; j < this.coordPropertiesGrid[i].length; j++) {
        let props = this.coordPropertiesGrid[i][j];
        if(!_.isEmpty(props)) {
          this.coordProperties.push({
            i: i,
            j: j,
            properties: props
          });
        }
      }
    }
  }
  setTileInstancesOnSerialize(levelCtx: CanvasRenderingContext2D, tileHashes: { [tileHash: string]: GridCoords }[], tileGrids: TileData[][][]) {
    this.tileInstances = [];
    for(let i = 0; i < this.height; i++) {
      let row: string[] = [];
      this.tileInstances.push(row)
      for(let j = 0; j < this.width; j++) {
        //@ts-ignore
        let tileHashKey = Helpers.getTileHash(levelCtx, i, j, this.name);
        if(!tileHashKey) {
          console.warn("Tile hash at " + String(i) + "," + String(j) + " not found!");
          row.push("");
          continue;
        }
        let id = "";
        let coords: GridCoords = undefined;
        for(let n = 0; n < tileHashes.length; n++) {
          coords = tileHashes[n][tileHashKey];
          if(coords) {
            let tileData = tileGrids[n][coords.i][coords.j];
            id = tileData.getId();
            break;
          }
        }
        row.push(id);
      }
    }
  }
  onDeserialize() {
    this.init();
    for(let coordProperty of this.coordProperties) {
      this.coordPropertiesGrid[coordProperty.i][coordProperty.j] = coordProperty.properties;
    }
  }
  destroy() {
    for(let layer of this.layers) {
      layer.remove();
    }
  }
}
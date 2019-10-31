import { TileData } from "./TileData";
import { Rect } from "./Rect";
import * as _ from "lodash";
import { GridRect } from "./GridRect";

export class TileAnimation {
  frameTime: number = 0.16;  //Time for each frame of animation
  tiles: TileData[] = [];
  tileIds: string[] = [];
  constructor(tiles: TileData[], frameTime: number) {
    this.tiles = tiles;
    this.frameTime = frameTime;
  }
  get tilesetPath() {
    return this.tiles[0].tileset.path;
  }
  getNonSerializedKeys() {
    return ["tiles"];
  }
  onSerialize() {
    this.tileIds = _.map(this.tiles, (tile) => {
      return tile.getId();
    });
  }
  setTileDatas(tileDatas: TileData[]) {
    this.tiles = _.map(this.tileIds, (tileId) => {
      return _.find(tileDatas, (tileData: TileData) => { return tileData.getId() === tileId; });
    });
  }
  get rect(): GridRect {
    let minI = _.minBy(this.tiles, (tile) => { return tile.gridCoords.i; }).gridCoords.i;
    let maxI = _.maxBy(this.tiles, (tile) => { return tile.gridCoords.i; }).gridCoords.i;
    let minJ = _.minBy(this.tiles, (tile) => { return tile.gridCoords.j; }).gridCoords.j;
    let maxJ = _.maxBy(this.tiles, (tile) => { return tile.gridCoords.j; }).gridCoords.j;
    return new GridRect(minI, minJ, maxI, maxJ);
  }
}
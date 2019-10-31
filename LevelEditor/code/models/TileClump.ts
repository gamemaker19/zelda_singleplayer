import { TileData } from "./TileData";
import { Rect } from "./Rect";
import * as _ from "lodash";
import { GridRect } from "./GridRect";

export class TileClump {
  name: string = "";
  tiles: TileData[] = [];
  tileIds: string[] = [];
  tag: string = "";
  constructor(tiles: TileData[], name: string) {
    this.tiles = tiles;
    this.name = name;
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
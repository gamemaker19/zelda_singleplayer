import { TileData } from "./TileData";
import * as Helpers from "../helpers";
import { Point } from "./point";
import { Rect } from "./Rect";
import { consts } from "../consts";
import * as _ from "lodash";
import { Level } from "./Level";
import { GridCoords } from "./GridCoords";

export class TileInstance {
  tileData: TileData;
  tileDataId: string;
  destPoint: GridCoords;
  constructor(tileData: TileData, destPoint: GridCoords) {
    this.tileData = tileData;
    this.destPoint = destPoint;
  }
  getNonSerializedKeys() {
    return ["tileData"];
  }
  onSerialize() {
    this.tileDataId = this.tileData.getId();
  }
  onDeserialize() {
  }
  setTileData(tileDatas: TileData[]) {
    this.tileData = _.find(tileDatas, (tileData: TileData) => { return tileData.getId() === this.tileDataId; });
  }
  draw(ctx: CanvasRenderingContext2D) {
    let rect = this.tileData.gridCoords.getRect();
    Helpers.drawImage(ctx, this.tileData.tileset.imgEl, rect.x1, rect.y1, rect.w, rect.h, this.destPoint.j * consts.TILE_WIDTH, this.destPoint.i * consts.TILE_WIDTH);
  }
  getRect() {
    return this.destPoint.getRect();
  }

}
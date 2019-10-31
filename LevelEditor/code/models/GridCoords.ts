import * as Helpers from "../helpers";
import { Rect } from "./Rect";
import { consts } from "../consts";

export class GridCoords {
  i: number;
  j: number;
  constructor(i: number, j: number) {
    this.i = i;
    this.j = j;
  }
  getRect() {
    return new Rect(this.j * consts.TILE_WIDTH, this.i * consts.TILE_WIDTH, (this.j + 1) * consts.TILE_WIDTH, (this.i + 1) * consts.TILE_WIDTH);
  }
  getRectCustomWidth(width: number) {
    return new Rect(this.j * width, this.i * width, (this.j + 1) * width, (this.i + 1) * width);
  }
  clone() {
    return new GridCoords(this.i, this.j);
  }
  equals(other: GridCoords) {
    if(!other) return false;
    return this.i === other.i && this.j === other.j;
  }
}
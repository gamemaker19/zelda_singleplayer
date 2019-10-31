import { Point } from "./point";
import { Shape } from "./Shape";
import { GridCoords } from "./GridCoords";
import { Rect } from "./Rect";
import { consts } from "../consts";

export class GridRect {
  
  topLeftGridCoords: GridCoords;
  botRightGridCoords: GridCoords;

  constructor(i1: number, j1: number, i2: number, j2: number) {
    this.topLeftGridCoords = new GridCoords(i1, j1);
    this.botRightGridCoords = new GridCoords(i2, j2);
  }
  toString() {
    return String(this.topLeftGridCoords.i) + "_" + String(this.topLeftGridCoords.j) + "_" + String(this.botRightGridCoords.i) + "_" + String(this.botRightGridCoords.j);
  }
  equals(other: GridRect) {
    return this.topLeftGridCoords.equals(other.topLeftGridCoords) && this.botRightGridCoords.equals(other.botRightGridCoords);
  }
  getRect() {
    return new Rect(this.j1 * consts.TILE_WIDTH, this.i1 * consts.TILE_WIDTH, (this.j2 + 1) * consts.TILE_WIDTH, (this.i2 + 1) * consts.TILE_WIDTH);
  }
  get i1() { return this.topLeftGridCoords.i; }
  get j1() { return this.topLeftGridCoords.j; }
  get i2() { return this.botRightGridCoords.i; }
  get j2() { return this.botRightGridCoords.j; }
}
import * as _ from "lodash";
import { Point } from "./point";

export class Obj {
  name: string;
  spriteOrImage: string;
  snapToTile: boolean;
  snapOffset: Point;
  constructor(name: string, image: string, snapToTile: boolean, snapOffset: Point) {
    this.name = name;
    this.spriteOrImage = image;
    this.snapToTile = snapToTile;
    this.snapOffset = snapOffset;
  }


}
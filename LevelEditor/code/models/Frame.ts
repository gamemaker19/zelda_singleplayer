import { Rect } from "./Rect";
import { Point } from "./Point";
import { Hitbox } from "./Hitbox";
import { POI } from "./POI";

export class Frame {

  rect: Rect;
  duration: number;
  offset: Point;
  hitboxes: Hitbox[];
  POIs: POI[];
  childFrames: Frame[] = [];
  parentFrameIndex: number;
  zIndex: number = 0;
  xDir: number = 1;
  yDir: number = 1;
  tags: string = "";

  constructor(rect: Rect, duration: number, offset: Point) {
    this.rect = rect;
    this.duration = duration;
    this.offset = offset;
    this.hitboxes = [];
    this.POIs = [];
  }

}
import { Point } from "./Point";
import { Rect } from "./Rect";
import { Selectable } from "../selectable";

export class Hitbox implements Selectable {

  tags: string;
  width: number;
  height: number;
  offset: Point;

  constructor() {
    this.tags = "";
    this.width = 20;
    this.height = 40;
    this.offset = new Point(0,0);
  }

  move(deltaX: number, deltaY: number) {
    this.offset.x += deltaX;
    this.offset.y += deltaY;
  }

  resizeCenter(w: number, h: number) {
    this.width += w;
    this.height += h;
  }

  getRect() {
    return new Rect(this.offset.x, this.offset.y, this.offset.x + this.width, this.offset.y + this.height);
  }

}
import { Point } from "./point";
import { runInThisContext } from "vm";
import { Selectable } from "../selectable";
import { Rect } from "./Rect";

export class POI implements Selectable {
  tags: string;
  x: number;
  y: number;
  constructor(tags: string, x: number, y: number) {
    this.tags = tags;
    this.x = x;
    this.y = y;
  }
  move(deltaX: number, deltaY: number): void {
    this.x += deltaX;
    this.y += deltaY;
  }
  resizeCenter(w: number, h: number): void {
  }
  getRect(): Rect {
    return new Rect(this.x - 2, this.y - 2, this.x + 2, this.y + 2);
  }
}
import { Point } from "./models/Point";

export class HitData {
  normal: Point;
  hitPoint: Point;
  constructor(normal: Point, hitPoint: Point) {
    this.normal = normal;
    this.hitPoint = hitPoint;
  }
}
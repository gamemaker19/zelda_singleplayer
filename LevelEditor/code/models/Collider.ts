import { Point } from "./Point";
import { Shape } from "./Shape";

export class Collider {

  _shape: Shape;
  isTrigger: boolean;
  wallOnly: boolean = false;
  isClimbable: boolean = true;
  //gameObject: GameObject;
  offset: Point;
  isStatic: boolean = false;
  flag: number = 0;

  constructor(points: Point[], isTrigger: boolean, isClimbable: boolean, isStatic: boolean, flag: number, offset: Point) {
    this._shape = new Shape(points);
    this.isTrigger = isTrigger;
    //this.gameObject = gameObject;
    this.isClimbable = isClimbable;
    this.isStatic = isStatic;
    this.flag = flag;
    this.offset = offset;
  }

  get shape() {
    let offset = new Point(0, 0);
    return this._shape.clone(offset.x, offset.y);
  }

  /*
  clone(x: number, y: number, gameObject: GameObject) {
    let shape = this.shape.clone(x, y);
    //@ts-ignore
    return _.cloneDeep(this);
  }
  */

}
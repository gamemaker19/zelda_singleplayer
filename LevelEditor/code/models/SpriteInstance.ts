import { Point } from "./point";
import { Sprite } from "./Sprite";
import { Obj } from "./Obj";
import * as _ from "lodash";
import { Rect } from "./Rect";

export class SpriteInstance {
  name: string;
  properties: string;
  pos: Point;
  sprite: Sprite;
  obj: Obj;
  constructor(name: string, x: number, y: number, obj: Obj, sprites: Sprite[]) {
    this.name = name;
    this.pos = new Point(x, y);  
    this.obj = obj;
    this.sprite = _.find(sprites, (sprite) => {
      return sprite.name === this.obj.spriteOrImage;
    });
  }
  setSprite(sprites: Sprite[]) {
    this.sprite = _.find(sprites, (sprite) => {
      return sprite.name === this.obj.spriteOrImage;
    });
  }
  getPositionalRect() {
    let w = this.sprite.frames[0].rect.w;
    let h = this.sprite.frames[0].rect.h;
    let x1 = this.pos.x - w/2;
    let y1 = this.pos.y - h/2;
    if(this.sprite.alignment === "topleft") {
      x1 += w/2;
      y1 += h/2;
    }
    let x2 = x1 + w;
    let y2 = y1 + h;
    let rect = new Rect(x1, y1, x2, y2);
    return rect;
  }
  getNonSerializedKeys() {
    return ["sprite"];
  }
  draw(ctx: CanvasRenderingContext2D) {
    if(this.sprite && this.sprite.spritesheet && this.sprite.spritesheet.imgEl) {
      this.sprite.draw(ctx, 0, this.pos.x, this.pos.y);
    }
    else if(this.sprite) {
      /*
      c1.drawImage(
        this.nonSpriteImgEl,
        Math.round(0), //source x
        Math.round(0), //source y
        Math.round(this.nonSpriteImgEl.width), //source width
        Math.round(this.nonSpriteImgEl.height), //source height
        Math.round(this.pos.x - ICON_WIDTH/2),  //dest x
        Math.round(this.pos.y - ICON_WIDTH / 2),  //dest y
        Math.round(ICON_WIDTH), //dest width
        Math.round(ICON_WIDTH)  //dest height
      );
      */
    }
  }
}
export class ShapeInstance {
  name: string;
  properties: string;
  constructor(name: string, properties: string) {
    this.name = name;
    this.properties = properties;
  }
  draw(ctx: CanvasRenderingContext2D) {

  }
}
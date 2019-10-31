import { CanvasUI } from "../canvasUI";

export class Spritesheet {

  imgEl: HTMLImageElement = undefined;
  path: string = "";

  constructor(path: string) {
    this.path = path;
  }

  get imgArr() {
    //@ts-ignore
    return window.imgArrMap[this.path];
  }

  set imgArr(imgArr: any) {
    //@ts-ignore
    if(!window.imgArrMap) window.imgArrMap = {};
    //@ts-ignore
    window.imgArrMap[this.path] = imgArr;
  }

  loadImage(callback: Function) {
    this.imgEl = document.createElement("img");
    this.imgEl.onload = () => {
      callback();
    };
    this.imgEl.src = this.path;
  }

}
import Vue from "vue";
import * as _ from "lodash";
import { Sprite } from "./models/Sprite";
import { Frame } from "./models/Frame";
import { Spritesheet } from "./models/Spritesheet";
import * as Helpers from "./helpers";
import { Hitbox } from "./models/Hitbox";
import { Rect } from "./models/Rect";
import { Point } from "./models/Point";
import { CanvasUI, MouseButton, KeyCode } from "./canvasUI";
import { Selectable } from "./selectable";
import { GlobalInput } from "./globalInput";
import { Level } from "./models/Level";
import { Obj } from "./models/Obj";
import { SpriteInstance } from "./models/SpriteInstance";
import { getObjectList } from "./objects";
import { TileData, HitboxMode, ZIndex } from "./models/TileData";
import { TileAnimation } from "./models/TileAnimation";
import { TileClump } from "./models/TileClump";
import { TileInstance } from "./models/TileInstance";
import { consts } from "./consts";
import { GridCoords } from "./models/GridCoords";
import { GridRect } from "./models/GridRect";
import { Shape } from "./models/Shape";
import { toUnicode } from "punycode";

class Data {
  constructor() {
  }
}

//@ts-ignore
window.initImageEditor = function() {

let data = new Data();
//@ts-ignore
window.data = data;

class ImageEditorInput extends GlobalInput {

  constructor() {
    super();
  }

  onKeyDown(keyCode: KeyCode, firstFrame: boolean) {
  }

  onKeyUp(keyCode: KeyCode) {
  }

}
new ImageEditorInput();

class ImageCanvas extends CanvasUI {

  uiCanvas: HTMLCanvasElement;
  uiCtx: CanvasRenderingContext2D;
  selection: GridCoords;

  constructor() {
    super("#level-canvas", "rgba(0,0,0,0)");
    this.isNoScrollZoom = false;
    this.zoom = 1;
    this.uiCanvas = <HTMLCanvasElement>document.getElementById("ui-canvas");
    this.uiCtx = this.uiCanvas.getContext("2d");
    
    let imgEl = document.createElement("img");
    imgEl.src = "assets/spritesheets/tileanims.png";
    imgEl.onload = () => {
      this.setSize(imgEl.width, imgEl.height);
      this.ctx.drawImage(imgEl, 0, 0);
      this.redrawUICanvas();
    };
  }

  setSize(width: number, height: number) {
    this.canvas.width = width;
    this.canvas.height = height;
    this.baseWidth = width;
    this.baseHeight = height;
    this.uiCanvas.width = width;
    this.uiCanvas.height = height;
  }

  redrawUICanvas() {
    if(this.isNoScrollZoom) {
      this.uiCtx.setTransform(this.zoom, 0, 0, this.zoom, -this.offsetX, -this.offsetY);
    }
    else {
      this.uiCanvas.width = this.baseWidth * this.zoom;
      this.uiCanvas.height = this.baseHeight * this.zoom;
      this.uiCtx.scale(this.zoom, this.zoom);
    }
    this.uiCtx.clearRect(0, 0, this.canvas.width, this.canvas.height);
    if(true) {
      //Draw columns
      for(let i = 1; i < this.canvas.width / consts.TILE_WIDTH; i++) {
        Helpers.drawLine(this.uiCtx, i * consts.TILE_WIDTH, 0, i * consts.TILE_WIDTH, this.canvas.height, "red", 1);
      }
      //Draw rows
      for(let i = 1; i < this.canvas.height / consts.TILE_WIDTH; i++) {
        Helpers.drawLine(this.uiCtx, 0, i * consts.TILE_WIDTH, this.canvas.width, i * consts.TILE_WIDTH, "red", 1);
      }
    }
    if(this.selection) {
      let mouseX = this.getMouseGridCoords().j * 16;
      let mouseY = this.getMouseGridCoords().i * 16;
      this.uiCtx.drawImage(this.canvas, this.selection.j * 16, this.selection.i * 16, 16, 16, mouseX, mouseY, 16, 16);
      Helpers.drawRect(this.uiCtx, this.selection.getRect(), "", "green", 2, 1);
    }
  }

  redraw() {
    //super.redraw();
  }

  onMouseWheel(delta: number) {
    resetVue();
  }

  onMouseMove() {
    this.redrawUICanvas();
  }
  
  onMouseLeave() {
  }

  onLeftMouseDown() {
    if(!this.selection) {
      this.selection = this.getMouseGridCoords();
      this.redrawUICanvas();
    }
    else if(this.getMouseGridCoords().j > 4) {
      let mouseX = this.getMouseGridCoords().j * 16;
      let mouseY = this.getMouseGridCoords().i * 16;
      this.ctx.drawImage(this.canvas, this.selection.j * 16, this.selection.i * 16, 16, 16, mouseX, mouseY, 16, 16);
    }
  }

  onLeftMouseUp() {
  }

  onKeyDown(keyCode: KeyCode, firstFrame: boolean) {
    if(keyCode == KeyCode.ESCAPE) {
      this.selection = undefined;
      this.redrawUICanvas();
    }
  }
}
let imageCanvas = new ImageCanvas();

let methods = {
};

let computed = {
}

var app1 = new Vue({
  el: '#app1',
  data: data,
  computed: computed,
  methods: methods,
  created: function() {
  }
});

//@ts-ignore
window.app1 = app1;

function resetVue() {
  app1.$forceUpdate();
}

}
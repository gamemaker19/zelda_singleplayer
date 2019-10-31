import { Rect } from "./models/Rect";
import * as Helpers from "./helpers";
import { consts } from "./consts";
import { GridCoords } from "./models/GridCoords";
import { GridRect } from "./models/GridRect";

export class CanvasUI {
  
  canvas: HTMLCanvasElement;
  ctx: CanvasRenderingContext2D;
  wrapper: HTMLElement;
  ctrlHeld: boolean = false;
  baseWidth: number = 0;
  baseHeight: number = 0;
  zoom: number = 1;
  noScrollZoom: number = 1;
  mousedown: boolean = false;
  middlemousedown: boolean = false;
  rightmousedown: boolean = false;
  keysHeld: Set<KeyCode> = new Set();
  color: string = "white";
  isNoScrollZoom: boolean = false;  //Determines whether to use zoom with an outer scroll div, or just hard canvas zoom (without outer scrollbars)
  offsetX: number = 0;
  offsetY: number = 0;

  //All these mouse values below are "normalized" to the current zoom. Only rawMouseX and rawMouseY variables are the real values
  mouseX: number = 0;
  mouseY: number = 0;
  deltaX: number = 0;
  deltaY: number = 0;
  lastClickX: number = 0;
  lastClickY: number = 0;
  private dragStartX: number;
  private dragStartY: number;
  private dragEndX: number;
  private dragEndY: number;
  get dragLeftX() { return Math.min(this.dragStartX, this.dragEndX); }
  get dragRightX() { return Math.max(this.dragStartX, this.dragEndX); }
  get dragTopY() { return Math.min(this.dragStartY, this.dragEndY); }
  get dragBotY() { return Math.max(this.dragStartY, this.dragEndY); }

  getDragGridRect() {
    return new GridRect(Math.floor(this.dragTopY / consts.TILE_WIDTH), Math.floor(this.dragLeftX / consts.TILE_WIDTH),
      Math.floor(this.dragBotY / consts.TILE_WIDTH), Math.floor(this.dragRightX / consts.TILE_WIDTH));
  }

  /*
  get gridMouseX() {
    return Math.floor(this.mouseX / consts.TILE_WIDTH);
  }

  get gridMouseY() {
    return Math.floor(this.mouseY / consts.TILE_WIDTH);
  }
  */

  getMouseGridCoords() {
    return new GridCoords(Math.floor(this.mouseY / consts.TILE_WIDTH), Math.floor(this.mouseX / consts.TILE_WIDTH));
  }

  getMouseGridCoordsCustomWidth(width: number) {
    return new GridCoords(Math.floor(this.mouseY / width), Math.floor(this.mouseX / width));
  }

  setSize(width: number, height: number) {
    this.canvas.width = width;
    this.canvas.height = height;
    this.baseWidth = width;
    this.baseHeight = height;
  }

  saveScrollPos(imageKey: string) {
    let scrollTop = this.wrapper.scrollTop;
    let scrollLeft = this.wrapper.scrollLeft;
    let scrollTopKey = document.title + "_" + this.constructor.name + "_" + imageKey + "_scrollTop";
    let scrollLeftKey = document.title + "_" + this.constructor.name + + "_" + imageKey + "_scrollLeft";
    localStorage.setItem(scrollTopKey, String(scrollTop));
    localStorage.setItem(scrollLeftKey, String(scrollLeft));
  }

  loadScrollPos(imageKey: string) {
    let scrollTopKey = document.title + "_" + this.constructor.name + "_" + imageKey + "_scrollTop";
    let scrollLeftKey = document.title + "_" + this.constructor.name + + "_" + imageKey + "_scrollLeft";
    let scrollTop = localStorage.getItem(scrollTopKey);
    let scrollLeft = localStorage.getItem(scrollLeftKey);
    if(scrollTop) this.wrapper.scrollTop = Number(scrollTop);
    if(scrollLeft) this.wrapper.scrollLeft = Number(scrollLeft);
  }

  constructor(canvasId: string, color?: string) {
    if(color) this.color = color;

    this.canvas = <HTMLCanvasElement>$(canvasId)[0];
    this.ctx = this.canvas.getContext("2d");
    this.wrapper = $(this.canvas).parent()[0];

    this.baseWidth = this.canvas.width;
    this.baseHeight = this.canvas.height;

    //this.ctx.webkitImageSmoothingEnabled = false;
    //this.ctx.mozImageSmoothingEnabled = false;
    this.ctx.imageSmoothingEnabled = false; /// future

    this.wrapper.onkeydown = (e: KeyboardEvent) => {
      let keyCode = <KeyCode>e.keyCode;
      this.onKeyDown(keyCode, !this.keysHeld.has(keyCode));
      this.keysHeld.add(keyCode);
      e.preventDefault();
    }
    
    this.wrapper.onkeyup = (e: KeyboardEvent) => {
      let keyCode = <KeyCode>e.keyCode;
      this.keysHeld.delete(keyCode);
      this.onKeyUp(e.keyCode);
      e.preventDefault();
    }
  
    this.wrapper.onscroll = (event: UIEvent) => {

    }

    this.canvas.onmousemove = (event: MouseEvent) => {
      
      let oldMouseX = this.mouseX;
      let oldMouseY = this.mouseY;

      let offsetLeft = this.wrapper.offsetLeft;
      let scrollLeft = this.wrapper.scrollLeft;
      let offsetTop = this.wrapper.offsetTop;
      let scrollTop = this.wrapper.scrollTop;

      let rawMouseX = event.pageX - offsetLeft + scrollLeft;
      let rawMouseY = event.pageY - offsetTop + scrollTop;

      if(!this.isNoScrollZoom) {
        this.mouseX = rawMouseX / this.zoom;
        this.mouseY = rawMouseY / this.zoom;
      }
      else {
        this.mouseX = rawMouseX;
        this.mouseY = rawMouseY;
      }

      this.mouseX += this.offsetX;
      this.mouseY += this.offsetY;

      this.deltaX = this.mouseX - oldMouseX;
      this.deltaY = this.mouseY - oldMouseY;

      if(this.mousedown) {
        this.dragEndX = this.mouseX;
        this.dragEndY = this.mouseY;
      }

      this.onMouseMove(this.deltaX, this.deltaY);

    }

    this.canvas.onmousedown = (e: MouseEvent) => {
      //console.log(mouseX + "," + mouseY)
      if(e.which === 1) {
        this.wrapper.focus();
        if(!this.mousedown) {
          this.lastClickX = this.dragStartX;
          this.lastClickY = this.dragStartY;
          this.dragStartX = this.mouseX;
          this.dragStartY = this.mouseY;
          this.dragEndX = this.mouseX;
          this.dragEndY = this.mouseY;
        }
        this.mousedown = true;
        e.preventDefault();
        this.onLeftMouseDown();
      }
      else if(e.which === 2) {
        this.middlemousedown = true;
        e.preventDefault();
        //this.onMouseDown(MouseButton.MIDDLE);
      }
      else if(e.which === 3) {
        this.rightmousedown = true;
        e.preventDefault();
        //this.onMouseDown(MouseButton.RIGHT);
      }

    }

    this.canvas.onmouseup = (e: MouseEvent) => {  
      if(e.which === 1) {
        this.mousedown = false;
        e.preventDefault();
        this.onLeftMouseUp();
      }
      else if(e.which === 2) {
        this.middlemousedown = false;
        e.preventDefault();
        //this.onMouseUp(MouseButton.MIDDLE);
      }
      else if(e.which === 3) {
        this.rightmousedown = true;
        e.preventDefault();
        //this.onMouseUp(MouseButton.RIGHT);
      }
    }
    
    this.canvas.onwheel = (e: MouseWheelEvent) => {
      if(this.isHeld(KeyCode.CONTROL)) {
        let delta = -(e.deltaY/180);
        this.zoom += delta;
        if(this.zoom < 1) this.zoom = 1;
        if(this.zoom > 5) this.zoom = 5;
        this.redraw();
        this.onMouseWheel(delta);
        e.preventDefault();
        return false;
      }
    }

    this.canvas.onmouseleave = () => {
      this.mousedown = false;
      this.onMouseLeave();
    }

  }

  isHeld(keyCode: KeyCode) {
    return this.keysHeld.has(keyCode);
  }

  redraw() {
    if(this.isNoScrollZoom) {
      this.ctx.setTransform(this.zoom, 0, 0, this.zoom, -(this.zoom - 1) * this.canvas.width/2, -(this.zoom - 1) * this.canvas.height/2);
    }
    else {
      this.canvas.width = this.baseWidth * this.zoom;
      this.canvas.height = this.baseHeight * this.zoom;
      this.ctx.scale(this.zoom, this.zoom);
    }

    this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
    Helpers.drawRect(this.ctx, new Rect(0, 0, this.canvas.width, this.canvas.height), this.color, "", null);
  }

  onMouseLeave() {}

  onMouseMove(deltaX: number, deltaY: number) {
  }
  
  onLeftMouseDown() {
  }

  onLeftMouseUp() {
  }

  onKeyDown(keyCode: KeyCode, firstFrame: boolean) {
  }

  onKeyUp(keyCode: KeyCode) {

  }

  onMouseWheel(delta: number) {
    
  }

}

export enum MouseButton {
  LEFT = 1,
  MIDDLE = 2,
  RIGHT = 3
}

export enum KeyCode {
  CANCEL = 3,
  HELP = 6,
  BACK_SPACE = 8,
  TAB = 9,
  CLEAR = 12,
  ENTER = 13,
  ENTER_SPECIAL = 14,
  SHIFT = 16,
  CONTROL = 17,
  ALT = 18,
  PAUSE = 19,
  CAPS_LOCK = 20,
  KANA = 21,
  EISU = 22,
  JUNJA = 23,
  FINAL = 24,
  HANJA = 25,
  ESCAPE = 27,
  CONVERT = 28,
  NONCONVERT = 29,
  ACCEPT = 30,
  MODECHANGE = 31,
  SPACE = 32,
  PAGE_UP = 33,
  PAGE_DOWN = 34,
  END = 35,
  HOME = 36,
  LEFT = 37,
  UP = 38,
  RIGHT = 39,
  DOWN = 40,
  SELECT = 41,
  PRINT = 42,
  EXECUTE = 43,
  PRINTSCREEN = 44,
  INSERT = 45,
  DELETE = 46,
  NUM_0 = 48,
  NUM_1 = 49,
  NUM_2 = 50,
  NUM_3 = 51,
  NUM_4 = 52,
  NUM_5 = 53,
  NUM_6 = 54,
  NUM_7 = 55,
  NUM_8 = 56,
  NUM_9 = 57,
  COLON = 58,
  SEMICOLON = 59,
  LESS_THAN = 60,
  EQUALS = 61,
  GREATER_THAN = 62,
  QUESTION_MARK = 63,
  AT = 64,
  A = 65,
  B = 66,
  C = 67,
  D = 68,
  E = 69,
  F = 70,
  G = 71,
  H = 72,
  I = 73,
  J = 74,
  K = 75,
  L = 76,
  M = 77,
  N = 78,
  O = 79,
  P = 80,
  Q = 81,
  R = 82,
  S = 83,
  T = 84,
  U = 85,
  V = 86,
  W = 87,
  X = 88,
  Y = 89,
  Z = 90,
  SLEEP = 95,
  NUMPAD0 = 96,
  NUMPAD1 = 97,
  NUMPAD2 = 98,
  NUMPAD3 = 99,
  NUMPAD4 = 100,
  NUMPAD5 = 101,
  NUMPAD6 = 102,
  NUMPAD7 = 103,
  NUMPAD8 = 104,
  NUMPAD9 = 105,
  MULTIPLY = 106,
  ADD = 107,
  SEPARATOR = 108,
  SUBTRACT = 109,
  DECIMAL = 110,
  DIVIDE = 111,
  F1 = 112,
  F2 = 113,
  F3 = 114,
  F4 = 115,
  F5 = 116,
  F6 = 117,
  F7 = 118,
  F8 = 119,
  F9 = 120,
  F10 = 121,
  F11 = 122,
  F12 = 123,
  F13 = 124,
  F14 = 125,
  F15 = 126,
  F16 = 127,
  F17 = 128,
  F18 = 129,
  F19 = 130,
  F20 = 131,
  F21 = 132,
  F22 = 133,
  F23 = 134,
  F24 = 135,
  NUM_LOCK = 144,
  SCROLL_LOCK = 145,
  WIN_OEM_FJ_JISHO = 146,
  WIN_OEM_FJ_MASSHOU = 147,
  WIN_OEM_FJ_TOUROKU = 148,
  WIN_OEM_FJ_LOYA = 149,
  WIN_OEM_FJ_ROYA = 150,
  CIRCUMFLEX = 160,
  EXCLAMATION = 161,
  DOUBLE_QUOTE = 162,
  HASH = 163,
  DOLLAR = 164,
  PERCENT = 165,
  AMPERSAND = 166,
  UNDERSCORE = 167,
  OPEN_PAREN = 168,
  CLOSE_PAREN = 169,
  ASTERISK = 170,
  PLUS = 171,
  PIPE = 172,
  HYPHEN_MINUS = 173,
  OPEN_CURLY_BRACKET = 174,
  CLOSE_CURLY_BRACKET = 175,
  TILDE = 176,
  VOLUME_MUTE = 181,
  VOLUME_DOWN = 182,
  VOLUME_UP = 183,
  COMMA = 188,
  MINUS = 189,
  PERIOD = 190,
  SLASH = 191,
  BACK_QUOTE = 192,
};
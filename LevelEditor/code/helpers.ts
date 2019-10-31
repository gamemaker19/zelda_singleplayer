import { Rect } from "./models/Rect";
import { Point } from "./models/Point";
import { Shape } from "./models/Shape";
import { Color } from "./color";
import * as _ from "lodash";
import { createClassFromName } from "./classes";
import { consts } from "./consts";

export function inRect(x: number, y: number, rect: Rect): boolean {
  let rx:number = rect.x1;
  let ry:number = rect.y1;
  let rx2:number = rect.x2;
  let ry2:number = rect.y2;
  return x >= rx && x <= rx2 && y >= ry && y <= ry2;
}

export function inCircle(x: number, y: number, circleX: number, circleY: number, r: number): boolean {

  if(Math.sqrt( Math.pow(x - circleX, 2) + Math.pow(y - circleY, 2)) <= r) {
      return true;
  }
  return false;
}

export function toZero(num: number, inc: number, dir: number) {
  if(dir === 1) {
    num -= inc;
    if(num < 0) num = 0;
    return num;
  }
  else if(dir === -1) {
    num += inc;
    if(num > 0) num = 0;
    return num;
  }
  else {
    throw "Must pass in -1 or 1 for dir";
  }
}

export function incrementRange(num: number, min: number, max: number) {
  num++;
  if(num >= max) num = min;
  return num;
}

export function decrementRange(num: number, min: number, max: number) {
  num--;
  if(num < min) num = max - 1;
  return num;
}

export function clamp01(num: number) {
  if(num < 0) num = 0;
  if(num > 1) num = 1;
  return num;
}

//Inclusive
export function randomRange(start: number, end: number) {
  /*
  end++;
  let dist = end - start;
  return Math.floor(Math.random() * dist) + start;
  */
  //@ts-ignore
  return _.random(start, end);
}

export function clampMax(num: number, max: number) {
  return num < max ? num : max;
}

export function clampMin(num: number, min: number) {
  return num > min ? num : min;
}

export function clampMin0(num: number) {
  return clampMin(num, 0);
}

export function clamp(num: number, min: number, max: number) {
  if(num < min) return min;
  if(num > max) return max;
  return num;
}

export function sin(degrees: number) {
  let rads = degrees * Math.PI / 180;
  return Math.sin(rads);
}

export function cos(degrees: number) {
  let rads = degrees * Math.PI / 180;
  return Math.cos(rads);
}

export function atan(value: number) {
  return Math.atan(value) * 180 / Math.PI;
}

export function moveTo(num: number, dest: number, inc: number) {
  inc *= Math.sign(dest - num);
  num += inc;
  return num;
}

export function lerp(num: number, dest: number, timeScale: number) {
  num = num + (dest - num)*timeScale;
  return num;
}

export function lerpNoOver(num: number, dest: number, timeScale: number) {
  num = num + (dest - num)*timeScale;
  if(Math.abs(num - dest) < 1) num = dest;
  return num;
}

//Expects angle and destAngle to be > 0 and < 360
export function lerpAngle(angle: number, destAngle: number, timeScale: number) {
  let dir = 1;
  if(Math.abs(destAngle - angle) > 180) {
    dir = -1;
  }
  angle = angle + dir*(destAngle - angle) * timeScale;
  return to360(angle);
}

export function to360(angle: number) {
  if(angle < 0) angle += 360;
  if(angle > 360) angle -= 360;
  return angle;
}

export function getHex(r: number, g: number, b: number, a: number) {
  return "#" + r.toString(16) + g.toString(16) + b.toString(16) + a.toString(16);
}

export function roundEpsilon(num: number) {
  let numRound = Math.round(num);
  let diff = Math.abs(numRound - num);
  if(diff < 0.0001) {
    return numRound;
  }
  return num;
}

let autoInc = 0;
export function getAutoIncId() {
  autoInc++;
  return autoInc;
}

export function stringReplace(str: string, pattern: string, replacement: string) {
  return str.replace(new RegExp(pattern, 'g'), replacement);
}

export function noCanvasSmoothing(c: CanvasRenderingContext2D) {
  c.webkitImageSmoothingEnabled = false;
  c.mozImageSmoothingEnabled = false;
  c.imageSmoothingEnabled = false; /// future
}

let helperCanvas = document.createElement("canvas");
let helperCtx = helperCanvas.getContext("2d");
noCanvasSmoothing(helperCtx);

let helperCanvas2 = document.createElement("canvas");
let helperCtx2 = helperCanvas2.getContext("2d");
noCanvasSmoothing(helperCtx2);

let helperCanvas3 = document.createElement("canvas");
let helperCtx3 = helperCanvas3.getContext("2d");
noCanvasSmoothing(helperCtx3);

export function drawImage(ctx: CanvasRenderingContext2D, imgEl: HTMLImageElement, sX: number, sY: number, sW?: number, sH?: number, 
  x?: number, y?: number, flipX?: number, flipY?: number, options?: string, alpha?: number, scaleX?: number, scaleY?: number): void {
  
  if(!sW) {
    ctx.drawImage(imgEl, (sX), sY);
    return;
  }

  ctx.globalAlpha = (alpha === undefined || alpha === undefined) ? 1 : alpha;

  helperCanvas.width = sW;
  helperCanvas.height = sH;
  
  helperCtx.save();
  scaleX = scaleX || 1;
  scaleY = scaleY || 1;
  flipX = (flipX || 1);
  flipY = (flipY || 1);
  helperCtx.scale(flipX * scaleX, flipY * scaleY);

  helperCtx.clearRect(0, 0, helperCanvas.width, helperCanvas.height);
  helperCtx.drawImage(
    imgEl,
    sX, //source x
    sY, //source y
    sW, //source width
    sH, //source height
    0,  //dest x
    0, //dest y
    flipX * sW, //dest width
    flipY * sH  //dest height
  );

  ctx.drawImage(helperCanvas, x, y);
  
  ctx.globalAlpha = 1;
  helperCtx.restore();
}

/*
export function createAndDrawRect(container: PIXI.Container, rect: Rect, fillColor?: number, strokeColor?: number, strokeWidth?: number, fillAlpha?: number): PIXI.Graphics {
  let rectangle = new PIXI.Graphics();
  if(fillAlpha === undefined) fillAlpha = 1;
  //if(!fillColor) fillColor = 0x00FF00;

  if(strokeColor) {
    rectangle.lineStyle(strokeWidth, strokeColor, fillAlpha);
  }

  if(fillColor !== undefined) 
    rectangle.beginFill(fillColor, fillAlpha);
  
  rectangle.drawRect(rect.x1, rect.y1, rect.w, rect.h);
  if(fillColor !== undefined)
    rectangle.endFill();
  
  container.addChild(rectangle);
  return rectangle;
}
*/

export function drawRect(ctx: CanvasRenderingContext2D, rect: Rect, fillColor?: string, strokeColor?: string, strokeWidth?: number, fillAlpha?: number): void {
  let rx: number = Math.round(rect.x1);
  let ry: number = Math.round(rect.y1);
  let rx2: number = Math.round(rect.x2);
  let ry2: number = Math.round(rect.y2);

  ctx.beginPath();
  ctx.rect(rx, ry, rx2 - rx, ry2 - ry);

  if(fillAlpha) {
    ctx.globalAlpha = fillAlpha;
  }

  if(strokeColor) {
    strokeWidth = strokeWidth ? strokeWidth : 1;
    ctx.lineWidth = strokeWidth;
    ctx.strokeStyle = strokeColor;
    ctx.stroke();
  }

  if(fillColor) {
    ctx.fillStyle = fillColor;
    ctx.fill();
  }

  ctx.globalAlpha = 1;
}

export function drawPolygon(ctx: CanvasRenderingContext2D, shape: Shape, closed: boolean, fillColor?: string, lineColor?: string, lineThickness?: number, fillAlpha?: number): void {

  let vertices = shape.points;

  if(fillAlpha) {
    ctx.globalAlpha = fillAlpha;
  }

  ctx.beginPath();
  ctx.moveTo(vertices[0].x, vertices[0].y);

  for(let i: number = 1; i < vertices.length; i++) {
      ctx.lineTo(vertices[i].x, vertices[i].y);
  }

  if(closed) {
      ctx.closePath();

      if(fillColor) {
          ctx.fillStyle = fillColor;
          ctx.fill();
      }
  }

  if(lineColor) {
      ctx.lineWidth = lineThickness;
      ctx.strokeStyle = lineColor;
      ctx.stroke();
  }

  ctx.globalAlpha = 1;
}

export function drawText(ctx: CanvasRenderingContext2D, text: string, x: number, y: number, fillColor: string, outlineColor: string, size: number, hAlign: string, vAlign: string, font: string) {
  ctx.save();
  fillColor = fillColor || "black";
  size = size || 14;
  hAlign = hAlign || "center";  //start,end,left,center,right
  vAlign = vAlign || "middle";  //Top,Bottom,Middle,Alphabetic,Hanging
  font = font || "Arial";
  ctx.font = size + "px " + font;
  ctx.fillStyle = fillColor;
  ctx.textAlign = hAlign;
  ctx.textBaseline = vAlign;
  ctx.fillText(text,x,y);
  if(outlineColor) {
    ctx.lineWidth = 1;
    ctx.strokeStyle = outlineColor;
    ctx.strokeText(text,x,y);
  }
  ctx.restore();
}

export function drawCircle(ctx: CanvasRenderingContext2D, x: number, y: number, r: number, fillColor?: string, lineColor?: string, lineThickness?: number) {
  ctx.beginPath();
  ctx.arc(x, y, r, 0, 2*Math.PI, false);
  
  if(fillColor) {
      ctx.fillStyle = fillColor;
      ctx.fill();
  }
  
  if(lineColor) {
      ctx.lineWidth = lineThickness;
      ctx.strokeStyle = lineColor;
      ctx.stroke();
  }

}

export function drawLine(ctx: CanvasRenderingContext2D, x: number, y: number, x2: number, y2: number, color?: string, thickness?: number) {

  if(!thickness) thickness = 1;
  if(!color) color = 'black';

  ctx.beginPath();
  ctx.moveTo(x, y);
  ctx.lineTo(x2, y2);
  ctx.lineWidth = thickness;
  ctx.strokeStyle = color;
  ctx.stroke();
}

export function linepointNearestMouse(x0: number, y0: number, x1: number, y1: number, x: number, y: number): Point {
  function lerp(a: number,b: number,x: number):number{ return(a+x*(b-a)); };
  let dx: number=x1-x0;
  let dy: number=y1-y0;
  let t: number = ((x-x0)*dx+(y-y0)*dy)/(dx*dx+dy*dy);
  let lineX: number = lerp(x0, x1, t);
  let lineY: number = lerp(y0, y1, t);
  return new Point(lineX,lineY);
}

export function inLine(mouseX: number, mouseY: number, x0: number, y0: number, x1: number, y1: number): boolean {

  let threshold: number = 4;

  let small_x: number = Math.min(x0,x1);
  let big_x: number = Math.max(x0,x1);

  if(mouseX < small_x - (threshold*0.5) || mouseX > big_x + (threshold*0.5)){
    return false;
  }

  let linepoint: Point = linepointNearestMouse(x0, y0, x1, y1, mouseX, mouseY);
  let dx: number = mouseX - linepoint.x;
  let dy: number = mouseY - linepoint.y;
  let distance: number = Math.abs(Math.sqrt(dx*dx+dy*dy));
  if(distance < threshold) {
    return true;
  }
  else {
    return false;
  }
}

export function getInclinePushDir(inclineNormal: Point, pushDir: Point) {
  let bisectingPoint = inclineNormal.normalize().add(pushDir.normalize());
  bisectingPoint = bisectingPoint.normalize();
  //Snap to the nearest axis
  if(Math.abs(bisectingPoint.x) >= Math.abs(bisectingPoint.y)) {
    bisectingPoint.y = 0;
  }
  else {
    bisectingPoint.x = 0;
  }
  return bisectingPoint.normalize();
}

export function keyCodeToString(charCode: number) {

  if(charCode === 0) return "left mouse";
  if(charCode === 1) return "middle mouse";
  if(charCode === 2) return "right mouse";
  if(charCode === 3) return "wheel up";
  if(charCode === 4) return "wheel down";

  if (charCode == 8) return "backspace"; //  backspace
  if (charCode == 9) return "tab"; //  tab
  if (charCode == 13) return "enter"; //  enter
  if (charCode == 16) return "shift"; //  shift
  if (charCode == 17) return "ctrl"; //  ctrl
  if (charCode == 18) return "alt"; //  alt
  if (charCode == 19) return "pause/break"; //  pause/break
  if (charCode == 20) return "caps lock"; //  caps lock
  if (charCode == 27) return "escape"; //  escape
  if (charCode == 33) return "page up"; // page up, to avoid displaying alternate character and confusing people	         
  if (charCode == 34) return "page down"; // page down
  if (charCode == 35) return "end"; // end
  if (charCode == 36) return "home"; // home
  if (charCode == 37) return "left arrow"; // left arrow
  if (charCode == 38) return "up arrow"; // up arrow
  if (charCode == 39) return "right arrow"; // right arrow
  if (charCode == 40) return "down arrow"; // down arrow
  if (charCode == 45) return "insert"; // insert
  if (charCode == 46) return "delete"; // delete
  if (charCode == 91) return "left window"; // left window
  if (charCode == 92) return "right window"; // right window
  if (charCode == 93) return "select key"; // select key
  if (charCode == 96) return "numpad 0"; // numpad 0
  if (charCode == 97) return "numpad 1"; // numpad 1
  if (charCode == 98) return "numpad 2"; // numpad 2
  if (charCode == 99) return "numpad 3"; // numpad 3
  if (charCode == 100) return "numpad 4"; // numpad 4
  if (charCode == 101) return "numpad 5"; // numpad 5
  if (charCode == 102) return "numpad 6"; // numpad 6
  if (charCode == 103) return "numpad 7"; // numpad 7
  if (charCode == 104) return "numpad 8"; // numpad 8
  if (charCode == 105) return "numpad 9"; // numpad 9
  if (charCode == 106) return "multiply"; // multiply
  if (charCode == 107) return "add"; // add
  if (charCode == 109) return "subtract"; // subtract
  if (charCode == 110) return "decimal point"; // decimal point
  if (charCode == 111) return "divide"; // divide
  if (charCode == 112) return "F1"; // F1
  if (charCode == 113) return "F2"; // F2
  if (charCode == 114) return "F3"; // F3
  if (charCode == 115) return "F4"; // F4
  if (charCode == 116) return "F5"; // F5
  if (charCode == 117) return "F6"; // F6
  if (charCode == 118) return "F7"; // F7
  if (charCode == 119) return "F8"; // F8
  if (charCode == 120) return "F9"; // F9
  if (charCode == 121) return "F10"; // F10
  if (charCode == 122) return "F11"; // F11
  if (charCode == 123) return "F12"; // F12
  if (charCode == 144) return "num lock"; // num lock
  if (charCode == 145) return "scroll lock"; // scroll lock
  if (charCode == 186) return ";"; // semi-colon
  if (charCode == 187) return "="; // equal-sign
  if (charCode == 188) return ","; // comma
  if (charCode == 189) return "-"; // dash
  if (charCode == 190) return "."; // period
  if (charCode == 191) return "/"; // forward slash
  if (charCode == 192) return "`"; // grave accent
  if (charCode == 219) return "["; // open bracket
  if (charCode == 220) return "\\"; // back slash
  if (charCode == 221) return "]"; // close bracket
  if (charCode == 222) return "'"; // single quote
  if (charCode == 32) return "space";
  return String.fromCharCode(charCode);
}

export function deserializeES6(obj: any) {

  if(Array.isArray(obj)) {
    for(var i = 0; i < obj.length; i++) {
      obj[i] = deserializeES6(obj[i]);
    }
  }
  else if(typeof obj === "object") {

    let className: string = obj.className;
    if(className) {
      //@ts-ignore
      //var tempObj = Object.create(window[className].prototype);
      let tempObj = createClassFromName(className);
      Object.assign(tempObj, obj);
      obj = tempObj;
    }

    if(obj.onDeserialize) {
      obj.onDeserialize();
    }

    for(var key in obj) {
      if(!obj.hasOwnProperty(key)) continue;
      obj[key] = deserializeES6(obj[key]);
    }
  }
  if(typeof obj === "string" && $.isNumeric(obj)) {
    obj = Number(obj);
  }
  if(typeof obj === "string" && obj === "true") {
    obj = true;
  }
  if(typeof obj === "string" && obj === "false") {
    obj = false;
  }
  return obj;
  
}

export function serializeES6(obj: any) {
  
  var retStr = "";

  if(Array.isArray(obj)) {
    retStr += "[";
    for(var i = 0; i < obj.length; i++) {
      retStr += serializeES6(obj[i]) + ",";
    }
    if(retStr[retStr.length-1] === ",") retStr = retStr.substring(0,retStr.length-1);
    retStr += "]";
  }
  else if(typeof obj === "object") {
    if(obj.onSerialize) {
      obj.onSerialize();
    }
    retStr += "{";
    let nonSerializedKeys = obj.getNonSerializedKeys ? obj.getNonSerializedKeys() : [];
    for(var key in obj) {
      if(nonSerializedKeys.indexOf(key) > -1) continue;
      /*
      if(key === "spritesheet") continue;
      if(key === "background") continue;
      if(key === "sprite") continue;
      if(key === "nonSpriteImgEl") continue;
      if(key === "obj") continue;
      if(key === "propertiesJson") continue;
      */
      if(!obj.hasOwnProperty(key)) continue;
      retStr += '"' + key + '":' + serializeES6(obj[key]) + ",";
    }
    retStr += '"className":"' + obj.constructor.name + '"}';
  }
  else {
    if(obj === undefined || obj === undefined || obj === "") {
      retStr += '""';
    }
    else if(isNaN(obj)) {
      retStr = '"' + String(obj) + '"';
    }
    else {
      retStr = String(obj);
    }
  }
  return retStr;
  
}

class PixelData {
  x: number;
  y: number;
  rgb: Color;
  neighbors: PixelData[];
  constructor(x: number, y: number, rgb: Color, neighbors: PixelData[]) {
    this.x = x;
    this.y = y;
    this.rgb = rgb;
    this.neighbors = neighbors;
  }
}

export function get2DArrayFromImage(imageData: ImageData) {
  let data = imageData.data;
  let arr = [];
  let row = [];
  for(let i=0; i<data.length; i+=4) {
    
    if(i % (imageData.width*4) === 0) {
      if(i > 0) {
        arr.push(row);
      }
      row = [];
    }

    let red = data[i];
    let green = data[i+1];
    let blue = data[i+2];
    let alpha = data[i+3];
    
    row.push(new PixelData(-1, -1, new Color(red, green, blue, alpha), []));

    if(i === data.length - 4) {
      arr.push(row);
    }
  }

  for(let i = 0; i < arr.length; i++) {
    for(let j = 0; j < arr[i].length; j++) {
      arr[i][j].x = j;
      arr[i][j].y = i;
    }
  }

  for(let i = 0; i < arr.length; i++) {
    for(let j = 0; j < arr[i].length; j++) {
      arr[i][j].neighbors.push(get2DArrayEl(arr, i-1, j-1));
      arr[i][j].neighbors.push(get2DArrayEl(arr, i-1, j));
      arr[i][j].neighbors.push(get2DArrayEl(arr, i-1, j+1));
      arr[i][j].neighbors.push(get2DArrayEl(arr, i, j-1));
      arr[i][j].neighbors.push(get2DArrayEl(arr, i, j));
      arr[i][j].neighbors.push(get2DArrayEl(arr, i, j+1));
      arr[i][j].neighbors.push(get2DArrayEl(arr, i+1, j-1));
      arr[i][j].neighbors.push(get2DArrayEl(arr, i+1, j));
      arr[i][j].neighbors.push(get2DArrayEl(arr, i+1, j+1));
      _.pull(arr[i][j].neighbors, undefined);
    }
  }

  return arr;
}

export function getPixelClumpRect(x: number, y: number, imageArr: PixelData[][]) {
  x = Math.round(x);
  y = Math.round(y);
  var selectedNode = imageArr[y][x];
  if(!selectedNode) {
    return undefined;
  }
  if(selectedNode.rgb.a === 0) {
    console.log("Clicked transparent pixel");
    return undefined;
  }

  var queue = [];
  queue.push(selectedNode);

  var minX = Infinity;
  var minY = Infinity;
  var maxX = -1;
  var maxY = -1;

  var num  = 0;
  var visitedNodes = new Set();
  while(queue.length > 0) {
    var node = queue.shift();
    num++;
    if(node.x < minX) minX = node.x;
    if(node.y < minY) minY = node.y;
    if(node.x > maxX) maxX = node.x;
    if(node.y > maxY) maxY = node.y;

    for(var neighbor of node.neighbors) {
      if(visitedNodes.has(neighbor)) continue;
      if(queue.indexOf(neighbor) === -1) {
        queue.push(neighbor);
      }
    }
    visitedNodes.add(node);
  }
  //console.log(num);
  return new Rect(Math.round(minX), Math.round(minY), Math.round(maxX+1), Math.round(maxY+1));

}

export function getSelectedPixelRect(x: number, y: number, endX: number, endY: number, imageArr: PixelData[][]) {
  
  x = Math.round(x);
  y = Math.round(y);

  var minX = Infinity;
  var minY = Infinity;
  var maxX = -1;
  var maxY = -1;

  for(var i = y; i <= endY; i++) {
    for(var j = x; j <= endX; j++) {
      if(imageArr[i][j].rgb.a !== 0) {
        if(i < minY) minY = i;
        if(i > maxY) maxY = i;
        if(j < minX) minX = j;
        if(j > maxX) maxX = j;
      }
    }
  }

  if(!isFinite(minX) || !isFinite(minY) || maxX === -1 || maxY === -1) return;

  return new Rect(Math.round(minX), Math.round(minY), Math.round(maxX+1), Math.round(maxY+1));
}

export function get2DArrayEl(arr: PixelData[][], i: number, j: number) {
  if(i < 0 || i >= arr.length) return undefined;
  if(j < 0 || j >= arr[0].length) return undefined;
  if(arr[i][j].rgb.a === 0) return undefined;
  return arr[i][j];
}

export function baseName(filepath: string)
{
   var base = new String(filepath).substring(filepath.lastIndexOf('/') + 1); 
    if(base.lastIndexOf(".") != -1)       
        base = base.substring(0, base.lastIndexOf("."));
   return base;
}

export function make2DArray(w: number, h: number, val: any) : any {
  var arr = [];
  for(let i = 0; i < h; i++) {
      arr[i] = [];
      for(let j = 0; j < w; j++) {
          arr[i][j] = val;
      }
  }
  return arr;
}
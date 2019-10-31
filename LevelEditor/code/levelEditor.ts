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
import { Line } from "./models/Line";
import { POINT_CONVERSION_COMPRESSED } from "constants";

enum Tool {
  Select = 0,
  PlaceTile,
  RectangleTile,
  SelectInstance,
  CreateInstance,
  PaintElevation
} 

class BFSNode {
  i: number = 0;
  j: number = 0;
  visited: boolean = false;
  constructor(i: number, j: number) {
    this.i = i;
    this.j = j;
    this.visited = false;
  }
}

class Data {
  spritesheets: Spritesheet[] = [];
  sprites: Sprite[] = [];
  levels: Level[] = [];
  selectedLevel: Level = undefined;
  objs: Obj[] = getObjectList();
  selectedObj: Obj = undefined;
  selectedInstances: SpriteInstance[] = [];
  isPlaying: boolean = false;
  zoom: number = 1;
  newLevelName: string = "";
  showInstances: boolean = true;
  showNavMesh: boolean = false;
  selectedTileset: Spritesheet = undefined;
  tilesets: Spritesheet[] = [];
  tileSelectedCoords: GridCoords[] = [];
  levelSelectedCoords: GridCoords[] = [];
  tileDatas: TileData[] = [];
  tileDataGrids: { [tilesetName: string]: TileData[][] } = {};
  tileAnimations: TileAnimation[] = [];
  tileClumps: TileClump[] = [];
  multiEditName: string = "";
  multiEditHitboxMode: number = 0;
  multiEditTag: string = "";
  multiEditZIndex: number = 0;
  multiEditTileSprite: string = "";
  showLevelGrid: boolean = true;
  selectedTool: Tool = 0;
  loadCount: number = -1;
  maxLoadCount: number = 0;
  selectionProperties: string = "";
  undoJsons: string[] = [];
  undoIndex: number = 0;
  selectionElevation: number = 0;
  selectedInstanceProperties: string = "";
  showTileHitboxes: boolean = false;
  showElevations: boolean = false;
  showTilesWithTag: string = "";
  showTilesWithZIndex1: boolean = false;
  showTilesWithSprite: string = "";
  paintElevationHeight: number = 0;
  layerIndex: number = 0;
  levelImages: Spritesheet[] = [];
  lastSelectedTileSprite: string = "";
  mode16x16: boolean = false;
  customHitboxPoints: string = "";
  showRoomLines: boolean = false;
  clonedTiles: GridRect = undefined;
  showOverridesWithKey: string = "";
  lastNavMeshCoords: string = "";
  constructor() {
  }
}

let hashCache: any = {};

//@ts-ignore
window.initLevelEditor = function() {

let data = new Data();
//@ts-ignore
window.data = data;

class LevelEditorInput extends GlobalInput {

  constructor() {
    super();
  }

  onKeyDown(keyCode: KeyCode, firstFrame: boolean) {
    
    if(data.selectedTool == Tool.SelectInstance) {
      if(keyCode === KeyCode.E && data.selectedInstances.length === 1) {
        let index = data.selectedLevel.instances.indexOf(data.selectedInstances[0]);
        index++;
        if(index >= data.selectedLevel.instances.length) index = 0;
        data.selectedInstances[0] = data.selectedLevel.instances[index];
        let instance = data.selectedInstances[0];
        levelCanvas.wrapper.scrollTop = instance.pos.y - $(levelCanvas.wrapper).height() / 2;
        levelCanvas.wrapper.scrollLeft = instance.pos.x - $(levelCanvas.wrapper).width() / 2;
        levelCanvas.redraw();
        levelCanvas.redrawUICanvas();
        resetVue();
      }
      else if(keyCode === KeyCode.Q && data.selectedInstances.length === 1) {
        let index = data.selectedLevel.instances.indexOf(data.selectedInstances[0]);
        index--;
        if(index < 0) index = data.selectedLevel.instances.length - 1;
        data.selectedInstances[0] = data.selectedLevel.instances[index];
        let instance = data.selectedInstances[0];
        levelCanvas.wrapper.scrollTop = instance.pos.y - $(levelCanvas.wrapper).height() / 2;
        levelCanvas.wrapper.scrollLeft = instance.pos.x - $(levelCanvas.wrapper).width() / 2;
        levelCanvas.redraw();
        levelCanvas.redrawUICanvas();
        resetVue();
      }
    }
  }

  onKeyUp(keyCode: KeyCode) {
  }

}
new LevelEditorInput();

class LevelCanvas extends CanvasUI {

  prevGridCoords: GridCoords;
  lastMouseMoveGridCoords: GridCoords;
  lastMouseMoveGridCoords8x8: GridCoords;
  mouseLeftCanvas: boolean = false;

  uiCanvas: HTMLCanvasElement;
  uiCtx: CanvasRenderingContext2D;

  constructor() {
    super("#level-canvas", "rgba(0,0,0,0)");
    this.isNoScrollZoom = false;
    this.zoom = 1;
    this.uiCanvas = <HTMLCanvasElement>document.getElementById("ui-canvas");
    this.uiCtx = this.uiCanvas.getContext("2d");
  }

  setSize(width: number, height: number) {
    this.canvas.width = width;
    this.canvas.height = height;
    this.baseWidth = width;
    this.baseHeight = height;
    this.uiCanvas.width = width;
    this.uiCanvas.height = height;
  }

  redrawUICanvas(redrawInstances: boolean = true) {
    let tileWidth = 8;
    if(data.mode16x16) tileWidth = 16;
    if(this.isNoScrollZoom) {
      this.uiCtx.setTransform(this.zoom, 0, 0, this.zoom, -this.offsetX, -this.offsetY);
    }
    else {
      this.uiCanvas.width = this.baseWidth * this.zoom;
      this.uiCanvas.height = this.baseHeight * this.zoom;
      this.uiCtx.scale(this.zoom, this.zoom);
    }
    this.uiCtx.clearRect(0, 0, this.canvas.width, this.canvas.height);
    if(data.showLevelGrid) {
      //Draw columns
      for(let i = 1; i < this.canvas.width / tileWidth; i++) {
        Helpers.drawLine(this.uiCtx, i * tileWidth, 0, i * tileWidth, this.canvas.height, "red", 1);
      }
      //Draw rows
      for(let i = 1; i < this.canvas.height / tileWidth; i++) {
        Helpers.drawLine(this.uiCtx, 0, i * tileWidth, this.canvas.width, i * tileWidth, "red", 1);
      }
      if(data.selectedLevel) {
        //Draw scroll lines
        for(let scrollLine of data.selectedLevel.scrollLines) {
          Helpers.drawLine(this.uiCtx, scrollLine.point1.x, scrollLine.point1.y, scrollLine.point2.x, scrollLine.point2.y, "yellow", 3);
        }
      }
    }
    if(data.showRoomLines) {
      //Draw columns
      for(let i = 1; i < this.canvas.width / 256; i++) {
        Helpers.drawLine(this.uiCtx, i * 256, 0, i * 256, this.canvas.height, "white", 1);
      }
      //Draw rows
      for(let i = 1; i < this.canvas.height / 256; i++) {
        Helpers.drawLine(this.uiCtx, 0, i * 256, this.canvas.width, i * 256, "white", 1);
      }
    }

    if(data.selectedTool == Tool.Select && data.levelSelectedCoords) {
      for(let point of data.levelSelectedCoords) {
        Helpers.drawRect(this.uiCtx, point.getRect(), "", "green", 2);
      }
    }
    if(data.selectedTool == Tool.Select || data.selectedTool == Tool.RectangleTile || data.selectedTool == Tool.SelectInstance) {
      if(this.mousedown) {
        Helpers.drawRect(this.uiCtx, new Rect(this.dragLeftX, this.dragTopY, this.dragRightX, this.dragBotY), "", "blue", 1);
      }
    }
    if(data.selectedTool == Tool.PlaceTile && !this.mouseLeftCanvas) {
      if(data.clonedTiles) {
        let rect = data.clonedTiles.getRect();
        let destPoint = this.getMouseGridCoords();
        if(data.mode16x16 && this.mouseY % 16 >= 8) {
          destPoint.i--;
        }
        if(data.mode16x16 && this.mouseX % 16 >= 8) {
          destPoint.j--;
        }
        //@ts-ignore
        Helpers.drawImage(this.uiCtx, data.selectedLevel.layers[0], rect.x1, rect.y1, rect.w, rect.h, destPoint.j * consts.TILE_WIDTH, destPoint.i * consts.TILE_WIDTH);
      }
      else {
        let selectedGridCoords = data.tileSelectedCoords;
        if(!selectedGridCoords || selectedGridCoords.length === 0) return;
        for(let gridCoord of selectedGridCoords) {
          let topLeftSelectedTileGridCoord = getTopLeftSelectedTileGridCoord();
          let offsettedGridCoords = new GridCoords(gridCoord.i - topLeftSelectedTileGridCoord.i, gridCoord.j - topLeftSelectedTileGridCoord.j);
          let destPoint = this.getMouseGridCoords();
          if(data.mode16x16 && this.mouseY % 16 >= 8) {
            destPoint.i--;
          }
          if(data.mode16x16 && this.mouseX % 16 >= 8) {
            destPoint.j--;
          }
          
          destPoint.i += offsettedGridCoords.i;
          destPoint.j += offsettedGridCoords.j;

          let rect = gridCoord.getRect();
          Helpers.drawImage(this.uiCtx, data.selectedTileset.imgEl, rect.x1, rect.y1, rect.w, rect.h, destPoint.j * consts.TILE_WIDTH, destPoint.i * consts.TILE_WIDTH);
        }
      }
    }
    if(data.selectedTool == Tool.CreateInstance && !this.mouseLeftCanvas) {
      let sprite = _.find(data.sprites, (sprite => { return sprite.name === data.selectedObj.spriteOrImage; }));
      let a = (this.getMouseGridCoordsCustomWidth(consts.TILE_WIDTH).j * consts.TILE_WIDTH);
      let b = (this.getMouseGridCoordsCustomWidth(consts.TILE_WIDTH).i * consts.TILE_WIDTH);
      let x = a + data.selectedObj.snapOffset.x;
      let y = b + data.selectedObj.snapOffset.y;
      let rect = sprite.frames[0].rect;
      sprite.draw(this.uiCtx, 0, x, y);
    }
    if(data.showElevations) {
      let grid = data.selectedLevel.coordPropertiesGrid;
      for(let i = 0; i < grid.length; i++) {
        for(let j = 0; j < grid[i].length; j++) {
          let elevation = grid[i][j].elevation;
          let color = "";
          if(elevation === -3) color = "red";
          else if(elevation === -2) color = "orange";
          else if(elevation === -1) color = "yellow";
          else if(elevation === 1) color = "blue";
          else if(elevation === 2) color = "indigo";
          else if(elevation === 3) color = "violet";
          else continue;
          Helpers.drawRect(this.uiCtx, new GridCoords(i, j).getRect(), color, "", 0, 0.5);
        }
      }
    }
    if(data.selectedLevel && data.showInstances) {
      for(let instance of data.selectedLevel.instances) {
        instance.draw(this.uiCtx);
      }
    }
    
    if(data.selectedTool == Tool.SelectInstance) {
      for(let instance of data.selectedInstances) {
        Helpers.drawRect(this.uiCtx, instance.getPositionalRect(), "", "yellow", 3);
      }
    }
    if(data.showOverridesWithKey) {
      let grid = data.selectedLevel.coordPropertiesGrid;
      for(let i = 0; i < grid.length; i++) {
        for(let j = 0; j < grid[i].length; j++) {
          if(grid[i][j][data.showOverridesWithKey]) {
            Helpers.drawRect(this.uiCtx, new GridCoords(i, j).getRect(), "red", "", 0, 0.5);
          }
        }
      }
    }
    if(data.showNavMesh && data.selectedLevel && data.selectedLevel.coordPropertiesGrid) {
      let grid = data.selectedLevel.coordPropertiesGrid;
      for(let i = 0; i < grid.length; i++) {
        for(let j = 0; j < grid[i].length; j++) {
          if(grid[i][j]["navmesh"]) {
            Helpers.drawRect(this.uiCtx, new GridCoords(i, j).getRect(), "yellow", "", 0, 0.5);
          }
          let neighbors = grid[i][j]["neighbors"];
          if(!neighbors) continue;
          for(let neighbor of neighbors) {
            let ni = Number(neighbor.split(",")[0]);
            let nj = Number(neighbor.split(",")[1]);
            Helpers.drawLine(this.uiCtx, 4 + j * 8, 4 + i * 8, 4 + nj * 8, 4 + ni * 8, "yellow", 2);
          }
        }
      }
    }
  }

  redraw() {
    //super.redraw();
    //this.redrawUICanvas();
  }

  /*
  onMouseWheel(delta: number) {
    resetVue();
    this.redraw();
    this.redrawUICanvas();
  }
  */

  onMouseMove() {
    if(this.mousedown) {
      if(data.selectedTool == Tool.PlaceTile) {
        let topI = this.getMouseGridCoords().i;
        let topJ = this.getMouseGridCoords().j;
        if(this.isHeld(KeyCode.SHIFT)) {
          topI = this.prevGridCoords.i;
        }
        if(this.isHeld(KeyCode.CONTROL)) {
          topJ = this.prevGridCoords.j;
        }
        if(topI !== this.prevGridCoords.i || topJ !== this.prevGridCoords.j) {
          this.placeTile(new GridCoords(topI, topJ));
        }
        this.prevGridCoords = new GridCoords(topI, topJ);
      }
      else if(data.selectedTool == Tool.PaintElevation) {
        data.selectedLevel.coordPropertiesGrid[this.getMouseGridCoords().i][this.getMouseGridCoords().j].elevation = Number(data.paintElevationHeight);
      }
      this.redrawUICanvas();
    }
    else {
      this.mouseLeftCanvas = false;
      if(data.selectedTool == Tool.PlaceTile && !this.getMouseGridCoords().equals(this.lastMouseMoveGridCoords)) {
        this.redrawUICanvas(false);
      }
      else if(data.selectedTool == Tool.CreateInstance && !this.getMouseGridCoordsCustomWidth(consts.TILE_WIDTH).equals(this.lastMouseMoveGridCoords8x8)) {
        this.redrawUICanvas(false);
      }
      this.lastMouseMoveGridCoords = this.getMouseGridCoords();
      this.lastMouseMoveGridCoords8x8 = this.getMouseGridCoordsCustomWidth(consts.TILE_WIDTH);
    }
  }
  
  onMouseLeave() {
    this.mouseLeftCanvas = true;
    this.redrawUICanvas();
  }

  onLeftMouseDown() {
    if(data.selectedTool == Tool.Select) {
      
    }
    else if(data.selectedTool == Tool.CreateInstance) {
      let x = this.mouseX;
      let y = this.mouseY;
      if(data.selectedObj.snapToTile) {
        let sprite = _.find(data.sprites, (sprite => { return sprite.name === data.selectedObj.spriteOrImage; }));
        let a = (this.getMouseGridCoordsCustomWidth(consts.TILE_WIDTH).j * consts.TILE_WIDTH);
        let b = (this.getMouseGridCoordsCustomWidth(consts.TILE_WIDTH).i * consts.TILE_WIDTH);
        let alignOffset = sprite.getAlignOffset(sprite.frames[0]);
        x = a + data.selectedObj.snapOffset.x;
        y = b + data.selectedObj.snapOffset.y;
        if(_.some(data.selectedLevel.instances, (instance: SpriteInstance) => { return instance.pos.x === x && instance.pos.y === y && instance.obj === data.selectedObj; })) {
          console.log("Same instance found at point! Not creating...");
          return;
        }
      }
      let instance = new SpriteInstance(data.selectedObj.name, x, y, data.selectedObj, data.sprites);
      data.selectedLevel.instances.push(instance);
      //data.selectedObj = undefined;
      //data.selectedInstances = [instance];
      //data.selectedTool = Tool.SelectInstance;
      app1.addUndoJson();
      app1.sortInstances();
      this.redraw();
      this.redrawUICanvas();
    }
    else if(data.selectedTool == Tool.PlaceTile) {
      this.prevGridCoords = this.getMouseGridCoords();
      this.placeTile(this.getMouseGridCoords());
    }
    else if(data.selectedTool == Tool.PaintElevation) {
      data.selectedLevel.coordPropertiesGrid[this.getMouseGridCoords().i][this.getMouseGridCoords().j].elevation = Number(data.paintElevationHeight);
      this.redrawUICanvas();
    }
  }

  placeTile(gridCoords: GridCoords) {

    if(data.clonedTiles) {
      let rect = data.clonedTiles.getRect();
      let destPoint = this.getMouseGridCoords();
      if(data.mode16x16 && this.mouseY % 16 >= 8) {
        destPoint.i--;
      }
      if(data.mode16x16 && this.mouseX % 16 >= 8) {
        destPoint.j--;
      }
      let ctx = data.selectedLevel.layers[data.layerIndex].getContext("2d");
      //@ts-ignore
      Helpers.drawImage(ctx, data.selectedLevel.layers[0], rect.x1, rect.y1, rect.w, rect.h, destPoint.j * consts.TILE_WIDTH, destPoint.i * consts.TILE_WIDTH);
      return;
    }

    let selectedGridCoords = data.tileSelectedCoords;
    if(!selectedGridCoords || selectedGridCoords.length === 0) return;

    for(let selectedGridCoord of selectedGridCoords) {
      let topLeftSelectedTileGridCoord = getTopLeftSelectedTileGridCoord();
      let offsettedGridCoords = new GridCoords(selectedGridCoord.i - topLeftSelectedTileGridCoord.i, selectedGridCoord.j - topLeftSelectedTileGridCoord.j);
      offsettedGridCoords.i += gridCoords.i;
      offsettedGridCoords.j += gridCoords.j;

      if(data.mode16x16 && this.mouseY % 16 >= 8) {
        offsettedGridCoords.i--;
      }
      if(data.mode16x16 && this.mouseX % 16 >= 8) {
        offsettedGridCoords.j--;
      }

      data.selectedLevel.layers[data.layerIndex].getContext("2d").drawImage(data.selectedTileset.imgEl, selectedGridCoord.j * consts.TILE_WIDTH, selectedGridCoord.i * consts.TILE_WIDTH, consts.TILE_WIDTH, consts.TILE_WIDTH, offsettedGridCoords.j * consts.TILE_WIDTH, offsettedGridCoords.i * consts.TILE_WIDTH, consts.TILE_WIDTH, consts.TILE_WIDTH);
    }
  }

  onLeftMouseUp() {
    if(data.selectedTool == Tool.Select) {
      if(!data.selectedLevel) return;
      let dragRect = this.getDragGridRect();
      
      //SHIFT box selection
      if(this.isHeld(KeyCode.SHIFT)) {
        let lastI = Math.floor(this.lastClickY / consts.TILE_WIDTH);
        let lastJ = Math.floor(this.lastClickX / consts.TILE_WIDTH);
        let currentI = this.getMouseGridCoords().i;
        let currentJ = this.getMouseGridCoords().j;
        dragRect.topLeftGridCoords.i = Math.min(lastI, currentI);
        dragRect.topLeftGridCoords.j = Math.min(lastJ, currentJ);
        dragRect.botRightGridCoords.i = Math.max(lastI, currentI);
        dragRect.botRightGridCoords.j = Math.max(lastJ, currentJ);
      }
  
      /*
      //ALT selects all tiles in a clump or animation
      if(this.isHeld(KeyCode.ALT)) {
        let tile = _.find(data.tileDatas, (tile: TileData) => {
          return tile.rect.x2 === botX && tile.rect.y2 === botY;
        });
        if(tile) {
          let clump = _.find(data.tileClumps, (tileClump: TileClump) => {
            return tileClump.tiles.indexOf(tile) > -1;
          });
          let anim = _.find(data.tileAnimations, (tileAnimation: TileAnimation) => {
            return tileAnimation.tiles.indexOf(tile) > -1;
          });
          if(clump) {
            rect = clump.rect;
          }
          if(anim) {
            rect = anim.rect;
          }
        }
      }
      */

      if(!this.isHeld(KeyCode.CONTROL)) {
        data.levelSelectedCoords = [];
      }
      for(let i = dragRect.topLeftGridCoords.i; i <= dragRect.botRightGridCoords.i; i++) {
        for(let j = dragRect.topLeftGridCoords.j; j <= dragRect.botRightGridCoords.j; j++) {
          if(!_.some(data.levelSelectedCoords, (coord) => { return coord.i === i && coord.j === j; })) {
            data.levelSelectedCoords.push(new GridCoords(i, j));
          }
        }
      }

      app1.updateSelectionProperties();
      let elevation = data.selectedLevel.coordPropertiesGrid[this.getMouseGridCoords().i][this.getMouseGridCoords().j].elevation;
      if(!elevation) elevation = 0;
      data.selectionElevation = elevation;
      
      levelCanvas.redrawUICanvas();
      tileCanvas.redraw();
    }
    else if(data.selectedTool == Tool.SelectInstance) {
      if(!data.selectedLevel) return;
      let oldSelectedInstances = data.selectedInstances;
      data.selectedInstances = [];
      for(let instance of data.selectedLevel.instances) {
        let rect = instance.getPositionalRect();
        let dragRect = new Rect(this.dragLeftX, this.dragTopY, this.dragRightX, this.dragBotY);
        if(rect.overlaps(dragRect)) {
          if(oldSelectedInstances.indexOf(instance) === -1) {
            data.selectedInstances.push(instance);
            break;
          }
        }
      }
      levelCanvas.redrawUICanvas();
    }
    else if(data.selectedTool == Tool.RectangleTile) {
      if(!data.selectedLevel) return;
      if(app1.tileSelectedCoords.length === 0) return;
      
      /*
      //SHIFT box selection
      if(this.isHeld(KeyCode.SHIFT) && data.levelSelectedRect) {
        topX = data.levelSelectedRect.x1;     
        topY = data.levelSelectedRect.y1;
      }
      */
     
      let tileCoordToPlace = app1.tileSelectedCoords[0];
      let rect = this.getDragGridRect();
      for(let i = rect.i1; i <= rect.i2; i++) {
        for(let j = rect.j1; j <= rect.j2; j++) {
          let gridCoords = new GridCoords(i, j);
          data.selectedLevel.layers[data.layerIndex].getContext("2d").drawImage(data.selectedTileset.imgEl, tileCoordToPlace.j * consts.TILE_WIDTH, tileCoordToPlace.i * consts.TILE_WIDTH, consts.TILE_WIDTH, consts.TILE_WIDTH, gridCoords.j * consts.TILE_WIDTH, gridCoords.i * consts.TILE_WIDTH, consts.TILE_WIDTH, consts.TILE_WIDTH);
        }
      }

      app1.addUndoJson();

      levelCanvas.redraw();
      levelCanvas.redrawUICanvas();
      tileCanvas.redraw();
    }
    else if(data.selectedTool == Tool.PlaceTile) {
      app1.addUndoJson();
    }
    else if(data.selectedTool == Tool.PaintElevation) {
      app1.addUndoJson();
    }
  }

  onKeyDown(keyCode: KeyCode, firstFrame: boolean) {
    //LEVEL HOTKEYS
    if(keyCode === KeyCode.Z && this.isHeld(KeyCode.CONTROL)) {
      app1.undo();
      return;
    }
    else if(keyCode === KeyCode.Y && this.isHeld(KeyCode.CONTROL)) {
      app1.redo();
      return;
    }

    if(keyCode === KeyCode.ESCAPE) {
      data.selectedTool = Tool.Select;
      data.clonedTiles = undefined; //getLevelSelectedGridRect();
      data.levelSelectedCoords = [];
      this.redrawUICanvas();
      return;
    }

    if(keyCode == KeyCode.W)
    {
      if(data.selectedInstances.length > 0) {
        data.selectedInstances[0].pos.y -= 8;
        this.redrawUICanvas();
      }
    }
    else if(keyCode == KeyCode.S)
    {
      if(data.selectedInstances.length > 0) {
        data.selectedInstances[0].pos.y += 8;
        this.redrawUICanvas();
      }
    }
    else if(keyCode == KeyCode.N)
    {
      if(data.levelSelectedCoords.length === 1) {
        let gridCoords = data.levelSelectedCoords[0];
        let neighbors = data.lastNavMeshCoords ? [data.lastNavMeshCoords] : [];
        let myCoords = String(gridCoords.i) + "," + String(gridCoords.j);
        if(data.lastNavMeshCoords) {
          let ni = Number(data.lastNavMeshCoords.split(",")[0]);
          let nj = Number(data.lastNavMeshCoords.split(",")[1]);
          data.selectedLevel.coordPropertiesGrid[ni][nj].neighbors.push(myCoords);
        }
        data.selectedLevel.coordPropertiesGrid[gridCoords.i][gridCoords.j] = {"navmesh":"1","neighbors":neighbors};
        //data.lastNavMeshCoords = myCoords;
        this.redrawUICanvas();
      }
      else if(data.levelSelectedCoords.length === 2) {
        let node1 = data.selectedLevel.coordPropertiesGrid[data.levelSelectedCoords[0].i][data.levelSelectedCoords[0].j];
        let node2 = data.selectedLevel.coordPropertiesGrid[data.levelSelectedCoords[1].i][data.levelSelectedCoords[1].j];
        if(!node1.neighbors) node1.neighbors = [];
        if(!node2.neighbors) node2.neighbors = [];
        node1.neighbors.push(String(data.levelSelectedCoords[1].i) + "," + String(data.levelSelectedCoords[1].j));
        node2.neighbors.push(String(data.levelSelectedCoords[0].i) + "," + String(data.levelSelectedCoords[0].j));
        this.redrawUICanvas();
      }
    }
    else if(keyCode == KeyCode.F)
    {
      if(data.levelSelectedCoords.length === 1) {
        let tileHash: { [tileId: string]: TileData } = {};
        for(let tile of data.tileDatas) {
          tileHash[tile.getId()] = tile;
        }
        let grid = data.selectedLevel.tileInstances;
        let searchGrid: BFSNode[][] = Helpers.make2DArray(grid[0].length, grid.length, undefined);
        for(let i = 0; i < searchGrid.length; i++) {
          for(let j = 0; j < searchGrid[i].length; j++) {
            searchGrid[i][j] = new BFSNode(i, j);
          }
        }
        let node = searchGrid[data.levelSelectedCoords[0].i][data.levelSelectedCoords[0].j];
        let visitedNodes = [ node ];
        let loop = 0;
        let hash: any = {};
        while(visitedNodes.length > 0) {
          //loop++; if(loop > 1000) { throw "INFINITE LOOP!"; }
          let lastNode = visitedNodes.shift();
          lastNode.visited = true;
          let i = lastNode.i;
          let j = lastNode.j;
          if(hash[i + "," + j]) {
            continue;
          }
          if(data.selectedLevel.coordPropertiesGrid[i][j].noLand) {
            continue;
          }
          hash[i + "," + j] = true;
          data.selectedLevel.coordPropertiesGrid[i][j].noLand = 1;
          try {
            let topI = i - 1;
            let top = searchGrid[topI][j];
            let isEmpty = tileHash[grid[topI][j]].hitboxMode === HitboxMode.None && tileHash[grid[topI][j]].zIndex === ZIndex.Default;
            if(!top.visited && isEmpty) {
              visitedNodes.push(top);
            }
          } catch { }
          try {
            let botI = i + 1;
            let bot = searchGrid[botI][j];
            let isEmpty = tileHash[grid[botI][j]].hitboxMode === HitboxMode.None && tileHash[grid[botI][j]].zIndex === ZIndex.Default;
            if(!bot.visited && isEmpty) {
              visitedNodes.push(bot);
            }
          } catch { }
          try {
            let leftJ = j - 1;
            let left = searchGrid[i][leftJ];
            let isEmpty = tileHash[grid[i][leftJ]].hitboxMode === HitboxMode.None && tileHash[grid[i][leftJ]].zIndex === ZIndex.Default;
            if(!left.visited && isEmpty) {
              visitedNodes.push(left);
            }
          } catch { }
          try {
            let rightJ = j + 1;
            let right = searchGrid[i][rightJ];
            let isEmpty = tileHash[grid[i][rightJ]].hitboxMode === HitboxMode.None && tileHash[grid[i][rightJ]].zIndex === ZIndex.Default;
            if(!right.visited && isEmpty) {
              visitedNodes.push(right);
            }
          } catch { }
        }
        this.redrawUICanvas();
        resetVue();
      }

    }
    else if(keyCode == KeyCode.SLASH)
    {
      if(data.levelSelectedCoords.length === 1) {
        let gridCoords = data.levelSelectedCoords[0];
        let neighbors = data.selectedLevel.coordPropertiesGrid[gridCoords.i][gridCoords.j]["neighbors"];
        let myCoords = String(gridCoords.i) + "," + String(gridCoords.j);
        for(let neighbor of neighbors) {
          let ni = Number(neighbor.split(",")[0]);
          let nj = Number(neighbor.split(",")[1]);
          _.pull(data.selectedLevel.coordPropertiesGrid[ni][nj]["neighbors"], myCoords);
        }
        data.selectedLevel.coordPropertiesGrid[gridCoords.i][gridCoords.j] = {};
        data.lastNavMeshCoords = "";
        this.redrawUICanvas();
      }
    }
    else if(keyCode == KeyCode.BACK_QUOTE)
    {
      data.lastNavMeshCoords = "";
    }

    /*
    if(keyCode == KeyCode.NUM_1) data.selectedTool = Tool.Select;
    else if(keyCode == KeyCode.NUM_2) data.selectedTool = Tool.PlaceTile;
    else if(keyCode == KeyCode.NUM_3) data.selectedTool = Tool.RectangleTile;
    else if(keyCode == KeyCode.NUM_4) data.selectedTool = Tool.SelectInstance;
    else if(keyCode == KeyCode.NUM_5) data.selectedTool = Tool.CreateInstance;
    else if(keyCode == KeyCode.NUM_6) data.selectedTool = Tool.PaintElevation;
    */

    /*
    let additionalOffset = (30 * consts.TILE_WIDTH) * (this.isHeld(KeyCode.CONTROL) ? 1 : 0);
    let movedScreen = false;
    if(keyCode === KeyCode.A) {
      this.wrapper.scrollLeft -= consts.TILE_WIDTH + additionalOffset;
      movedScreen = true;
    }
    else if(keyCode === KeyCode.D) {
      this.wrapper.scrollLeft += consts.TILE_WIDTH + additionalOffset;
      movedScreen = true;
    }
    else if(keyCode === KeyCode.W) {
      this.wrapper.scrollTop -= consts.TILE_WIDTH + additionalOffset;
      movedScreen = true;
    }
    else if(keyCode === KeyCode.S) {
      this.wrapper.scrollTop += consts.TILE_WIDTH + additionalOffset;
      movedScreen = true;
    }
    if(movedScreen) {
      let maxOffsetX = (this.baseWidth) - (30 * consts.TILE_WIDTH);
      let maxOffsetY = (this.baseHeight) - (30 * consts.TILE_WIDTH);
      if(this.wrapper.scrollLeft < 0) this.wrapper.scrollLeft = 0;
      if(this.wrapper.scrollTop < 0) this.wrapper.scrollTop = 0;
      if(this.wrapper.scrollLeft > maxOffsetX) this.wrapper.scrollLeft = maxOffsetX;
      if(this.wrapper.scrollTop > maxOffsetY) this.wrapper.scrollTop = maxOffsetY;
      levelCanvas.redraw();
      levelCanvas.redrawUICanvas();
      resetVue();
    }
    */

    /*
    let additionalResize = 29 * (this.isHeld(KeyCode.CONTROL) ? 1 : 0);
    let resizedScreen = false;
    let resizeX = 0;
    let resizeY = 0;
    if(keyCode === KeyCode.DELETE) {
      resizeX -= 1 + additionalResize;
      resizedScreen = true;
    }
    else if(keyCode === KeyCode.PAGE_DOWN) {
      resizeX += 1 + additionalResize;
      resizedScreen = true;
    }
    else if(keyCode === KeyCode.HOME) {
      resizeY -= 1 + additionalResize;
      resizedScreen = true;
    }
    else if(keyCode === KeyCode.END) {
      resizeY += 1 + additionalResize;
      resizedScreen = true;
    }
    if(resizedScreen) {
      data.selectedLevel.width += Math.abs(resizeX);
      data.selectedLevel.height += Math.abs(resizeY);
      app1.onLevelSizeChange();
      if(resizeX < 0) {
        let context = data.selectedLevel.layers[data.layerIndex].getContext("2d");
        let imageData = context.getImageData(0, 0, (data.selectedLevel.width - Math.abs(resizeX)) * 16, (data.selectedLevel.height - Math.abs(resizeY)) * 16);
        context.clearRect(0, 0, data.selectedLevel.width * 16, data.selectedLevel.height * 16);
        context.putImageData(imageData, Math.abs(resizeX) * 16, 0);
      }
      else if(resizeY < 0) {
        let context = data.selectedLevel.layers[data.layerIndex].getContext("2d");
        let imageData = context.getImageData(0, 0, (data.selectedLevel.width - Math.abs(resizeX)) * 16, (data.selectedLevel.height - Math.abs(resizeY)) * 16);
        context.clearRect(0, 0, data.selectedLevel.width * 16, data.selectedLevel.height * 16);
        context.putImageData(imageData, 0, Math.sign(resizeY) * 16);
      }
      levelCanvas.redraw();
      levelCanvas.redrawUICanvas();
    }
    */

    if(data.selectedTool == Tool.Select && firstFrame) {

      if(keyCode === KeyCode.C) {
        data.selectedTool = Tool.PlaceTile;
        data.clonedTiles = getLevelSelectedGridRect();
        this.redrawUICanvas();
        return;
      }

      if(keyCode === KeyCode.L) {
        let gridRect = getLevelSelectedGridRect();
        if(gridRect) {
          let line = new Line(new Point(gridRect.j1 * consts.TILE_WIDTH, gridRect.i1 * consts.TILE_WIDTH), new Point(gridRect.j2 * consts.TILE_WIDTH, gridRect.i2 * consts.TILE_WIDTH));
          if(!_.some(data.selectedLevel.scrollLines, (curLine) => {
            return line.point1.equals(curLine.point1) && line.point2.equals(curLine.point2);
          })) {
            data.selectedLevel.scrollLines.push(line);
            //app1.addUndoJson();
            this.redrawUICanvas();
          }
        }
      }
      else if(keyCode == KeyCode.K) {
        for(let line of data.selectedLevel.scrollLines) {
          let lineRect: Rect = undefined;
          if(line.point1.x === line.point2.x) {
            lineRect = new Rect(line.point1.x - consts.TILE_WIDTH / 2, line.point1.y, line.point1.x + consts.TILE_WIDTH / 2, line.point2.y);
          }
          else if(line.point1.y === line.point2.y) {
            lineRect = new Rect(line.point1.x, line.point1.y - consts.TILE_WIDTH / 2, line.point2.x, line.point1.y + consts.TILE_WIDTH / 2);
          }
          else {
            continue;
          }
          let gridRect = getLevelSelectedGridRect();
          let selectionRect = new Rect(gridRect.j1 * consts.TILE_WIDTH, gridRect.i1 * consts.TILE_WIDTH, (gridRect.j2 + 1) * consts.TILE_WIDTH, (gridRect.i2 + 1) * consts.TILE_WIDTH);
          if(lineRect.overlaps(selectionRect)) {
            _.remove(data.selectedLevel.scrollLines, line);
            this.redrawUICanvas();
            break;
          }
        }
      }

      let incX = 0;
      let incY = 0;
      if(keyCode === KeyCode.LEFT) {
        incX = -1;
      }
      else if(keyCode === KeyCode.RIGHT) {
        incX = 1;
      }
      else if(keyCode === KeyCode.UP) {
        incY = -1;
      }
      else if(keyCode === KeyCode.DOWN) {
        incY = 1;
      }
      if(incX !== 0 || incY !== 0) {
        let context = data.selectedLevel.layers[data.layerIndex].getContext("2d");
        let savedImageDatas = [];
        let gridCoordsMovedTo = new Set<string>();
        for(let gridCoords of app1.levelSelectedCoords) {
          let imageData = context.getImageData(gridCoords.j * consts.TILE_WIDTH, gridCoords.i * consts.TILE_WIDTH, consts.TILE_WIDTH, consts.TILE_WIDTH);
          savedImageDatas.push(imageData);
        }
        for(let i = 0; i < app1.levelSelectedCoords.length; i++) {
          let gridCoords = app1.levelSelectedCoords[i];
          context.putImageData(savedImageDatas[i], (gridCoords.j + incX) * consts.TILE_WIDTH, (gridCoords.i + incY) * consts.TILE_WIDTH);
          gridCoordsMovedTo.add(String(gridCoords.i + incY) + "," + String(gridCoords.j + incX));
        }
        for(let gridCoords of app1.levelSelectedCoords) {
          if(!gridCoordsMovedTo.has(String(gridCoords.i) + "," + String(gridCoords.j))) {
            context.clearRect(gridCoords.j * consts.TILE_WIDTH, gridCoords.i * consts.TILE_WIDTH, consts.TILE_WIDTH, consts.TILE_WIDTH);
          }
        }
        for(let gridCoords of app1.levelSelectedCoords) {
          gridCoords.i += incY;
          gridCoords.j += incX;
        }
        app1.addUndoJson();
        levelCanvas.redraw();
        levelCanvas.redrawUICanvas();
      }
      if(keyCode === KeyCode.DELETE) {
        let context = data.selectedLevel.layers[data.layerIndex].getContext("2d");
        for(let gridCoords of app1.levelSelectedCoords) {
          context.clearRect(gridCoords.j * consts.TILE_WIDTH, gridCoords.i * consts.TILE_WIDTH, consts.TILE_WIDTH, consts.TILE_WIDTH);
        }
        app1.addUndoJson();
        levelCanvas.redrawUICanvas();
      }
    }
    else if(data.selectedTool == Tool.SelectInstance && data.selectedInstances.length > 0) {
      let incX = 0;
      let incY = 0;
      if(keyCode === KeyCode.LEFT) {
        incX = -1;
      }
      else if(keyCode === KeyCode.RIGHT) {
        incX = 1;
      }
      else if(keyCode === KeyCode.UP) {
        incY = -1;
      }
      else if(keyCode === KeyCode.DOWN) {
        incY = 1;
      }
      if(incX !== 0 || incY !== 0) {
        for(let instance of data.selectedInstances) {
          instance.pos.x += incX;
          instance.pos.y += incY;
        }
        app1.addUndoJson();
        levelCanvas.redraw();
        levelCanvas.redrawUICanvas();
      }
      if(keyCode === KeyCode.DELETE) {
        for(let instance of data.selectedInstances) {
          _.remove(data.selectedLevel.instances, instance);
        }
        levelCanvas.redraw();
        levelCanvas.redrawUICanvas();
        data.selectedInstances = [];
        app1.addUndoJson();
      }
      if(keyCode === KeyCode.E && data.selectedInstances.length === 1) {
        let index = data.selectedLevel.instances.indexOf(data.selectedInstances[0]);
        index++;
        if(index >= data.selectedLevel.instances.length) index = 0;
        data.selectedInstances[0] = data.selectedLevel.instances[index];
        resetVue();
      }
      else if(keyCode === KeyCode.Q && data.selectedInstances.length === 1) {
        let index = data.selectedLevel.instances.indexOf(data.selectedInstances[0]);
        index--;
        if(index < 0) index = data.selectedLevel.instances.length - 1;
        data.selectedInstances[0] = data.selectedLevel.instances[index];
        resetVue();
      }
    }
  }
}
let levelCanvas = new LevelCanvas();

class TileCanvas extends CanvasUI {
  
  uiCanvas: HTMLCanvasElement;
  uiCtx: CanvasRenderingContext2D;

  constructor() {
    super("#tile-canvas");
    this.uiCanvas = <HTMLCanvasElement>document.getElementById("ui-tile-canvas");
    this.uiCtx = this.uiCanvas.getContext("2d");
    this.uiCanvas.width = this.canvas.width;
    this.uiCanvas.height = this.canvas.height;
  }

  onLeftMouseDown() {
  }
  
  onLeftMouseUp() {
    if(!data.selectedLevel) return;
    if(!data.selectedTileset) return;

    let dragRect = this.getDragGridRect();

    if(data.mode16x16) {
      if(dragRect.i1 % 2 !== 0) dragRect.topLeftGridCoords.i--;
      if(dragRect.j1 % 2 !== 0) dragRect.topLeftGridCoords.j--;
      if(dragRect.i2 % 2 === 0) dragRect.botRightGridCoords.i++;
      if(dragRect.j2 % 2 === 0) dragRect.botRightGridCoords.j++;
    }
    
    //SHIFT box selection
    if(this.isHeld(KeyCode.SHIFT)) {
      let lastI = Math.floor(this.lastClickY / consts.TILE_WIDTH);
      let lastJ = Math.floor(this.lastClickX / consts.TILE_WIDTH);
      let currentI = this.getMouseGridCoords().i;
      let currentJ = this.getMouseGridCoords().j;
      dragRect.topLeftGridCoords.i = Math.min(lastI, currentI);
      dragRect.topLeftGridCoords.j = Math.min(lastJ, currentJ);
      dragRect.botRightGridCoords.i = Math.max(lastI, currentI);
      dragRect.botRightGridCoords.j = Math.max(lastJ, currentJ);
    }

    /*
    //ALT selects all tiles in a clump or animation
    if(this.isHeld(KeyCode.ALT)) {
      let tile = _.find(data.tileDatas, (tile: TileData) => {
        return tile.rect.x2 === botX && tile.rect.y2 === botY;
      });
      if(tile) {
        let clump = _.find(data.tileClumps, (tileClump: TileClump) => {
          return tileClump.tiles.indexOf(tile) > -1;
        });
        let anim = _.find(data.tileAnimations, (tileAnimation: TileAnimation) => {
          return tileAnimation.tiles.indexOf(tile) > -1;
        });
        if(clump) {
          rect = clump.rect;
        }
        if(anim) {
          rect = anim.rect;
        }
      }
    }
    */

    if(!this.isHeld(KeyCode.CONTROL)) {
      data.tileSelectedCoords = [];
    }
    for(let i = dragRect.topLeftGridCoords.i; i <= dragRect.botRightGridCoords.i; i++) {
      for(let j = dragRect.topLeftGridCoords.j; j <= dragRect.botRightGridCoords.j; j++) {
        if(!_.some(data.tileSelectedCoords, (coord) => { return coord.i === i && coord.j === j; })) {
          data.tileSelectedCoords.push(new GridCoords(i, j));
          data.clonedTiles = undefined;
        }
      }
    }
    
    app1.initMultiEditParams();

    if(data.selectedTool != Tool.PlaceTile && data.selectedTool != Tool.RectangleTile) {
      data.selectedTool = Tool.PlaceTile;
    }

    //levelCanvas.redraw();
    tileCanvas.redraw();
  }

  redrawUICanvas() {

    this.uiCanvas.width = this.canvas.width;
    this.uiCanvas.height = this.canvas.height;

    this.uiCtx.clearRect(0, 0, this.canvas.width, this.canvas.height);

    if(data.showTileHitboxes) {
      for(let i = 0; i < this.canvas.height / 8; i++) {
        for(let j = 0; j < this.canvas.width / 8; j++) {
          let tileData = app1.getTileGrid()[i][j];
          if(!tileData) continue;
          if(tileData.hitboxMode === HitboxMode.Tile) {
            Helpers.drawRect(this.uiCtx, new GridCoords(i, j).getRect(), "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.BoundingRect) {
            Helpers.drawRect(this.uiCtx, new GridCoords(i, j).getRect(), "orange", "", 0, 0.5);
          }
          let x = j * consts.TILE_WIDTH;
          let y = i * consts.TILE_WIDTH;
          let x2 = (j + 1) * consts.TILE_WIDTH;
          let y2 = (i + 1) * consts.TILE_WIDTH;
          let topLeftPt = new Point(x, y);
          let topRightPt = new Point(x2, y);
          let botLeftPt = new Point(x, y2);
          let botRightPt = new Point(x2, y2);
          let xMid = x + consts.TILE_WIDTH / 2;
          let yMid = y + consts.TILE_WIDTH / 2;

          if(tileData.hitboxMode === HitboxMode.DiagBotLeft) {
            Helpers.drawPolygon(this.uiCtx, new Shape([topLeftPt, botRightPt, botLeftPt]), true, "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.DiagBotRight) {
            Helpers.drawPolygon(this.uiCtx, new Shape([botLeftPt, botRightPt, topRightPt]), true, "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.DiagTopLeft) {
            Helpers.drawPolygon(this.uiCtx, new Shape([topLeftPt, topRightPt, botLeftPt]), true, "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.DiagTopRight) {
            Helpers.drawPolygon(this.uiCtx, new Shape([topLeftPt, topRightPt, botRightPt]), true, "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.BoxTopLeft) {
            Helpers.drawRect(this.uiCtx, new Rect(x, y, xMid, yMid), "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.BoxTopRight) {
            Helpers.drawRect(this.uiCtx, new Rect(xMid, y, x2, yMid), "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.BoxBotLeft) {
            Helpers.drawRect(this.uiCtx, new Rect(x, yMid, xMid, y2), "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.BoxBotRight) {
            Helpers.drawRect(this.uiCtx, new Rect(xMid, yMid, x2, y2), "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.BoxTop) {
            Helpers.drawRect(this.uiCtx, new Rect(x, y, x2, yMid), "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.BoxBot) {
            Helpers.drawRect(this.uiCtx, new Rect(x, yMid, x2, y2), "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.BoxLeft) {
            Helpers.drawRect(this.uiCtx, new Rect(x, y, xMid, y2), "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.BoxRight) {
            Helpers.drawRect(this.uiCtx, new Rect(xMid, y, x2, y2), "blue", "", 0, 0.5);
          }

          else if(tileData.hitboxMode === HitboxMode.SmallDiagTopLeft) {
            Helpers.drawPolygon(this.uiCtx, new Shape([topLeftPt, topLeftPt.addxy(consts.TILE_WIDTH / 2, 0), topLeftPt.addxy(0, consts.TILE_WIDTH / 2)]), true, "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.SmallDiagTopRight) {
            Helpers.drawPolygon(this.uiCtx, new Shape([topRightPt, topRightPt.addxy(-consts.TILE_WIDTH / 2, 0), topRightPt.addxy(0, consts.TILE_WIDTH / 2)]), true, "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.SmallDiagBotLeft) {
            Helpers.drawPolygon(this.uiCtx, new Shape([botLeftPt, botLeftPt.addxy(consts.TILE_WIDTH / 2, 0), botLeftPt.addxy(0, -consts.TILE_WIDTH / 2)]), true, "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.SmallDiagBotRight) {
            Helpers.drawPolygon(this.uiCtx, new Shape([botRightPt, botRightPt.addxy(-consts.TILE_WIDTH / 2, 0), botRightPt.addxy(0, -consts.TILE_WIDTH / 2)]), true, "blue", "", 0, 0.5);
          }

          else if(tileData.hitboxMode === HitboxMode.LargeDiagTopLeft) {
            Helpers.drawPolygon(this.uiCtx, new Shape([topLeftPt, topRightPt, botRightPt.addxy(0, -consts.TILE_WIDTH / 2), botRightPt.addxy(-consts.TILE_WIDTH / 2, 0), botLeftPt]), true, "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.LargeDiagTopRight) {
            Helpers.drawPolygon(this.uiCtx, new Shape([topLeftPt, topRightPt, botRightPt, botLeftPt.addxy(consts.TILE_WIDTH / 2, 0), botLeftPt.addxy(0, -consts.TILE_WIDTH / 2)]), true, "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.LargeDiagBotLeft) {
            Helpers.drawPolygon(this.uiCtx, new Shape([topLeftPt, topRightPt.addxy(-consts.TILE_WIDTH / 2, 0), topRightPt.addxy(0, consts.TILE_WIDTH / 2), botRightPt, botLeftPt]), true, "blue", "", 0, 0.5);
          }
          else if(tileData.hitboxMode === HitboxMode.LargeDiagBotRight) {
            Helpers.drawPolygon(this.uiCtx, new Shape([topLeftPt.addxy(consts.TILE_WIDTH / 2, 0), topRightPt, botRightPt, botLeftPt, topLeftPt.addxy(0, consts.TILE_WIDTH / 2)]), true, "blue", "", 0, 0.5);
          }

          else if(tileData.hitboxMode === HitboxMode.Custom) {
            let pts = [];
            let customHitboxPoints = String(tileData.customHitboxPoints);
            for(let i = 0; i < customHitboxPoints.length; i++) {
              let char = customHitboxPoints[i];
              let point: Point = undefined;
              if(char === "0") point = topLeftPt;
              if(char === "1") point = topLeftPt.addxy(consts.TILE_WIDTH / 2, 0);
              if(char === "2") point = topRightPt;
              if(char === "3") point = topLeftPt.addxy(0, consts.TILE_WIDTH / 2);
              if(char === "4") point = topLeftPt.addxy(consts.TILE_WIDTH / 2, consts.TILE_WIDTH / 2);
              if(char === "5") point = topLeftPt.addxy(consts.TILE_WIDTH, consts.TILE_WIDTH / 2);
              if(char === "6") point = botLeftPt;
              if(char === "7") point = botLeftPt.addxy(consts.TILE_WIDTH / 2, 0);
              if(char === "8") point = botRightPt;
              pts.push(point);
            }
            if(pts.length > 2) {
              Helpers.drawPolygon(this.uiCtx, new Shape(pts), true, "blue", "", 0, 0.5);
            }
          }
        }
      }
    }
    if(data.showTilesWithTag) {
      for(let i = 0; i < this.canvas.height / 8; i++) {
        for(let j = 0; j < this.canvas.width / 8; j++) {
          let tileData = app1.getTileGrid()[i][j];
          let gridCoords = new GridCoords(i, j);
          if(tileData && tileData.hasTag(data.showTilesWithTag)) {
            Helpers.drawRect(this.uiCtx, gridCoords.getRect(), "red", "", 0, 0.5);
          }
        }
      }
    }
    if(data.showTilesWithZIndex1) {
      for(let i = 0; i < this.canvas.height / 8; i++) {
        for(let j = 0; j < this.canvas.width / 8; j++) {
          let tileData = app1.getTileGrid()[i][j];
          let gridCoords = new GridCoords(i, j);
          if(tileData && tileData.zIndex === ZIndex.Foreground1) {
            Helpers.drawRect(this.uiCtx, gridCoords.getRect(), "red", "", 0, 0.5);
          }
        }
      }
    }

    /*
    for(let i = 0; i < this.canvas.height / 8; i++) {
      for(let j = 0; j < this.canvas.width / 8; j++) {
        let tileData = app1.getTileGrid()[i][j];
        let gridCoords = new GridCoords(i, j);
        if(tileData.spriteName.startsWith("TileWaterEdge") || tileData.spriteName.startsWith("TileOWaterEdge")) {
          Helpers.drawRect(this.uiCtx, gridCoords.getRect(), "red", "", 0, 0.5);
        }
      }
    }
    */

    /*
    if(data.tileSelectedCoords.length > 0) {
      for(let i = 0; i < this.canvas.height / 8; i++) {
        for(let j = 0; j < this.canvas.width / 8; j++) {
          let tileGrid = app1.getTileGrid();
          if(tileGrid[data.tileSelectedCoords[0].i][data.tileSelectedCoords[0].j] === tileGrid[i][j]) {
            Helpers.drawRect(this.uiCtx, new GridCoords(i, j).getRect(), "red", "", 0, 0.5);
          }
        }
      }
    }
    */
  }
  
  onMouseMove() {
    if(this.mousedown) {
      this.redraw();
    }
  }
  
  onMouseLeave() {
    this.redraw();
  }
  
  onKeyDown(keyCode: KeyCode, firstFrame: boolean) {
    //TILE_HOTKEYS
    if(!firstFrame) return;
    if(keyCode === KeyCode.S) {
      data.multiEditHitboxMode = 1;
      app1.setMultiEditHitboxMode();
    }
    else if(keyCode === KeyCode.Q) {
      if(this.isHeld(KeyCode.CONTROL)) data.multiEditHitboxMode = 17;
      else if(this.isHeld(KeyCode.SHIFT)) data.multiEditHitboxMode = 21;
      else data.multiEditHitboxMode = 3;
      app1.setMultiEditHitboxMode();
    }
    else if(keyCode === KeyCode.E) {
      if(this.isHeld(KeyCode.CONTROL)) data.multiEditHitboxMode = 18;
      else if(this.isHeld(KeyCode.SHIFT)) data.multiEditHitboxMode = 22;
      else data.multiEditHitboxMode = 4;
      app1.setMultiEditHitboxMode();
    }
    else if(keyCode === KeyCode.Z) {
      if(this.isHeld(KeyCode.CONTROL)) data.multiEditHitboxMode = 19;
      else if(this.isHeld(KeyCode.SHIFT)) data.multiEditHitboxMode = 23;
      else data.multiEditHitboxMode = 5;
      app1.setMultiEditHitboxMode();
    }
    else if(keyCode === KeyCode.C) {
      if(this.isHeld(KeyCode.CONTROL)) data.multiEditHitboxMode = 20;
      else if(this.isHeld(KeyCode.SHIFT)) data.multiEditHitboxMode = 24;
      else data.multiEditHitboxMode = 6;
      app1.setMultiEditHitboxMode();
    }
    else if(keyCode === KeyCode.W) {
      data.multiEditHitboxMode = 9;
      app1.setMultiEditHitboxMode();
    }
    else if(keyCode === KeyCode.X) {
      data.multiEditHitboxMode = 10;
      app1.setMultiEditHitboxMode();
    }
    else if(keyCode === KeyCode.A) {
      data.multiEditHitboxMode = 11;
      app1.setMultiEditHitboxMode();
    }
    else if(keyCode === KeyCode.D) {
      data.multiEditHitboxMode = 12;
      app1.setMultiEditHitboxMode();
    }
    else if(keyCode === KeyCode.NUM_1) {
      data.multiEditHitboxMode = 0;
      app1.setMultiEditHitboxMode();
    }
    else if(keyCode === KeyCode.NUM_2) {
      data.multiEditZIndex = 0;
      app1.setMultiEditZIndex();
    }
    else if(keyCode === KeyCode.NUM_3) {
    }
    else if(keyCode === KeyCode.NUM_4) {
    }

    else if(keyCode === KeyCode.O) {
      app1.multiEditTag = "swater";
      app1.setMultiEditTag();
    }
    else if(keyCode === KeyCode.P) {
      app1.multiEditTag = "water";
      app1.setMultiEditTag();
    }
    else if(keyCode === KeyCode.L) {
      app1.multiEditTag = "ledge";
      app1.setMultiEditTag();
    }
    else if(keyCode === KeyCode.K) {
      app1.multiEditTag = "ledgewall";
      app1.setMultiEditTag();
    }
    else if(keyCode === KeyCode.F) {
      app1.multiEditZIndex = ZIndex.Foreground1;
      app1.setMultiEditZIndex();
    }
    else if(keyCode === KeyCode.T) {
      app1.multiEditTileSprite = data.lastSelectedTileSprite;
      app1.setMultiEditTileSprite();
    }
    //Download selected tile
    else if(keyCode === KeyCode.B) {
      var canvas = document.createElement("canvas");
      let rect = getTileSelectedGridRect();
      let w = (rect.j2 - rect.j1 + 1) * consts.TILE_WIDTH;
      let h = (rect.i2 - rect.i1 + 1) * consts.TILE_WIDTH;
      canvas.width = w;
      canvas.height = h;
      var ctx = canvas.getContext("2d");
      let sourceX = rect.j1 * consts.TILE_WIDTH;
      let sourceY = rect.i1 * consts.TILE_WIDTH;
      ctx.drawImage(data.selectedTileset.imgEl, sourceX, sourceY, w, h, 0, 0, w, h);
      var dt = canvas.toDataURL('image/png');
      console.log(dt);
      canvas.remove();
    }
    /*
    else if(keyCode === KeyCode.R) {
      app1.multiEditTag = "ledgeupleft";
      app1.setMultiEditTag();
    }
    else if(keyCode === KeyCode.T) {
      app1.multiEditTag = "ledgeup";
      app1.setMultiEditTag();
    }
    else if(keyCode === KeyCode.Y) {
      app1.multiEditTag = "ledgeupright";
      app1.setMultiEditTag();
    }
    else if(keyCode === KeyCode.F) {
      app1.multiEditTag = "ledgeleft";
      app1.setMultiEditTag();
    }
    else if(keyCode === KeyCode.H) {
      app1.multiEditTag = "ledgeright";
      app1.setMultiEditTag();
    }
    else if(keyCode === KeyCode.V) {
      app1.multiEditTag = "ledgedownleft";
      app1.setMultiEditTag();
    }
    else if(keyCode === KeyCode.B) {
      app1.multiEditTag = "ledgedown";
      app1.setMultiEditTag();
    }
    else if(keyCode === KeyCode.N) {
      app1.multiEditTag = "ledgedownright";
      app1.setMultiEditTag();
    }
    */
  }

  redraw() {
    super.redraw();
    this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
    
    if(!data.selectedLevel) return;
    if(!data.selectedTileset) return;

    if(data.selectedTileset && data.selectedTileset.imgEl) {
      this.ctx.drawImage(data.selectedTileset.imgEl, 0, 0);
    }
  
    let widthFactor = 1;
    if(data.mode16x16) widthFactor = 2;

    //Draw columns
    for(let i = 1; i < this.canvas.width / (consts.TILE_WIDTH * widthFactor); i++) {
      Helpers.drawLine(this.ctx, (i) * (consts.TILE_WIDTH * widthFactor), 0, (i) * (consts.TILE_WIDTH * widthFactor), this.canvas.height, "red", 1);
    }

    //Draw rows
    for(let i = 1; i < this.canvas.height / (consts.TILE_WIDTH * widthFactor); i++) {
      Helpers.drawLine(this.ctx, 0, (i) * (consts.TILE_WIDTH * widthFactor), this.canvas.width, (i) * (consts.TILE_WIDTH * widthFactor), "red", 1);
    }
  
    if(!data.mode16x16) {
      for(let gridCoords of data.tileSelectedCoords) {
        Helpers.drawRect(this.ctx, gridCoords.getRect(), "", "green", 2);
      }
    }
    else {
      for(let gridCoords of data.tileSelectedCoords) {
        if(gridCoords.i % 2 !== 0 || gridCoords.j % 2 !== 0) continue;
        let rect = gridCoords.getRect();
        rect.botRightPoint.x += 8;
        rect.botRightPoint.y += 8;
        Helpers.drawRect(this.ctx, rect, "", "green", 2);
      }
    }

    if(this.mousedown) {
      Helpers.drawRect(this.ctx, new Rect(this.dragLeftX, this.dragTopY, this.dragRightX, this.dragBotY), "", "blue", 1);
    }

  }

}
let tileCanvas = new TileCanvas();

let methods = {
  isLoaded() {
    return data.loadCount >= data.maxLoadCount;
  },
  addTileClump() {
    /*
    this.addTileDatas();
    let tileClump = new TileClump(app1.getTileSelectedTiles(), "");
    if(data.tileClumps.indexOf(tileClump) === -1) {
      data.tileClumps.push(tileClump);
    }
    */
    resetVue();
  },
  removeTileClump() {
    _.remove(data.tileClumps, app1.selectedTileClump());
    resetVue();
  },
  addTileAnimation() {
    /*
    this.addTileDatas();
    let anim = new TileAnimation(app1.getTileSelectedTiles(), 0.16);
    if(data.tileAnimations.indexOf(anim) === -1) {
      data.tileAnimations.push(anim);
    }
    */
    resetVue();
  },
  removeTileAnimation() {
    _.remove(data.tileAnimations, app1.selectedTileAnimation());
    resetVue();
  },
  sortInstances() {
    data.selectedLevel.instances.sort((a: SpriteInstance, b: SpriteInstance) => {
      var compare = a.name.localeCompare(b.name, "en", { numeric: true });
      if(compare < 0) return -1;
      if(compare > 0) return 1;
      if(compare === 0) {
        if(a.properties && b.properties) {
          let aProps = JSON.parse(a.properties.replace(/'/g, '"'));
          let bProps = JSON.parse(b.properties.replace(/'/g, '"'));
          if(aProps.entrance && bProps.entrance) {
            let aId = aProps.entrance.split(",")[0];
            let bId = bProps.entrance.split(",")[0];
            return aId.localeCompare(bId, "en", { numeric: true });
          }
        }
        return 0;
      }
    });
  },
  getTileSelectedTiles() {
    let tileDatas = new Set<TileData>();
    let grid = app1.getTileGrid();
    for(let gridCoord of data.tileSelectedCoords) {
      let tileData = grid[gridCoord.i][gridCoord.j];
      if(tileData) {
        tileDatas.add(tileData);
      }
    }
    return Array.from(tileDatas);
  },
  getTileGrid() {
    return data.tileDataGrids[data.selectedTileset.path];
  },
  getSelectionCoords() {
    let rect = getLevelSelectedGridRect();
    if(rect) {
      return "(" + (rect.topLeftGridCoords.j) + "," + (rect.topLeftGridCoords.i) + ")," + 
           "(" + (rect.botRightGridCoords.j) + "," + (rect.botRightGridCoords.i) + ")";
    }
    return "(Disjoint selections)"
  },
  getLevelSelectionTileData() {
    let selectedCoords = data.levelSelectedCoords;
    if(selectedCoords && selectedCoords.length > 0) {
      let selection = data.selectedLevel.tileInstances[selectedCoords[0].i][selectedCoords[0].j]; 
      if(selection) {
        return selection;
      }
    }
    return "";
  },
  getTileSelectionTileData() {
    let selectedCoords = data.tileSelectedCoords;
    let grid = app1.getTileGrid();
    if(selectedCoords && selectedCoords.length > 0) {
      let selection = grid[selectedCoords[0].i][selectedCoords[0].j]; 
      if(selection) {
        return selection.getId();
      }
    }
    return "";
  },
  getLevelOffset() {
    return "(" + levelCanvas.offsetX + "," + levelCanvas.offsetY + ")";
  },
  onLevelSizeChange() {
    data.selectedLevel.resize();
    levelCanvas.setSize(data.selectedLevel.width * consts.TILE_WIDTH, data.selectedLevel.height * consts.TILE_WIDTH);    
    resetVue();
    levelCanvas.redrawUICanvas();
    levelCanvas.redraw();
  },
  onSelectionElevationChange() {
    for(let gridCoords of data.levelSelectedCoords) {
      data.selectedLevel.coordPropertiesGrid[gridCoords.i][gridCoords.j].elevation = Number(data.selectionElevation);
    }
  },
  onSelectionPropertyChange() {
    for(let gridCoords of data.levelSelectedCoords) {
      data.selectedLevel.coordPropertiesGrid[gridCoords.i][gridCoords.j] = JSON.parse(data.selectionProperties);
    }
  },
  updateSelectionProperties() {
    let firstCoords = data.levelSelectedCoords[0];
    data.selectionProperties = JSON.stringify(data.selectedLevel.coordPropertiesGrid[firstCoords.i][firstCoords.j]);
  },
  onSelectedInstancePropertyChange() {
    for(let instance of data.selectedInstances) {
      instance.properties = data.selectedInstanceProperties;
    }
  },
  addLevel() {
    var newLevel = new Level(this.newLevelName, 30, 30);
    app1.changeLevel(newLevel, false);
    data.levels.push(newLevel);
    //data.selectedObj = null;
    data.selectedInstances = [];
    resetVue();
  },
  changeLevel(newLevel: Level, isUndo: boolean) {
    if(data.selectedLevel) {
      data.selectedLevel.destroy();
    }
    data.selectedLevel = newLevel;
    data.selectedObj = null;
    data.selectedInstances = [];
    levelCanvas.setSize(data.selectedLevel.width * consts.TILE_WIDTH, data.selectedLevel.height * consts.TILE_WIDTH);
    levelCanvas.loadScrollPos(newLevel.name);
    if(isUndo) {
      
    }
    else {
      let selectedTilesetIndex = 0;
      let selectedTilesetIndexStr = localStorage.getItem("selected_tileset_index_" + data.selectedLevel.name);
      if(selectedTilesetIndexStr) selectedTilesetIndex = Number(selectedTilesetIndexStr);
      if(selectedTilesetIndex < 0) selectedTilesetIndex = 0;
      data.selectedLevel.setLayers(data.levelImages);
      data.selectedTileset = data.tilesets[selectedTilesetIndex];
      app1.onTilesetChange(data.selectedTileset);
      app1.addUndoJson();
    }
    levelCanvas.redraw();
    levelCanvas.redrawUICanvas();
    tileCanvas.redraw();
  },
  saveLevel() {
    /*
    for(var instance of this.selectedLevel.instances) {
      instance.normalizePoints();
    }
    ?resourceName=tiledata
    ?resourceName=tileanimation
    ?resourceName=tileclump
    */
    saveScrollPositions();
    let tileDataJsons = Helpers.serializeES6(getTileDatas());
    //console.log(tileDataJsons);
    let tileAnimationJsons = Helpers.serializeES6(data.tileAnimations);
    let tileClumpJsons = Helpers.serializeES6(data.tileClumps);
    
    let levelJson = Helpers.serializeES6(data.selectedLevel);
    $.post("save-resource?resourceName=tiledata", { items: JSON.parse(tileDataJsons) }).then(response => {
      console.log("Successfully saved tiledatas");
    }, error => {
      console.log("Failed to save tiledatas");
    })
    .then(response => {
      $.post("save-resource?resourceName=tileanimation", { items: JSON.parse(tileAnimationJsons) }).then(response => {
        console.log("Successfully saved tileanimations");
      }, error => {
        console.log("Failed to save tileanimations");
      });
    })
    .then(response => {
      $.post("save-resource?resourceName=tileclump", { items: JSON.parse(tileClumpJsons) }).then(response => {
        console.log("Successfully saved tileclumps");
      }, error => {
        console.log("Failed to save tileclumps");
      });
    })
    .then(response => {
      $.post("save-level", JSON.parse(levelJson)).then(response => {
        console.log("Successfully saved level");
      }, error => {
        console.log("Failed to save level");
      });
    })
    .then(response => {
      let imagesToSave = _.map(data.selectedLevel.layers, (layer) => {
        let imgBase64 = layer.toDataURL().split("data:image/png;base64,")[1];
        return imgBase64;
      });
      $.post("save-level-images", { layers: imagesToSave, levelName: data.selectedLevel.name }).then(response => {
        console.log("Successfully saved level images");
      }, error => {
        console.log("Failed to save level");
      });
    });
    
  },
  redraw(redrawTileUICanvas: boolean = false) {
    levelCanvas.redraw();
    levelCanvas.redrawUICanvas();
    tileCanvas.redraw();
    if(redrawTileUICanvas) {
      tileCanvas.redrawUICanvas();
    }
  },
  changeObj(newObj: Obj) {
    data.selectedInstances = [];
    data.selectedObj = newObj;
    data.selectedTool = Tool.CreateInstance;
    levelCanvas.redraw();
  },
  onInstanceClick(instance: SpriteInstance) {
    data.selectedInstances = [instance];
    data.selectedTool = Tool.SelectInstance;
    levelCanvas.wrapper.scrollTop = instance.pos.y - $(levelCanvas.wrapper).height() / 2;
    levelCanvas.wrapper.scrollLeft = instance.pos.x - $(levelCanvas.wrapper).width() / 2;
    levelCanvas.redraw();
    levelCanvas.redrawUICanvas();
  },
  onTilesetChange(tileset: Spritesheet) {
    data.selectedTileset = tileset;
    if(data.selectedLevel) {
      localStorage.setItem("selected_tileset_index_" + data.selectedLevel.name, String(data.tilesets.indexOf(data.selectedTileset)));
    }
    tileCanvas.setSize(data.selectedTileset.imgEl.width, data.selectedTileset.imgEl.height);
    tileCanvas.ctx.drawImage(data.selectedTileset.imgEl, 0, 0);
    levelCanvas.redraw();
    tileCanvas.redraw();
    tileCanvas.setSize(tileset.imgEl.width, tileset.imgEl.height);
    tileCanvas.ctx.drawImage(tileset.imgEl, 0, 0);      
    tileCanvas.loadScrollPos(data.selectedTileset.path);
  },
  selectedTileClump() {
    if(!getTileSelectedGridRect()) return undefined;
    for(let tileClump of data.tileClumps) {
      let rectsEqual = tileClump.rect.equals(getTileSelectedGridRect());
      if(tileClump.tilesetPath === data.selectedTileset.path && rectsEqual) {
        return tileClump;
      }
    }
    return undefined;
  },
  selectedTileAnimation() {
    if(!getTileSelectedGridRect()) return undefined;
    for(let tileAnimation of data.tileAnimations) {
      if(tileAnimation.tilesetPath === data.selectedTileset.path && tileAnimation.rect.equals(getTileSelectedGridRect())) {
        return tileAnimation;
      }
    }
    return undefined;
  },
  initMultiEditParams() {
    let selectedTiles = app1.getTileSelectedTiles();
    let firstName = selectedTiles[0].name;
    if(_.every(selectedTiles, (selectedTile) => { return selectedTile.name === firstName })) {
      data.multiEditName = firstName;
    }
    else {
      data.multiEditName = "";
    }
    let firstMode = selectedTiles[0].hitboxMode;
    if(_.every(selectedTiles, (selectedTile) => { return selectedTile.hitboxMode === firstMode })) {
      data.multiEditHitboxMode = firstMode;
    }
    else {
      data.multiEditHitboxMode = undefined;
    }
    let firstTag = selectedTiles[0].tag;
    if(_.every(selectedTiles, (selectedTile) => { return selectedTile.tag === firstTag })) {
      data.multiEditTag = firstTag;
    }
    else {
      data.multiEditTag = "";
    }
    let firstZIndex = selectedTiles[0].zIndex;
    if(_.every(selectedTiles, (selectedTile) => { return selectedTile.zIndex === firstZIndex })) {
      data.multiEditZIndex = firstZIndex;
    }
    else {
      data.multiEditZIndex = undefined;
    }
    let firstSpriteName = selectedTiles[0].spriteName;
    if(_.every(selectedTiles, (selectedTile) => { return selectedTile.spriteName === firstSpriteName })) {
      data.multiEditTileSprite = firstSpriteName;
    }
    else {
      data.multiEditTileSprite = undefined;
    }
    let customHitboxPoints = selectedTiles[0].customHitboxPoints;
    if(_.every(selectedTiles, (selectedTile) => { return selectedTile.customHitboxPoints === customHitboxPoints })) {
      data.customHitboxPoints = customHitboxPoints;
    }
    else {
      data.customHitboxPoints = "";
    }
  },
  setMultiEditName() {
    let selectedTiles = app1.getTileSelectedTiles();
    for(let selectedTile of selectedTiles) {
      selectedTile.name = data.multiEditName;
    }
  },
  setMultiEditHitboxMode() {
    data.showTileHitboxes = true;
    let selectedTiles = app1.getTileSelectedTiles();
    for(let selectedTile of selectedTiles) {
      selectedTile.hitboxMode = Number(data.multiEditHitboxMode);
    }
    tileCanvas.redraw();
    tileCanvas.redrawUICanvas();
  },
  setMultiEditPoints() {
    let selectedTiles = app1.getTileSelectedTiles();
    for(let selectedTile of selectedTiles) {
      selectedTile.customHitboxPoints = data.customHitboxPoints;
    }
    tileCanvas.redraw();
    tileCanvas.redrawUICanvas();
  },
  setMultiEditTag() {
    let selectedTiles = app1.getTileSelectedTiles();
    for(let selectedTile of selectedTiles) {
      selectedTile.setTag(data.multiEditTag);
    }
    tileCanvas.redraw();
    tileCanvas.redrawUICanvas();
  },
  setMultiEditZIndex() {
    let selectedTiles = app1.getTileSelectedTiles();
    for(let selectedTile of selectedTiles) {
      selectedTile.zIndex = Number(data.multiEditZIndex);
    }
    tileCanvas.redraw();
    tileCanvas.redrawUICanvas();
  },
  setMultiEditTileSprite() {
    let selectedTiles = app1.getTileSelectedTiles();
    for(let selectedTile of selectedTiles) {
      selectedTile.spriteName = data.multiEditTileSprite;
    }
    data.lastSelectedTileSprite = data.multiEditTileSprite;
    tileCanvas.redraw();
    tileCanvas.redrawUICanvas();
  },
  getTileSpriteNames() {
    let spriteNames = [""];
    for(let sprite of data.sprites) {
      if(sprite.name.startsWith("Tile")) {
        spriteNames.push(sprite.name);
      }
    }
    return spriteNames;
  },
  addUndoJson() {
    if(this.undoIndex < this.undoJsons.length - 1) {
      this.undoJsons.length = this.undoIndex + 1;
    }
    let json = Helpers.serializeES6(data.selectedLevel);
    this.undoJsons.push(json);
    if(this.undoJsons.length > consts.MAX_UNDOS) {
      this.undoJsons.shift();
    }
    this.undoIndex = this.undoJsons.length - 1;
  },
  undo() {
    this.undoIndex--;
    if(this.undoIndex < 0) this.undoIndex = 0;
    else {
      let obj = JSON.parse(this.undoJsons[this.undoIndex]);
      let level = Helpers.deserializeES6(obj);
      for(let i = 0; i < data.levels.length; i++) {
        if(data.levels[i].name === level.name) {
          data.levels[i] = level;
        }
      }
      app1.changeLevel(level, true);
    }
  },
  redo() {
    this.undoIndex++;
    if(this.undoIndex >= this.undoJsons.length) this.undoIndex = this.undoJsons.length - 1;
    else {
      let obj = JSON.parse(this.undoJsons[this.undoIndex]);
      let level = Helpers.deserializeES6(obj);
      for(let i = 0; i < data.levels.length; i++) {
        if(data.levels[i].name === level.name) {
          data.levels[i] = level;
        }
      }
      app1.changeLevel(level, true);
    }
  },
  downloadImage() {
    var link = document.getElementById('link');
    link.setAttribute('download', 'level.png');
    link.setAttribute('href', data.selectedLevel.layers[0].toDataURL("image/png").replace("image/png", "image/octet-stream"));
    link.click();
  },
  addLayer() {
    data.selectedLevel.addCanvas(undefined);
    resetVue();
  },
  onLayerChange(newLayerIndex: number) {
  }
};

let computed = {
  displayZoom: {
    get () {
      return data.zoom * 100;
    },
    set (value: number) {
      data.zoom = value / 100;
    }
  }
}

var app1 = new Vue({
  el: '#app1',
  data: data,
  computed: computed,
  methods: methods,
  created: function() {
    $.get("get-spritesheets").then(response => {
      //console.log("Got spritesheets");
      data.spritesheets = _.map(response, (spritesheet) => {
        return new Spritesheet(spritesheet);
      });
      return $.get("get-tilesets");
    }, error => {
      console.log("Error getting spritesheets");      
    })
    .then(response => {
      //console.log("Got tilesets");
      data.tilesets = _.map(response, (tileset) => {
        return new Spritesheet(tileset);
      });
      return $.get("get-resource?resourceName=tiledata");
    }, error => {
      console.log("Error getting tilesets");
    })
    .then(response => {
      //console.log("Got tiledatas");
      data.tileDatas = Helpers.deserializeES6(response);
      for(let tileData of data.tileDatas) {
        tileData.setTileset(data.tilesets);
      }
      return $.get("get-resource?resourceName=tileanimation");
    }, error => {
      console.log("Error getting tiledatas");
    })
    .then(response => {
      //console.log("Got tileanimations");
      data.tileAnimations = Helpers.deserializeES6(response);
      for(let tileAnimation of data.tileAnimations) {
        tileAnimation.setTileDatas(data.tileDatas);
      }
      return $.get("get-resource?resourceName=tileclump");
    }, error => {
      console.log("Error getting tile animations");
    })
    .then(response => {
      //console.log("Got tileclumps");
      data.tileClumps = Helpers.deserializeES6(response);
      for(let tileClump of data.tileClumps) {
        tileClump.setTileDatas(data.tileDatas);
      }
      return $.get("get-level-images");
    }, error => {
      console.log("Error getting tile clumps");
    })
    .then(response => {
      data.levelImages = _.map(response, (levelImagePath) => {
        return new Spritesheet(levelImagePath);
      });
      return $.get("get-sprites");
    }, error => {
      console.log("Error getting level images");
    })
    .then(response => {
      //console.log("Got sprites");
      data.sprites = Helpers.deserializeES6(response);
      for(let sprite of data.sprites) {
        sprite.setSpritesheet(data.spritesheets);
      }
      return $.get("get-levels");
    }, error => {
      console.log("Error getting sprites");
    })
    .then(response => {
      //console.log("Got levels");
      data.levels = Helpers.deserializeES6(response);
      for(let level of data.levels) {
        for(let instance of level.instances) {
          instance.setSprite(data.sprites);
        }
      }
      data.loadCount = 0;
      data.maxLoadCount = data.tilesets.length + data.spritesheets.length + data.levelImages.length + 1;
      for(let tileset of data.tilesets) {
        tileset.loadImage(() => { data.loadCount++; checkIfDoneLoading(); })
      }
      for(let spritesheet of data.spritesheets) {
        spritesheet.loadImage(() => { data.loadCount++; checkIfDoneLoading(); })
      }
      for(let levelImage of data.levelImages) {
        levelImage.loadImage(() => { data.loadCount++; checkIfDoneLoading(); });
      }
      console.log("LOADING HASHCACHE");
      $.getJSON("hashCache/hashCaches.json", function(json: any) {
        console.log("LOADED HASHCACHE");
        hashCache = json;
        data.loadCount++;
        checkIfDoneLoading();
      }).fail(function() {
        console.log("ERROR: HASH CACHE NOT FOUND");
      });
    }, error => {
      console.log("Error getting levels");
    });
  }
});

function checkIfDoneLoading() {
  if(data.loadCount >= data.maxLoadCount) {
    loadHashCache(hashCache);
  }
}

function loadHashCache(json: any) {
  for(let tileset of data.tilesets) {
    let tilesetName = Helpers.baseName(tileset.path);
    let currentHashCache = json[tilesetName];
    if(!currentHashCache) {
      throw "Error: tile set with name \"" + tilesetName + "\" does not have a hash cache entry! Generate one with the C# utility program.";
    }

    let tileDataCache: { [key: string]: TileData } = {};
    for(let tileData of data.tileDatas) {
      if(tileData.tilesetPath === tileset.path) {
        tileDataCache[tilesetName + "," + String(tileData.gridCoords.i) + "," + String(tileData.gridCoords.j)] = tileData;
      }
    }

    let grid: TileData[][] = [];
    for(let i = 0; i < tileset.imgEl.height / consts.TILE_WIDTH; i++) {
      let row: TileData[] = [];
      grid.push(row);
      for(let j = 0; j < tileset.imgEl.width / consts.TILE_WIDTH; j++) {
        let linkedCoords = currentHashCache[String(i) + "," + String(j)];
        if(!linkedCoords) {
          row.push(undefined);
          continue;
        }
        let otherI = Number(linkedCoords.split(",")[0]);
        let otherJ = Number(linkedCoords.split(",")[1]);
        
        let tileData = tileDataCache[tilesetName + "," + String(i) + "," + String(j)];
        if(!tileData) {
          tileData = grid[otherI][otherJ];
        }
        if(!tileData) {
          tileData = new TileData(tileset, new GridCoords(i, j), "");
          tileDataCache[tilesetName + "," + String(tileData.gridCoords.i) + "," + String(tileData.gridCoords.j)] = tileData;
        }
        row.push(tileData);
      }
    }
    console.log("LOADED GRID");
    data.tileDataGrids[tileset.path] = grid;
  }
}

function getTileDatas() {
  let tileDatas = new Set<TileData>();
  for(let key of Object.keys(data.tileDataGrids)) {
    let grid = data.tileDataGrids[key];
    for(let i = 0; i < grid.length; i++) {
      for(let j = 0; j < grid[i].length; j++) {
        if(grid[i][j]) {
          tileDatas.add(grid[i][j]);
        }
      }
    }
  }
  
  return Array.from(tileDatas);
}
//@ts-ignore
window.getTileDatas = getTileDatas;

let app2 = new Vue({
  el: '#app2',
  data: data,
  methods: methods,
  computed: computed
});

let app3 = new Vue({
  el: '#app3',
  data: data,
  methods: methods,
  computed: computed
});

let app4 = new Vue({
  el: '#app4',
  data: data,
  methods: methods,
  computed: computed
});

let app0 = new Vue({
  el: '#app0',
  data: data,
  methods: methods,
  computed: computed
});

//@ts-ignore
window.app1 = app1;

function resetVue() {
  app1.$forceUpdate();
  app2.$forceUpdate();
  app3.$forceUpdate();
  app4.$forceUpdate();
  app0.$forceUpdate();
}

//If undefined is returned, then a rectangle wasn't selected, disjoint selection
function getLevelSelectedGridRect() {
  if(data.levelSelectedCoords.length === 0) return undefined;
  let topLeftSelectedLevelCoord = new GridCoords(_.minBy(data.levelSelectedCoords, "i").i, _.minBy(data.levelSelectedCoords, "j").j);
  let botRightSelectedLevelCoord = new GridCoords(_.maxBy(data.levelSelectedCoords, "i").i, _.maxBy(data.levelSelectedCoords, "j").j);
  let area = (botRightSelectedLevelCoord.j - topLeftSelectedLevelCoord.j + 1) * (botRightSelectedLevelCoord.i - topLeftSelectedLevelCoord.i + 1);
  if(area !== data.levelSelectedCoords.length) return undefined;
  return new GridRect(topLeftSelectedLevelCoord.i, topLeftSelectedLevelCoord.j, botRightSelectedLevelCoord.i, botRightSelectedLevelCoord.j);
}

function getTileSelectedGridRect() {
  if(data.tileSelectedCoords.length === 0) return undefined;
  let topLeftSelectedTileCoord = new GridCoords(_.minBy(data.tileSelectedCoords, "i").i, _.minBy(data.tileSelectedCoords, "j").j);
  let botRightSelectedTileCoord = new GridCoords(_.maxBy(data.tileSelectedCoords, "i").i, _.maxBy(data.tileSelectedCoords, "j").j);
  let area = (botRightSelectedTileCoord.j - topLeftSelectedTileCoord.j + 1) * (botRightSelectedTileCoord.i - topLeftSelectedTileCoord.i + 1);
  if(area !== data.tileSelectedCoords.length) return undefined;
  return new GridRect(topLeftSelectedTileCoord.i, topLeftSelectedTileCoord.j, botRightSelectedTileCoord.i, botRightSelectedTileCoord.j);
}

function getTopLeftSelectedTileGridCoord() {
  return new GridCoords(_.minBy(data.tileSelectedCoords, "i").i, _.minBy(data.tileSelectedCoords, "j").j);
}

function saveScrollPositions() {
  levelCanvas.saveScrollPos(data.selectedLevel.name);
  tileCanvas.saveScrollPos(data.selectedTileset.path);
}

window.onbeforeunload = () => {
  saveScrollPositions();
}

}
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
import { POI } from "./models/POI";
import { GridCoords } from "./models/GridCoords";
import { GridRect } from "./models/GridRect";

class Data {
  origSprites: { [path: string]: Sprite } = {};
  sprites: Sprite[] = [];
  selectedSprite: Sprite = undefined;
  spritesheets: Spritesheet[] = [];
  selectedSpritesheet: Spritesheet = undefined;
  selection: Selectable = undefined;
  selectedFrame: Frame = undefined;
  isAnimPlaying: boolean = false;
  addPOIMode: boolean = false;
  alignments: string[] = [ "topleft", "topmid", "topright", "midleft", "center", "midright", "botleft", "botmid", "botright"];
  wrapModes: string[] = ["loop", "once", "pingpong", "pingpongonce"];
  offsetX: number = 0;
  offsetY: number = 0;
  hideGizmos: boolean = false;
  flipX: boolean = false;
  flipY: boolean = false;
  bulkDuration: number = 0;
  newSpriteName: string = "";
  ghost: Ghost = undefined;
  lastSelectedFrameIndex: number = 0;
  tileMode: boolean = false;
  tileModeOffsetX: boolean = false;
  tileModeOffsetY: boolean = false;
  spriteFilter: string = "";
  selectedFilterMode: string = "contains";
  tileWidth: number = 8;
  constructor() {

  }
}

class Ghost {
  sprite: Sprite;
  frame: Frame;
  constructor(sprite: Sprite, frame: Frame) {
    this.sprite = sprite;
    this.frame = frame;
  }
}

//@ts-ignore
window.initSpriteEditor = function() {

let data = new Data();
//@ts-ignore
window.data = data;

class SpriteEditorInput extends GlobalInput {

  constructor() {
    super();
  }

  onKeyDown(keyCode: KeyCode, firstFrame: boolean) {
    if(firstFrame) {
      if(keyCode === KeyCode.E) {
        app1.selectNextFrame();
      } 
      else if(keyCode === KeyCode.Q) {
        app1.selectPrevFrame();
      }
    }
    spriteCanvas.redraw();
  }

  onKeyUp(keyCode: KeyCode) {
  }

}
new SpriteEditorInput();

class SpriteCanvas extends CanvasUI {

  constructor() {
    super("#canvas1", "lightgray");
    this.isNoScrollZoom = true;
    this.zoom = 5;
  }

  redraw() {
    
    super.redraw();

    if(!data.selectedSprite) return;

    let frame: Frame = undefined;

    if(!data.isAnimPlaying) {
      if(data.selectedFrame && data.selectedSpritesheet && data.selectedSpritesheet.imgEl) {
        frame = data.selectedFrame;
      }
    }
    else {
      frame = data.selectedSprite.frames[animFrameIndex];
    }

    if(!frame) return;

    let cX = this.canvas.width/2;
    let cY = this.canvas.height/2;

    let frameIndex = data.selectedSprite.frames.indexOf(frame);
    /*
    if(frameIndex < 0 && frame.parentFrameIndex !== undefined) {
      frameIndex = frame.parentFrameIndex;
    }
    */

    if(frameIndex < 0) {
      data.selectedSprite.drawFrame(this.ctx, frame, cX, cY, frame.xDir, frame.yDir);
    }
    else {
      data.selectedSprite.draw(this.ctx, frameIndex, cX, cY, frame.xDir, frame.yDir);
    }

    if(data.ghost) {
      data.ghost.sprite.draw(this.ctx, data.ghost.sprite.frames.indexOf(data.ghost.frame), cX, cY, frame.xDir, frame.yDir, "", 0.5);  
    }

    if(!data.hideGizmos) {
      for(let hitbox of getVisibleHitboxes()) {

        let hx: number; let hy: number;
        let halfW = hitbox.width * 0.5;
        let halfH = hitbox.height * 0.5;
        let w = halfW * 2;
        let h = halfH * 2;
        if(data.selectedSprite.alignment === "topleft") {
          hx = cX; hy = cY;
        }
        else if(data.selectedSprite.alignment === "topmid") {
          hx = cX - halfW; hy = cY;
        }
        else if(data.selectedSprite.alignment === "topright") {
          hx = cX - w; hy = cY;
        }
        else if(data.selectedSprite.alignment === "midleft") {
          hx = cX; hy = cY - halfH;
        }
        else if(data.selectedSprite.alignment === "center") {
          hx = cX - halfW; hy = cY - halfH;
        }
        else if(data.selectedSprite.alignment === "midright") {
          hx = cX - w; hy = cY - halfH;
        }
        else if(data.selectedSprite.alignment === "botleft") {
          hx = cX; hy = cY - h;
        }
        else if(data.selectedSprite.alignment === "botmid") {
          hx = cX - halfW; hy = cY - h;
        }
        else if(data.selectedSprite.alignment === "botright") {
          hx = cX - w; hy = cY - h;
        }

        let offsetRect = new Rect(
          hx + hitbox.offset.x, hy + hitbox.offset.y, hx + hitbox.width + hitbox.offset.x, hy + hitbox.height + hitbox.offset.y
        );

        let strokeColor;
        let strokeWidth;
        if(data.selection === hitbox) {
          strokeColor = "blue";
          strokeWidth = 2;
        }

        Helpers.drawRect(this.ctx, offsetRect, "blue", strokeColor, strokeWidth, 0.25);
      }
      
      let len = 1000;
      Helpers.drawLine(this.ctx, cX, cY - len, cX, cY + len, "red", 1);
      Helpers.drawLine(this.ctx, cX - len, cY, cX + len, cY, "red", 1);
      Helpers.drawCircle(this.ctx, cX, cY, 1, "red");
      //drawStroked(c1, "+", cX, cY);
      
      for(let poi of frame.POIs) {
        Helpers.drawCircle(this.ctx, cX + poi.x, cY + poi.y, 1, "green");
      }

    }

  }

  onMouseUp(whichMouse: MouseButton) {
  }

  onMouseWheel(delta: number) {
  }

  onMouseMove(deltaX: number, deltaY: number) {
    if(data.selection && this.mousedown) {
      data.selection.move(deltaX, deltaY);
      this.redraw();
    }
  }

  onLeftMouseDown() {
    let cX = (this.canvas.width)/2;
    let cY = (this.canvas.height)/2;
    if(data.addPOIMode) {
      data.addPOIMode = false;
      app1.addPOI(data.selectedFrame, (this.mouseX - cX)/this.zoom, (this.mouseY - cY)/this.zoom);
      return;
    }
    let found = false;
    let selectables = app1.getSelectables();
    for(let selectable of selectables) {
      let rect = selectable.getRect().clone(cX/this.zoom, cY/this.zoom);
      if(Helpers.inRect((this.mouseX - cX)/this.zoom, (this.mouseY - cY)/this.zoom, rect)) {
        data.selection = selectable;
        found = true;
      }
    }
    if(!found) data.selection = null;
    this.redraw();
  }

  onKeyDown(keyCode: KeyCode, firstFrame: boolean) {
    
    if(keyCode === KeyCode.ESCAPE) {
      data.selection = null;
      data.ghost = null;
    }

    if(data.selectedFrame) {
      if(keyCode === KeyCode.G) {
        data.ghost = new Ghost(data.selectedSprite, data.selectedFrame);
      }
    }

    if(data.selection && firstFrame) {
      if(keyCode === KeyCode.A) {
        data.selection.move(-1, 0);
      }
      else if(keyCode === KeyCode.D) {
        data.selection.move(1, 0);
      }
      else if(keyCode === KeyCode.W) {
        data.selection.move(0, -1);
      }
      else if(keyCode === KeyCode.S) {
        data.selection.move(0, 1);
      }
      else if(keyCode === KeyCode.LEFT) {
        data.selection.resizeCenter(-1, 0);
      }
      else if(keyCode === KeyCode.RIGHT) {
        data.selection.resizeCenter(1, 0);
      }
      else if(keyCode === KeyCode.DOWN) {
        data.selection.resizeCenter(0, -1);
      }
      else if(keyCode === KeyCode.UP) {
        data.selection.resizeCenter(0, 1);
      }
    }
    else if(data.selectedFrame && firstFrame) {
      if(keyCode === KeyCode.A) {
        data.selectedFrame.offset.x -= 1;
      }
      else if(keyCode === KeyCode.D) {
        data.selectedFrame.offset.x += 1;
      }
      else if(keyCode === KeyCode.W) {
        data.selectedFrame.offset.y -= 1;
      }
      else if(keyCode === KeyCode.S) {
        data.selectedFrame.offset.y += 1;
      }
    }
    this.redraw();
  }
}
let spriteCanvas = new SpriteCanvas();

class SpritesheetCanvas extends CanvasUI {

  constructor() {
    super("#canvas2");
  }

  onLeftMouseDown() {
    if(data.selectedSprite === null) return;

    for(let frame of data.selectedSprite.frames) {
      if(Helpers.inRect(this.mouseX,this.mouseY,frame.rect)) {
        data.selectedFrame = frame;
        this.redraw();
        spriteCanvas.redraw();
        return;
      }
    }

    if(data.selectedSpritesheet === null) return;

    let rect: Rect = undefined;
    if(!data.tileMode) {
      //No frame clicked, see if continous image was clicked, if so add to pending
      rect = Helpers.getPixelClumpRect(this.mouseX, this.mouseY, data.selectedSpritesheet.imgArr);
    }
    else {
      if(data.tileModeOffsetX || data.tileModeOffsetY) {
        let finalX = this.getMouseGridCoordsCustomWidth(data.tileWidth).j;
        let finalY = this.getMouseGridCoordsCustomWidth(data.tileWidth).i;
        if(data.tileModeOffsetX) {
          let x = this.mouseX / data.tileWidth;
          let intX = Math.floor(x);
          if(x - intX < 0.5) finalX = intX - 0.5;
          else finalX = intX + 0.5;
        }
        if(data.tileModeOffsetY) {
          let y = this.mouseY / data.tileWidth;
          let intY = Math.floor(y);
          if(y - intY < 0.5) finalY = intY - 0.5;
          else finalY = intY + 0.5;
        }
        rect = new GridCoords(finalY, finalX).getRectCustomWidth(data.tileWidth);
      }
      else {
        rect = this.getMouseGridCoordsCustomWidth(data.tileWidth).getRectCustomWidth(data.tileWidth);
      }
    }
    
    if(rect) {
      data.selectedFrame = new Frame(rect, 0.066, new Point(0,0));
      this.redraw();
      spriteCanvas.redraw();
    }

    spriteCanvas.redraw();
    event.preventDefault();
  }
  
  onLeftMouseUp() {
    let area = (Math.abs(this.dragBotY - this.dragTopY) * Math.abs(this.dragRightX - this.dragLeftX));
    if(area > 10) {
      if(!data.tileMode) {
        getSelectedPixels();
      }
      else {
        let topLeft = new GridCoords(Math.floor(this.dragTopY / data.tileWidth), Math.floor(this.dragLeftX / data.tileWidth));
        let botRight = new GridCoords(Math.floor(this.dragBotY / data.tileWidth), Math.floor(this.dragRightX / data.tileWidth));
        let rect = new Rect(topLeft.j * data.tileWidth, topLeft.i * data.tileWidth, (botRight.j+1) * data.tileWidth, (botRight.i+1) * data.tileWidth);
        data.selectedFrame = new Frame(rect, 0.066, new Point(0,0));
        this.redraw();
        spriteCanvas.redraw();
      }
    }
  }
  
  onMouseMove() {
    if(this.mousedown) {
      this.redraw();
    }
  }
  
  onMouseLeave() {
    this.redraw();
  }
  
  onMouseWheel(delta: number) {
    /*
    if(this.isHeld(KeyCode.CONTROL)) {
      this.zoom += (delta/180)*0.1;
    }
    resetVue();
    this.redraw();
    */
  }

  onKeyDown(keyCode: KeyCode, firstFrame: boolean) {
    
    if(data.selectedFrame) {
      if(keyCode === KeyCode.F) {
        app1.addPendingFrame();
      }
      else if(keyCode === KeyCode.C && data.selectedSprite.frames.length > 0) {
        data.selectedFrame.parentFrameIndex = data.lastSelectedFrameIndex;
        data.selectedSprite.frames[data.lastSelectedFrameIndex].childFrames.push(data.selectedFrame);
        spriteCanvas.redraw();
        spriteSheetCanvas.redraw();
      }
    }

    this.redraw();
  }

  redraw() {
    super.redraw();
    this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
    
    if(data.selectedSpritesheet && data.selectedSpritesheet.imgEl) {
      this.ctx.drawImage(data.selectedSpritesheet.imgEl, 0, 0);
    }
  
    if(data.tileMode) {
      //Draw columns
      for(let i = 1; i < this.canvas.width / data.tileWidth; i++) {
        Helpers.drawLine(this.ctx, i * data.tileWidth, 0, i * data.tileWidth, this.canvas.height, "red", 1);
      }
      //Draw rows
      for(let i = 1; i < this.canvas.height / data.tileWidth; i++) {
        Helpers.drawLine(this.ctx, 0, i * data.tileWidth, this.canvas.width, i * data.tileWidth, "red", 1);
      }
    }
  
    if(this.mousedown) {
      Helpers.drawRect(this.ctx, new Rect(this.dragLeftX, this.dragTopY, this.dragRightX, this.dragBotY), "", "blue", 1);
    }
  
    if(data.selectedSprite) {
      let i = 0;
      for(let frame of data.selectedSprite.frames) {
        Helpers.drawRect(this.ctx,frame.rect, "", "blue", 1);
        Helpers.drawText(this.ctx,String(i+1),frame.rect.x1, frame.rect.y1, "red", undefined, 12, "left", "Top", "Arial");
        i++;
      }
    }
  
    if(data.selectedFrame) {
      Helpers.drawRect(this.ctx, data.selectedFrame.rect, "", "green", 2);
    }
    
  }

}
let spriteSheetCanvas = new SpritesheetCanvas();

let methods = {
  onSpriteFilterChange() {
  },
  getFilteredSprites() {
    let filters = data.spriteFilter.split(",");
    if(filters[0] === "") return data.sprites;
    return _.filter(data.sprites, (sprite: Sprite) => {
      if(data.selectedFilterMode === "exactmatch") {
        return filters.indexOf(sprite.name) >= 0;
      }
      else if(data.selectedFilterMode === "contains") {
        for(let filter of filters) {
          if(sprite.name.toLowerCase().includes(filter.toLowerCase())) {
            return true;
          }
        }
      }
      else if(data.selectedFilterMode === "startswith") {
        for(let filter of filters) {
          if(sprite.name.startsWith(filter)) {
            return true;
          }
        }
      }
      else if(data.selectedFilterMode === "endswith") {
        for(let filter of filters) {
          if(sprite.name.endsWith(filter)) {
            return true;
          }
        }
      }
      return false;
    });
  },
  onSpritesheetChange(newSheet: Spritesheet, isNew: boolean) {

    let newSpriteAndSheetSel = isNew && (data.selectedSpritesheet !== undefined);
    
    if(newSpriteAndSheetSel) {
      data.selectedSprite.spritesheet = data.selectedSpritesheet;
      return;
    }

    /*
    if(newSheet === data.selectedSpritesheet) {
      return;
    }
    */

    data.selectedSpritesheet = newSheet;

    if(!data.selectedSpritesheet) {
      spriteCanvas.redraw();
      spriteSheetCanvas.redraw();
      return;
    }

    if(data.selectedSprite) {
      data.selectedSprite.spritesheet = newSheet;
    }

    if(newSheet.imgEl) {
      return;
    }
    let spritesheetImg = document.createElement("img");
    spritesheetImg.onload = function() {
      spriteSheetCanvas.setSize(spritesheetImg.width, spritesheetImg.height);
      spriteSheetCanvas.ctx.drawImage(spritesheetImg, 0, 0);      
      let imageData = spriteSheetCanvas.ctx.getImageData(0,0,spriteSheetCanvas.canvas.width,spriteSheetCanvas.canvas.height);
      newSheet.imgArr = Helpers.get2DArrayFromImage(imageData);
      newSheet.imgEl = spritesheetImg;
      spriteCanvas.redraw();
      spriteSheetCanvas.redraw();
    };
    spritesheetImg.src = newSheet.path;
  },
  getSpriteDisplayName(sprite: Sprite) {
    return sprite.name + (app1.isSpriteChanged(sprite) ? '*' : '');
  },
  addSprite() {
    let spritename = prompt("Enter a sprite name");
    let newSprite = new Sprite(spritename, undefined);
    this.changeSprite(newSprite, true);
    data.sprites.push(newSprite);
    data.selectedFrame = null;
    data.selection = null;
    resetVue();
  },
  changeSprite(newSprite: Sprite, isNew: boolean) {
    data.selectedSprite = newSprite;
    this.onSpritesheetChange(newSprite.spritesheet, isNew);
    data.selection = null;
    data.selectedFrame = data.selectedSprite.frames[0];
    data.lastSelectedFrameIndex = 0;
    spriteCanvas.redraw();
    spriteSheetCanvas.redraw();
  },
  addHitboxToSprite(sprite: Sprite) {
    let hitbox = new Hitbox();
    hitbox.width = data.selectedFrame.rect.w;
    hitbox.height = data.selectedFrame.rect.h;
    sprite.hitboxes.push(hitbox);
    this.selectHitbox(hitbox);
    spriteCanvas.redraw();
  },
  addHitboxToFrame(frame: Frame) {
    let hitbox = new Hitbox();
    hitbox.width = data.selectedFrame.rect.w;
    hitbox.height = data.selectedFrame.rect.h;
    frame.hitboxes.push(hitbox);
    this.selectHitbox(hitbox);
    spriteCanvas.redraw();
  },
  selectHitbox(hitbox: Hitbox) {
    data.selection = hitbox;
    spriteCanvas.wrapper.focus();
    spriteCanvas.redraw();
  },
  deleteHitbox(hitboxArr: Hitbox[], hitbox: Hitbox) {
    _.pull(hitboxArr, hitbox);
    resetVue();
    spriteCanvas.redraw();
  },
  isSelectedFrameAdded() {
    return _.includes(data.selectedSprite.frames, data.selectedFrame);
  },
  addPendingFrame(index: number = undefined) {
    data.selectedFrame = _.cloneDeep(data.selectedFrame);
    if(index === undefined) {
      data.selectedSprite.frames.push(data.selectedFrame);
    }
    else {
      data.selectedSprite.frames[index] = data.selectedFrame;
    }
    spriteCanvas.redraw();
    spriteSheetCanvas.redraw();
  },
  selectFrame(frame: Frame) {
    data.selectedFrame = frame;
    if(frame.parentFrameIndex === undefined) {
      data.lastSelectedFrameIndex = data.selectedSprite.frames.indexOf(frame);
    }
    else {
      data.lastSelectedFrameIndex = frame.parentFrameIndex;
    }
    spriteCanvas.redraw();
    spriteSheetCanvas.redraw();
    resetVue();
  },
  copyFrame(frame: Frame, dir: number) {
    let index = data.selectedSprite.frames.indexOf(frame);
    if(dir === -1) dir = 0;
    data.selectedSprite.frames.splice(index + dir, 0, _.cloneDeep(frame));
  },
  moveFrame(frame: Frame, dir: number) {
    let index = data.selectedSprite.frames.indexOf(frame);
    if(index + dir < 0 || index + dir >= data.selectedSprite.frames.length) return;
    let temp = data.selectedSprite.frames[index];
    data.selectedSprite.frames[index] = data.selectedSprite.frames[index + dir];
    data.selectedSprite.frames[index + dir] = temp;
  },
  deleteFrame(frame: Frame) {
    if(frame.parentFrameIndex === undefined) {
      _.pull(data.selectedSprite.frames, frame);
      data.selectedFrame = data.selectedSprite.frames[0];
      data.lastSelectedFrameIndex = 0;
    }
    else {
      let parentFrame = data.selectedSprite.frames[frame.parentFrameIndex];
      if(!parentFrame) parentFrame = data.selectedSprite.frames[0];
      _.pull(parentFrame.childFrames, frame);
    }
    spriteCanvas.redraw();
    spriteSheetCanvas.redraw();
    resetVue();
  },
  selectNextFrame() {
    data.selection = null;
    let frameIndex = data.selectedSprite.frames.indexOf(data.selectedFrame);
    let selectedFrame = data.selectedSprite.frames[frameIndex + 1];
    if(!selectedFrame) selectedFrame = data.selectedSprite.frames[0] || null;
    this.selectFrame(selectedFrame);
  },
  selectPrevFrame() {
    data.selection = null;
    let frameIndex = data.selectedSprite.frames.indexOf(data.selectedFrame);
    let selectedFrame = data.selectedSprite.frames[frameIndex - 1];
    if(!selectedFrame) selectedFrame = data.selectedSprite.frames[data.selectedSprite.frames.length-1] || null;
    this.selectFrame(selectedFrame);
  },
  playAnim() {
    data.isAnimPlaying = !data.isAnimPlaying;
    if(!data.isAnimPlaying) {
      animFrameIndex = 0;
    }
  },
  saveSprite() {
    let jsonStr = Helpers.serializeES6(data.selectedSprite);
    $.post("save-sprite", JSON.parse(jsonStr)).then(response => {
      console.log("Successfully saved sprite");
      data.origSprites[data.selectedSprite.name] = _.clone(data.selectedSprite);
      resetVue();
    }, error => {
      console.log("Failed to save sprite");
    });
  },
  saveSprites() {

    let jsonStr = "[";
    console.log("Saving sprites:");
    for(let sprite of data.sprites) {
      if(app1.isSpriteChanged(sprite)) {
        jsonStr += Helpers.serializeES6(sprite);
        jsonStr += ",";
        console.log(sprite.name);
      }
    }
    if(jsonStr[jsonStr.length - 1] === ",") jsonStr = jsonStr.slice(0, -1);
    jsonStr += "]";
    //console.log(jsonStr);
    $.post("save-sprites", { "data": JSON.parse(jsonStr) }).then(response => {
      console.log("Successfully saved sprites");
      for(let sprite of data.sprites) {
        if(app1.isSpriteChanged(sprite)) {
          data.origSprites[sprite.name] = _.clone(sprite);
        }
      }
      resetVue();
    }, error => {
      console.log("Failed to save sprites");
    });
  },
  onSpriteAlignmentChange() {
    spriteCanvas.redraw();
  },
  redraw() {
    spriteCanvas.redraw();
    spriteSheetCanvas.redraw();
  },
  onBulkDurationChange() {
    for(let frame of data.selectedSprite.frames) {
      frame.duration = data.bulkDuration;
    }
    resetVue();
  },
  onLoopStartChange() {
    resetVue();
  },
  onWrapModeChange() {
    spriteCanvas.redraw();
    resetVue();
  },
  reverseFrames() {
    _.reverse(data.selectedSprite.frames);
    spriteCanvas.redraw();
    resetVue();
  },
  addPOI(frame: Frame, x: number, y: number) {
    var poi = new POI("", x, y);
    frame.POIs.push(poi);
    this.selectPOI(poi);
    spriteCanvas.redraw();
  },
  selectPOI(poi: POI) {
    this.selection = poi;
    spriteCanvas.wrapper.focus();
    spriteCanvas.redraw();
  },
  deletePOI(poi: POI) {
    _.pull(data.selectedFrame.POIs, poi);
    spriteCanvas.redraw();
    resetVue();
  },
  getSelectables(): Selectable[] {
    let selectables: Selectable[] = [];
    for(let hitbox of data.selectedSprite.hitboxes) {
      selectables.push(hitbox);
    }
    for(let hitbox of data.selectedFrame.hitboxes) {
      selectables.push(hitbox);
    }
    for(let poi of data.selectedFrame.POIs) {
      selectables.push(poi);
    }
    return selectables;
  },
  isSpriteChanged(sprite: Sprite) {
    return !_.isEqual(sprite, data.origSprites[sprite.name]);
  }
};

let computed = {
  displayZoom: {
    get () {
      return this.zoom * 100;
    },
    set (value: number) {
      this.zoom = value;
    }
  }
}

let app1 = new Vue({
  el: '#app1',
  data: data,
  computed: computed,
  methods: methods,
  //LOAD SECTION, LOADING
  created: function() {
    $.get("get-spritesheets").then(response => {
      //console.log(response);
      data.spritesheets = _.map(response, (spritesheet) => {
        return new Spritesheet(spritesheet);
      });
      $.get("get-sprites").then(response => {
        //console.log(response);
        data.sprites = Helpers.deserializeES6(response) || [];
        for(let sprite of data.sprites) {
          sprite.setSpritesheet(data.spritesheets);
          data.origSprites[sprite.name] = _.clone(sprite);
        }
      }, error => {
        console.log("Error getting sprites");
      });

    }, error => {
      console.log("Error getting sprites");      
    });
    spriteCanvas.redraw();
    spriteSheetCanvas.redraw();
  }
});

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

function resetVue() {
  app1.$forceUpdate();
  app2.$forceUpdate();
  app3.$forceUpdate();
}

let animFrameIndex = 0;
let animTime = 0;

setInterval(mainLoop, 1000 / 60);

function getSelectedPixels() {
  if(!data.selectedSpritesheet) return;
  let rect = Helpers.getSelectedPixelRect(spriteSheetCanvas.dragLeftX, spriteSheetCanvas.dragTopY, spriteSheetCanvas.dragRightX, spriteSheetCanvas.dragBotY, data.selectedSpritesheet.imgArr);
  if(rect) {
    data.selectedFrame = new Frame(rect, 0.066, new Point(0,0));
    spriteCanvas.redraw();
    spriteSheetCanvas.redraw();
  }
}

function mainLoop() {
  if(data.isAnimPlaying) {
    animTime += 1000 / 60;
    let frames = data.selectedSprite.frames;
    if(animTime >= frames[animFrameIndex].duration * 1000) {
      animFrameIndex++;
      if(animFrameIndex >= frames.length) {
        animFrameIndex = 0;
      }
      animTime = 0;
    }
    spriteCanvas.redraw();
  }
}

function getVisibleHitboxes() {
  let hitboxes: Hitbox[] = [];
  if(data.selectedSprite) {
    hitboxes = hitboxes.concat(data.selectedSprite.hitboxes);
  }
  if(data.selectedFrame) {
    hitboxes = hitboxes.concat(data.selectedFrame.hitboxes);
  }
  return hitboxes;
}

function drawSprite() {

}

}
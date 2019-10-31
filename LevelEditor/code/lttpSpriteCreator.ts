import { Sprite } from "./models/Sprite";
import { Spritesheet } from "./models/Spritesheet";
import { Frame } from "./models/Frame";
import { Rect } from "./models/Rect";
import { Point } from "./models/point";
import * as Helpers from "./helpers";
import * as _ from "lodash";

/*
class AnimationType
{
  Name: string;
  Steps: Step[];
}

class Step
{
  Sprites: SpriteStep[];
  Length: number;
  Shadow: string;
}

class SpriteStep
{
  Row: string;
  Col: number;
  pos: number[];
  size: string;
  trans: string;
}
*/

function getRowFromStr(str: string) {
  if(str === "AA") return 26;
  if(str === "AB") return 27;
  if(str === "SWORD") return 28;
  if(str === "SHIELD") return 36;
  if(str === "CANE") return 39;
  if(str === "SHADOW") return 35;
  if(str === "ITEMSHADOW") return 35;
  if(str === "BOOK") return 35;
  if(str === "ROD") return 40;
  if(str === "HAMMER") return 41;
  if(str === "HOOKSHOT") return 42;
  if(str === "NET") return 43;
  if(str === "BOW") return 44;
  if(str === "SHOVEL") return 45;
  if(str === "BUSH") return 35;
  if(str === "ITEMSHADOW") return 45;
  if(str === "PENDANT") return 35;
  if(str === "CRYSTAL") return 35;
  if(str === "DUCK") return 45;
  if(str === "GRASS") return 48;
  if(str === "BED") return 46;

  if(str.length > 1) return -1;
  return str.charCodeAt(0) - "A".charCodeAt(0)
}

function getSizeFromStr(str: string) {
  if(str === "FULL") return new Point(16, 16);
  if(str === "RIGHT_HALF") return new Point(8, 16);
  if(str === "LEFT_HALF") return new Point(8, 16);
  if(str === "BOTTOM_HALF") return new Point(16, 8);
  if(str === "TOP_HALF") return new Point(16, 8);
  if(str === "TOP_LEFT") return new Point(8, 8);
  if(str === "TOP_RIGHT") return new Point(8, 8);
  if(str === "BOTTOM_LEFT") return new Point(8, 8);
  if(str === "BOTTOM_RIGHT") return new Point(8, 8);
  if(str === "WIDE_24X8") return new Point(24, 8);
  if(str === "LARGE_16X24") return new Point(16, 24);
  if(str === "TALL_8X24") return new Point(8, 24);
  if(str === "LARGE_32X24") return new Point(32, 24);
  if(str === "EMPTY") return new Point(0, 0);
  return undefined;
}

function getSizeOffsetFromStr(str: string) {
  if(str === "FULL") return new Point(0, 0);
  if(str === "RIGHT_HALF") return new Point(8, 0);
  if(str === "LEFT_HALF") return new Point(0, 0);
  if(str === "BOTTOM_HALF") return new Point(0, 8);
  if(str === "TOP_HALF") return new Point(0, 0);
  if(str === "TOP_LEFT") return new Point(0, 0);
  if(str === "TOP_RIGHT") return new Point(8, 0);
  if(str === "BOTTOM_LEFT") return new Point(0, 8);
  if(str === "BOTTOM_RIGHT") return new Point(8, 8);
  if(str === "WIDE_24X8") return new Point(0, 0);
  if(str === "LARGE_16X24") return new Point(0, 0);
  if(str === "TALL_8X24") return new Point(0, 0);
  if(str === "LARGE_32X24") return new Point(0, 0);
  if(str === "EMPTY") return new Point(0, 0);
  return undefined;
}

function spriteMapping(origName: string) {
  if(origName === "Stand") return "LinkIdleRight";
  if(origName === "Stand (up)") return "LinkIdleUp";
  if(origName === "Stand (down)") return "LinkIdleDown";
  if(origName === "Walk") return "LinkIdleMoveRight";
  if(origName === "Walk (up)") return "LinkIdleMoveUp";
  if(origName === "Walk (down)") return "LinkIdleMoveDown";
  if(origName === "Swim") return "LinkSwimMoveRight";
  if(origName === "Swim (up)") return "LinkSwimMoveUp";
  if(origName === "Swim (down)") return "LinkSwimMoveDown";
  if(origName === "Treading water") return "LinkSwimRight";
  if(origName === "Treading water (up)") return "LinkSwimUp";
  if(origName === "Treading water (down)") return "LinkSwimDown";
  if(origName === "Attack") return "LinkAttackRight";
  if(origName === "Attack (up)") return "LinkAttackUp";
  if(origName === "Attack (down)") return "LinkAttackDown";
  if(origName === "Spin attack") return "LinkSpinRight";
  if(origName === "Spin attack (left)") return "LinkSpinLeft";
  if(origName === "Spin attack (up)") return "LinkSpinUp";
  if(origName === "Spin attack (down)") return "LinkSpinDown";
  if(origName === "Dash spinup") return "LinkDashChargeRight";
  if(origName === "Dash spinup (up)") return "LinkDashChargeUp";
  if(origName === "Dash spinup (down)") return "LinkDashChargeDown";
  if(origName === "Dash release") return "LinkDashRight";
  if(origName === "Dash release (up)") return "LinkDashUp";
  if(origName === "Dash release (down)") return "LinkDashDown";
  if(origName === "Fall") return "LinkFall";
  if(origName === "Zap") return "LinkZap";
  if(origName === "Bonk") return "LinkHurtRight";
  if(origName === "Bonk (up)") return "LinkHurtUp";
  if(origName === "Bonk (down)") return "LinkHurtDown";
  if(origName === "Death spin") return "LinkDie";
  if(origName === "Grab") return "LinkGrabRight";
  if(origName === "Grab (up)") return "LinkGrabUp";
  if(origName === "Grab (down)") return "LinkGrabDown";
  if(origName === "Lift") return "LinkLiftRight";
  if(origName === "Lift (up)") return "LinkLiftUp";
  if(origName === "Lift (down)") return "LinkLiftDown";
  if(origName === "Carry") return "LinkCarryRight";
  if(origName === "Carry (up)") return "LinkCarryUp";
  if(origName === "Carry (down)") return "LinkCarryDown";
  if(origName === "Throw") return "LinkThrowRight";
  if(origName === "Throw (up)") return "LinkThrowUp";
  if(origName === "Throw (down)") return "LinkThrowDown";
  if(origName === "Push") return "LinkPushRight";
  if(origName === "Push (up)") return "LinkPushUp";
  if(origName === "Push (down)") return "LinkPushDown";
  if(origName === "Tree pull") return "LinkTreePull";
  if(origName === "Salute") return "LinkSalute";
  if(origName === "Item get") return "LinkItemGet";
  if(origName === "Crystal get") return "LinkCrystalGet";
  if(origName === "Swag duck") return "LinkSwagDuck";
  if(origName === "Bow") return "LinkBowRight";
  if(origName === "Bow (up)") return "LinkBowUp";
  if(origName === "Bow (down)") return "LinkBowDown";
  if(origName === "Boomerang") return "LinkBoomerangRight";
  if(origName === "Boomerang (up)") return "LinkBoomerangUp";
  if(origName === "Boomerang (down)") return "LinkBoomerangDown";
  if(origName === "Hookshot") return "LinkHookshotRight";
  if(origName === "Hookshot (up)") return "LinkHookshotUp";
  if(origName === "Hookshot (down)") return "LinkHookshotDown";
  if(origName === "Powder") return "LinkPowderRight";
  if(origName === "Powder (up)") return "LinkPowderUp";
  if(origName === "Powder (down)") return "LinkPowderDown";
  if(origName === "Rod") return "LinkRodRight";
  if(origName === "Rod (up)") return "LinkRodUp";
  if(origName === "Rod (down)") return "LinkRodDown";
  if(origName === "Bombos") return "LinkBombos";
  if(origName === "Ether") return "LinkEther";
  if(origName === "Quake") return "LinkQuake";
  if(origName === "Hammer") return "LinkHammerRight";
  if(origName === "Hammer (up)") return "LinkHammerUp";
  if(origName === "Hammer (down)") return "LinkHammerDown";
  if(origName === "Shovel") return "LinkShovel";
  if(origName === "Bug net") return "LinkBugNet";
  if(origName === "Read book") return "LinkReadBook";
  if(origName === "Prayer") return "LinkPrayer";
  if(origName === "Cane") return "LinkCaneRight";
  if(origName === "Cane (up)") return "LinkCaneUp";
  if(origName === "Cane (down)") return "LinkCaneDown";
  if(origName === "Bunny stand") return "LinkBunnyIdleRight";
  if(origName === "Bunny stand (up)") return "LinkBunnyIdleUp";
  if(origName === "Bunny stand (down)") return "LinkBunnyIdleDown";
  if(origName === "Bunny walk") return "LinkBunnyIdleMoveRight";
  if(origName === "Bunny walk (up)") return "LinkBunnyIdleMoveUp";
  if(origName === "Bunny walk (down)") return "LinkBunnyIdleMoveDown";
  if(origName === "Walk upstairs (1F)") return "LinkWalkUpstairs1F";
  if(origName === "Walk upstairs (2F)") return "LinkWalkUpstairs2F";
  if(origName === "Walk downstairs (2F)") return "LinkWalkDownstairs2F";
  if(origName === "Walk downstairs (1F)") return "LinkWalkDownstairs1F";
  if(origName === "Poke") return "LinkPokeRight";
  if(origName === "Poke (up)") return "LinkPokeUp";
  if(origName === "Poke (down)") return "LinkPokeDown";
  if(origName === "Tall grass") return "LinkTallGrassRight";
  if(origName === "Tall grass (up)") return "LinkTallGrassUp";
  if(origName === "Tall grass (down)") return "LinkTallGrassDown";
  if(origName === "Map dungeon") return "LinkMapDungeon";
  if(origName === "Map world") return "LinkMapWorld";
  if(origName === "Sleep") return "LinkSleep";
  if(origName === "Awake") return "LinkAwake";
  throw "Sprite Name " + origName + " not mapped"
}

function saveSprites(sprites: Sprite[]) {
  let jsonStr = "[";
  for(let sprite of sprites) {
    jsonStr += Helpers.serializeES6(sprite);
    jsonStr += ",";
  }
  if(jsonStr[jsonStr.length - 1] === ",") jsonStr = jsonStr.slice(0, -1);
  jsonStr += "]";
  //console.log(jsonStr);
  $.post("save-sprites", { "data": JSON.parse(jsonStr) }).then(response => {
    console.log("Successfully saved sprites");
  }, error => {
    console.log("Failed to save sprites");
  });
}

//@ts-ignore
window.createSprites = function() {
  $.getJSON("AnimationData.json", function(json) {
    let sprites: Sprite[] = [];
    Object.keys(json).forEach(function(key, index) {
      let animType = json[key];
      let spriteName = spriteMapping(animType.name);
      let sprite = new Sprite(spriteName, undefined);
      sprite.spritesheetPath = "assets/spritesheets/link2.png";
      sprite.alignment = "topleft";
      sprite.wrapMode = "loop";
      for(let j = 0; j < animType.steps.length; j++) {
        let step = animType.steps[j];
        let duration = step.length / 60;
        let framez: Frame[] = [];
        if(step.length === 10000) duration = 1;
        for(let i = 0; i < step.sprites.length; i++) {
          let spriteStep = step.sprites[i];

          let size = getSizeFromStr(spriteStep.size);
          if(!size) continue;

          let row = getRowFromStr(spriteStep.row);
          if(row === -1) continue;
          let col = spriteStep.col;

          let so = getSizeOffsetFromStr(spriteStep.size);

          let offset = new Point(spriteStep.pos[0], spriteStep.pos[1]);
          let rect = Rect.CreateWH((col * 16) + so.x, (row * 16) + so.y, size.x, size.y);

          let frame = new Frame(rect, duration, offset);
          if(spriteStep.trans === "Y_FLIP") frame.xDir = -1;
          if(spriteStep.trans === "X_FLIP") frame.yDir = -1;
          if(spriteStep.trans === "XY_FLIP") { frame.xDir = -1; frame.yDir = -1; }
          frame.tags = spriteStep.row.toLowerCase();

          framez.push(frame);
        }
        //Lowest, and size is full
        let largestY = -100000;
        let largestI = 0;
        for(let i = 0; i < framez.length; i++) {
          if(framez[i].offset.y > largestY && framez[i].rect.area >= 256 && framez[i].rect.y1 <= 27*16) {
            largestY = framez[i].offset.y;
            largestI = i;
          }
        }
        /*
        //Closest to origin
        let closestDist = 100000;
        let largestI = 0;
        for(let i = 0; i < framez.length; i++) {
          if(framez[i].offset.magnitude < closestDist) {
            closestDist = framez[i].offset.magnitude;
            largestI = i;
          }
        }
        */
        let mainFrame = framez[largestI];
        for(let i = 0; i < framez.length; i++) {
          if(i !== largestI) {
            framez[i].parentFrameIndex = largestI;
            framez[i].offset.inc(mainFrame.offset.times(-1));
            mainFrame.childFrames.push(framez[i]);
          }
        }
        sprite.frames.push(mainFrame);

      }
      sprites.push(sprite);
    });
    //console.log(sprites);
    //@ts-ignore
    window.lttpSprites = sprites;
    //@ts-ignore
    initSpriteEditor();
    //saveSprites(sprites);
  });
}
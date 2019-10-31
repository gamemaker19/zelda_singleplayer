import { Rect } from "./Rect";
import { Hitbox } from "./Hitbox";
import { Spritesheet } from "./Spritesheet";
import * as _ from "lodash";
import * as Helpers from "../helpers";
import { GridCoords } from "./GridCoords";

export enum HitboxMode {
  None = 0,
  Tile = 1,
  BoundingRect = 2,
  DiagTopLeft = 3,
  DiagTopRight = 4,
  DiagBotLeft = 5,
  DiagBotRight = 6,
  Pixels = 7,
  Custom = 8,
  BoxTop = 9,
  BoxBot = 10,
  BoxLeft = 11,
  BoxRight = 12,
  BoxTopLeft = 13,
  BoxTopRight = 14,
  BoxBotLeft = 15,
  BoxBotRight = 16,
  SmallDiagTopLeft = 17,
  SmallDiagTopRight = 18,
  SmallDiagBotLeft = 19,
  SmallDiagBotRight = 20,
  LargeDiagTopLeft = 21,
  LargeDiagTopRight = 22,
  LargeDiagBotLeft = 23,
  LargeDiagBotRight = 24
}

export enum ZIndex {
  Background3 = -3,
  Background2 = -2,
  Background1 = -1,
  Default = 0,
  Foreground1 = 1,
  Foreground2 = 2,
  Foreground3 = 3,
  Foreground1TopHalf = 4,
  Foreground1Unmasked = 5
}

export class TileData {
  name: string = "";
  tileset: Spritesheet = undefined;
  tilesetPath: string = "";
  gridCoords: GridCoords = undefined;
  hitbox: Hitbox = undefined;
  hitboxMode: HitboxMode = 0;
  customHitboxPoints: string = "";
  tag: string = "";
  zIndex: ZIndex = 0;
  spriteName: string = "";
  constructor(tileset: Spritesheet, gridCoords: GridCoords, name: string) {
    this.tileset = tileset;
    this.gridCoords = gridCoords;
    this.name = name;
  }
  getNonSerializedKeys() {
    return ["tileset"];
  }
  getId() {
    return Helpers.baseName(this.tileset.path) + "_" + String(this.gridCoords.i) + "_" + String(this.gridCoords.j);
  }
  onSerialize() {
    this.tilesetPath = this.tileset.path;
    if (this.hitboxMode === HitboxMode.SmallDiagTopLeft) { this.customHitboxPoints = "130"; this.hitboxMode = HitboxMode.Custom; }
    if (this.hitboxMode === HitboxMode.SmallDiagTopRight) { this.customHitboxPoints = "125"; this.hitboxMode = HitboxMode.Custom; }
    if (this.hitboxMode === HitboxMode.SmallDiagBotLeft) { this.customHitboxPoints = "367"; this.hitboxMode = HitboxMode.Custom; }
    if (this.hitboxMode === HitboxMode.SmallDiagBotRight) { this.customHitboxPoints = "578"; this.hitboxMode = HitboxMode.Custom; }
    if (this.hitboxMode === HitboxMode.LargeDiagTopLeft) { this.customHitboxPoints = "25760"; this.hitboxMode = HitboxMode.Custom; }
    if (this.hitboxMode === HitboxMode.LargeDiagTopRight) { this.customHitboxPoints = "28730"; this.hitboxMode = HitboxMode.Custom; }
    if (this.hitboxMode === HitboxMode.LargeDiagBotLeft) { this.customHitboxPoints = "15860"; this.hitboxMode = HitboxMode.Custom; }
    if (this.hitboxMode === HitboxMode.LargeDiagBotRight) { this.customHitboxPoints = "12863"; this.hitboxMode = HitboxMode.Custom; }
  }
  onDeserialize() {
    if(this.tag.startsWith(",")) this.tag = this.tag.substring(1);
    /*
    if(this.customHitboxPoints === "013") { this.hitboxMode = HitboxMode.SmallDiagTopLeft; this.customHitboxPoints = ""; }
    if(this.customHitboxPoints === "125") { this.hitboxMode = HitboxMode.SmallDiagTopRight; this.customHitboxPoints = ""; }
    if(this.customHitboxPoints === "367") { this.hitboxMode = HitboxMode.SmallDiagBotLeft; this.customHitboxPoints = ""; }
    if(this.customHitboxPoints === "578") { this.hitboxMode = HitboxMode.SmallDiagBotRight; this.customHitboxPoints = ""; }
    if(this.customHitboxPoints === "02576") { this.hitboxMode = HitboxMode.LargeDiagTopLeft; this.customHitboxPoints = ""; }
    if(this.customHitboxPoints === "02873") { this.hitboxMode = HitboxMode.LargeDiagTopRight; this.customHitboxPoints = ""; }
    if(this.customHitboxPoints === "01586") { this.hitboxMode = HitboxMode.LargeDiagBotLeft; this.customHitboxPoints = ""; }
    if(this.customHitboxPoints === "12863") { this.hitboxMode = HitboxMode.LargeDiagBotRight; this.customHitboxPoints = ""; }
    */
  }
  setTileset(tilesets: Spritesheet[]) {
    this.tileset = _.find(tilesets, (tileset: Spritesheet) => { return tileset.path === this.tilesetPath; });
  }
  setTag(tagToSet: string) {
    let tags = this.tag.split(",");
    _.remove(tags, function(tag) { return !tag; });
    for(let tag of tags) {
      if(tag === tagToSet) return;
    }
    this.tag = tagToSet;
  }
  hasTag(tagToCheck: string) {
    let tags = this.tag.split(",");
    for(let tag of tags) {
      if(tag === tagToCheck) return true;
    }
    return false;
  }
}
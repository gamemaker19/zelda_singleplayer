import { Obj } from "./models/Obj";
import { Point } from "./models/point";

let objects: Obj[] = [
  new Obj("NPCTest", "NPCIdleDown", false, undefined),
  new Obj("Octorok", "OctorokWalkDown", false, undefined),
  new Obj("RockBigGray", "RockBigGray", true, new Point(0, 0)),
  new Obj("RockPile", "RockPile", true, new Point(0, 0)),
  new Obj("WaterRock", "TileWaterRock", true, new Point(0, 0)),
  new Obj("Entrance", "EditorPOI", true, new Point(0, 0)),
  new Obj("DoorSanctuary", "DoorSanctuary", true, new Point(0, 0)),
  new Obj("DoorCastle", "DoorCastle", true, new Point(0, 0)),
  new Obj("Door", "Door", true, new Point(0, 0)),
  new Obj("Weathercock", "Weathercock", true, new Point(0, 0)),
  new Obj("DesertPalaceStone", "DesertPalaceStone", true, new Point(0, 0)),
  new Obj("Whirlpool", "TileWhirlpool", true, new Point(0, 0)),
  new Obj("TorchLit", "TorchLit", true, new Point(0, 0)),
  new Obj("TorchUnlit", "TorchUnlit", true, new Point(0, 0)),
  new Obj("TorchBig", "TorchBig", true, new Point(0, 0)),
  new Obj("ChestSmall", "ChestSmall", true, new Point(0, 0)),
  new Obj("ChestBig", "ChestBig", true, new Point(0, 0)),
  new Obj("Pot", "Pot", true, new Point(0, 0)),
  new Obj("MasterSwordWoods", "MasterSwordWoods", true, new Point(0, 0)),
  new Obj("BigFairy", "BigFairy", true, new Point(0, 0)),
  new Obj("Fairy", "Fairy", true, new Point(0, 0)),
  new Obj("Generic", "EditorPOI", true, new Point(0, 0)),
];

function getObjectList() {
  return objects;
}
export { getObjectList };
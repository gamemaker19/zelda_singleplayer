
import { Collider } from './models/Collider';
import { Frame } from './models/Frame';
import { GridCoords } from './models/GridCoords';
import { GridRect } from './models/GridRect';
import { Hitbox } from './models/Hitbox';
import { Level } from './models/Level';
import { Line } from './models/Line';
import { Obj } from './models/Obj';
import { POI } from './models/POI';
import { Point } from './models/Point';
import { Rect } from './models/Rect';
import { Shape } from './models/Shape';
import { ShapeInstance } from './models/ShapeInstance';
import { Sprite } from './models/Sprite';
import { SpriteInstance } from './models/SpriteInstance';
import { Spritesheet } from './models/Spritesheet';
import { TileAnimation } from './models/TileAnimation';
import { TileClump } from './models/TileClump';
import { TileData } from './models/TileData';
import { TileInstance } from './models/TileInstance';

export function createClassFromName(className: string)
{
//@ts-ignore
if (className === 'Collider') return new Collider();
//@ts-ignore
if (className === 'Frame') return new Frame();
//@ts-ignore
if (className === 'GridCoords') return new GridCoords();
//@ts-ignore
if (className === 'GridRect') return new GridRect();
//@ts-ignore
if (className === 'Hitbox') return new Hitbox();
//@ts-ignore
if (className === 'Level') return new Level();
//@ts-ignore
if (className === 'Line') return new Line();
//@ts-ignore
if (className === 'Obj') return new Obj();
//@ts-ignore
if (className === 'POI') return new POI();
//@ts-ignore
if (className === 'Point') return new Point();
//@ts-ignore
if (className === 'Rect') return new Rect();
//@ts-ignore
if (className === 'Shape') return new Shape();
//@ts-ignore
if (className === 'ShapeInstance') return new ShapeInstance();
//@ts-ignore
if (className === 'Sprite') return new Sprite();
//@ts-ignore
if (className === 'SpriteInstance') return new SpriteInstance();
//@ts-ignore
if (className === 'Spritesheet') return new Spritesheet();
//@ts-ignore
if (className === 'TileAnimation') return new TileAnimation();
//@ts-ignore
if (className === 'TileClump') return new TileClump();
//@ts-ignore
if (className === 'TileData') return new TileData();
//@ts-ignore
if (className === 'TileInstance') return new TileInstance();

if (className === 'Object') return new Object();
}

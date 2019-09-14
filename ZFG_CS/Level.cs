using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Priority_Queue;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZFG_CS
{
    public class Level
    {
        public string name = "";
        public int gridWidth = 0;
        public int gridHeight = 0;
        public float shakeX = 0;
        public float shakeY = 0;
        public float shakeTime = 0;
        public bool shakeDampen = false;
        public bool isIndoor = false;
        public List<List<TileSlot>> tileSlots;
        //public List<List<Cluster>> clusters;
        //public Dictionary<ALLEGRO_SAMPLE_INSTANCE*, Sound*> sounds;
        public HashSet<Actor> actors = new HashSet<Actor>();
        public HashSet<Actor> removedActors = new HashSet<Actor>();
        public HashSet<Actor> deletedActors = new HashSet<Actor>();
        public Dictionary<string, Entrance> entrances = new Dictionary<string, Entrance>();
        public string musicKey = "";
        public List<Line> scrollLines = new List<Line>();
        public int pixelWidth() { return gridWidth * 8; }
        public int pixelHeight() { return gridHeight * 8; }
        List<GridCoords> clusterNeighborSlots = new List<GridCoords>();
        //List<ClusterExitNode*> clusterExitNodes;
        List<GridCoords> noLandSlots = new List<GridCoords>();

        public void init(string levelPath)
        {
            string levelJsonStr = File.ReadAllText(levelPath);
            dynamic levelJson = JsonConvert.DeserializeObject(levelJsonStr);

            gridWidth = Convert.ToInt32(levelJson["width"]);
            gridHeight = Convert.ToInt32(levelJson["height"]);
            name = levelJson["name"];

            tileSlots = new List<List<TileSlot>>(new List<TileSlot>[gridHeight]);
            for (int i = 0; i < gridHeight; i++)
            {
                tileSlots[i] = new List<TileSlot>(new TileSlot[gridWidth]);
            }

            for (int i = 0; i < gridHeight; i++)
            {
                for (int j = 0; j < gridWidth; j++)
                {
                    tileSlots[i][j] = new TileSlot();
                }
            }

            JArray tileJsons = levelJson["tileInstances"];
            for (int i = 0; i < tileJsons.Count; i++)
            {
                JArray row = (JArray)tileJsons[i];
                for (int j = 0; j < row.Count; j++)
                {
                    string tileDataId = Convert.ToString(row[j]);
                    TileData tileData = Global.tileDatas[tileDataId];
                    tileSlots[i][j].gridCoords = new GridCoords(i, j);
                    tileSlots[i][j].tileInstances.Add(tileData);
                }
            }

            List<Door> doors = new List<Door>();
            for (int i = 0; i < tileSlots.Count; i++)
            {
                for (int j = 0; j < tileSlots[i].Count; j++)
                {
                    if (tileSlots[i][j].tileInstances[0] == null)
                    {
                        tileSlots[i][j].tileInstances.Clear();
                        continue;
                    }
                    string tileDataId = tileSlots[i][j].tileInstances[0].getKey();
                    if (tileDataId == "")
                    {
                        tileSlots[i][j].tileInstances.Clear();
                        continue;
                    }

                    TileData tileData = Global.tileDatas[tileDataId];

                    //TILE TRANSLATIONS

                    //Doors
                    if (tileDataId == "zeldamainmap_30_171")
                    {
                        tileSlots[i][j].tileInstances[0] = Global.tileDatas["zeldamainmap_15_169"];
                        Door door = new Door(this, new Point(j * 8 + 8, i * 8 + 8), "Door");
                        doors.Add(door);
                    }
                    else if (tileDataId == "zeldamainmap_30_172")
                    {
                        tileSlots[i][j].tileInstances[0] = Global.tileDatas["zeldamainmap_15_169"];
                    }
                    else if (tileDataId == "zeldamainmap_31_171")
                    {
                        tileSlots[i][j].tileInstances[0] = Global.tileDatas["zeldamainmap_165_95"];
                    }
                    else if (tileDataId == "zeldamainmap_31_172")
                    {
                        tileSlots[i][j].tileInstances[0] = Global.tileDatas["zeldamainmap_165_95"];
                    }

                    bool isObj = false;
                    bool isBig = false;
                    Point objPos = new Point(j * 8 + 8, i * 8 + 8);
                    
                    if (tileData.hasTag("rocksmallgray"))
                    {
                        isObj = true;
                        Actor obj = WorldObjectFactories.createThrowable(this, objPos, "RockSmallGray", "RockSmallBase", "PotBreak", true, false, true, 1, "rock break");
                        obj.throwable.itemRequired = Item.powerGlove;
                    }
                    else if (tileData.hasTag("rockbiggray"))
                    {
                        isObj = true;
                        isBig = true;
                        Actor obj = WorldObjectFactories.createThrowable(this, objPos, "RockBigGray", "RockBigBase", "PotBreak", true, true, true, 2, "rock break");
                        obj.throwable.itemRequired = Item.powerGlove;
                    }
                    else if (tileData.hasTag("rocksmallblack"))
                    {
                        isObj = true;
                        Actor obj = WorldObjectFactories.createThrowable(this, objPos, "RockSmallBlack", "RockSmallBase", "PotBreak", true, false, true, 2, "rock break");
                        obj.throwable.itemRequired = Item.titansMitt;
                    }
                    else if (tileData.hasTag("rockbigblack"))
                    {
                        isObj = true;
                        isBig = true;
                        Actor obj = WorldObjectFactories.createThrowable(this, objPos, "RockBigBlack", "RockBigBase", "PotBreak", true, true, true, 4, "rock break");
                        obj.throwable.itemRequired = Item.titansMitt;
                    }
                    else if (tileData.hasTag("bush") || tileData.hasTag("yellowbush"))
                    {
                        isObj = true;
                        Actor obj = WorldObjectFactories.createThrowable(this, objPos, "Bush", "BushBase", "BushBreak", true, false, true, 0.5f, "grass destroyed");
                        obj.name = "Bush";
                        obj.throwable.breakOnSword = true;
                        if (tileData.hasTag("yellowbush"))
                        {
                            obj.shader = Global.shaders["replaceColorBush"];
                        }
                    }
                    else if (tileData.hasTag("stake"))
                    {
                        isObj = true;
                        Actor obj = new Stake(this, objPos);
                    }
                    else if (tileData.hasTag("sign"))
                    {
                        isObj = true;
                        Sign sign = new Sign(this, objPos);
                        //WorldObjectFactories.createThrowable("RockSmallGray", "RockBase", "RockBreak");
                    }
                    else if (tileData.hasTag("rockpile"))
                    {
                        isObj = true;
                        isBig = true;
                        RockPile rockPile = new RockPile(this, objPos.addxy(8, 8), "RockPile", true);
                    }

                    TileData backgroundTile = Global.tileDatas["zeldamainmap_8_88"];
                    if (tileData.hasTag("yellowbush"))
                    {
                        backgroundTile = Global.tileDatas["zeldamainmap_88_484"];
                    }
                    if (tileData.zIndex == 1)
                    {
                        tileSlots[i][j].tileInstances.Insert(0, backgroundTile);
                    }
                    if (isObj)
                    {
                        tileSlots[i][j].tileInstances.Clear();
                        tileSlots[i + 1][j].tileInstances.Clear();
                        tileSlots[i][j + 1].tileInstances.Clear();
                        tileSlots[i + 1][j + 1].tileInstances.Clear();
                        tileSlots[i][j].tileInstances.Add(backgroundTile);
                        tileSlots[i + 1][j].tileInstances.Add(backgroundTile);
                        tileSlots[i][j + 1].tileInstances.Add(backgroundTile);
                        tileSlots[i + 1][j + 1].tileInstances.Add(backgroundTile);
                    }
                    if (isBig)
                    {
                        for (int i2 = i; i2 < i + 4; i2++)
                        {
                            for (int j2 = j; j2 < j + 4; j2++)
                            {
                                tileSlots[i2][j2].tileInstances.Clear();
                                tileSlots[i2][j2].tileInstances.Add(backgroundTile);
                            }
                        }
                    }
                }

            }

            if (levelJson["instances"] != null)
            {
                //INSTANCE SETUP
                JArray instances = levelJson["instances"];
                for (int j = 0; j < instances.Count; j++)
                {
                    dynamic instance = (dynamic)instances[j];
                    int x = Convert.ToInt32(instance["pos"]["x"]);
                    int y = Convert.ToInt32(instance["pos"]["y"]);
                    string objName = instance["obj"]["name"];
                    string name = instance["name"];

                    bool hasProperties = false;

                    bool reveal = false;
                    bool outOfReach = false;

                    Item itemRequired = null;
                    dynamic propertiesJson = null;
                    if (instance["properties"] != null)
                    {
                        string properties = instance["properties"];
                        properties = properties.Replace('\'', '"');
                        if (properties != "")
                        {
                            hasProperties = true;
                            propertiesJson = JsonConvert.DeserializeObject(properties);
                            if (propertiesJson["reveal"] != null)
                            {
                                reveal = true;
                            }
                            if (propertiesJson["outOfReach"] != null)
                            {
                                outOfReach = true;
                            }
                            if (propertiesJson["itemRequired"] != null)
                            {
                                string itemRequiredStr = propertiesJson["itemRequired"];
                                if (itemRequiredStr == "hammer") itemRequired = Item.hammer;
                                else if (itemRequiredStr == "powerGlove") itemRequired = Item.powerGlove;
                                else if (itemRequiredStr == "titansMitt") itemRequired = Item.titansMitt;
                                else outOfReach = true;
                            }
                        }
                    }

                    if (objName == "WaterRock")
                    {
                        Actor actor = new Actor(this, new Point(x, y), "TileWaterRock");
                    }
                    else if (objName == "RockBigGray")
                    {
                        Actor obj = WorldObjectFactories.createThrowable(this, new Point(x, y), "RockBigGray", "", "PotBreak", true, false, true, 2, "rock break");
                        obj.throwable.itemRequired = Item.powerGlove;
                        obj.throwable.revealSound = reveal;
                    }
                    else if (objName == "DoorSanctuary")
                    {
                        Door door = new Door(this, new Point(x, y), "DoorSanctuary");
                    }
                    else if (objName == "DoorCastle")
                    {
                        Door door = new Door(this, new Point(x, y + 4), "DoorCastle");
                        doors.Add(door);
                    }
                    else if (objName == "Door")
                    {
                        Door door = new Door(this, new Point(x, y), "Door");
                    }
                    else if (objName == "MasterSwordWoods")
                    {
                        if(Options.main.enableMasterSword)
                        {
                            Actor masterSwordWoods = new Actor(this, new Point(x, y), "MasterSwordWoods");
                            masterSwordWoods.name = "MasterSwordWoods";
                            masterSwordWoods.zIndex = (int)ZIndex.Link + 1;
                            Global.game.unpulledMasterSword = masterSwordWoods;
                        }
                    }
                    else if (objName == "RockPile")
                    {
                        RockPile rockPile = new RockPile(this, new Point(x, y), "RockPile", false);
                        rockPile.shader = Global.shaders["replaceAlphaGrass"];
                        rockPile.revealSound = reveal;
                    }
                    else if (objName == "Entrance" && hasProperties)
                    {
                        string entranceClump = propertiesJson["entrance"];
                        var pieces = entranceClump.Split(',');
                        string entranceId = pieces[0];
                        string exitId = pieces[1];
                        string exitLevel = pieces[2];
                        string dirStr = pieces[3];
                        Direction dir = Direction.Down;
                        if (dirStr == "up") dir = Direction.Up;
                        if (dirStr == "down") dir = Direction.Down;
                        if (dirStr == "left") dir = Direction.Left;
                        if (dirStr == "right") dir = Direction.Right;
                        float yOffset = 0;
                        if (name == "lttp_overworld") yOffset = 2;

                        Entrance entrance = new Entrance(this, new Point(x, y + yOffset), entranceId, exitLevel, exitId, dir, dirStr == "fall", dirStr == "oneway");
                        entrances[entranceId] = entrance;
                        if (pieces.Length > 4 && pieces[4] == "1")
                        {
                            entrance.sprite.hitboxes[0].halveHeight();
                        }
                        if (pieces.Length > 5)
                        {
                            if (pieces[5] == "0")
                            {
                                entrance.noMusicChange = true;
                            }
                            else
                            {
                                entrance.overrideMusicKey = pieces[5];
                            }
                        }
                    }
                    else if (objName == "Pot")
                    {
                        Actor actor = WorldObjectFactories.createThrowable(this, new Point(x, y), "Pot", "PotBase", "PotBreak", true, false, true, 1, "rock break");
                    }
                    else if (objName == "TorchLit")
                    {
                        new Actor(this, new Point(x, y), "TorchLit");
                    }
                    else if (objName == "TorchUnlit")
                    {
                        new Actor(this, new Point(x, y), "TorchLit");
                    }
                    else if (objName == "TorchBig")
                    {
                        new Actor(this, new Point(x, y), "TorchBig");
                    }
                    else if (objName == "ChestSmall")
                    {
                        Chest chest = new Chest(this, new Point(x, y), false);
                        chest.outOfReach = outOfReach;
                        chest.itemRequired = itemRequired;
                        Global.game.chests.Add(chest);
                    }
                    else if (objName == "ChestBig")
                    {
                        Chest chest = new Chest(this, new Point(x, y), true);
                        chest.outOfReach = outOfReach;
                        chest.itemRequired = itemRequired;
                        Global.game.chests.Add(chest);
                    }
                    else if (objName == "Fairy")
                    {
                        new Fairy(this, new Point(x, y));
                    }
                    else if (objName == "BigFairy")
                    {
                        new BigFairy(this, new Point(x, y));
                    }
                    else if (name == "PotionShopRed")
                    {
                        new SaleItem(this, new Point(x, y), Item.redPotion, 120);
                    }
                    else if (name == "PotionShopGreen")
                    {
                        new SaleItem(this, new Point(x, y), Item.greenPotion, 60);
                    }
                    else if (name == "PotionShopBlue")
                    {
                        new SaleItem(this, new Point(x, y), Item.bluePotion, 160);
                    }
                    else if (name == "FightersSword")
                    {
                        new FieldItem(this, new Point(x + 8, y + 2), new InventoryItem(Item.sword1), false);
                    }
                }
            }

            //OVERRIDES
            if (levelJson["coordProperties"] != null)
            {
                JArray propertiesJson = levelJson["coordProperties"];
                for (int n = 0; n < propertiesJson.Count; n++)
                {
                    dynamic propertyJson = (dynamic)propertiesJson[n];
                    int pI = Convert.ToInt32(propertyJson["i"]);
                    int pJ = Convert.ToInt32(propertyJson["j"]);
                    dynamic properties = propertyJson["properties"];
                    if (properties["cover"] != null)
                    {
                        tileSlots[pI][pJ].noCollision = true;
                        tileSlots[pI][pJ].zIndex = 1;
                    }
                    if (properties["zIndex"] != null)
                    {
                        int zIndex = (properties["zIndex"]);
                        tileSlots[pI][pJ].zIndex = zIndex;
                    }
                    if (properties["noCol"] != null)
                    {
                        tileSlots[pI][pJ].noCollision = true;
                    }
                    if (properties["noLand"] != null)
                    {
                        tileSlots[pI][pJ].noLand = true;
                    }
                    if (properties["neighbors"] != null)
                    {
                        /*
                        tileSlots[i][j].exitNode = new ClusterExitNode(GridCoords(i, j));
                        clusterExitNodes.Add(tileSlots[i][j].exitNode);
                        for (int m = 0; m < properties["neighbors"].Count; m++)
                        {
                            string coords = properties["neighbors"][m];
                            int coordsI = (Helpers.split(coords, ',')[0]);
                            int coordsJ = (Helpers.split(coords, ',')[1]);
                            tileSlots[i][j].nodeNeighbors.Add(&tileSlots[coordsI][coordsJ]);
                        }
                        */
                    }
                }
            }

            /*
            for (int i = 0; i < tileSlots.Count; i++)
            {
                for (int j = 0; j < tileSlots[i].Count; j++)
                {
                    if (!tileSlots[i][j].exitNode) continue;
                    for (var nodeNeighbor : tileSlots[i][j].nodeNeighbors)
                    {
                        tileSlots[i][j].exitNode.neighbors.Add(nodeNeighbor.exitNode);
                    }
                }
            }
            */

            for (int i2 = 0; i2 < tileSlots.Count; i2++)
            {
                for (int j2 = 0; j2 < tileSlots[i2].Count; j2++)
                {
                    bool canLand = true;
                    foreach (var tileInstance in tileSlots[i2][j2].tileInstances)
                    {
                        if (tileInstance.hasTag("water"))
                        {
                            canLand = false;
                        }
                    }
                    if (tileSlots[i2][j2].noLand)
                    {
                        canLand = false;
                    }
                    if (!tileSlots[i2][j2].noCollision && tileSlots[i2][j2].hasCollisionTile())
                    {
                        canLand = false;
                    }
                    if (name == "lttp_overworld" && i2 < 128 && j2 > 191 && j2 < 448)
                    {
                        //Death mountain
                        canLand = false;
                    }
                    if (name == "lttp_overworld" && i2 < 128 && j2 < 128)
                    {
                        //Lost woods
                        canLand = false;
                    }
                    if (!tileSlots[i2][j2].canLandMisc())
                    {
                        canLand = false;
                    }
                    tileSlots[i2][j2].canLand = canLand;
                }
            }

            //In order for a tile spot to be landable, it must form a 16x16 box of landables 
            for (int i2 = 0; i2 < tileSlots.Count; i2++)
            {
                for (int j2 = 0; j2 < tileSlots[i2].Count; j2++)
                {
                    TileSlot topLeft = getTileSlotIfExists(i2 - 1, j2 - 1);
                    bool topLeftCanLand = (topLeft != null && topLeft.canLand);

                    TileSlot top = getTileSlotIfExists(i2 - 1, j2);
                    bool topCanLand = (top != null && top.canLand);

                    TileSlot topRight = getTileSlotIfExists(i2 - 1, j2 + 1);
                    bool topRightCanLand = (topRight != null && topRight.canLand);

                    TileSlot left = getTileSlotIfExists(i2, j2 - 1);
                    bool leftCanLand = (left != null && left.canLand);

                    TileSlot right = getTileSlotIfExists(i2, j2 + 1);
                    bool rightCanLand = (right != null && right.canLand);

                    TileSlot botLeft = getTileSlotIfExists(i2 + 1, j2 - 1);
                    bool botLeftCanLand = (botLeft != null && botLeft.canLand);

                    TileSlot bot = getTileSlotIfExists(i2 + 1, j2);
                    bool botCanLand = (bot != null && bot.canLand);

                    TileSlot botRight = getTileSlotIfExists(i2 + 1, j2 + 1);
                    bool botRightCanLand = (botRight != null && botRight.canLand);

                    bool foundCanLand = false;
                    if (topLeftCanLand && topCanLand && leftCanLand) foundCanLand = true;
                    if (topCanLand && topRightCanLand && rightCanLand) foundCanLand = true;
                    if (leftCanLand && botLeftCanLand && botCanLand) foundCanLand = true;
                    if (rightCanLand && botCanLand && botRightCanLand) foundCanLand = true;
                    //if (topLeftCanLand && topCanLand && topRightCanLand && leftCanLand && rightCanLand && botLeftCanLand && botCanLand && botRightCanLand) foundCanLand = true;
                    if (!foundCanLand) tileSlots[i2][j2].canLand = false;

                    bool foundNoProjCol = false;
                    if (tileSlots[i2][j2].tileInstances.Count == 1 && tileSlots[i2][j2].tileInstances[0].hitboxMode == HitboxMode.Tile)
                    {
                        for (int n = 1; n < 2; n++)
                        {
                            TileSlot below = getTileSlotIfExists(i2 + n, j2);
                            if (below == null) break;
                            if (!below.hasCollisionTile())
                            {
                                foundNoProjCol = true;
                                break;
                            }
                            if (!(below.tileInstances.Count == 1 && below.tileInstances[0].hitboxMode == HitboxMode.Tile)) break;
                        }
                    }
                    if(foundNoProjCol)
                    {
                        tileSlots[i2][j2].noProjCollide = true;
                    }

                }
            }

            if (levelJson["scrollLines"] != null)
            {
                dynamic scrollLinesJson = levelJson["scrollLines"];
                for (int i2 = 0; i2 < scrollLinesJson.Count; i2++)
                {
                    dynamic scrollLineJson = scrollLinesJson[i2];
                    int x1 = (scrollLineJson["point1"]["x"]);
                    int y1 = (scrollLineJson["point1"]["y"]);
                    int x2 = (scrollLineJson["point2"]["x"]);
                    int y2 = (scrollLineJson["point2"]["y"]);
                    Line line = new Line(new Point(x1, y1), new Point(x2, y2));
                    scrollLines.Add(line);
                }
            }

            foreach (var entrance in entrances)
            {
                foreach (Door door in doors)
                {
                    if (entrance.Value.pos.distTo(door.pos) < 24)
                    {
                        entrance.Value.door = door;
                    }
                }
            }

            if (name == "cave" || name == "house")
            {
                isIndoor = true;
            }
        }
        
        public Point getCamPos()
        {
            return Global.view.Center.toPoint();
        }

        public Point getTopLeftCamPos()
        {
            Point offset = new Point(0, 0);
            return Global.view.Center.toPoint() - new Point(Global.screenW / 2, Global.screenH / 2) + offset;
        }

        public Point getBotRightCamPos()
        {
            return Global.view.Center.toPoint() + new Point(Global.screenW / 2, Global.screenH / 2);
        }

        public void setCamPos(float x, float y)
        {
            Point offset = new Point(0, 0);
            if (shakeDampen)
            {
                offset.x = 3 * (float)Math.Sin(shakeX * 100);
                offset.y = 3 * (float)Math.Sin(shakeY * 100);
            }
            else if (shakeTime > 0)
            {
                offset.x = 2 * (float)Math.Sin(shakeX * 1000 * Global.frameCount);
                offset.y = 2 * (float)Math.Sin(shakeY * 1000 * Global.frameCount);
            }
            Global.view.Center = new Vector2f(x + offset.x, y + offset.y);
            Global.window.SetView(Global.view);
        }

        public int getLeftJ()
        {
            float left = (getTopLeftCamPos().x) / 8;
            return (int)Helpers.clampMin(left - 1, 0);
        }

        public int getRightJ()
        {
            float right = (getBotRightCamPos().x) / 8;
            return (int)right + 1;
        }

        public int getTopI()
        {
            float top = (getTopLeftCamPos().y) / 8;
            return (int)Helpers.clampMin(top - 1, 0);
        }

        public int getBotI()
        {
            float bot = (getBotRightCamPos().y) / 8;
            return (int)bot + 1;
        }

        public int actorCompare(Actor actor1, Actor actor2)
        {
            if (actor1.zIndex < actor2.zIndex)
            {
                return -1;
            }
            else if (actor1.zIndex > actor2.zIndex)
            {
                return 1;
            }
            else
            {
                return (actor1.autoIncId < actor2.autoIncId) ? -1 : 1;
            }
        }

        public List<Actor> getActorsInScreen()
        {
            List<Actor> actorsInScreen = new List<Actor>();
            int leftJ = getLeftJ() - 2;
            int rightJ = getRightJ() + 2;
            int topI = getTopI() - 2;
            int botI = getBotI() + 2;
            foreach (var actor in actors)
            {
                if (actor.pos.x < leftJ * 8 || actor.pos.x > rightJ * 8 || actor.pos.y < topI * 8 || actor.pos.y > botI * 8) continue;
                actorsInScreen.Add(actor);
            }
            actorsInScreen.Sort(actorCompare);
            return actorsInScreen;
        }

        public bool isActorInTileWithTag(Actor actor, string tags)
        {
            Collider collider = actor.getMainCollider(true);
            if (collider == null) return false;
            var actorShape = collider.getShape(actor);
            Rect actorRect = actorShape.toRect();
            float totalArea = 0;
            List<GridCoords> gridCoords = getGridCoords(actorShape);
            var tagsVec = tags.Split(',').ToList();

            //var tileRects = new List<string>();
            foreach (GridCoords gridCoord in gridCoords)
            {
                int i = gridCoord.i;
                int j = gridCoord.j;
                List<TileData> tileDatas = tileSlots[i][j].tileInstances;
                foreach (TileData tileData in tileDatas)
                {
                    if (tileData.hasAnyTag(tagsVec))
                    {
                        //if (actor.elevation > tileSlots[i][j].elevation) continue;
                        Rect tileRect = new Rect(j * 8, i * 8, (j + 1) * 8, (i + 1) * 8);
                        float area = Helpers.getIntersectArea(actorRect, tileRect);
                        //tileRects.Add(tileRect.toString());
                        totalArea += area;
                    }
                }
            }

            float areaPercent = totalArea / actorRect.area();
            if(areaPercent > 0.25 && areaPercent < 0.5)
            {
                var a = 0;
            }
            //Console.WriteLine(areaPercent);

            float waterPercent = 0.5f;
            float other = 0.5f;
            float percent = tagsVec.Contains("water") ? waterPercent : other;

            if (areaPercent > percent) return true;
            return false;
        }

        public void render()
        {
            int topI = getTopI();
            int botI = getBotI();
            int leftJ = getLeftJ();
            int rightJ = getRightJ();
            
            for (int i = topI; i < botI && i < tileSlots.Count; i++)
            {
                for (int j = leftJ; j < rightJ && j < tileSlots[i].Count; j++)
                {
                    for (int k = 0; k < tileSlots[i][j].tileInstances.Count; k++)
                    {
                        TileData tileData = tileSlots[i][j].tileInstances[k];
                        int zIndex = tileData.zIndex;
                        if (tileSlots[i][j].zIndex != 0) zIndex = tileSlots[i][j].zIndex;
                        if (tileData.spriteName == "")
                        {
                            if (zIndex < 1)
                            {
                                DrawWrappers.DrawTexture(tileData.bitmap, tileData.rect.x1, tileData.rect.y1, tileData.rect.w(), tileData.rect.h(), j * 8, i * 8, zIndex * (int)ZIndex.Foreground);
                            }
                            else if (zIndex == 1)
                            {
                                Shader s = Global.shaders["greenTransparent"];
                                DrawWrappers.DrawTexture(tileData.bitmap, tileData.rect.x1, tileData.rect.y1, tileData.rect.w(), tileData.rect.h(), j * 8, i * 8, zIndex * (int)ZIndex.Foreground, shader: s);
                            }
                        }
                        else
                        {
                            tileData.sprite.draw(tileData.spriteOffset.x + j * 8, tileData.spriteOffset.y + i * 8, 1, 1, 0, 1, tileData.shader, zIndex * (int)ZIndex.Foreground, true);
                        }
                        if (Global.showHitboxes)
                        {
                            showHitboxes(0, 0, i, j, tileData);
                            //if (tileSlots[i][j].noProjCollide) DrawWrappers.DrawRect(8 * j, 8 * i, 8 * (j + 1), 8 * (i + 1), true, new Color(255, 255, 0, 128), 1, ZIndex.HUD, true);
                            if (tileSlots[i][j].canLand) DrawWrappers.DrawRect(8 * j, 8 * i, 8 * (j + 1), 8 * (i + 1), true, new Color(255, 255, 0, 128), 1, ZIndex.HUD, true);
                        }
                    }
                }
            }
            foreach (Actor actor in getActorsInScreen())
            {
                actor.render();
            }

	        if (Global.showHitboxes)
	        {
                /*
		        for (var scrollLine : scrollLines)
		        {
			        wal_draw_rectangle(scrollLine.point1.x, scrollLine.point1.y, scrollLine.point2.x, scrollLine.point2.y, false, al_color_name("yellow"), 2, ZIndex.HUD, true);
		        }
		        for (var slot : clusterNeighborSlots)
		        {
			        wal_draw_rectangle(8 * slot.j, 8 * slot.i, 8 * (slot.j + 1), 8 * (slot.i + 1), true, al_color_name("blue"), 1, ZIndex.HUD, true);
		        }
		        for (var exitNode : clusterExitNodes)
		        {
			        for (var neighbor : exitNode.neighbors)
			        {
				        wal_draw_line(4 + exitNode.coords.j * 8, 4 + exitNode.coords.i * 8, 4 + neighbor.coords.j * 8, 4 + neighbor.coords.i * 8, al_color_name("yellow"), 2, ZIndex.HUD, true);
			        }
		        }
		        */
                foreach (var slot in Global.game.shortestPath)
		        {
			        DrawWrappers.DrawRect(8 * slot.gridCoords.j, 8 * slot.gridCoords.i, 8 * (slot.gridCoords.j + 1), 8 * (slot.gridCoords.i + 1), true, Color.Blue, 1, ZIndex.HUD, true);
		        }
            }

            drawStormCircle();

            List<float> keys = new List<float>();
	        foreach (var key in DrawWrappers.walDrawObjects)
	        {
		        keys.Add(key.Key);
	        }
	        float stormKey = ZIndex.HUD - 41523;
            keys.Add(stormKey);

            keys.Sort();

	        foreach (var key in keys)
	        {
		        if (key == stormKey)
		        {
                    Global.circleBuffer.Display();
                    Sprite sprite = new Sprite(Global.circleBuffer.Texture);
                    sprite.Position = new Vector2f(0, 0);
                    Global.window.SetView(Global.view);
                    Global.window.Draw(sprite);
                    //DrawWrappers.DrawTexture(Global.circleBuffer, 0, 0, Global.windowW, Global.windowH, 0, 0, Global.windowW, Global.windowH, 0);
                    continue;
		        }

		        var drawLayer = DrawWrappers.walDrawObjects[key];
                Global.window.Draw(drawLayer);
            }

	        DrawWrappers.walDrawObjects.Clear();
        }
        
        public void drawStormCircle()
        {
            Global.circleBuffer.Clear(Color.Transparent);
            if (name == "lttp_overworld")
            {
                RenderStates states = new RenderStates(Global.circleBuffer.Texture);
                states.BlendMode = new BlendMode(BlendMode.Factor.One, BlendMode.Factor.One, BlendMode.Equation.Subtract);
                
                byte a = (int)(0.33f * 255);
                Color stormColor = new Color(252, 186, 3, a);

                RectangleShape rect = new RectangleShape(new Vector2f(pixelWidth(), pixelHeight()));
                rect.Position = new Vector2f(0, 0);
                rect.FillColor = stormColor;
                Global.circleBuffer.Draw(rect, states);

                CircleShape circle1 = new CircleShape(Global.game.currentStormRadius);
                circle1.FillColor = stormColor;
                circle1.Position = new Vector2f(
                    (Global.game.currentStormCenter.x) - circle1.Radius,
                    (Global.game.currentStormCenter.y) - circle1.Radius);
                circle1.SetPointCount(1000);

                Global.circleBuffer.Draw(circle1, states);

                float x = Mathf.Floor(getTopLeftCamPos().x);
                float y = Mathf.Floor(getTopLeftCamPos().y);

                Point randScreenPos = new Point(Helpers.randomRange((int)x, (int)x + (int)Global.screenW), Helpers.randomRange((int)y, (int)y + (int)Global.screenH));
                if (randScreenPos.distTo(Global.game.currentStormCenter) > Global.game.currentStormRadius)
                {
                    new ParticleTwilight(this, randScreenPos);
                }
            }
            else if (Global.game.camCharacter != null && Global.game.camCharacter.inStorm())
            {
                RenderStates states = new RenderStates(Global.circleBuffer.Texture);
                states.BlendMode = new BlendMode(BlendMode.Factor.One, BlendMode.Factor.One, BlendMode.Equation.Subtract);

                byte a = (int)(0.33f * 255);
                Color stormColor = new Color(252, 186, 3, a);

                RectangleShape rect = new RectangleShape(new Vector2f(pixelWidth(), pixelHeight()));
                rect.Position = new Vector2f(0, 0);
                rect.FillColor = stormColor;
                Global.circleBuffer.Draw(rect, states);

                float x = Mathf.Floor(getTopLeftCamPos().x);
                float y = Mathf.Floor(getTopLeftCamPos().y);

                Point randScreenPos = new Point(Helpers.randomRange((int)x, (int)x + (int)Global.screenW), Helpers.randomRange((int)y, (int)y + (int)Global.screenH));
                new ParticleTwilight(this, randScreenPos);
            }
        }

        public void update()
        {
            //var actorsInScreen = getActorsInScreen();
            
            Dictionary<string, int> actorCounts = new Dictionary<string, int>();
            foreach (Actor actor in actors)
            {
                string name = actor.GetType().Name;
                if (!actorCounts.ContainsKey(name))
                {
                    actorCounts[name] = 0;
                }
                actorCounts[name]++;
            }
            
            HashSet<Actor> actorsEnumerate = new HashSet<Actor>(actors);
            foreach (Actor actor in actorsEnumerate)
            {
                actor.preUpdate();
                actor.update();
                actor.collidedInFrame.Clear();
            }

	        foreach (Actor actor in removedActors)
	        {
		        actors.Remove(actor);
                removeActorGrid(actor);
	        }
	        removedActors.Clear();

	        if (shakeDampen)
	        {
		        if (shakeX > 0) shakeX = Helpers.clampMin(shakeX - Global.spf, 0);
		        if (shakeY > 0) shakeY = Helpers.clampMin(shakeY - Global.spf, 0);
	        }
	        else if(shakeTime > 0)
	        {
		        shakeTime -= Global.spf;
		        if (shakeTime <= 0)
		        {
			        shakeTime = 0;
			        shakeX = 0;
			        shakeY = 0;
		        }
	        }

            /*
	        foreach (var pair in sounds)
	        {
		        pair.second.update();
	        }
            */
        }

        public void addActor(Actor actor)
        {
            actor.autoIncId = Global.game.autoIncId++;
            actor.level = this;
            actors.Add(actor);
            updateActorGrid(actor);
        }

        //Any time you do any action that:
        //	-Moves an actor
        //	-Changes an actor's collision boxes, or removes it
        //You should call this for performance. No longer mandatory to avoid having to manually call this every single time and risk null pointer exception in countless places in the code
        public void removeActorGrid(Actor actor)
        {
            Collider collider = actor.getMainCollider(true);
            if (collider == null)
            {
                return;
            }
            Shape shape = collider.getShape(actor);
            List<GridCoords> gridCoords = getGridCoords(shape);
            foreach (GridCoords gridCoord in gridCoords)
            {
                int i = gridCoord.i;
                int j = gridCoord.j;
                tileSlots[i][j].actors.Remove(actor);
                actor.gridActorSets.Remove(tileSlots[i][j].actors);
            }
        }

        public void updateActorGrid(Actor actor)
        {
            Rect boundingRect = actor.getBoundingRect();
            Shape shape = boundingRect.toShape();
            List<GridCoords> gridCoords = getGridCoords(shape);
            foreach (GridCoords gridCoord in gridCoords)
            {
                int i = gridCoord.i;
                int j = gridCoord.j;
                tileSlots[i][j].actors.Add(actor);
                actor.gridActorSets.Add(tileSlots[i][j].actors);
            }
        }

        public List<GridCoords> getGridCoords(Shape shape)
        {
            List<GridCoords> gridCoords = new List<GridCoords>();
            float pixelHeight = gridHeight * 8;
            float pixelWidth = gridWidth * 8;
            int minI = (int)Math.Floor((shape.minY() / pixelHeight) * gridHeight);
            int minJ = (int)Math.Floor((shape.minX() / pixelWidth) * gridWidth);
            int maxI = (int)Math.Floor((shape.maxY() / pixelHeight) * gridHeight);
            int maxJ = (int)Math.Floor((shape.maxX() / pixelWidth) * gridWidth);
            for (int i = minI; i <= maxI; i++)
            {
                for (int j = minJ; j <= maxJ; j++)
                {
                    if (i < 0 || j < 0 || i >= gridHeight || j >= gridWidth) continue;
                    gridCoords.Add(new GridCoords(i, j));
                }
            }
            return gridCoords;
        }

        public bool gridCoordsInBounds(GridCoords gridCoords)
        {
            return gridCoords.i >= 0 && gridCoords.j >= 0 && gridCoords.i < gridHeight && gridCoords.j < gridWidth;
        }

        public Point collideDataCompareOrigin;
        public int collideDataCompare(CollideData a, CollideData b)
        {
            float aDist = collideDataCompareOrigin.distTo(a.intersectionPoints[0]);
            float bDist = collideDataCompareOrigin.distTo(b.intersectionPoints[0]);
            if (aDist < bDist) return -1;
            else if (aDist > bDist) return 1;
            else return 0;
        }

        public List<CollideData> raycastAll(Actor actor, Point origin, Point direction)
        {
            List<CollideData> retColliders = new List<CollideData>();
            float remaining = direction.magnitude;
            HashSet<Actor> usedActors = new HashSet<Actor>();
            Point progress = Point.Zero;
            Point dirNorm = direction.normalized;
            int loop = 0;
            while (remaining > 0)
            {
                loop++; if (loop > 1000000) { throw new Exception("INFINITE LOOP IN RAYCASTALL"); }
                float factor = remaining < 8 ? remaining : 8;
                progress += dirNorm * factor;
                Point current = origin + progress;
                int j = (int)Math.Floor(current.x / 8);
                int i = (int)Math.Floor(current.y / 8);
                if (!gridCoordsInBounds(new GridCoords(i, j))) break;
                foreach (var tileData in tileSlots[i][j].tileInstances)
                {
                    if (actor != null)
                    {
                        if (!canCollide(actor, tileData, i, j)) continue;
                    }
                    Collider tileCollider = tileData.getCollider(i, j, this);
                    if (tileCollider != null)
                    {
                        CollideData collideData = new CollideData();
                        collideData.collidedTile = tileData;
                        collideData.collider = tileCollider;
                        collideData.tileI = i;
                        collideData.tileJ = j;
                        retColliders.Add(collideData);
                        break;
                    }
                }

                HashSet<Actor> actors = tileSlots[i][j].actors;
                List<CollideData> collidedActors = new List<CollideData>();
                foreach (Actor otherActor in actors)
                {
                    if (!canCollide(actor, otherActor)) continue;
                    if (usedActors.Contains(otherActor)) continue;
                    usedActors.Add(otherActor);
                    var myCollider = actor != null ? actor.getMainCollider(true) : null;
                    List<Collider> colliders = otherActor.getColliders();

                    foreach (Collider otherCollider in colliders)
                    {
                        Rect otherActorRect = otherCollider.getShape(otherActor).toRect();
                        Line line = new Line(origin, origin + direction);
                        CollideData collideData = Helpers.rectIntersectsLine(otherActorRect, line, origin);

                        if (collideData != null)
                        {
                            CollideData value = collideData;
                            value.collidedActor = otherActor;
                            value.collider = otherCollider;
                            value.myCollider = myCollider == null ? null : myCollider;
                            value.tileI = i;
                            value.tileJ = j;
                            value.isTrigger = value.myCollider.isTrigger || value.collider.isTrigger;
                            value.normal = otherActor.pos.dirTo(actor.pos);
                            value.rayTo = actor.pos.rayTo(otherActor.pos);
                            collidedActors.Add(value);
                        }
                    }
                }

                collideDataCompareOrigin = origin;
                collidedActors.Sort(collideDataCompare);

                foreach (var c in collidedActors)
                {
                    retColliders.Add(c);
                }

                remaining -= 8;
            }
            return retColliders;
        }

        public CollideData raycast(Actor actor, Point origin, Point direction)
        {
            var cds = raycastAll(actor, origin, direction);
            if (cds.Count > 0) return cds[0];
            return null;
        }

        public bool canCollide(Actor actor, TileData tileData, int i, int j)
        {
            if (!actor.isSolid) return false;
            if (actor.projectile != null && tileData.isLedge()) return false;
            if (actor.startedInCollision) return false;
            if (actor.elevation == 0 && actor.projectile != null)
            {
                GridCoords actorGridCoords = new GridCoords(actor.pos);
                if (!(i == actorGridCoords.i && j == actorGridCoords.j))
                {
                    return false;
                }
            }
            if (actor.elevation > 0)
            {
                return false;
            }
            if (actor.projectile != null && actor.throwable != null && tileSlots[i][j].noProjCollide)
            {
                return false;
            }
            return true;
        }

        public bool canCollide(Actor actor1, Actor actor2)
        {
            if (actor1 != null && !actor1.isSolid) return false;
            if (!actor2.isSolid) return false;
            if (actor1 == actor2) return false;
            if (actor1 != null && actor1.elevation != actor2.elevation)
            {
                //if (actor1.isLink() && actor2.throwable != null) return true;
                //if (actor2.isLink() && actor1.throwable != null) return true;
                return false;
            }
            if (actor1 != null && actor1.getMainCollider(true) == null) return false;
            if (actor2.getMainCollider(true) == null) return false;
            return true;
        }

        public bool isCover(int i, int j)
        {
            if (!gridCoordsInBounds(new GridCoords(i, j))) return false;
            return tileSlots[i][j].zIndex == 1;
        }

        public void freeActor(Actor actor)
        {
            int i = (int)actor.pos.y / 8;
            int j = (int)actor.pos.x / 8;

            List<GridCoords> coordsToTry = new List<GridCoords>();
            coordsToTry.Add(new GridCoords(i + 1, j));
            coordsToTry.Add(new GridCoords(i - 1, j));
            coordsToTry.Add(new GridCoords(i, j - 1));
            coordsToTry.Add(new GridCoords(i, j + 1));
            coordsToTry.Add(new GridCoords(i + 2, j));
            coordsToTry.Add(new GridCoords(i - 2, j));
            coordsToTry.Add(new GridCoords(i, j - 2));
            coordsToTry.Add(new GridCoords(i, j + 2));
            coordsToTry.Add(new GridCoords(i - 1, j - 1));
            coordsToTry.Add(new GridCoords(i - 1, j + 1));
            coordsToTry.Add(new GridCoords(i + 1, j - 1));
            coordsToTry.Add(new GridCoords(i + 1, j + 1));
            coordsToTry.Add(new GridCoords(i - 2, j - 2));
            coordsToTry.Add(new GridCoords(i - 2, j + 2));
            coordsToTry.Add(new GridCoords(i + 2, j - 2));
            coordsToTry.Add(new GridCoords(i + 2, j + 2));

            foreach (var coord in coordsToTry)
            {
                Point tilePos = new Point(coord.j * 8, coord.i * 8);
                Point offset = tilePos - actor.pos;
                if (gridCoordsInBounds(new GridCoords(i, j)) && getActorCollisions(actor, offset).Count == 0)
                {
                    actor.changePos(tilePos, false);
                    return;
                }
            }
        }

        public List<CollideData> getActorCollisions(Actor actor, Point offset)
        {
            List<CollideData> retColliders = new List<CollideData>();
            if (!actor.isSolid) return retColliders;

            if (offset.isNonZero())
            {
                actor.move(offset, false, false);
            }

            foreach (Collider collider in actor.getColliders())
            {
                Shape shape = collider.getShape(actor);
                List<GridCoords> gridCoords = getGridCoords(shape);

                HashSet<Actor> usedActors = new HashSet<Actor>();
                foreach (GridCoords gridCoord in gridCoords)
                {
                    int i = gridCoord.i;
                    int j = gridCoord.j;

                    if (tileSlots[i][j].tileInstances.Count == 0) continue;
                    TileData tileData = tileSlots[i][j].tileInstances[0];

                    if (tileData.hasTag("tallgrass") && collider.tags.Contains("sword") && actor.canStateCut())
                    {
                        //GridCoords actorCoords = GridCoords(actor.pos);
                        //if ((actor.dir == LEFT || actor.dir == RIGHT) && (actorCoords.i != i)) continue;
                        //if ((actor.dir == UP || actor.dir == DOWN) && (actorCoords.j != j)) continue;

                        int topI = (i % 2 != 0 ? i - 1 : i);
                        int topJ = (j % 2 != 0 ? j - 1 : j);
                        tileSlots[topI][topJ].tileInstances[0] = Global.tileDatas["zeldamainmap_32_32"];
                        tileSlots[topI][topJ + 1].tileInstances[0] = Global.tileDatas["zeldamainmap_32_33"];
                        tileSlots[topI + 1][topJ].tileInstances[0] = Global.tileDatas["zeldamainmap_33_32"];
                        tileSlots[topI + 1][topJ + 1].tileInstances[0] = Global.tileDatas["zeldamainmap_33_33"];
                        new Anim(this, new Point(8 + j * 8, 8 + i * 8), "GrassBreak");
                        actor.playSound("grass destroyed");
                        WorldObjectFactories.createRandomPickup(this, new Point(j * 8, i * 8), true);
                        continue;
                    }
                    else if (actor.name == "BombExplosion" && actor.sprite.frameIndex == actor.sprite.frames.Count - 2)
                    {
                        //One-off: bombable cave walls in overworld
                        Point offset2 = new Point();
                        if (tileData.hasTag("rockcrack1")) offset2 = new Point(-1, 0);
                        else if (tileData.hasTag("rockcrack2")) offset2 = new Point(-2, 0);
                        else if (tileData.hasTag("rockcrack3")) offset2 = new Point(-1, -1);
                        else if (tileData.hasTag("rockcrack4")) offset2 = new Point(-2, -1);
                        if (offset2.x != 0 && offset2.y != 0)
                        {
                            int topI = (int)(i + offset2.x);
                            int topJ = (int)(j + offset2.y);
                            tileSlots[topI][topJ].tileInstances[0] = Global.tileDatas["zeldamainmap_394_456"];
                            tileSlots[topI][topJ + 1].tileInstances[0] = Global.tileDatas["zeldamainmap_15_169"];
                            tileSlots[topI][topJ + 2].tileInstances[0] = Global.tileDatas["zeldamainmap_15_169"];
                            tileSlots[topI][topJ + 3].tileInstances[0] = Global.tileDatas["zeldamainmap_394_459"];
                            tileSlots[topI + 1][topJ].tileInstances[0] = Global.tileDatas["zeldamainmap_395_456"];
                            tileSlots[topI + 1][topJ + 1].tileInstances[0] = Global.tileDatas["zeldamainmap_395_457"];
                            tileSlots[topI + 1][topJ + 2].tileInstances[0] = Global.tileDatas["zeldamainmap_395_458"];
                            tileSlots[topI + 1][topJ + 3].tileInstances[0] = Global.tileDatas["zeldamainmap_395_459"];

                            topI -= 2;
                            for (int i2 = topI; i2 < topI + 2; i2++)
                            {
                                for (int j2 = topJ; j2 < topJ + 4; j2++)
                                {
                                    tileSlots[i2][j2].zIndex = 1;
                                }
                            }
                            actor.playSound("secret");
                            continue;
                        }

                        //One-off: bombable shed in Kakariko
                        if (this.name == "lttp_overworld" && i == 304 && j == 13 && tileSlots[i][j].tileInstances[0].getKey() == "zeldamainmap_214_74")
                        {
                            tileSlots[i][j - 1].tileInstances[0] = Global.tileDatas["zeldamainmap_304_12"];
                            tileSlots[i][j].tileInstances[0] = Global.tileDatas["zeldamainmap_15_169"];
                            tileSlots[i][j + 1].tileInstances[0] = Global.tileDatas["zeldamainmap_15_169"];
                            tileSlots[i][j + 2].tileInstances[0] = Global.tileDatas["zeldamainmap_304_15"];

                            tileSlots[i + 1][j - 1].tileInstances[0] = Global.tileDatas["zeldamainmap_305_12"];
                            tileSlots[i + 1][j].tileInstances[0] = Global.tileDatas["zeldamainmap_165_95"];
                            tileSlots[i + 1][j + 1].tileInstances[0] = Global.tileDatas["zeldamainmap_305_14"];
                            tileSlots[i + 1][j + 2].tileInstances[0] = Global.tileDatas["zeldamainmap_305_15"];
                            actor.playSound("secret");
                        }

                        if (tileData.hasTag("bombable"))
                        {
                            bombWallAtPos(i, j, false);
                            continue;
                        }

                    }

                    foreach (TileData tileData2 in tileSlots[i][j].tileInstances)
                    {
                        if (!canCollide(actor, tileData2, i, j)) continue;
                        Collider tileCollider = tileData2.getCollider(i, j, this);
                        if (tileCollider == null) continue;

                        CollideData collideData = Helpers.shapesIntersect(shape, tileCollider.getShape(tileData2));
                        if (collideData != null)
                        {
                            CollideData value = collideData;
                            value.collidedTile = tileData2;
                            value.collider = tileCollider;
                            value.myCollider = collider;
                            value.tileI = i;
                            value.tileJ = j;
                            value.isTrigger = collider.isTrigger || value.collider.isTrigger;
                            value.rayTo = actor.pos.rayTo(tileCollider.getShape(tileData2).center());

                            if (tileData2.hitboxMode == HitboxMode.DiagTopLeft) value.diagDir = 1;
                            if (tileData2.hitboxMode == HitboxMode.DiagTopRight) value.diagDir = 2;
                            if (tileData2.hitboxMode == HitboxMode.DiagBotLeft) value.diagDir = 3;
                            if (tileData2.hitboxMode == HitboxMode.DiagBotRight) value.diagDir = 4;

                            retColliders.Add(value);
                        }
                    }

                    HashSet<Actor> actors = tileSlots[i][j].actors;
                    foreach (Actor otherActor in actors)
                    {
                        if (!canCollide(actor, otherActor)) continue;
                        if (usedActors.Contains(otherActor)) continue;
                        usedActors.Add(otherActor);

                        List<Collider> colliders = otherActor.getColliders();
                        foreach (Collider actorCollider in colliders)
                        {
                            Shape otherShape = actorCollider.getShape(otherActor);

                            if (actor.isLink() && otherActor.isLink() && !collider.isTrigger && !actorCollider.isTrigger)
                            {
                                continue;
                            }

                            //if (!collider.isTrigger && !actorCollider.isTrigger && actor.isLink() && Helpers.setContains(actor.getChar().actorsTouchingInFrame, otherActor)) continue;

                            CollideData collideData = Helpers.shapesIntersect(otherShape, shape);
                            if (collideData != null)
                            {
                                CollideData value = collideData;
                                value.collidedActor = otherActor;
                                value.collider = actorCollider;
                                value.myCollider = collider;
                                value.tileI = i;
                                value.tileJ = j;
                                value.isTrigger = collider.isTrigger || value.collider.isTrigger;
                                value.rayTo = actor.pos.rayTo(otherActor.pos);
                                retColliders.Add(value);
                            }
                        }
                    }
                }
            }

            if (offset.isNonZero())
            {
                actor.move(offset * -1, false, false);
            }

            return retColliders;
        }

        void bombWallAtPos(int i, int j, bool linked)
        {
            int jOffLeft = 0;
            while (tileSlots[i][j + jOffLeft - 1].tileInstances[0].hasTag("bombable"))
            {
                jOffLeft--;
            }
            int iOffUp = 0;
            while (tileSlots[i + iOffUp - 1][j].tileInstances[0].hasTag("bombable"))
            {
                iOffUp--;
            }

            Point topLeft = new Point(j + jOffLeft - 1, i + iOffUp - 1);

            var key = tileSlots[(int)topLeft.y][(int)topLeft.x].tileInstances[0].getKey();
            var bombTidGrid = Global.bombableWallMap[key];

            for (int i2 = 0; i2 < bombTidGrid.Count; i2++)
            {
                for (int j2 = 0; j2 < bombTidGrid[i2].Count; j2++)
                {
                    tileSlots[(int)topLeft.y + i2][(int)topLeft.x + j2].tileInstances[0] = Global.tileDatas[bombTidGrid[i2][j2]];
                }
            }

            if (!linked)
            {
                playSound("secret", new Point(j * 8, i * 8));
                string coordKey = name + "_" + topLeft.y.ToString() + "_" + topLeft.x.ToString();
                if (Global.bombableWallLinkage.ContainsKey(coordKey))
                {
                    GridCoords coords = Global.bombableWallLinkage[coordKey];
                    bombWallAtPos(coords.i + 1, coords.j + 1, true);
                }
            }
        }

        public void removeActor(Actor actor)
        {
            if (removedActors.Contains(actor)) return;

            removeActorGrid(actor);
            foreach (var actorSet in actor.gridActorSets)
            {
                actorSet.Remove(actor);
            }
            removedActors.Add(actor);
        }

        public void showHitboxes(float xOff, float yOff, float i, float j, TileData tileData)
        {
            float x = xOff + j * 8;
            float y = yOff + i * 8;
            float xMid = x + 4;
            float yMid = y + 4;
            float x2 = x + 8;
            float y2 = y + 8;
            if (tileData.hitboxMode == HitboxMode.Tile)
            {
                DrawWrappers.DrawRect(x, y, x + 8, y + 8, true, new Color(255, 0, 0, 128), 1, (int)ZIndex.HUD);
            }
            else if (tileData.hitboxMode == HitboxMode.BoxTop)
            {
                DrawWrappers.DrawRect(x, y, x + 8, y + 4, true, new Color(255, 0, 0, 128), 1, (int)ZIndex.HUD);
            }
            else if (tileData.hitboxMode == HitboxMode.BoxBot)
            {
                DrawWrappers.DrawRect(x, yMid, x + 8, yMid + 4, true, new Color(255, 0, 0, 128), 1, (int)ZIndex.HUD);
            }
            else if (tileData.hitboxMode == HitboxMode.BoxLeft)
            {
                DrawWrappers.DrawRect(x, y, x + 4, y + 8, true, new Color(255, 0, 0, 128), 1, (int)ZIndex.HUD);
            }
            else if (tileData.hitboxMode == HitboxMode.BoxRight)
            {
                DrawWrappers.DrawRect(xMid, y, xMid + 4, y + 8, true, new Color(255, 0, 0, 128), 1, (int)ZIndex.HUD);
            }
            else if (tileData.hitboxMode == HitboxMode.BoxTopLeft)
            {
                DrawWrappers.DrawRect(x, y, x + 4, y + 4, true, new Color(255, 0, 0, 128), 1, (int)ZIndex.HUD);
            }
            else if (tileData.hitboxMode == HitboxMode.BoxTopRight)
            {
                DrawWrappers.DrawRect(xMid, y, xMid + 4, y + 4, true, new Color(255, 0, 0, 128), 1, (int)ZIndex.HUD);
            }
            else if (tileData.hitboxMode == HitboxMode.BoxBotLeft)
            {
                DrawWrappers.DrawRect(x, yMid, x + 4, yMid + 4, true, new Color(255, 0, 0, 128), 1, (int)ZIndex.HUD);
            }
            else if (tileData.hitboxMode == HitboxMode.BoxBotRight)
            {
                DrawWrappers.DrawRect(xMid, yMid, xMid + 4, yMid + 4, true, new Color(255, 0, 0, 128), 1, (int)ZIndex.HUD);
            }
            else if (tileData.hitboxMode == HitboxMode.DiagTopLeft)
            {
                DrawWrappers.DrawPolygon(new List<Point>() { new Point(x, y), new Point(x2, y), new Point(x, y2) }, new Color(255, 0, 0, 128), true, ZIndex.HUD, true);
            }
            else if (tileData.hitboxMode == HitboxMode.DiagTopRight)
            {
                DrawWrappers.DrawPolygon(new List<Point>() { new Point(x, y), new Point(x2, y), new Point(x2, y2) }, new Color(255, 0, 0, 128), true, ZIndex.HUD, true);
            }
            else if (tileData.hitboxMode == HitboxMode.DiagBotLeft)
            {
                DrawWrappers.DrawPolygon(new List<Point>() { new Point(x, y), new Point(x, y2), new Point(x2, y2) }, new Color(255, 0, 0, 128), true, ZIndex.HUD, true);
            }
            else if (tileData.hitboxMode == HitboxMode.DiagBotRight)
            {
                DrawWrappers.DrawPolygon(new List<Point>() { new Point(x2, y), new Point(x2, y2), new Point(x, y2) }, new Color(255, 0, 0, 128), true, ZIndex.HUD, true);
            }
        }

        public void shake(float shakeX, float shakeY, bool dampen, float shakeTime)
        {
            this.shakeX = shakeX;
            this.shakeY = shakeY;
            this.shakeDampen = dampen;
            this.shakeTime = shakeTime;
        }

        public bool inLostWoods()
        {
            if (Global.game.camCharacter == null) return false;
            return (this.name == "lttp_overworld" && Global.game.camCharacter.pos.x < 1024 && Global.game.camCharacter.pos.y < 1024) ||
                (this.name == "house" && Global.game.camCharacter.pos.x > 128 * 8 && Global.game.camCharacter.pos.y > 288 * 8);
        }

        public bool inLostWoodsNotSwordArea()
        {
            if (Global.game.camCharacter == null) return false;
            return (this.name == "lttp_overworld" && Global.game.camCharacter.pos.x < 1024 && Global.game.camCharacter.pos.y < 1024);
        }

        public bool inKakarikoVillage()
        {
            if (Global.game.camCharacter == null) return false;
            return this.name == "lttp_overworld" &&
                ((Global.game.camCharacter.pos.x < 128 * 8 && Global.game.camCharacter.pos.y > 192 * 8 && Global.game.camCharacter.pos.y < 384 * 8) ||
                (Global.game.camCharacter.pos.x < 192 * 8 && Global.game.camCharacter.pos.y > 256 * 8 && Global.game.camCharacter.pos.y < 320 * 8));
        }

        public void playSound(string soundKey, Point pos)
        {
            if (Global.game.camCharacter == null) return;
            float distToCam = pos.distTo(Global.game.camCharacter.pos);
            Sound sound = new Sound(Global.soundBuffers[soundKey]);
            float volume = distToCam < 128.0f ? 1 : 0;
            if (volume <= 0)
            {
                volume = 0;
                return;
            }
            sound.Volume = volume * 100 * Options.main.soundVolume;
            sound.Play();
            Global.sounds.Add(sound);
        }

        public void startMusic(string overrideKey = "")
        {
            //House music should just play the current music quieter
            if (overrideKey == "" && musicKey == "house")
            {
                Global.music.volume = Global.defaultMusicVolume / 2;
                return;
            }
            //Not house music: set it to 1 as default
            if (Global.music != null && Global.music.music.Volume < (Global.defaultMusicVolume * 3) / 5)
            {
                Global.music.volume = Global.defaultMusicVolume;
                return;
            }
            if (Global.music != null)
            {
                Global.music.music.Stop();
            }
            Global.music = Global.musics[overrideKey == "" ? musicKey : overrideKey];
            Global.music.volume = 50;
            Global.music.music.Play();
        }

        public TileSlot getTileSlotIfExists(int i, int j)
        {
            if (!gridCoordsInBounds(new GridCoords(i, j))) return null;
            return tileSlots[i][j];
        }

        public List<TileSlot> getShortestPath(Character character, Point start, Point end, Actor target)
        {
            List<TileSlot> returnVec = new List<TileSlot>();

            if (Global.game.throttledChars.ContainsKey(character))
            {
                return returnVec;
            }

            int startClusterI = (int)(start.y / 256);
            int startClusterJ = (int)(start.x / 256);

            int endClusterI = (int)(end.y / 256);
            int endClusterJ = (int)(end.x / 256);

            int startClusterGridI = startClusterI * 32;
            int endClusterGridI = (startClusterI + 1) * 32;

            int startClusterGridJ = startClusterJ * 32;
            int endClusterGridJ = (startClusterJ + 1) * 32;

            int startI = (int)(start.y / 8);
            int startJ = (int)(start.x / 8);

            int endI = (int)(end.y / 8);
            int endJ = (int)(end.x / 8);

            //override for now that does A* on the whole map
            startClusterGridI = 0;
            startClusterGridJ = 0;
            endClusterGridI = gridHeight;
            endClusterGridJ = gridWidth;
            endClusterI = startClusterI;
            endClusterJ = startClusterJ;

            for (int i = startClusterGridI; i < endClusterGridI; i++)
            {
                for (int j = startClusterGridJ; j < endClusterGridJ; j++)
                {
                    tileSlots[i][j].cameFrom = null;
                    tileSlots[i][j].gScore = 1000000;
                    tileSlots[i][j].fScore = 1000000;
                }
            }

            //Simple case: A* in current cluster
            if (startClusterI == endClusterI && startClusterJ == endClusterJ)
            {
                HashSet<TileSlot> openSet = new HashSet<TileSlot>();
                HashSet<TileSlot> closedSet = new HashSet<TileSlot>();

                SimplePriorityQueue<TileSlot> queue = new SimplePriorityQueue<TileSlot>();
                
                GridCoords startCoords = new GridCoords(start);
                TileSlot startSlot = tileSlots[startCoords.i][startCoords.j];
                startSlot.gScore = 0;
                startSlot.fScore = startSlot.gridCoords.distTo(new GridCoords(endI, endJ));   //h(n)
                openSet.Add(startSlot);
                queue.EnqueueWithoutDuplicates(startSlot, startSlot.fScore);
                int iterations = 0;
                while (openSet.Count > 0)
                {
                    iterations++;
                    TileSlot current = queue.Dequeue();

                    if (iterations > 10000)
                    {
                        //Throttle bad pathfinding actor
                        if (!Global.game.throttledChars.ContainsKey(character))
                        {
                            Global.game.throttledChars[character] = 1;
                        }
                        //cout << "Too many iterations in A*, aborting" << endl;
                        return returnVec;
                    }

                    if (current.gridCoords.i == endI && current.gridCoords.j == endJ)
                    {
                        returnVec.Add(current);
                        while (current.cameFrom != null)
                        {
                            current = current.cameFrom;
                            returnVec.Insert(0, current);
                        }
                        return returnVec;
                    }

                    openSet.Remove(current);
                    closedSet.Add(current);

                    List<TileSlot> neighbors = new List<TileSlot>();
                    int i = current.gridCoords.i;
                    int j = current.gridCoords.j;

                    if (i - 1 >= startClusterGridI && tileSlots[i - 1][j].canAIMoveTo(character, target)) neighbors.Add(tileSlots[i - 1][j]);
                    if (i + 1 < endClusterGridI && tileSlots[i + 1][j].canAIMoveTo(character, target)) neighbors.Add(tileSlots[i + 1][j]);
                    if (j - 1 >= startClusterGridJ && tileSlots[i][j - 1].canAIMoveTo(character, target)) neighbors.Add(tileSlots[i][j - 1]);
                    if (j + 1 < endClusterGridJ && tileSlots[i][j + 1].canAIMoveTo(character, target)) neighbors.Add(tileSlots[i][j + 1]);

                    //if (i - 1 >= startClusterGridI && j - 1 >= startClusterGridJ && tileSlots[i - 1][j - 1].canAIMoveTo(character)) neighbors.Add(&tileSlots[i - 1][j - 1]);
                    //if (i - 1 >= startClusterGridI && j + 1 < endClusterGridJ && tileSlots[i - 1][j + 1].canAIMoveTo(character)) neighbors.Add(&tileSlots[i - 1][j + 1]);
                    //if (i + 1 < endClusterGridI && j - 1 >= startClusterGridJ && tileSlots[i + 1][j - 1].canAIMoveTo(character)) neighbors.Add(&tileSlots[i + 1][j - 1]);
                    //if (i + 1 < endClusterGridI && j + 1 < endClusterGridJ && tileSlots[i + 1][j + 1].canAIMoveTo(character)) neighbors.Add(&tileSlots[i + 1][j + 1]);

                    foreach (var neighbor in neighbors)
                    {
                        if (closedSet.Contains(neighbor))
                        {
                            continue;
                        }
                        float tentative_gScore = current.gScore + 1;
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                            queue.EnqueueWithoutDuplicates(neighbor, neighbor.fScore);
                        }
                        else if (tentative_gScore >= neighbor.gScore)
                        {
                            continue;
                        }
                        neighbor.cameFrom = current;
                        neighbor.gScore = tentative_gScore;
                        neighbor.fScore = neighbor.gScore + neighbor.gridCoords.distTo(new GridCoords(endI, endJ));
                    }
                }
                if (returnVec.Count > 0)
                {
                    return returnVec;
                }
            }
            //if (Global.logAI) cout << "Failed to get to destination at " << end.x / 8 << "," << end.y / 8 << endl;
            
            //Complex case: either first case nothing found, or in different cluster.
            //Construct a graph consisting of all exit nodes, in addition to all the nodes the start position is connected to
            //and all the nodes the end position is connected to. Then A* through this graph.

            return returnVec;
        }
    }
}

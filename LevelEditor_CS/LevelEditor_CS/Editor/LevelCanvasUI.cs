using LevelEditor_CS.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LevelEditor_CS.LevelEditor;

namespace LevelEditor_CS.Editor
{
    public class LevelCanvasUI : CanvasUI
    {
        public GridCoords prevGridCoords;
        public GridCoords lastMouseMoveGridCoords;
        public GridCoords lastMouseMoveGridCoords8x8;
        public bool mouseLeftCanvas = false;
        public LevelEditor levelEditor;

        public LevelCanvasUI(PictureBox pictureBox, Panel panel, LevelEditor levelEditor) : base(pictureBox, panel, panel.Width, panel.Height, Color.Transparent)
        {
            this.levelEditor = levelEditor;
        }

        public override void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            base.pictureBox_Paint(sender, e);

            var tileWidth = 8;
            if (levelEditor.mode16x16) tileWidth = 16;

            if (levelEditor.showLevelGrid)
            {
                //Draw columns
                for (var i = 1; i < CanvasWidth / tileWidth; i++)
                {
                    Helpers.drawLine(e.Graphics, i * tileWidth, 0, i * tileWidth, CanvasHeight, Color.Red, 1);
                }
                //Draw rows
                for (var i = 1; i < CanvasHeight / tileWidth; i++)
                {
                    Helpers.drawLine(e.Graphics, 0, i * tileWidth, CanvasWidth, i * tileWidth, Color.Red, 1);
                }
                if (levelEditor.selectedLevel != null)
                {
                    //Draw scroll lines
                    foreach (var scrollLine in levelEditor.selectedLevel.scrollLines)
                    {
                        Helpers.drawLine(e.Graphics, scrollLine.point1.x, scrollLine.point1.y, scrollLine.point2.x, scrollLine.point2.y, Color.Yellow, 3);
                    }
                }
            }
            if (levelEditor.showRoomLines)
            {
                //Draw columns
                for (var i = 1; i < CanvasWidth / 256; i++)
                {
                    Helpers.drawLine(e.Graphics, i * 256, 0, i * 256, CanvasHeight, Color.White, 1);
                }
                //Draw rows
                for (var i = 1; i < CanvasHeight / 256; i++)
                {
                    Helpers.drawLine(e.Graphics, 0, i * 256, CanvasWidth, i * 256, Color.White, 1);
                }
            }

            if (levelEditor.selectedTool == Tool.Select && levelEditor.levelSelectedCoords != null)
            {
                foreach (var point in levelEditor.levelSelectedCoords)
                {
                    Helpers.drawRect(e.Graphics, point.getRect(), null, Color.Green, 2);
                }
            }
            if (levelEditor.selectedTool == Tool.Select || levelEditor.selectedTool == Tool.RectangleTile || levelEditor.selectedTool == Tool.SelectInstance)
            {
                if (this.mousedown)
                {
                    Helpers.drawRect(e.Graphics, new Rect(this.dragLeftX, this.dragTopY, this.dragRightX, this.dragBotY), null, Color.Blue, 1);
                }
            }
            if (levelEditor.selectedTool == Tool.PlaceTile && !this.mouseLeftCanvas)
            {
                if (levelEditor.clonedTiles != null)
                {
                    var rect = levelEditor.clonedTiles.getRect();
                    var destPoint = this.getMouseGridCoords();
                    if (levelEditor.mode16x16 && this.mouseY % 16 >= 8)
                    {
                        destPoint.i--;
                    }
                    if (levelEditor.mode16x16 && this.mouseX % 16 >= 8)
                    {
                        destPoint.j--;
                    }
                    Helpers.drawImage(e.Graphics, levelEditor.selectedLevel.layers[0], rect.x1, rect.y1, rect.w, rect.h, destPoint.j * Consts.TILE_WIDTH, destPoint.i * Consts.TILE_WIDTH);
                }
                else
                {
                    var selectedGridCoords = levelEditor.tileSelectedCoords;
                    if (selectedGridCoords == null || selectedGridCoords.Count == 0) return;
                    foreach (var gridCoord in selectedGridCoords)
                    {
                        var topLeftSelectedTileGridCoord = getTopLeftSelectedTileGridCoord();
                        var offsettedGridCoords = new GridCoords(gridCoord.i - topLeftSelectedTileGridCoord.i, gridCoord.j - topLeftSelectedTileGridCoord.j);
                        var destPoint = this.getMouseGridCoords();
                        if (levelEditor.mode16x16 && this.mouseY % 16 >= 8)
                        {
                            destPoint.i--;
                        }
                        if (levelEditor.mode16x16 && this.mouseX % 16 >= 8)
                        {
                            destPoint.j--;
                        }

                        destPoint.i += offsettedGridCoords.i;
                        destPoint.j += offsettedGridCoords.j;

                        var rect = gridCoord.getRect();
                        Helpers.drawImage(e.Graphics, levelEditor.selectedTileset.image, rect.x1, rect.y1, rect.w, rect.h, destPoint.j * Consts.TILE_WIDTH, destPoint.i * Consts.TILE_WIDTH);
                    }
                }
            }
            if (levelEditor.selectedTool == Tool.CreateInstance && !this.mouseLeftCanvas)
            {
                var sprite = levelEditor.sprites.Where(s => { return s.name == levelEditor.selectedObj.spriteOrImage; }).FirstOrDefault();
                var a = (this.getMouseGridCoordsCustomWidth(Consts.TILE_WIDTH).j * Consts.TILE_WIDTH);
                var b = (this.getMouseGridCoordsCustomWidth(Consts.TILE_WIDTH).i * Consts.TILE_WIDTH);
                var x = a + levelEditor.selectedObj.snapOffset.x;
                var y = b + levelEditor.selectedObj.snapOffset.y;
                var rect = sprite.frames[0].rect;
                sprite.draw(e.Graphics, 0, x, y);
            }
            /*
            if (levelEditor.showElevations)
            {
                var grid = levelEditor.selectedLevel.coordPropertiesGrid;
                for (var i = 0; i < grid.Count; i++)
                {
                    for (var j = 0; j < grid[i].Count; j++)
                    {
                        var elevation = grid[i][j].elevation;
                        var color = "";
                        if (elevation == -3) color = "red";
                        else if (elevation == -2) color = "orange";
                        else if (elevation == -1) color = "yellow";
                        else if (elevation == 1) color = "blue";
                        else if (elevation == 2) color = "indigo";
                        else if (elevation == 3) color = "violet";
                        else continue;
                        Helpers.drawRect(e.Graphics, new GridCoords(i, j).getRect(), color, "", 0, 0.5);
                    }
                }
            }
            */
            if (levelEditor.selectedLevel != null && levelEditor.showInstances)
            {
                foreach (SpriteInstance instance in levelEditor.selectedLevel.instances)
                {
                    instance.draw(e.Graphics);
                }
            }

            if (levelEditor.selectedTool == Tool.SelectInstance)
            {
                foreach (var instance in levelEditor.selectedInstances)
                {
                    Helpers.drawRect(e.Graphics, instance.getPositionalRect(), null, Color.Yellow, 3);
                }
            }
            if (!string.IsNullOrEmpty(levelEditor.showOverridesWithKey))
            {
                var grid = levelEditor.selectedLevel.coordPropertiesGrid;
                for (var i = 0; i < grid.Count; i++)
                {
                    for (var j = 0; j < grid[i].Count; j++)
                    {
                        if (grid[i][j][levelEditor.showOverridesWithKey])
                        {
                            Helpers.drawRect(e.Graphics, new GridCoords(i, j).getRect(), Color.Red, null, 0, 0.5f);
                        }
                    }
                }
            }

            /*
            if (levelEditor.showNavMesh && levelEditor.selectedLevel && levelEditor.selectedLevel.coordPropertiesGrid)
            {
                var grid = levelEditor.selectedLevel.coordPropertiesGrid;
                for (var i = 0; i < grid.Count; i++)
                {
                    for (var j = 0; j < grid[i].Count; j++)
                    {
                        if (grid[i][j]["navmesh"])
                        {
                            Helpers.drawRect(e.Graphics, new GridCoords(i, j).getRect(), "yellow", "", 0, 0.5);
                        }
                        var neighbors = grid[i][j]["neighbors"];
                        if (!neighbors) continue;
                        foreach (var neighbor in neighbors)
                        {
                            var ni = int.Parse(neighbor.split(",")[0]);
                            var nj = int.Parse(neighbor.split(",")[1]);
                            Helpers.drawLine(e.Graphics, 4 + j * 8, 4 + i * 8, 4 + nj * 8, 4 + ni * 8, "yellow", 2);
                        }
                    }
                }
            }
            */
        }

        public void onMouseMove()
        {
            if (this.mousedown)
            {
                if (levelEditor.selectedTool == Tool.PlaceTile)
                {
                    var topI = this.getMouseGridCoords().i;
                    var topJ = this.getMouseGridCoords().j;
                    if (this.isHeld(Keys.Shift))
                    {
                        topI = this.prevGridCoords.i;
                    }
                    if (this.isHeld(Keys.LControlKey))
                    {
                        topJ = this.prevGridCoords.j;
                    }
                    if (topI != this.prevGridCoords.i || topJ != this.prevGridCoords.j)
                    {
                        this.placeTile(new GridCoords(topI, topJ));
                    }
                    this.prevGridCoords = new GridCoords(topI, topJ);
                }
                /*
                else if (levelEditor.selectedTool == Tool.PaintElevation)
                {
                    levelEditor.selectedLevel.coordPropertiesGrid[this.getMouseGridCoords().i][this.getMouseGridCoords().j].elevation = levelEditor.paintElevationHeight;
                }
                */
                this.redraw();
            }
            else
            {
                this.mouseLeftCanvas = false;
                if (levelEditor.selectedTool == Tool.PlaceTile && !this.getMouseGridCoords().equals(this.lastMouseMoveGridCoords))
                {
                    this.redraw();
                }
                else if (levelEditor.selectedTool == Tool.CreateInstance && !this.getMouseGridCoordsCustomWidth(Consts.TILE_WIDTH).equals(this.lastMouseMoveGridCoords8x8))
                {
                    this.redraw();
                }
                this.lastMouseMoveGridCoords = this.getMouseGridCoords();
                this.lastMouseMoveGridCoords8x8 = this.getMouseGridCoordsCustomWidth(Consts.TILE_WIDTH);
            }
        }

        public override void onMouseLeave()
        {
            this.mouseLeftCanvas = true;
            this.redraw();
        }

        public override void onLeftMouseDown()
        {
            if (levelEditor.selectedTool == Tool.Select)
            {

            }
            else if (levelEditor.selectedTool == Tool.CreateInstance)
            {
                var x = this.mouseX;
                var y = this.mouseY;
                if (levelEditor.selectedObj.snapToTile)
                {
                    var sprite = levelEditor.sprites.Where(s => { return s.name == levelEditor.selectedObj.spriteOrImage; }).FirstOrDefault();
                    var a = (this.getMouseGridCoordsCustomWidth(Consts.TILE_WIDTH).j * Consts.TILE_WIDTH);
                    var b = (this.getMouseGridCoordsCustomWidth(Consts.TILE_WIDTH).i * Consts.TILE_WIDTH);
                    var alignOffset = sprite.getAlignOffset(sprite.frames[0]);
                    x = a + levelEditor.selectedObj.snapOffset.x;
                    y = b + levelEditor.selectedObj.snapOffset.y;
                    if (levelEditor.selectedLevel.instances.Any((SpriteInstance i) => { return i.pos.x == x && i.pos.y == y && i.obj == levelEditor.selectedObj; })) 
                    {
                        Console.WriteLine("Same instance found at point! Not creating...");
                        return;
                    }
                }
                var instance = new SpriteInstance(levelEditor.selectedObj.name, x, y, levelEditor.selectedObj, levelEditor.sprites);
                levelEditor.selectedLevel.instances.Add(instance);
                //levelEditor.selectedObj = undefined;
                //levelEditor.selectedInstances = [instance];
                //levelEditor.selectedTool = Tool.SelectInstance;
                levelEditor.addUndoJson();
                levelEditor.sortInstances();
                this.redraw();
            }
            else if (levelEditor.selectedTool == Tool.PlaceTile)
            {
                this.prevGridCoords = this.getMouseGridCoords();
                this.placeTile(this.getMouseGridCoords());
            }
            /*
            else if (levelEditor.selectedTool == Tool.PaintElevation)
            {
                levelEditor.selectedLevel.coordPropertiesGrid[this.getMouseGridCoords().i][this.getMouseGridCoords().j].elevation = Number(levelEditor.paintElevationHeight);
                this.redraw();
            }
            */
        }

        public void placeTile(GridCoords gridCoords)
        {
            if (levelEditor.clonedTiles != null)
            {
                var rect = levelEditor.clonedTiles.getRect();
                var destPoint = this.getMouseGridCoords();
                if (levelEditor.mode16x16 && this.mouseY % 16 >= 8)
                {
                    destPoint.i--;
                }
                if (levelEditor.mode16x16 && this.mouseX % 16 >= 8)
                {
                    destPoint.j--;
                }
                var ctx = levelEditor.selectedLevel.layers[levelEditor.layerIndex].getContext("2d");
                //@ts-ignore
                Helpers.drawImage(ctx, levelEditor.selectedLevel.layers[0], rect.x1, rect.y1, rect.w, rect.h, destPoint.j * Consts.TILE_WIDTH, destPoint.i * Consts.TILE_WIDTH);
                return;
            }

            var selectedGridCoords = levelEditor.tileSelectedCoords;
            if (selectedGridCoords == null || selectedGridCoords.Count == 0) return;

            foreach (var selectedGridCoord in selectedGridCoords)
            {
                var topLeftSelectedTileGridCoord = getTopLeftSelectedTileGridCoord();
                var offsettedGridCoords = new GridCoords(selectedGridCoord.i - topLeftSelectedTileGridCoord.i, selectedGridCoord.j - topLeftSelectedTileGridCoord.j);
                offsettedGridCoords.i += gridCoords.i;
                offsettedGridCoords.j += gridCoords.j;

                if (levelEditor.mode16x16 && this.mouseY % 16 >= 8)
                {
                    offsettedGridCoords.i--;
                }
                if (levelEditor.mode16x16 && this.mouseX % 16 >= 8)
                {
                    offsettedGridCoords.j--;
                }

                levelEditor.selectedLevel.layers[levelEditor.layerIndex].getContext("2d").drawImage(levelEditor.selectedTileset.imgEl, selectedGridCoord.j * Consts.TILE_WIDTH, selectedGridCoord.i * Consts.TILE_WIDTH, Consts.TILE_WIDTH, Consts.TILE_WIDTH, offsettedGridCoords.j * Consts.TILE_WIDTH, offsettedGridCoords.i * Consts.TILE_WIDTH, Consts.TILE_WIDTH, Consts.TILE_WIDTH);
            }
        }

        public override void onLeftMouseUp()
        {
            if (levelEditor.selectedTool == Tool.Select)
            {
                if (levelEditor.selectedLevel == null) return;
                var dragRect = this.getDragGridRect();

                //SHIFT box selection
                if (this.isHeld(Keys.Shift))
                {
                    var lastI = Mathf.Floor(this.lastClickY / Consts.TILE_WIDTH);
                    var lastJ = Mathf.Floor(this.lastClickX / Consts.TILE_WIDTH);
                    var currentI = this.getMouseGridCoords().i;
                    var currentJ = this.getMouseGridCoords().j;
                    dragRect.topLeftGridCoords.i = Math.Min(lastI, currentI);
                    dragRect.topLeftGridCoords.j = Math.Min(lastJ, currentJ);
                    dragRect.botRightGridCoords.i = Math.Max(lastI, currentI);
                    dragRect.botRightGridCoords.j = Math.Max(lastJ, currentJ);
                }

                if (!this.isHeld(Keys.LControlKey))
                {
                    levelEditor.levelSelectedCoords = new List<GridCoords>();
                }
                for (var i = dragRect.topLeftGridCoords.i; i <= dragRect.botRightGridCoords.i; i++)
                {
                    for (var j = dragRect.topLeftGridCoords.j; j <= dragRect.botRightGridCoords.j; j++)
                    {
                        if (!_.some(levelEditor.levelSelectedCoords, (coord) => { return coord.i == i && coord.j == j; }))
                        {
                            levelEditor.levelSelectedCoords.Add(new GridCoords(i, j));
                        }
                    }
                }

                levelEditor.updateSelectionProperties();
                var elevation = levelEditor.selectedLevel.coordPropertiesGrid[this.getMouseGridCoords().i][this.getMouseGridCoords().j].elevation;
                if (!elevation) elevation = 0;
                levelEditor.selectionElevation = elevation;

                levelEditor.redrawLevelCanvas();
                levelEditor.redrawTileCanvas();
            }
            else if (levelEditor.selectedTool == Tool.SelectInstance)
            {
                if (!levelEditor.selectedLevel) return;
                var oldSelectedInstances = levelEditor.selectedInstances;
                levelEditor.selectedInstances = [];
                for(var instance in levelEditor.selectedLevel.instances)
                {
                    var rect = instance.getPositionalRect();
                    var dragRect = new Rect(this.dragLeftX, this.dragTopY, this.dragRightX, this.dragBotY);
                    if (rect.overlaps(dragRect))
                    {
                        if (oldSelectedInstances.indexOf(instance) == -1)
                        {
                            levelEditor.selectedInstances.Add(instance);
                            break;
                        }
                    }
                }
                levelEditor.levelCanvas.Invalidate();
            }
            else if (levelEditor.selectedTool == Tool.RectangleTile)
            {
                if (!levelEditor.selectedLevel) return;
                if (levelEditor.tileSelectedCoords.Count == 0) return;

                /*
                //SHIFT box selection
                if(this.isHeld(Keys.Shift) && levelEditor.levelSelectedRect) {
                  topX = levelEditor.levelSelectedRect.x1;     
                  topY = levelEditor.levelSelectedRect.y1;
                }
                */

                var tileCoordToPlace = levelEditor.tileSelectedCoords[0];
                var rect = this.getDragGridRect();
                for (var i = rect.i1; i <= rect.i2; i++)
                {
                    for (var j = rect.j1; j <= rect.j2; j++)
                    {
                        var gridCoords = new GridCoords(i, j);
                        levelEditor.selectedLevel.layers[levelEditor.layerIndex].getContext("2d").drawImage(levelEditor.selectedTileset.imgEl, tileCoordToPlace.j * Consts.TILE_WIDTH, tileCoordToPlace.i * Consts.TILE_WIDTH, Consts.TILE_WIDTH, Consts.TILE_WIDTH, gridCoords.j * Consts.TILE_WIDTH, gridCoords.i * Consts.TILE_WIDTH, Consts.TILE_WIDTH, Consts.TILE_WIDTH);
                    }
                }

                levelEditor.addUndoJson();

                levelEditor.levelCanvas.Invalidate();
                levelEditor.levelCanvas.Invalidate();
                tileCanvas.redraw();
            }
            else if (levelEditor.selectedTool == Tool.PlaceTile)
            {
                levelEditor.addUndoJson();
            }
            else if (levelEditor.selectedTool == Tool.PaintElevation)
            {
                levelEditor.addUndoJson();
            }
        }

        public override void onKeyDown(Keys keyCode, bool firstFrame)
        {
            //LEVEL HOTKEYS
            if (keyCode == Keys.Z && this.isHeld(Keys.CONTROL))
            {
                levelEditor.undo();
                return;
            }
            else if (keyCode == Keys.Y && this.isHeld(Keys.CONTROL))
            {
                levelEditor.redo();
                return;
            }

            if (keyCode == Keys.ESCAPE)
            {
                levelEditor.selectedTool = Tool.Select;
                levelEditor.clonedTiles = undefined; //getLevelSelectedGridRect();
                levelEditor.levelSelectedCoords = [];
                this.redraw();
                return;
            }

            if (keyCode == Keys.W)
            {
                if (levelEditor.selectedInstances.Count > 0)
                {
                    levelEditor.selectedInstances[0].pos.y -= 8;
                    this.redraw();
                }
            }
            else if (keyCode == Keys.S)
            {
                if (levelEditor.selectedInstances.Count > 0)
                {
                    levelEditor.selectedInstances[0].pos.y += 8;
                    this.redraw();
                }
            }
            else if (keyCode == Keys.N)
            {
                if (levelEditor.levelSelectedCoords.Count == 1)
                {
                    var gridCoords = levelEditor.levelSelectedCoords[0];
                    var neighbors = levelEditor.lastNavMeshCoords ? new List<string>() { levelEditor.lastNavMeshCoords } : new List<string>();
                    var myCoords = gridCoords.i.ToString() + "," + gridCoords.j.ToString();
                    if (levelEditor.lastNavMeshCoords != null)
                    {
                        var ni = int.Parse(levelEditor.lastNavMeshCoords.Split(',')[0]);
                        var nj = int.Parse(levelEditor.lastNavMeshCoords.Split(',')[1]);
                        levelEditor.selectedLevel.coordPropertiesGrid[ni][nj].neighbors.Add(myCoords);
                    }
                    levelEditor.selectedLevel.coordPropertiesGrid[gridCoords.i][gridCoords.j] = { "navmesh":"1","neighbors":neighbors};
                    //levelEditor.lastNavMeshCoords = myCoords;
                    this.redraw();
                }
                else if (levelEditor.levelSelectedCoords.Count == 2)
                {
                    var node1 = levelEditor.selectedLevel.coordPropertiesGrid[levelEditor.levelSelectedCoords[0].i][levelEditor.levelSelectedCoords[0].j];
                    var node2 = levelEditor.selectedLevel.coordPropertiesGrid[levelEditor.levelSelectedCoords[1].i][levelEditor.levelSelectedCoords[1].j];
                    if (!node1.neighbors) node1.neighbors = [];
                    if (!node2.neighbors) node2.neighbors = [];
                    node1.neighbors.Add(levelEditor.levelSelectedCoords[1].i.ToString() + "," + levelEditor.levelSelectedCoords[1].j.ToString());
                    node2.neighbors.Add(levelEditor.levelSelectedCoords[0].i.ToString() + "," + levelEditor.levelSelectedCoords[0].j.ToString());
                    this.redraw();
                }
            }
            else if (keyCode == Keys.F)
            {
                if (levelEditor.levelSelectedCoords.Count == 1)
                {
                    var tileHash = new Dictionary<string, TileData>();
                    foreach (var tile in levelEditor.tileDatas)
                    {
                        tileHash[tile.getId()] = tile;
                    }
                    var grid = levelEditor.selectedLevel.tileInstances;
                    var searchGrid: BFSNode[][] = Helpers.make2DArray(grid[0].Count, grid.Count, undefined);
                    for (var i = 0; i < searchGrid.Count; i++)
                    {
                        for (var j = 0; j < searchGrid[i].Count; j++)
                        {
                            searchGrid[i][j] = new BFSNode(i, j);
                        }
                    }
                    var node = searchGrid[levelEditor.levelSelectedCoords[0].i][levelEditor.levelSelectedCoords[0].j];
                    var visitedNodes = [node];
                    var loop = 0;
                    var hash: any = { };
                    while (visitedNodes.Count > 0)
                    {
                        //loop++; if(loop > 1000) { throw "INFINITE LOOP!"; }
                        var lastNode = visitedNodes.shift();
                        lastNode.visited = true;
                        var i = lastNode.i;
                        var j = lastNode.j;
                        if (hash[i + "," + j])
                        {
                            continue;
                        }
                        if (levelEditor.selectedLevel.coordPropertiesGrid[i][j].noLand)
                        {
                            continue;
                        }
                        hash[i + "," + j] = true;
                        levelEditor.selectedLevel.coordPropertiesGrid[i][j].noLand = 1;
                        try
                        {
                            var topI = i - 1;
                            var top = searchGrid[topI][j];
                            var isEmpty = tileHash[grid[topI][j]].hitboxMode == HitboxMode.None && tileHash[grid[topI][j]].zIndex == ZIndex.Default;
                            if (!top.visited && isEmpty)
                            {
                                visitedNodes.Add(top);
                            }
                        }
                        catch { }
                        try
                        {
                            var botI = i + 1;
                            var bot = searchGrid[botI][j];
                            var isEmpty = tileHash[grid[botI][j]].hitboxMode == HitboxMode.None && tileHash[grid[botI][j]].zIndex == ZIndex.Default;
                            if (!bot.visited && isEmpty)
                            {
                                visitedNodes.Add(bot);
                            }
                        }
                        catch { }
                        try
                        {
                            var leftJ = j - 1;
                            var left = searchGrid[i][leftJ];
                            var isEmpty = tileHash[grid[i][leftJ]].hitboxMode == HitboxMode.None && tileHash[grid[i][leftJ]].zIndex == ZIndex.Default;
                            if (!left.visited && isEmpty)
                            {
                                visitedNodes.Add(left);
                            }
                        }
                        catch { }
                        try
                        {
                            var rightJ = j + 1;
                            var right = searchGrid[i][rightJ];
                            var isEmpty = tileHash[grid[i][rightJ]].hitboxMode == HitboxMode.None && tileHash[grid[i][rightJ]].zIndex == ZIndex.Default;
                            if (!right.visited && isEmpty)
                            {
                                visitedNodes.Add(right);
                            }
                        }
                        catch { }
                    }
                    this.redraw();
                    resetVue();
                }

            }
            else if (keyCode == Keys.SLASH)
            {
                if (levelEditor.levelSelectedCoords.Count == 1)
                {
                    var gridCoords = levelEditor.levelSelectedCoords[0];
                    var neighbors = levelEditor.selectedLevel.coordPropertiesGrid[gridCoords.i][gridCoords.j]["neighbors"];
                    var myCoords = gridCoords.i.ToString() + "," + gridCoords.j.ToString();
                    foreach (var neighbor in neighbors)
                    {
                        var ni = int.Parse(neighbor.split(",")[0]);
                        var nj = int.Parse(neighbor.split(",")[1]);
                        _.pull(levelEditor.selectedLevel.coordPropertiesGrid[ni][nj]["neighbors"], myCoords);
                    }
                    levelEditor.selectedLevel.coordPropertiesGrid[gridCoords.i][gridCoords.j] = { };
                    levelEditor.lastNavMeshCoords = "";
                    this.redraw();
                }
            }
            else if (keyCode == Keys.BACK_QUOTE)
            {
                levelEditor.lastNavMeshCoords = "";
            }

            if (levelEditor.selectedTool == Tool.Select && firstFrame)
            {

                if (keyCode == Keys.C)
                {
                    levelEditor.selectedTool = Tool.PlaceTile;
                    levelEditor.clonedTiles = getLevelSelectedGridRect();
                    this.redraw();
                    return;
                }

                if (keyCode == Keys.L)
                {
                    var gridRect = getLevelSelectedGridRect();
                    if (gridRect)
                    {
                        var line = new Line(new Models.Point(gridRect.j1 * Consts.TILE_WIDTH, gridRect.i1 * Consts.TILE_WIDTH), new Models.Point(gridRect.j2 * Consts.TILE_WIDTH, gridRect.i2 * Consts.TILE_WIDTH));
                        if (!_.some(levelEditor.selectedLevel.scrollLines, (curLine) =>
                        {
                            return line.point1.equals(curLine.point1) && line.point2.equals(curLine.point2);
                        }))
                        {
                            levelEditor.selectedLevel.scrollLines.Add(line);
                            //levelEditor.addUndoJson();
                            this.redraw();
                        }
                    }
                }
                else if (keyCode == Keys.K)
                {
                    foreach (var line in levelEditor.selectedLevel.scrollLines)
                    {
                        Rect lineRect = null;
                        if (line.point1.x == line.point2.x)
                        {
                            lineRect = new Rect(line.point1.x - Consts.TILE_WIDTH / 2, line.point1.y, line.point1.x + Consts.TILE_WIDTH / 2, line.point2.y);
                        }
                        else if (line.point1.y == line.point2.y)
                        {
                            lineRect = new Rect(line.point1.x, line.point1.y - Consts.TILE_WIDTH / 2, line.point2.x, line.point1.y + Consts.TILE_WIDTH / 2);
                        }
                        else
                        {
                            continue;
                        }
                        var gridRect = getLevelSelectedGridRect();
                        var selectionRect = new Rect(gridRect.j1 * Consts.TILE_WIDTH, gridRect.i1 * Consts.TILE_WIDTH, (gridRect.j2 + 1) * Consts.TILE_WIDTH, (gridRect.i2 + 1) * Consts.TILE_WIDTH);
                        if (lineRect.overlaps(selectionRect))
                        {
                            _.remove(levelEditor.selectedLevel.scrollLines, line);
                            this.redraw();
                            break;
                        }
                    }
                }

                var incX = 0;
                var incY = 0;
                if (keyCode == Keys.LEFT)
                {
                    incX = -1;
                }
                else if (keyCode == Keys.RIGHT)
                {
                    incX = 1;
                }
                else if (keyCode == Keys.UP)
                {
                    incY = -1;
                }
                else if (keyCode == Keys.DOWN)
                {
                    incY = 1;
                }
                if (incX != 0 || incY != 0)
                {
                    var context = levelEditor.selectedLevel.layers[levelEditor.layerIndex].getContext("2d");
                    var savedImageDatas = [];
                    var gridCoordsMovedTo = new HashSet<string>();
                    foreach (var gridCoords in levelEditor.levelSelectedCoords)
                    {
                        var imageData = context.getImageData(gridCoords.j * Consts.TILE_WIDTH, gridCoords.i * Consts.TILE_WIDTH, Consts.TILE_WIDTH, Consts.TILE_WIDTH);
                        savedImageDatas.Add(imageData);
                    }
                    for (var i = 0; i < levelEditor.levelSelectedCoords.Count; i++)
                    {
                        var gridCoords = levelEditor.levelSelectedCoords[i];
                        context.putImageData(savedImageDatas[i], (gridCoords.j + incX) * Consts.TILE_WIDTH, (gridCoords.i + incY) * Consts.TILE_WIDTH);
                        gridCoordsMovedTo.Add((gridCoords.i + incY) + "," + (gridCoords.j + incX).ToString());
                    }
                    foreach (var gridCoords in levelEditor.levelSelectedCoords)
                    {
                        if (!gridCoordsMovedTo.Contains(gridCoords.i.ToString() + "," + gridCoords.j.ToString()))
                        {
                            context.clearRect(gridCoords.j * Consts.TILE_WIDTH, gridCoords.i * Consts.TILE_WIDTH, Consts.TILE_WIDTH, Consts.TILE_WIDTH);
                        }
                    }
                    foreach (var gridCoords in levelEditor.levelSelectedCoords)
                    {
                        gridCoords.i += incY;
                        gridCoords.j += incX;
                    }
                    levelEditor.addUndoJson();
                    levelEditor.levelCanvas.Invalidate();
                    levelEditor.levelCanvas.Invalidate();
                }
                if (keyCode == Keys.DELETE)
                {
                    var context = levelEditor.selectedLevel.layers[levelEditor.layerIndex].getContext("2d");
        for(var gridCoords in levelEditor.levelSelectedCoords)
                    {
                        context.clearRect(gridCoords.j * Consts.TILE_WIDTH, gridCoords.i * Consts.TILE_WIDTH, Consts.TILE_WIDTH, Consts.TILE_WIDTH);
                    }
                    levelEditor.addUndoJson();
                    levelEditor.levelCanvas.Invalidate();
                }
            }
            else if (levelEditor.selectedTool == Tool.SelectInstance && levelEditor.selectedInstances.Count > 0)
            {
                var incX = 0;
                var incY = 0;
                if (keyCode == Keys.LEFT)
                {
                    incX = -1;
                }
                else if (keyCode == Keys.RIGHT)
                {
                    incX = 1;
                }
                else if (keyCode == Keys.UP)
                {
                    incY = -1;
                }
                else if (keyCode == Keys.DOWN)
                {
                    incY = 1;
                }
                if (incX != 0 || incY != 0)
                {
                    foreach (var instance in levelEditor.selectedInstances)
                    {
                        instance.pos.x += incX;
                        instance.pos.y += incY;
                    }
                    levelEditor.addUndoJson();
                    levelEditor.levelCanvas.Invalidate();
                    levelEditor.levelCanvas.Invalidate();
                }
                if (keyCode == Keys.DELETE)
                {
                    foreach (var instance in levelEditor.selectedInstances)
                    {
                        _.remove(levelEditor.selectedLevel.instances, instance);
                    }
                    levelEditor.levelCanvas.Invalidate();
                    levelEditor.levelCanvas.Invalidate();
                    levelEditor.selectedInstances = [];
                    levelEditor.addUndoJson();
                }
                if (keyCode == Keys.E && levelEditor.selectedInstances.Count == 1)
                {
                    var index = levelEditor.selectedLevel.instances.IndexOf(levelEditor.selectedInstances[0]);
                    index++;
                    if (index >= levelEditor.selectedLevel.instances.Count) index = 0;
                    levelEditor.selectedInstances[0] = levelEditor.selectedLevel.instances[index];
                    resetVue();
                }
                else if (keyCode == Keys.Q && levelEditor.selectedInstances.Count == 1)
                {
                    var index = levelEditor.selectedLevel.instances.IndexOf(levelEditor.selectedInstances[0]);
                    index--;
                    if (index < 0) index = levelEditor.selectedLevel.instances.Count - 1;
                    levelEditor.selectedInstances[0] = levelEditor.selectedLevel.instances[index];
                    resetVue();
                }
            }
        }
    }
}

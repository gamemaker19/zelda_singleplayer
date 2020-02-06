using GameEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Color = System.Drawing.Color;
using Rect = GameEditor.Models.Rect;

namespace GameEditor.Editor
{
    public class BFSNode
    {
        public int i = 0;
        public int j = 0;
        public bool visited = false;
        public BFSNode(int i, int j)
        {
            this.i = i;
            this.j = j;
            this.visited = false;
        }
    }

    public class LevelCanvasUI : CanvasUI
    {
        public GridCoords prevGridCoords;
        public GridCoords lastMouseMoveGridCoords;
        public GridCoords lastMouseMoveGridCoords8x8;
        public bool mouseLeftCanvas = false;
        public LevelEditor levelEditor;

        public LevelCanvasUI(ScrollViewer panel, LevelEditor levelEditor) : base(panel, (int)panel.Width, (int)panel.Height, Color.Transparent)
        {
            this.levelEditor = levelEditor;
        }

        public override void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            base.pictureBox_Paint(sender, e);
            var graphics = e.Graphics;

            var tileWidth = 8;
            if (levelEditor.mode16x16) tileWidth = 16;

            if (levelEditor.selectedLevel != null)
            {
                foreach (var layer in levelEditor.selectedLevel.layers)
                {
                    if (layer.isHidden) continue;
                    Helpers.drawImage(graphics, layer.bitmap, 0, 0);
                }
            }

            if (levelEditor.showLevelGrid)
            {
                //Draw columns
                for (var i = 1; i < CanvasWidth / tileWidth; i++)
                {
                    Helpers.drawLine(graphics, i * tileWidth, 0, i * tileWidth, CanvasHeight, Color.Red, 1);
                }
                //Draw rows
                for (var i = 1; i < CanvasHeight / tileWidth; i++)
                {
                    Helpers.drawLine(graphics, 0, i * tileWidth, CanvasWidth, i * tileWidth, Color.Red, 1);
                }
            }
            if (levelEditor.showRoomLines)
            {
                //Draw columns
                for (var i = 1; i < CanvasWidth / 256; i++)
                {
                    Helpers.drawLine(graphics, i * 256, 0, i * 256, CanvasHeight, Color.White, 1);
                }
                //Draw rows
                for (var i = 1; i < CanvasHeight / 256; i++)
                {
                    Helpers.drawLine(graphics, 0, i * 256, CanvasWidth, i * 256, Color.White, 1);
                }
            }

            if (levelEditor.selectedLevel != null)
            {
                //Draw scroll lines
                foreach (var scrollLine in levelEditor.selectedLevel.scrollLines)
                {
                    Helpers.drawLine(graphics, scrollLine.point1.x, scrollLine.point1.y, scrollLine.point2.x, scrollLine.point2.y, Color.Yellow, 3);
                }
            }

            if (levelEditor.selectedTool == Tool.Select && levelEditor.levelSelectedCoords != null)
            {
                foreach (var point in levelEditor.levelSelectedCoords)
                {
                    Helpers.drawRect(graphics, point.getRect(), null, Color.Green, 2);
                }
            }
            if (levelEditor.selectedTool == Tool.Select || levelEditor.selectedTool == Tool.RectangleTile || levelEditor.selectedTool == Tool.SelectInstance)
            {
                if (this.mousedown)
                {
                    Helpers.drawRect(graphics, new Rect(this.dragLeftX, this.dragTopY, this.dragRightX, this.dragBotY), null, Color.Blue, 1);
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
                    Helpers.drawImage(graphics, levelEditor.selectedLayerBitmap, destPoint.j * Consts.TILE_WIDTH, destPoint.i * Consts.TILE_WIDTH, rect.x1, rect.y1, rect.w, rect.h);
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
                        Helpers.drawImage(graphics, levelEditor.selectedTileset.image, destPoint.j * Consts.TILE_WIDTH, destPoint.i * Consts.TILE_WIDTH, rect.x1, rect.y1, rect.w, rect.h);
                    }
                }
            }
            if (levelEditor.selectedTool == Tool.CreateInstance && !this.mouseLeftCanvas)
            {
                var sprite = levelEditor.sprites.Where(s => { return s.name == levelEditor.selectedObj.spriteOrImage; }).FirstOrDefault();

                float a = mouseX;
                float b = mouseY;

                if (true)
                {
                    a = (this.getMouseGridCoordsCustomWidth(Consts.TILE_WIDTH).j * Consts.TILE_WIDTH);
                    b = (this.getMouseGridCoordsCustomWidth(Consts.TILE_WIDTH).i * Consts.TILE_WIDTH);
                }

                var x = a + levelEditor.selectedObj.snapOffset.x;
                var y = b + levelEditor.selectedObj.snapOffset.y;
                sprite.draw(graphics, 0, x, y);
            }

            if (levelEditor.selectedLevel != null && levelEditor.showInstances)
            {
                foreach (SpriteInstance instance in levelEditor.selectedLevel.instances)
                {
                    instance.draw(graphics);
                }
            }

            if (levelEditor.selectedTool == Tool.SelectInstance)
            {
                foreach (var instance in levelEditor.selectedInstances)
                {
                    Helpers.drawRect(graphics, instance.getPositionalRect(), null, Color.Yellow, 3);
                }
            }
            if (!string.IsNullOrEmpty(levelEditor.showOverridesWithKey))
            {
                var grid = levelEditor.selectedLevel.coordPropertiesGrid;
                for (var i = 0; i < grid.Count; i++)
                {
                    for (var j = 0; j < grid[i].Count; j++)
                    {
                        if (!string.IsNullOrEmpty(grid[i][j][levelEditor.showOverridesWithKey]))
                        {
                            Helpers.drawRect(graphics, new GridCoords(i, j).getRect(), Color.Red, null, 0, 0.5f);
                        }
                    }
                }
            }
        }

        public GridCoords getTopLeftSelectedTileGridCoord()
        {
            return new GridCoords(levelEditor.tileSelectedCoords.Min(c => c.i), levelEditor.tileSelectedCoords.Min(c => c.j));
        }

        public override void onMouseMove(float deltaX, float deltaY)
        {
            if (this.mousedown)
            {
                if (levelEditor.selectedTool == Tool.PlaceTile)
                {
                    var topI = this.getMouseGridCoords().i;
                    var topJ = this.getMouseGridCoords().j;
                    if (this.isHeld(Key.LeftShift))
                    {
                        topI = this.prevGridCoords.i;
                    }
                    if (this.isHeld(Key.LeftCtrl))
                    {
                        topJ = this.prevGridCoords.j;
                    }
                    if (topI != this.prevGridCoords.i || topJ != this.prevGridCoords.j)
                    {
                        this.placeTile(new GridCoords(topI, topJ));
                    }
                    this.prevGridCoords = new GridCoords(topI, topJ);
                }
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
                levelEditor.resetUI();
            }
            else if (levelEditor.selectedTool == Tool.CreateInstance)
            {
                var x = this.mouseX;
                var y = this.mouseY;
                //if (levelEditor.selectedObj.snapToTile)
                //{
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
                //}
                var instance = new SpriteInstance(levelEditor.selectedObj.name, x, y, levelEditor.selectedObj, levelEditor.sprites);
                levelEditor.selectedLevel.instances.Add(instance);
                //levelEditor.selectedObj = undefined;
                //levelEditor.selectedInstances = [instance];
                //levelEditor.selectedTool = Tool.SelectInstance;
                levelEditor.addUndoJson();
                levelEditor.sortInstances();
                levelEditor.resetUI();
                this.redraw();
            }
            else if (levelEditor.selectedTool == Tool.PlaceTile)
            {
                this.prevGridCoords = this.getMouseGridCoords();
                this.placeTile(this.getMouseGridCoords());
            }
            else if (levelEditor.selectedTool == Tool.FloodfillTile)
            {
                var grid = levelEditor.selectedLevelTiles;
                for (int i = 0; i < grid.Count; i++)
                {
                    for (int j = 0; j < grid[i].Count; j++)
                    {
                        if (string.IsNullOrEmpty(grid[i][j]))
                        {
                            var tile = levelEditor.getTileSelectedTiles()[0];
                            grid[i][j] = tile.getId();

                            using (Graphics canvas = Graphics.FromImage(levelEditor.selectedLayerBitmap))
                            {
                                var rect = tile.gridCoords.getRect();
                                Helpers.drawImage(canvas, levelEditor.selectedTileset.image, j * Consts.TILE_WIDTH, i * Consts.TILE_WIDTH, rect.x1, rect.y1, rect.w, rect.h);
                            }
                        }
                    }
                }
                this.redraw();
            }
        }

        public void placeTile(GridCoords gridCoords)
        {
            if (levelEditor.clonedTiles != null)
            {
                var rect = levelEditor.clonedTiles.getRect();
                var destPoint = gridCoords;
                if (levelEditor.mode16x16 && this.mouseY % 16 >= 8)
                {
                    destPoint.i--;
                }
                if (levelEditor.mode16x16 && this.mouseX % 16 >= 8)
                {
                    destPoint.j--;
                }
                using (Graphics canvas = Graphics.FromImage(levelEditor.selectedLayerBitmap))
                {
                    Helpers.drawImage(canvas, levelEditor.selectedLayerBitmap, destPoint.j * Consts.TILE_WIDTH, destPoint.i * Consts.TILE_WIDTH, rect.x1, rect.y1, rect.w, rect.h);
                }

                for (int i = levelEditor.clonedTiles.i1, iDiff = 0; i <= levelEditor.clonedTiles.i2; i++, iDiff++)
                {
                    for (int j = levelEditor.clonedTiles.j1, jDiff = 0; j <= levelEditor.clonedTiles.j2; j++, jDiff++)
                    {
                        try
                        {
                            levelEditor.selectedLevelTiles[destPoint.i + iDiff][destPoint.j + jDiff] =
                                levelEditor.selectedLevelTiles[i][j];
                        }
                        catch (ArgumentOutOfRangeException) { }
                    }
                }

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
                using (Graphics canvas = Graphics.FromImage(levelEditor.selectedLayerBitmap))
                {
                    Helpers.drawImage(canvas, levelEditor.selectedTileset.image, offsettedGridCoords.j * Consts.TILE_WIDTH, offsettedGridCoords.i * Consts.TILE_WIDTH, selectedGridCoord.j * Consts.TILE_WIDTH, selectedGridCoord.i * Consts.TILE_WIDTH, Consts.TILE_WIDTH, Consts.TILE_WIDTH);
                    placeTileInLevel(offsettedGridCoords, selectedGridCoord, levelEditor.selectedTileset);
                }
            }
        }

        public void placeTileInLevel(GridCoords levelCoords, GridCoords tileCoords, Spritesheet tileset)
        {
            try
            {
                levelEditor.selectedLevelTiles[levelCoords.i][levelCoords.j] = TileData.createId(tileset, tileCoords);
            }
            catch (ArgumentOutOfRangeException) { }
        }

        public override void onLeftMouseUp()
        {
            if (levelEditor.selectedTool == Tool.Select)
            {
                if (levelEditor.selectedLevel == null) return;
                var dragRect = this.getDragGridRect();

                //SHIFT box selection
                if (this.isHeld(Key.LeftShift))
                {
                    var lastI = Mathf.Floor(this.lastClickY / Consts.TILE_WIDTH);
                    var lastJ = Mathf.Floor(this.lastClickX / Consts.TILE_WIDTH);
                    var currentI = this.getMouseGridCoords().i;
                    var currentJ = this.getMouseGridCoords().j;
                    dragRect.topLeftGridCoords.i = (int)Math.Min(lastI, currentI);
                    dragRect.topLeftGridCoords.j = (int)Math.Min(lastJ, currentJ);
                    dragRect.botRightGridCoords.i = (int)Math.Max(lastI, currentI);
                    dragRect.botRightGridCoords.j = (int)Math.Max(lastJ, currentJ);
                }

                if (!this.isHeld(Key.LeftCtrl))
                {
                    levelEditor.levelSelectedCoords = new ObservableCollection<GridCoords>();
                }
                for (var i = dragRect.topLeftGridCoords.i; i <= dragRect.botRightGridCoords.i; i++)
                {
                    for (var j = dragRect.topLeftGridCoords.j; j <= dragRect.botRightGridCoords.j; j++)
                    {
                        if (!levelEditor.levelSelectedCoords.Any((coord) => { return coord.i == i && coord.j == j; }))
                        {
                            levelEditor.levelSelectedCoords.Add(new GridCoords(i, j));
                        }
                    }
                }

                levelEditor.resetUI();

                levelEditor.redrawLevelCanvas();
                levelEditor.redrawTileCanvas();
            }
            else if (levelEditor.selectedTool == Tool.SelectInstance)
            {
                if (levelEditor.selectedLevel == null) return;
                var oldSelectedInstances = levelEditor.selectedInstances;
                levelEditor.clearSelectedInstances();
                foreach (var instance in levelEditor.selectedLevel.instances)
                {
                    var rect = instance.getPositionalRect();
                    var dragRect = new Rect(this.dragLeftX, this.dragTopY, this.dragRightX, this.dragBotY);
                    if (rect.overlaps(dragRect))
                    {
                        if (oldSelectedInstances.IndexOf(instance) == -1)
                        {
                            levelEditor.addSelectedInstance(instance);
                            break;
                        }
                    }
                }
                levelEditor.resetUI();
                levelEditor.redrawLevelCanvas();
            }
            else if (levelEditor.selectedTool == Tool.RectangleTile)
            {
                if (levelEditor.selectedLevel == null) return;
                if (levelEditor.tileSelectedCoords.Count == 0) return;

                GridCoords tileCoordToPlace = levelEditor.tileSelectedCoords[0];
                var tileset = levelEditor.selectedTileset;
                /*
                if (levelEditor.clonedTiles != null)
                {
                    string tileId = levelEditor.selectedLevelTiles[levelEditor.clonedTiles.i1][levelEditor.clonedTiles.j1];
                    if(!string.IsNullOrEmpty(tileId))
                    {
                        TileData.getPieces(tileId, out var tilesetName, out tileCoordToPlace);
                        tileset = levelEditor.getTilesetWithName(tilesetName);
                    }
                }
                */

                var rect = this.getDragGridRect();
                for (var i = rect.i1; i <= rect.i2; i++)
                {
                    for (var j = rect.j1; j <= rect.j2; j++)
                    {
                        var gridCoords = new GridCoords((int)i, (int)j);
                        using (Graphics canvas = Graphics.FromImage(levelEditor.selectedLayerBitmap))
                        {
                            int w = Consts.TILE_WIDTH;
                            Helpers.drawImage(canvas, levelEditor.selectedTileset.image, gridCoords.j * w, gridCoords.i * w, tileCoordToPlace.j * w, tileCoordToPlace.i * w, w, w);
                            placeTileInLevel(gridCoords, tileCoordToPlace, tileset);
                        }
                    }
                }

                levelEditor.addUndoJson();

                levelEditor.redrawLevelCanvas();
                levelEditor.redrawTileCanvas();
            }
            else if (levelEditor.selectedTool == Tool.PlaceTile)
            {
                levelEditor.addUndoJson();
            }
        }

        public override void onKeyDown(Key keyCode, bool firstFrame)
        {
            //LEVEL HOTKEYS
            if (keyCode == Key.Z && this.isHeld(Key.LeftCtrl))
            {
                levelEditor.undo();
                return;
            }
            else if (keyCode == Key.Y && this.isHeld(Key.LeftCtrl))
            {
                levelEditor.redo();
                return;
            }

            if (keyCode == Key.S)
            {
                levelEditor.selectedTool = Tool.Select;
                levelEditor.clonedTiles = null; //getLevelSelectedGridRect();
                levelEditor.levelSelectedCoords = new ObservableCollection<GridCoords>();
                this.redraw();
                return;
            }

            if (keyCode == Key.R)
            {
                levelEditor.selectedTool = Tool.RectangleTile;
                levelEditor.clonedTiles = null; //getLevelSelectedGridRect();
                levelEditor.levelSelectedCoords = new ObservableCollection<GridCoords>();
                this.redraw();
                return;
            }

            if (keyCode == Key.I)
            {
                levelEditor.selectedTool = Tool.SelectInstance;
                levelEditor.clonedTiles = null; //getLevelSelectedGridRect();
                levelEditor.levelSelectedCoords = new ObservableCollection<GridCoords>();
                this.redraw();
                return;
            }

            if (keyCode == Key.W)
            {
                if (levelEditor.selectedInstances.Count > 0)
                {
                    levelEditor.selectedInstances[0].pos.y -= 8;
                    this.redraw();
                }
            }
            else if (keyCode == Key.S)
            {
                if (levelEditor.selectedInstances.Count > 0)
                {
                    levelEditor.selectedInstances[0].pos.y += 8;
                    this.redraw();
                }
            }
            else if (keyCode == Key.F)
            {
                if (levelEditor.levelSelectedCoords.Count == 1)
                {
                    var tileHash = new Dictionary<string, TileData>();
                    foreach (var tile in levelEditor.tileDatas)
                    {
                        tileHash[tile.getId()] = tile;
                    }
                    var grid = levelEditor.selectedLevelTiles;
                    List<List<BFSNode>> searchGrid = Helpers.make2DArray<BFSNode>(grid[0].Count, grid.Count, null);
                    for (var i = 0; i < searchGrid.Count; i++)
                    {
                        for (var j = 0; j < searchGrid[i].Count; j++)
                        {
                            searchGrid[i][j] = new BFSNode(i, j);
                        }
                    }
                    var node = searchGrid[levelEditor.levelSelectedCoords[0].i][levelEditor.levelSelectedCoords[0].j];
                    var visitedNodes = new List<BFSNode>() { node };
                    var loop = 0;
                    var hash = new Dictionary<string, bool>();
                    while (visitedNodes.Count > 0)
                    {
                        //loop++; if(loop > 1000) { throw "INFINITE LOOP!"; }
                        var lastNode = visitedNodes[0];
                        visitedNodes.RemoveAt(0);
                        lastNode.visited = true;
                        var i = lastNode.i;
                        var j = lastNode.j;
                        if (hash.ContainsKey(i.ToString() + "," + j.ToString()))
                        {
                            continue;
                        }
                        if (levelEditor.selectedLevel.coordPropertiesGrid[i][j].ContainsKey("noLand"))
                        {
                            continue;
                        }
                        hash[i + "," + j] = true;
                        levelEditor.selectedLevel.coordPropertiesGrid[i][j]["noLand"] = "1";
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
                }

            }
            if (levelEditor.selectedTool == Tool.Select && firstFrame)
            {
                if (keyCode == Key.C)
                {
                    levelEditor.selectedTool = Tool.PlaceTile;
                    levelEditor.clonedTiles = levelEditor.getLevelSelectedGridRect();
                    this.redraw();
                    return;
                }

                if (keyCode == Key.L)
                {
                    var gridRect = levelEditor.getLevelSelectedGridRect();
                    if (gridRect != null)
                    {
                        var line = new Line(new Models.Point(gridRect.j1 * Consts.TILE_WIDTH, gridRect.i1 * Consts.TILE_WIDTH), new Models.Point(gridRect.j2 * Consts.TILE_WIDTH, gridRect.i2 * Consts.TILE_WIDTH));
                        if (!levelEditor.selectedLevel.scrollLines.Any((curLine) =>
                       {
                           return line.point1.Equals(curLine.point1) && line.point2.Equals(curLine.point2);
                       }))
                        {
                            levelEditor.selectedLevel.scrollLines.Add(line);
                            levelEditor.addUndoJson();
                            this.redraw();
                        }
                    }
                }
                else if (keyCode == Key.K)
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
                        var gridRect = levelEditor.getLevelSelectedGridRect();
                        var selectionRect = new Rect(gridRect.j1 * Consts.TILE_WIDTH, gridRect.i1 * Consts.TILE_WIDTH, (gridRect.j2 + 1) * Consts.TILE_WIDTH, (gridRect.i2 + 1) * Consts.TILE_WIDTH);
                        if (lineRect.overlaps(selectionRect))
                        {
                            levelEditor.selectedLevel.scrollLines.Remove(line);
                            this.redraw();
                            break;
                        }
                    }
                }

                int leftResize = 0;
                int rightResize = 0;
                int topResize = 0;
                int botResize = 0;
                if (keyCode == Key.NumPad1)
                {
                    leftResize = 1;
                    botResize = 1;
                }
                else if (keyCode == Key.NumPad2)
                {
                    botResize = 1;
                }
                else if (keyCode == Key.NumPad3)
                {
                    rightResize = 1;
                    botResize = 1;
                }
                else if (keyCode == Key.NumPad4)
                {
                    leftResize = 1;
                }
                else if (keyCode == Key.NumPad6)
                {
                    rightResize = 1;
                }
                else if (keyCode == Key.NumPad7)
                {
                    leftResize = 1;
                    topResize = 1;
                }
                else if (keyCode == Key.NumPad8)
                {
                    topResize = 1;
                }
                else if (keyCode == Key.NumPad9)
                {
                    topResize = 1;
                    rightResize = 1;
                }

                if (leftResize != 0 || rightResize != 0 || topResize != 0 || botResize != 0)
                {
                    levelEditor.selectedLevel.resize(leftResize, rightResize, topResize, botResize);
                    setSize(levelEditor.selectedLevel.width * Consts.TILE_WIDTH, levelEditor.selectedLevel.height * Consts.TILE_WIDTH);
                    levelEditor.redrawLevelCanvas();
                    levelEditor.resetUI();
                }

                if (keyCode == Key.Delete)
                {
                    using (Graphics canvas = Graphics.FromImage(levelEditor.selectedLayerBitmap))
                    {
                        foreach (var gridCoords in levelEditor.levelSelectedCoords)
                        {
                            Helpers.drawRect(canvas, Rect.CreateWH(gridCoords.j * Consts.TILE_WIDTH, gridCoords.i * Consts.TILE_WIDTH, Consts.TILE_WIDTH, Consts.TILE_WIDTH), Color.Transparent);
                            levelEditor.selectedLevelTiles[gridCoords.i][gridCoords.j] = "";
                        }
                    }

                    levelEditor.selectedLevel.redrawLayer(levelEditor.selectedLayer, levelEditor.levelSelectedCoords);

                    levelEditor.addUndoJson();
                    levelEditor.redrawLevelCanvas();
                }
            }
            if (levelEditor.selectedTool == Tool.Select)
            {
                var incX = 0;
                var incY = 0;

                if (keyCode == Key.Left)
                {
                    incX = -1;
                }
                else if (keyCode == Key.Right)
                {
                    incX = 1;
                }
                else if (keyCode == Key.Up)
                {
                    incY = -1;
                }
                else if (keyCode == Key.Down)
                {
                    incY = 1;
                }

                bool resizeModifierHeld = (this.isHeld(Key.LeftCtrl) || this.isHeld(Key.LeftShift) || this.isHeld(Key.RightCtrl) || this.isHeld(Key.RightShift));
                if ((incX != 0 || incY != 0) && !resizeModifierHeld)
                {
                    using (Graphics canvas = Graphics.FromImage(levelEditor.selectedLayerBitmap))
                    {
                        var savedTiles = new Dictionary<string, string>();
                        foreach (var coord in levelEditor.levelSelectedCoords)
                        {
                            savedTiles[coord.ToString()] = levelEditor.selectedLevelTiles[coord.i][coord.j];
                            levelEditor.selectedLevelTiles[coord.i][coord.j] = levelEditor.selectedLevel.defaultTileId;

                            if (!string.IsNullOrEmpty(levelEditor.selectedLevel.defaultTileId))
                            {
                                TileData.getPieces(levelEditor.selectedLevel.defaultTileId, out var tilesetName, out var savedTileCoords);
                                var tileset = levelEditor.getTilesetWithName(tilesetName);
                                tileset.init(false);
                                Helpers.drawImage(canvas, tileset.image, (coord.j) * Consts.TILE_WIDTH, (coord.i) * Consts.TILE_WIDTH, savedTileCoords.j * Consts.TILE_WIDTH, savedTileCoords.i * Consts.TILE_WIDTH, Consts.TILE_WIDTH, Consts.TILE_WIDTH);
                            }
                            else
                            {
                                Helpers.drawRect(canvas, Rect.CreateWH(coord.j * Consts.TILE_WIDTH, coord.i * Consts.TILE_WIDTH, Consts.TILE_WIDTH, Consts.TILE_WIDTH), Color.Transparent);
                            }
                        }
                        foreach (var coord in levelEditor.levelSelectedCoords)
                        {
                            string savedTileId = savedTiles[coord.ToString()];
                            levelEditor.selectedLevelTiles[coord.i + incY][coord.j + incX] = savedTileId;
                            if (!string.IsNullOrEmpty(savedTileId))
                            {
                                TileData.getPieces(savedTileId, out var tilesetName, out var savedTileCoords);
                                var tileset = levelEditor.getTilesetWithName(tilesetName);
                                tileset.init(false);
                                Helpers.drawImage(canvas, tileset.image, (coord.j + incX) * Consts.TILE_WIDTH, (coord.i + incY) * Consts.TILE_WIDTH, savedTileCoords.j * Consts.TILE_WIDTH, savedTileCoords.i * Consts.TILE_WIDTH, Consts.TILE_WIDTH, Consts.TILE_WIDTH);
                            }
                        }
                        foreach (var coord in levelEditor.levelSelectedCoords)
                        {
                            coord.i += incY;
                            coord.j += incX;
                        }
                        //levelEditor.selectedLevel.redrawLayer(levelEditor.selectedLayer, levelEditor.tileDataGrids);
                        levelEditor.addUndoJson();
                        levelEditor.redrawLevelCanvas();
                    }
                }
                else if ((incX != 0 || incY != 0) && resizeModifierHeld)
                {
                    //Code to resize selection here
                    GridRect selectionRect = levelEditor.getLevelSelectedGridRect();
                    if (selectionRect == null) return;
                    var gridCoordsToRedraw = new List<GridCoords>();

                    if (incX < 0)
                    {
                        for (int i = selectionRect.i1; i <= selectionRect.i2; i++)
                        {
                            var gridCoordToAdd = new GridCoords(i, selectionRect.j1 - 1);
                            if (gridCoordToAdd.outOfBounds(levelEditor.selectedLevel.width, levelEditor.selectedLevel.height)) continue;
                            gridCoordsToRedraw.Add(gridCoordToAdd);
                            levelEditor.levelSelectedCoords.Add(gridCoordToAdd);
                            levelEditor.selectedLevelTiles[gridCoordToAdd.i][gridCoordToAdd.j] =
                                levelEditor.selectedLevelTiles[i][selectionRect.j1];
                        }
                    }
                    else if (incX > 0)
                    {
                        for (int i = selectionRect.i1; i <= selectionRect.i2; i++)
                        {
                            var gridCoordToAdd = new GridCoords(i, selectionRect.j2 + 1);
                            if (gridCoordToAdd.outOfBounds(levelEditor.selectedLevel.width, levelEditor.selectedLevel.height)) continue;
                            gridCoordsToRedraw.Add(gridCoordToAdd);
                            levelEditor.levelSelectedCoords.Add(gridCoordToAdd);
                            levelEditor.selectedLevelTiles[gridCoordToAdd.i][gridCoordToAdd.j] =
                                levelEditor.selectedLevelTiles[i][selectionRect.j2];
                        }
                    }
                    else if (incY < 0)
                    {
                        for (int j = selectionRect.j1; j <= selectionRect.j2; j++)
                        {
                            var gridCoordToAdd = new GridCoords(selectionRect.i1 - 1, j);
                            if (gridCoordToAdd.outOfBounds(levelEditor.selectedLevel.width, levelEditor.selectedLevel.height)) continue;
                            gridCoordsToRedraw.Add(gridCoordToAdd);
                            levelEditor.levelSelectedCoords.Add(gridCoordToAdd);
                            levelEditor.selectedLevelTiles[gridCoordToAdd.i][gridCoordToAdd.j] =
                                levelEditor.selectedLevelTiles[selectionRect.i1][j];
                        }
                    }
                    else if (incY > 0)
                    {
                        for (int j = selectionRect.j1; j <= selectionRect.j2; j++)
                        {
                            var gridCoordToAdd = new GridCoords(selectionRect.i2 + 1, j);
                            if (gridCoordToAdd.outOfBounds(levelEditor.selectedLevel.width, levelEditor.selectedLevel.height)) continue;
                            gridCoordsToRedraw.Add(gridCoordToAdd);
                            levelEditor.levelSelectedCoords.Add(gridCoordToAdd);
                            levelEditor.selectedLevelTiles[gridCoordToAdd.i][gridCoordToAdd.j] =
                                levelEditor.selectedLevelTiles[selectionRect.i2][j];
                        }
                    }

                    levelEditor.selectedLevel.redrawLayer(levelEditor.selectedLayer, gridCoordsToRedraw);
                    levelEditor.addUndoJson();
                    levelEditor.redrawLevelCanvas();
                }
            }

            if (levelEditor.selectedTool == Tool.SelectInstance && levelEditor.selectedInstances.Count > 0 && firstFrame)
            {
                var incX = 0;
                var incY = 0;
                if (keyCode == Key.Left)
                {
                    incX = -1;
                }
                else if (keyCode == Key.Right)
                {
                    incX = 1;
                }
                else if (keyCode == Key.Up)
                {
                    incY = -1;
                }
                else if (keyCode == Key.Down)
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
                    levelEditor.redrawLevelCanvas();
                }
                if (keyCode == Key.Delete)
                {
                    foreach (var instance in levelEditor.selectedInstances)
                    {
                        levelEditor.selectedLevel.instances.Remove(instance);
                    }
                    levelEditor.redrawLevelCanvas();
                    levelEditor.resetUI();
                    levelEditor.clearSelectedInstances();
                    levelEditor.addUndoJson();
                }
                if (keyCode == Key.E && levelEditor.selectedInstances.Count == 1)
                {
                    var index = levelEditor.selectedLevel.instances.IndexOf(levelEditor.selectedInstances[0]);
                    index++;
                    if (index >= levelEditor.selectedLevel.instances.Count) index = 0;
                    levelEditor.selectedInstances[0] = levelEditor.selectedLevel.instances[index];
                    levelEditor.resetUI();
                }
                else if (keyCode == Key.Q && levelEditor.selectedInstances.Count == 1)
                {
                    var index = levelEditor.selectedLevel.instances.IndexOf(levelEditor.selectedInstances[0]);
                    index--;
                    if (index < 0) index = levelEditor.selectedLevel.instances.Count - 1;
                    levelEditor.selectedInstances[0] = levelEditor.selectedLevel.instances[index];
                    levelEditor.resetUI();
                }
            }
        }
    }
}

using LevelEditor_CS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using static LevelEditor_CS.LevelEditor;
using Color = System.Drawing.Color;
using Point = LevelEditor_CS.Models.Point;

namespace LevelEditor_CS.Editor
{
    public class TileCanvasUI : CanvasUI
    {
        public LevelEditor levelEditor;

        public TileCanvasUI(PictureBox pictureBox, Panel panel, LevelEditor levelEditor) : base(pictureBox, panel, panel.Width, panel.Height, Color.Transparent)
        {
            this.levelEditor = levelEditor;
        }

        public override void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            base.pictureBox_Paint(sender, e);

            if (levelEditor.selectedLevel == null) return;
            if (levelEditor.selectedTileset == null) return;

            if (levelEditor.selectedTileset != null && levelEditor.selectedTileset.image != null)
            {
                e.Graphics.DrawImage(levelEditor.selectedTileset.image, 0, 0);
            }

            var widthFactor = 1;
            if (levelEditor.mode16x16) widthFactor = 2;

            //Draw columns
            for (var i = 1; i < CanvasWidth / (Consts.TILE_WIDTH * widthFactor); i++)
            {
                Helpers.drawLine(e.Graphics, (i) * (Consts.TILE_WIDTH * widthFactor), 0, (i) * (Consts.TILE_WIDTH * widthFactor), CanvasHeight, Color.Red, 1);
            }

            //Draw rows
            for (var i = 1; i < CanvasHeight / (Consts.TILE_WIDTH * widthFactor); i++)
            {
                Helpers.drawLine(e.Graphics, 0, (i) * (Consts.TILE_WIDTH * widthFactor), CanvasWidth, (i) * (Consts.TILE_WIDTH * widthFactor), Color.Red, 1);
            }

            if (!levelEditor.mode16x16)
            {
                foreach (var gridCoords in levelEditor.tileSelectedCoords)
                {
                    Helpers.drawRect(e.Graphics, gridCoords.getRect(), null, Color.Green, 2);
                }
            }
            else
            {
                foreach (var gridCoords in levelEditor.tileSelectedCoords)
                {
                    if (gridCoords.i % 2 != 0 || gridCoords.j % 2 != 0) continue;
                    var rect = gridCoords.getRect();
                    rect.botRightPoint.x += 8;
                    rect.botRightPoint.y += 8;
                    Helpers.drawRect(e.Graphics, rect, null, Color.Green, 2);
                }
            }

            if (this.mousedown)
            {
                Helpers.drawRect(e.Graphics, new Rect(this.dragLeftX, this.dragTopY, this.dragRightX, this.dragBotY), null, Color.Blue, 1);
            }

            redrawUICanvas(sender, e);
        }

        public void redrawUICanvas(object sender, PaintEventArgs e)
        {
            if (levelEditor.showTileHitboxes)
            {
                for (var i = 0; i < CanvasHeight / 8; i++)
                {
                    for (var j = 0; j < CanvasWidth / 8; j++)
                    {
                        var tileData = levelEditor.getTileGrid()[i][j];
                        if (tileData == null) continue;
                        if (tileData.hitboxMode == HitboxMode.Tile)
                        {
                            Helpers.drawRect(e.Graphics, new GridCoords(i, j).getRect(), Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.BoundingRect)
                        {
                            Helpers.drawRect(e.Graphics, new GridCoords(i, j).getRect(), Color.Orange, null, 0, 0.5f);
                        }
                        var x = j * Consts.TILE_WIDTH;
                        var y = i * Consts.TILE_WIDTH;
                        var x2 = (j + 1) * Consts.TILE_WIDTH;
                        var y2 = (i + 1) * Consts.TILE_WIDTH;
                        var topLeftPt = new Point(x, y);
                        var topRightPt = new Point(x2, y);
                        var botLeftPt = new Point(x, y2);
                        var botRightPt = new Point(x2, y2);
                        var xMid = x + Consts.TILE_WIDTH / 2;
                        var yMid = y + Consts.TILE_WIDTH / 2;

                        if (tileData.hitboxMode == HitboxMode.DiagBotLeft)
                        {
                            Helpers.drawPolygon(e.Graphics, new Shape(new List<Point>() { topLeftPt, botRightPt, botLeftPt }), true, Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.DiagBotRight)
                        {
                            Helpers.drawPolygon(e.Graphics, new Shape(new List<Point>() { botLeftPt, botRightPt, topRightPt }), true, Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.DiagTopLeft)
                        {
                            Helpers.drawPolygon(e.Graphics, new Shape(new List<Point>() { topLeftPt, topRightPt, botLeftPt }), true, Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.DiagTopRight)
                        {
                            Helpers.drawPolygon(e.Graphics, new Shape(new List<Point>() { topLeftPt, topRightPt, botRightPt }), true, Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.BoxTopLeft)
                        {
                            Helpers.drawRect(e.Graphics, new Rect(x, y, xMid, yMid), Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.BoxTopRight)
                        {
                            Helpers.drawRect(e.Graphics, new Rect(xMid, y, x2, yMid), Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.BoxBotLeft)
                        {
                            Helpers.drawRect(e.Graphics, new Rect(x, yMid, xMid, y2), Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.BoxBotRight)
                        {
                            Helpers.drawRect(e.Graphics, new Rect(xMid, yMid, x2, y2), Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.BoxTop)
                        {
                            Helpers.drawRect(e.Graphics, new Rect(x, y, x2, yMid), Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.BoxBot)
                        {
                            Helpers.drawRect(e.Graphics, new Rect(x, yMid, x2, y2), Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.BoxLeft)
                        {
                            Helpers.drawRect(e.Graphics, new Rect(x, y, xMid, y2), Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.BoxRight)
                        {
                            Helpers.drawRect(e.Graphics, new Rect(xMid, y, x2, y2), Color.Blue, null, 0, 0.5f);
                        }

                        else if (tileData.hitboxMode == HitboxMode.SmallDiagTopLeft)
                        {
                            Helpers.drawPolygon(e.Graphics, new Shape(new List<Point>() { topLeftPt, topLeftPt.addxy(Consts.TILE_WIDTH / 2, 0), topLeftPt.addxy(0, Consts.TILE_WIDTH / 2) }), true, Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.SmallDiagTopRight)
                        {
                            Helpers.drawPolygon(e.Graphics, new Shape(new List<Point>() { topRightPt, topRightPt.addxy(-Consts.TILE_WIDTH / 2, 0), topRightPt.addxy(0, Consts.TILE_WIDTH / 2) }), true, Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.SmallDiagBotLeft)
                        {
                            Helpers.drawPolygon(e.Graphics, new Shape(new List<Point>() { botLeftPt, botLeftPt.addxy(Consts.TILE_WIDTH / 2, 0), botLeftPt.addxy(0, -Consts.TILE_WIDTH / 2) }), true, Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.SmallDiagBotRight)
                        {
                            Helpers.drawPolygon(e.Graphics, new Shape(new List<Point>() { botRightPt, botRightPt.addxy(-Consts.TILE_WIDTH / 2, 0), botRightPt.addxy(0, -Consts.TILE_WIDTH / 2) }), true, Color.Blue, null, 0, 0.5f);
                        }

                        else if (tileData.hitboxMode == HitboxMode.LargeDiagTopLeft)
                        {
                            Helpers.drawPolygon(e.Graphics, new Shape(new List<Point>() { topLeftPt, topRightPt, botRightPt.addxy(0, -Consts.TILE_WIDTH / 2), botRightPt.addxy(-Consts.TILE_WIDTH / 2, 0), botLeftPt }), true, Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.LargeDiagTopRight)
                        {
                            Helpers.drawPolygon(e.Graphics, new Shape(new List<Point>() { topLeftPt, topRightPt, botRightPt, botLeftPt.addxy(Consts.TILE_WIDTH / 2, 0), botLeftPt.addxy(0, -Consts.TILE_WIDTH / 2) }), true, Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.LargeDiagBotLeft)
                        {
                            Helpers.drawPolygon(e.Graphics, new Shape(new List<Point>() { topLeftPt, topRightPt.addxy(-Consts.TILE_WIDTH / 2, 0), topRightPt.addxy(0, Consts.TILE_WIDTH / 2), botRightPt, botLeftPt }), true, Color.Blue, null, 0, 0.5f);
                        }
                        else if (tileData.hitboxMode == HitboxMode.LargeDiagBotRight)
                        {
                            Helpers.drawPolygon(e.Graphics, new Shape(new List<Point>() { topLeftPt.addxy(Consts.TILE_WIDTH / 2, 0), topRightPt, botRightPt, botLeftPt, topLeftPt.addxy(0, Consts.TILE_WIDTH / 2) }), true, Color.Blue, null, 0, 0.5f);
                        }

                        else if (tileData.hitboxMode == HitboxMode.Custom)
                        {
                            var pts = new List<Models.Point>();
                            var customHitboxPoints = tileData.customHitboxPoints.ToString();
                            for (var k = 0; k < customHitboxPoints.Length; k++)
                            {
                                var chr = customHitboxPoints[k];
                                Point point = null;
                                if (chr == '0') point = topLeftPt;
                                if (chr == '1') point = topLeftPt.addxy(Consts.TILE_WIDTH / 2, 0);
                                if (chr == '2') point = topRightPt;
                                if (chr == '3') point = topLeftPt.addxy(0, Consts.TILE_WIDTH / 2);
                                if (chr == '4') point = topLeftPt.addxy(Consts.TILE_WIDTH / 2, Consts.TILE_WIDTH / 2);
                                if (chr == '5') point = topLeftPt.addxy(Consts.TILE_WIDTH, Consts.TILE_WIDTH / 2);
                                if (chr == '6') point = botLeftPt;
                                if (chr == '7') point = botLeftPt.addxy(Consts.TILE_WIDTH / 2, 0);
                                if (chr == '8') point = botRightPt;
                                pts.Add(point);
                            }
                            if (pts.Count > 2)
                            {
                                Helpers.drawPolygon(e.Graphics, new Shape(pts), true, Color.Blue, null, 0, 0.5f);
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(levelEditor.showTilesWithTag))
            {
                for (var i = 0; i < CanvasHeight / 8; i++)
                {
                    for (var j = 0; j < CanvasWidth / 8; j++)
                    {
                        var tileData = levelEditor.getTileGrid()[i][j];
                        var gridCoords = new GridCoords(i, j);
                        if (tileData != null && tileData.hasTag(levelEditor.showTilesWithTag))
                        {
                            Helpers.drawRect(e.Graphics, gridCoords.getRect(), Color.Red, null, 0, 0.5f);
                        }
                    }
                }
            }
            if (levelEditor.showTilesWithZIndex1)
            {
                for (var i = 0; i < CanvasHeight / 8; i++)
                {
                    for (var j = 0; j < CanvasWidth / 8; j++)
                    {
                        var tileData = levelEditor.getTileGrid()[i][j];
                        var gridCoords = new GridCoords(i, j);
                        if (tileData != null && tileData.zIndex == ZIndex.Foreground1)
                        {
                            Helpers.drawRect(e.Graphics, gridCoords.getRect(), Color.Red, null, 0, 0.5f);
                        }
                    }
                }
            }

            /*
            for(var i = 0; i < CanvasHeight / 8; i++) {
              for(var j = 0; j < CanvasWidth / 8; j++) {
                var tileData = levelEditor.getTileGrid()[i][j];
                var gridCoords = new GridCoords(i, j);
                if(tileData.spriteName.startsWith("TileWaterEdge") || tileData.spriteName.startsWith("TileOWaterEdge")) {
                  Helpers.drawRect(e.Graphics, gridCoords.getRect(), Color.Red, null, 0, 0.5f);
                }
              }
            }
            */

            /*
            if(levelEditor.tileSelectedCoords.length > 0) {
              for(var i = 0; i < CanvasHeight / 8; i++) {
                for(var j = 0; j < CanvasWidth / 8; j++) {
                  var tileGrid = levelEditor.getTileGrid();
                  if(tileGrid[levelEditor.tileSelectedCoords[0].i][levelEditor.tileSelectedCoords[0].j] == tileGrid[i][j]) {
                    Helpers.drawRect(e.Graphics, new GridCoords(i, j).getRect(), Color.Red, null, 0, 0.5f);
                  }
                }
              }
            }
            */
        }

        public override void onLeftMouseDown()
        {
        }

        public override void onLeftMouseUp()
        {
            if (levelEditor.selectedLevel == null) return;
            if (levelEditor.selectedTileset == null) return;

            var dragRect = this.getDragGridRect();

            if (levelEditor.mode16x16)
            {
                if (dragRect.i1 % 2 != 0) dragRect.topLeftGridCoords.i--;
                if (dragRect.j1 % 2 != 0) dragRect.topLeftGridCoords.j--;
                if (dragRect.i2 % 2 == 0) dragRect.botRightGridCoords.i++;
                if (dragRect.j2 % 2 == 0) dragRect.botRightGridCoords.j++;
            }

            //SHIFT box selection
            if (this.isHeld(Keys.Shift))
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

            /*
            //ALT selects all tiles in a clump or animation
            if(this.isHeld(Keys.ALT)) {
              var tile = _.find(levelEditor.tileDatas, (tile: TileData) => {
                return tile.rect.x2 == botX && tile.rect.y2 == botY;
              });
              if(tile) {
                var clump = _.find(levelEditor.tileClumps, (tileClump: TileClump) => {
                  return tileClump.tiles.indexOf(tile) > -1;
                });
                var anim = _.find(levelEditor.tileAnimations, (tileAnimation: TileAnimation) => {
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

            if (!this.isHeld(Keys.Control))
            {
                levelEditor.tileSelectedCoords = new ObservableCollection<GridCoords>();
            }
            for (var i = dragRect.topLeftGridCoords.i; i <= dragRect.botRightGridCoords.i; i++)
            {
                for (var j = dragRect.topLeftGridCoords.j; j <= dragRect.botRightGridCoords.j; j++)
                {
                    if (!levelEditor.tileSelectedCoords.Any((coord) => { return coord.i == i && coord.j == j; }))
                    {
                        levelEditor.tileSelectedCoords.Add(new GridCoords(i, j));
                        levelEditor.clonedTiles = null;
                    }
                }
            }

            levelEditor.initMultiEditParams();

            if (levelEditor.selectedTool != Tool.PlaceTile && levelEditor.selectedTool != Tool.RectangleTile)
            {
                levelEditor.selectedTool = Tool.PlaceTile;
            }

            levelEditor.redrawTileCanvas();
        }

        public override void onMouseMove(float deltaX, float deltaY)
        {
            if (this.mousedown)
            {
                this.redraw();
            }
        }

        public override void onMouseLeave()
        {
            this.redraw();
        }

        public override void onKeyDown(Keys keyCode, bool firstFrame)
        {
            //TILE_HOTKEYS
            if (!firstFrame) return;
            if (keyCode == Keys.S)
            {
                levelEditor.multiEditHitboxMode = 1;
                levelEditor.setMultiEditHitboxMode();
            }
            else if (keyCode == Keys.Q)
            {
                if (this.isHeld(Keys.Control)) levelEditor.multiEditHitboxMode = 17;
                else if (this.isHeld(Keys.Shift)) levelEditor.multiEditHitboxMode = 21;
                else levelEditor.multiEditHitboxMode = 3;
                levelEditor.setMultiEditHitboxMode();
            }
            else if (keyCode == Keys.E)
            {
                if (this.isHeld(Keys.Control)) levelEditor.multiEditHitboxMode = 18;
                else if (this.isHeld(Keys.Shift)) levelEditor.multiEditHitboxMode = 22;
                else levelEditor.multiEditHitboxMode = 4;
                levelEditor.setMultiEditHitboxMode();
            }
            else if (keyCode == Keys.Z)
            {
                if (this.isHeld(Keys.Control)) levelEditor.multiEditHitboxMode = 19;
                else if (this.isHeld(Keys.Shift)) levelEditor.multiEditHitboxMode = 23;
                else levelEditor.multiEditHitboxMode = 5;
                levelEditor.setMultiEditHitboxMode();
            }
            else if (keyCode == Keys.C)
            {
                if (this.isHeld(Keys.Control)) levelEditor.multiEditHitboxMode = 20;
                else if (this.isHeld(Keys.Shift)) levelEditor.multiEditHitboxMode = 24;
                else levelEditor.multiEditHitboxMode = 6;
                levelEditor.setMultiEditHitboxMode();
            }
            else if (keyCode == Keys.W)
            {
                levelEditor.multiEditHitboxMode = 9;
                levelEditor.setMultiEditHitboxMode();
            }
            else if (keyCode == Keys.X)
            {
                levelEditor.multiEditHitboxMode = 10;
                levelEditor.setMultiEditHitboxMode();
            }
            else if (keyCode == Keys.A)
            {
                levelEditor.multiEditHitboxMode = 11;
                levelEditor.setMultiEditHitboxMode();
            }
            else if (keyCode == Keys.D)
            {
                levelEditor.multiEditHitboxMode = 12;
                levelEditor.setMultiEditHitboxMode();
            }
            else if (keyCode == Keys.NumPad1)
            {
                levelEditor.multiEditHitboxMode = 0;
                levelEditor.setMultiEditHitboxMode();
            }
            else if (keyCode == Keys.NumPad2)
            {
                levelEditor.multiEditZIndex = 0;
                levelEditor.setMultiEditZIndex();
            }
            else if (keyCode == Keys.NumPad3)
            {
            }
            else if (keyCode == Keys.NumPad4)
            {
            }

            else if (keyCode == Keys.O)
            {
                levelEditor.multiEditTag = "swater";
                levelEditor.setMultiEditTag();
            }
            else if (keyCode == Keys.P)
            {
                levelEditor.multiEditTag = "water";
                levelEditor.setMultiEditTag();
            }
            else if (keyCode == Keys.L)
            {
                levelEditor.multiEditTag = "ledge";
                levelEditor.setMultiEditTag();
            }
            else if (keyCode == Keys.K)
            {
                levelEditor.multiEditTag = "ledgewall";
                levelEditor.setMultiEditTag();
            }
            else if (keyCode == Keys.F)
            {
                levelEditor.multiEditZIndex = (int)ZIndex.Foreground1;
                levelEditor.setMultiEditZIndex();
            }
            else if (keyCode == Keys.T)
            {
                levelEditor.multiEditTileSprite = levelEditor.lastSelectedTileSprite;
                levelEditor.setMultiEditTileSprite();
            }
            //Download selected tile
            else if (keyCode == Keys.B)
            {
                /*
                var canvas = document.createElement("canvas");
                var rect = getTileSelectedGridRect();
                var w = (rect.j2 - rect.j1 + 1) * Consts.TILE_WIDTH;
                var h = (rect.i2 - rect.i1 + 1) * Consts.TILE_WIDTH;
                canvas.width = w;
                canvas.height = h;
                var ctx = canvas.getContext("2d");
                var sourceX = rect.j1 * Consts.TILE_WIDTH;
                var sourceY = rect.i1 * Consts.TILE_WIDTH;
                ctx.drawImage(levelEditor.selectedTileset.imgEl, sourceX, sourceY, w, h, 0, 0, w, h);
                var dt = canvas.toDataURL('image/png');
                console.log(dt);
                canvas.remove();
                */
            }
            /*
            else if(keyCode == Keys.R) {
              levelEditor.multiEditTag = "ledgeupleft";
              levelEditor.setMultiEditTag();
            }
            else if(keyCode == Keys.T) {
              levelEditor.multiEditTag = "ledgeup";
              levelEditor.setMultiEditTag();
            }
            else if(keyCode == Keys.Y) {
              levelEditor.multiEditTag = "ledgeupright";
              levelEditor.setMultiEditTag();
            }
            else if(keyCode == Keys.F) {
              levelEditor.multiEditTag = "ledgeleft";
              levelEditor.setMultiEditTag();
            }
            else if(keyCode == Keys.H) {
              levelEditor.multiEditTag = "ledgeright";
              levelEditor.setMultiEditTag();
            }
            else if(keyCode == Keys.V) {
              levelEditor.multiEditTag = "ledgedownleft";
              levelEditor.setMultiEditTag();
            }
            else if(keyCode == Keys.B) {
              levelEditor.multiEditTag = "ledgedown";
              levelEditor.setMultiEditTag();
            }
            else if(keyCode == Keys.N) {
              levelEditor.multiEditTag = "ledgedownright";
              levelEditor.setMultiEditTag();
            }
            */
        }

    }
}

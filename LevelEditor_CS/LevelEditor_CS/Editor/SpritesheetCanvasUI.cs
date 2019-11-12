﻿using LevelEditor_CS.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelEditor_CS.Editor
{
    public class SpritesheetCanvasUI : CanvasUI
    {
        public SpriteEditor spriteEditor;

        public SpritesheetCanvasUI(PictureBox pictureBox, Panel panel, SpriteEditor spriteEditor) : base(pictureBox, panel, 1000, 800, Color.LightGray)
        {
            this.spriteEditor = spriteEditor;
        }

        public override void onKeyDown(Keys key, bool firstFrame)
        {
            base.onKeyDown(key, firstFrame);
            if (spriteEditor.selectedFrame != null)
            {
                if (key == Keys.F)
                {
                    spriteEditor.addPendingFrame();
                }
                else if (key == Keys.C && spriteEditor.selectedSprite.frames.Count > 0)
                {
                    spriteEditor.selectedFrame.parentFrameIndex = spriteEditor.lastSelectedFrameIndex;
                    spriteEditor.selectedSprite.frames[spriteEditor.lastSelectedFrameIndex].childFrames.push(spriteEditor.selectedFrame);
                    spriteCanvas.redraw();
                    spriteSheetCanvas.redraw();
                }
            }

            this.redraw();
        }

        public override void onKeyUp(Keys key)
        {
            base.onKeyUp(key);
        }

        public override void onLeftMouseDown()
        {
            base.onLeftMouseDown();
            if (spriteEditor.selectedSprite == null) return;

            foreach (var frame in spriteEditor.selectedSprite.frames)
            {
                if (Helpers.inRect(this.mouseX, this.mouseY, frame.rect))
                {
                    spriteEditor.selectedFrame = frame;
                    this.redraw();
                    spriteCanvas.redraw();
                    return;
                }
            }

            if (spriteEditor.selectedSpritesheet == null) return;

            Rect rect = null;
            if (!spriteEditor.tileMode)
            {
                //No frame clicked, see if continous image was clicked, if so add to pending
                rect = Helpers.getPixelClumpRect(this.mouseX, this.mouseY, spriteEditor.selectedSpritesheet.imgArr);
            }
            else
            {
                if (spriteEditor.tileModeOffsetX || spriteEditor.tileModeOffsetY)
                {
                    var finalX = this.getMouseGridCoordsCustomWidth(spriteEditor.tileWidth).j;
                    var finalY = this.getMouseGridCoordsCustomWidth(spriteEditor.tileWidth).i;
                    if (spriteEditor.tileModeOffsetX)
                    {
                        var x = this.mouseX / spriteEditor.tileWidth;
                        var intX = Mathf.Floor(x);
                        if (x - intX < 0.5) finalX = intX - 0.5f;
                        else finalX = intX + 0.5f;
                    }
                    if (spriteEditor.tileModeOffsetY)
                    {
                        var y = this.mouseY / spriteEditor.tileWidth;
                        var intY = Mathf.Floor(y);
                        if (y - intY < 0.5) finalY = intY - 0.5f;
                        else finalY = intY + 0.5f;
                    }
                    rect = new GridCoords(finalY, finalX).getRectCustomWidth(spriteEditor.tileWidth);
                }
                else
                {
                    rect = this.getMouseGridCoordsCustomWidth(spriteEditor.tileWidth).getRectCustomWidth(spriteEditor.tileWidth);
                }
            }

            if (rect != null)
            {
                spriteEditor.selectedFrame = new Frame(rect, 0.066f, new Models.Point(0, 0));
                this.redraw();
                spriteCanvas.redraw();
            }

            spriteCanvas.redraw();
        }

        public override void onLeftMouseUp()
        {
            var area = (Math.Abs(this.dragBotY - this.dragTopY) * Math.Abs(this.dragRightX - this.dragLeftX));
            if (area > 10)
            {
                if (!spriteEditor.tileMode)
                {
                    getSelectedPixels();
                }
                else
                {
                    var topLeft = new GridCoords(Mathf.Floor(this.dragTopY / spriteEditor.tileWidth), Mathf.Floor(this.dragLeftX / spriteEditor.tileWidth));
                    var botRight = new GridCoords(Mathf.Floor(this.dragBotY / spriteEditor.tileWidth), Mathf.Floor(this.dragRightX / spriteEditor.tileWidth));
                    var rect = new Rect(topLeft.j * spriteEditor.tileWidth, topLeft.i * spriteEditor.tileWidth, (botRight.j + 1) * spriteEditor.tileWidth, (botRight.i + 1) * spriteEditor.tileWidth);
                    spriteEditor.selectedFrame = new Frame(rect, 0.066f, new Models.Point(0, 0));
                    this.redraw();
                    spriteCanvas.redraw();
                }
            }
        }

        public override void onMouseLeave()
        {
            base.onMouseLeave();
        }

        public override void onMouseMove(float deltaX, float deltaY)
        {
            base.onMouseMove(deltaX, deltaY);
            if (this.mousedown)
            {
                this.redraw();
            }
        }

        public override void onMouseWheel(float delta)
        {
            base.onMouseWheel(delta);
        }

        public override void redraw()
        {
            base.redraw();
            canvas.Clear(Color.Transparent);

            if (spriteEditor.selectedSpritesheet && spriteEditor.selectedSpritesheet.imgEl)
            {
                canvas.drawImage(spriteEditor.selectedSpritesheet.imgEl, 0, 0);
            }

            if (spriteEditor.tileMode)
            {
                //Draw columns
                for (var i = 1; i < this.canvas.width / spriteEditor.tileWidth; i++)
                {
                    Helpers.drawLine(canvas, i * spriteEditor.tileWidth, 0, i * spriteEditor.tileWidth, this.canvas.height, "red", 1);
                }
                //Draw rows
                for (var i = 1; i < this.canvas.height / spriteEditor.tileWidth; i++)
                {
                    Helpers.drawLine(canvas, 0, i * spriteEditor.tileWidth, this.canvas.width, i * spriteEditor.tileWidth, "red", 1);
                }
            }

            if (this.mousedown)
            {
                Helpers.drawRect(canvas, new Rect(this.dragLeftX, this.dragTopY, this.dragRightX, this.dragBotY), "", "blue", 1);
            }

            if (spriteEditor.selectedSprite != null)
            {
                var i = 0;
                foreach (var frame in spriteEditor.selectedSprite.frames)
                {
                    Helpers.drawRect(canvas, frame.rect, "", "blue", 1);
                    Helpers.drawText(canvas, String(i + 1), frame.rect.x1, frame.rect.y1, "red", null, 12, "left", "Top", "Arial");
                    i++;
                }
            }

            if (spriteEditor.selectedFrame != null)
            {
                Helpers.drawRect(canvas, spriteEditor.selectedFrame.rect, "", "green", 2);
            }
        }
    }
}

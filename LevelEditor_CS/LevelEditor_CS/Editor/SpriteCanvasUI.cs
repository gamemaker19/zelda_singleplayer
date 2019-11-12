using LevelEditor_CS.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelEditor_CS.Editor
{
    public class SpriteCanvasUI : CanvasUI
    {
        public SpriteEditor spriteEditor;

        public SpriteCanvasUI(PictureBox pictureBox, Panel panel, SpriteEditor spriteEditor) : base(pictureBox, panel, 1000, 800, Color.LightGray)
        {
            isNoScrollZoom = true;
            zoom = 5;
            this.spriteEditor = spriteEditor;
        }

        public override void redraw()
        {
            base.redraw();

            if (spriteEditor.selectedSprite == null) return;

            Frame frame = null;

            if (!spriteEditor.isAnimPlaying)
            {
                if (spriteEditor.selectedFrame != null && spriteEditor.selectedSpritesheet != null && spriteEditor.selectedSpritesheet.imgEl)
                {
                    frame = spriteEditor.selectedFrame;
                }
            }
            else
            {
                frame = spriteEditor.selectedSprite.frames[spriteEditor.animFrameIndex];
            }

            if (frame == null) return;

            var cX = this.canvas.width / 2;
            var cY = this.canvas.height / 2;

            var frameIndex = spriteEditor.selectedSprite.frames.IndexOf(frame);
            /*
            if(frameIndex < 0 && frame.parentFrameIndex !== undefined) {
              frameIndex = frame.parentFrameIndex;
            }
            */

            if (frameIndex < 0)
            {
                spriteEditor.selectedSprite.drawFrame(canvas, frame, cX, cY, frame.xDir, frame.yDir);
            }
            else
            {
                spriteEditor.selectedSprite.draw(canvas, frameIndex, cX, cY, frame.xDir, frame.yDir);
            }

            if (spriteEditor.ghost != null)
            {
                spriteEditor.ghost.sprite.draw(canvas, spriteEditor.ghost.sprite.frames.IndexOf(spriteEditor.ghost.frame), cX, cY, frame.xDir, frame.yDir, "", 0.5);
            }

            if (!spriteEditor.hideGizmos)
            {
                foreach (var hitbox in spriteEditor.getVisibleHitboxes())
                {

                    float hx = 0; 
                    float hy = 0;
                    var halfW = hitbox.width * 0.5;
                    var halfH = hitbox.height * 0.5;
                    var w = halfW * 2;
                    var h = halfH * 2;
                    if (spriteEditor.selectedSprite.alignment == "topleft")
                    {
                        hx = cX; hy = cY;
                    }
                    else if (spriteEditor.selectedSprite.alignment == "topmid")
                    {
                        hx = cX - halfW; hy = cY;
                    }
                    else if (spriteEditor.selectedSprite.alignment == "topright")
                    {
                        hx = cX - w; hy = cY;
                    }
                    else if (spriteEditor.selectedSprite.alignment == "midleft")
                    {
                        hx = cX; hy = cY - halfH;
                    }
                    else if (spriteEditor.selectedSprite.alignment == "center")
                    {
                        hx = cX - halfW; hy = cY - halfH;
                    }
                    else if (spriteEditor.selectedSprite.alignment == "midright")
                    {
                        hx = cX - w; hy = cY - halfH;
                    }
                    else if (spriteEditor.selectedSprite.alignment == "botleft")
                    {
                        hx = cX; hy = cY - h;
                    }
                    else if (spriteEditor.selectedSprite.alignment == "botmid")
                    {
                        hx = cX - halfW; hy = cY - h;
                    }
                    else if (spriteEditor.selectedSprite.alignment == "botright")
                    {
                        hx = cX - w; hy = cY - h;
                    }

                    var offsetRect = new Rect(
                      hx + hitbox.offset.x, hy + hitbox.offset.y, hx + hitbox.width + hitbox.offset.x, hy + hitbox.height + hitbox.offset.y
                    );

                    string strokeColor;
                    int strokeWidth;
                    if (spriteEditor.selection == hitbox)
                    {
                        strokeColor = "blue";
                        strokeWidth = 2;
                    }

                    Helpers.drawRect(canvas, offsetRect, Color.Blue, strokeColor, strokeWidth, 0.25f);
                }

                var len = 1000;
                Helpers.drawLine(canvas, cX, cY - len, cX, cY + len, "red", 1);
                Helpers.drawLine(canvas, cX - len, cY, cX + len, cY, "red", 1);
                Helpers.drawCircle(canvas, cX, cY, 1, "red");
                //drawStroked(c1, "+", cX, cY);

                foreach (var poi in frame.POIs)
                {
                    Helpers.drawCircle(canvas, cX + poi.x, cY + poi.y, 1, "green");
                }

            }
        }

        public override void onKeyDown(Keys key, bool firstFrame)
        {
            base.onKeyDown(key, firstFrame);
        }

        public override void onKeyUp(Keys key)
        {
            base.onKeyUp(key);
        }

        public override void onLeftMouseDown()
        {
            base.onLeftMouseDown();
        }

        public override void onLeftMouseUp()
        {
            base.onLeftMouseUp();
        }

        public override void onMouseLeave()
        {
            base.onMouseLeave();
        }

        public override void onMouseMove(float deltaX, float deltaY)
        {
            base.onMouseMove(deltaX, deltaY);
        }

        public override void onMouseWheel(float delta)
        {
            base.onMouseWheel(delta);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

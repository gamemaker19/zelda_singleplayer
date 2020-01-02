using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Color = System.Drawing.Color;
using Frame = GameEditor.Models.Frame;
using Rect = GameEditor.Models.Rect;

namespace GameEditor.Editor
{
    public class SpriteCanvasUI : CanvasUI
    {
        public SpriteEditor spriteEditor;

        public SpriteCanvasUI(ScrollViewer panel, SpriteEditor spriteEditor) : base(panel, (int)panel.Width, (int)panel.Height, Color.LightGray)
        {
            isNoScrollZoom = true;
            zoom = 5;
            this.spriteEditor = spriteEditor;
        }

        public override void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            base.pictureBox_Paint(sender, e);

            if (spriteEditor.selectedSprite == null) return;

            Frame frame = null;

            if (!spriteEditor.isAnimPlaying)
            {
                if (spriteEditor.selectedFrame != null && spriteEditor.selectedSpritesheet != null && spriteEditor.selectedSpritesheet.image != null)
                {
                    frame = spriteEditor.selectedFrame;
                }
            }
            else
            {
                frame = spriteEditor.selectedSprite.frames[spriteEditor.animFrameIndex];
            }

            if (frame == null) return;

            int cX = CanvasWidth / 2;
            int cY = CanvasHeight / 2;

            var frameIndex = spriteEditor.selectedSprite.frames.IndexOf(frame);

            //if(frameIndex < 0 && frame.parentFrameIndex !== undefined) {
            //  frameIndex = frame.parentFrameIndex;
            //}

            if (frameIndex < 0)
            {
                spriteEditor.selectedSprite.drawFrame(e.Graphics, frame, cX, cY, frame.xDir, frame.yDir);
            }
            else
            {
                spriteEditor.selectedSprite.draw(e.Graphics, frameIndex, cX, cY, frame.xDir, frame.yDir);
            }

            if (spriteEditor.ghost != null)
            {
                spriteEditor.ghost.sprite.draw(e.Graphics, spriteEditor.ghost.sprite.frames.IndexOf(spriteEditor.ghost.frame), cX, cY, frame.xDir, frame.yDir, "", 0.5f);
            }

            if (!spriteEditor.hideGizmos)
            {
                foreach (var hitbox in spriteEditor.getVisibleHitboxes())
                {
                    float hx = 0;
                    float hy = 0;
                    float halfW = hitbox.width * 0.5f;
                    float halfH = hitbox.height * 0.5f;
                    float w = halfW * 2;
                    float h = halfH * 2;
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

                    Color? strokeColor = null;
                    int strokeWidth = 0;
                    if (spriteEditor.selection == hitbox)
                    {
                        strokeColor = Color.Blue;
                        strokeWidth = 2;
                    }

                    Helpers.drawRect(e.Graphics, offsetRect, Color.Blue, strokeColor, strokeWidth, 0.25f);
                }

                var len = 1000;
                Helpers.drawLine(e.Graphics, cX, cY - len, cX, cY + len, Color.Red, 1);
                Helpers.drawLine(e.Graphics, cX - len, cY, cX + len, cY, Color.Red, 1);
                Helpers.drawCircle(e.Graphics, cX, cY, 1, Color.Red);
                //drawStroked(c1, "+", cX, cY);

                foreach (var poi in frame.POIs)
                {
                    Helpers.drawCircle(e.Graphics, cX + poi.x, cY + poi.y, 1, Color.Green);
                }
            }
        }

        public override void onKeyDown(Key keyCode, bool firstFrame)
        {
            if (keyCode == Key.Escape)
            {
                spriteEditor.selection = null;
                spriteEditor.ghost = null;
            }

            if (spriteEditor.selectedFrame != null)
            {
                if (keyCode == Key.G)
                {
                    spriteEditor.ghost = new Ghost(spriteEditor.selectedSprite, spriteEditor.selectedFrame);
                }
            }

            if (spriteEditor.selection != null && firstFrame)
            {
                if (keyCode == Key.A)
                {
                    spriteEditor.selection.move(-1, 0);
                }
                else if (keyCode == Key.D)
                {
                    spriteEditor.selection.move(1, 0);
                }
                else if (keyCode == Key.W)
                {
                    spriteEditor.selection.move(0, -1);
                }
                else if (keyCode == Key.S)
                {
                    spriteEditor.selection.move(0, 1);
                }
                else if (keyCode == Key.Left)
                {
                    spriteEditor.selection.resizeCenter(-1, 0);
                }
                else if (keyCode == Key.Right)
                {
                    spriteEditor.selection.resizeCenter(1, 0);
                }
                else if (keyCode == Key.Down)
                {
                    spriteEditor.selection.resizeCenter(0, -1);
                }
                else if (keyCode == Key.Up)
                {
                    spriteEditor.selection.resizeCenter(0, 1);
                }
            }
            else if (spriteEditor.selectedFrame != null && firstFrame)
            {
                if (keyCode == Key.A)
                {
                    spriteEditor.selectedFrame.offset.x -= 1;
                }
                else if (keyCode == Key.D)
                {
                    spriteEditor.selectedFrame.offset.x += 1;
                }
                else if (keyCode == Key.W)
                {
                    spriteEditor.selectedFrame.offset.y -= 1;
                }
                else if (keyCode == Key.S)
                {
                    spriteEditor.selectedFrame.offset.y += 1;
                }
            }
            this.redraw();
        }
    }
}

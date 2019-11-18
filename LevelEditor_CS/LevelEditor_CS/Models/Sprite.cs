using LevelEditor_CS.Editor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor_CS.Models
{
    public class Sprite
    {
        public string name = "";
        public List<Hitbox> hitboxes = new List<Hitbox>();
        public float loopStartFrame = 0;
        public List<Frame> frames = new List<Frame>();
        public string alignment = "center";
        public string wrapMode = "once"; //Can be "once", "loop" or "pingpong"
        public string spriteJson = "";
        public string spritesheetPath;

        [JsonIgnore]
        public List<Spritesheet> spritesheets;

        [JsonIgnore]
        public Spritesheet spritesheet
        {
            get
            {
                return spritesheets.Where(spritesheet => spritesheet.getBasePath() == spritesheetPath).FirstOrDefault();
            }
        }

        public Sprite(string name, List<Spritesheet> spritesheets)
        {
            this.name = name;
            this.spritesheets = spritesheets;
        }

        //Given the sprite's alignment, get the offset x and y on where to actually draw the sprite
        public Point getAnchor() 
        {
            float x = 0, y = 0;
            if(this.alignment == "topleft") {
                x = 0; y = 0;
            }
            else if(this.alignment == "topmid") {
                x = 0.5f; y = 0;
            }
            else if(this.alignment == "topright") {
                x = 1; y = 0;
            }
            else if(this.alignment == "midleft") {
                x = 0; y = 0.5f;
            }
            else if(this.alignment == "center") {
                x = 0.5f; y = 0.5f;
            }
            else if(this.alignment == "midright") {
                x = 1; y = 0.5f;
            }
            else if(this.alignment == "botleft") {
                x = 0; y = 1;
            }
            else if(this.alignment == "botmid") {
                x = 0.5f; y = 1;
            }
            else if(this.alignment == "botright") {
                x = 1; y = 1;
            }
            return new Point(x, y);
        }

        public void draw(Graphics canvas, int frameIndex, float x, float y, int flipX = 1, int flipY = 1, string options = "", float alpha = 1, float scaleX = 1, float scaleY = 1)
        {
            var frame = this.frames[frameIndex];
            var rect = frame.rect;
            var offset = this.getAlignOffset(frame, flipX, flipY);

            Helpers.drawImage(canvas, this.spritesheet.image, x + offset.x + frame.offset.x, y + offset.y + frame.offset.y, rect.x1, rect.y1, rect.w, rect.h, flipX, flipY, options, alpha, scaleX, scaleY);

            /*
            var wrappers : any = [];
            wrappers.push({
            closure: () => {
                Helpers.drawImage(ctx, this.spritesheet.imgEl, rect.x1, rect.y1, rect.w, rect.h, x + offset.x + frame.offset.x, y + offset.y + frame.offset.y, flipX, flipY, options, alpha, scaleX, scaleY);
            },
            zIndex: 0
            });

            for (var childFrame of frame.childFrames)
            {
                wrappers.push({
                closure: () => {
                    var childOffsetX = x + offset.x + frame.offset.x + childFrame.offset.x;
                    var childOffsetY = y + offset.y + frame.offset.y + childFrame.offset.y;
                    Helpers.drawImage(ctx, this.spritesheet.imgEl, childFrame.rect.x1, childFrame.rect.y1, childFrame.rect.w, childFrame.rect.h, childOffsetX, childOffsetY, childFrame.xDir, childFrame.yDir, options, alpha, scaleX, scaleY);
                },
                zIndex: childFrame.zIndex
                });
            }

            wrappers.sort((a: any, b: any) => {
              return a.zIndex - b.zIndex;
            });

            for(var wrapper of wrappers) {
              wrapper.closure();
            }
            */
        }

        public void drawFrame(Graphics canvas, Frame frame, float x, float y, int flipX = 1, int flipY = 1, string options = "", float alpha = 1, float scaleX = 1, float scaleY = 1)
        {
            var rect = frame.rect;
            var offset = this.getAlignOffset(frame, flipX, flipY);
            Helpers.drawImage(canvas, this.spritesheet.image, rect.x1, rect.y1, rect.w, rect.h, x + offset.x + frame.offset.x, y + offset.y + frame.offset.y, flipX, flipY, options, alpha, scaleX, scaleY);
        }

        //Returns actual width and heights, not 0-1 number
        public Point getAlignOffset(Frame frame, int flipX = 1, int flipY = 1)
        {
            var rect = frame.rect;

            var w = rect.w;
            var h = rect.h;

            var halfW = w * 0.5f;
            var halfH = h * 0.5f;

            if(flipX > 0) halfW = Mathf.Floor(halfW);
            else halfW = Mathf.Ceil(halfW);
            if(flipY > 0) halfH = Mathf.Floor(halfH);
            else halfH = Mathf.Ceil(halfH);

            float x = 0; 
            float y = 0;

            if(this.alignment == "topleft") {
                x = 0; y = 0;
            }
            else if(this.alignment == "topmid") {
                x = -halfW; y = 0;
            }
            else if(this.alignment == "topright") {
                x = -w; y = 0;
            }
            else if(this.alignment == "midleft") {
                x = flipX == -1 ? -w : 0; y = -halfH;
            }
            else if(this.alignment == "center") {
                x = -halfW; y = -halfH;
            }
            else if(this.alignment == "midright") {
                x = flipX == -1 ? 0 : -w; y = -halfH;
            }
            else if(this.alignment == "botleft") {
                x = 0; y = -h;
            }
            else if(this.alignment == "botmid") {
                x = -halfW; y = -h;
            }
            else if(this.alignment == "botright") {
                x = -w; y = -h;
            }
            else {
                throw new Exception("No alignment provided");
            }
            return new Point(x, y);
        }

        public List<Frame> getParentFrames()
        {
            var frames = new List<Frame>();
            foreach (var frame in this.frames)
            {
                if (frame.parentFrameIndex == -1)
                {
                    frames.Add(frame);
                }
            }
            return frames;
        }

        public List<Frame> getChildFrames(int parentFrameIndex)
        {
            var frames = new List<Frame>();
            foreach (var frame in this.frames)
            {
                if (frame.parentFrameIndex == parentFrameIndex)
                {
                    frames.Add(frame);
                }
            }
            return frames;
        }
    }
}

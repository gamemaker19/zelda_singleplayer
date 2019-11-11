using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public Spritesheet spritesheet;

        public Sprite(string name, Spritesheet spritesheet)
        {
            this.name = name;
            this.spritesheet = spritesheet;
        }

        [OnSerializing]
        public void onSerialize(StreamingContext context)
        {
            this.spritesheetPath = this.spritesheet.path;
        }

        [OnDeserialized]
        public void onDeserialize(StreamingContext context)
        {
            //this.spritesheet = new Spritesheet(this.spritesheetPath);
        }

        public void setSpritesheet(List<Spritesheet> spritesheets)
        {
            this.spritesheet = spritesheets.Where((spritesheet) => {
                return this.spritesheetPath == spritesheet.path;
            }).FirstOrDefault();
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

        /*
        public void draw(ctx: CanvasRenderingContext2D, frameIndex: number, x: number, y: number, flipX?: number, flipY?: number, options?: string, alpha?: number, scaleX?: number, scaleY?: number)
        {
            flipX = flipX || 1;
            flipY = flipY || 1;
            let frame = this.frames[frameIndex];
            let rect = frame.rect;
            let offset = this.getAlignOffset(frame, flipX, flipY);

            let wrappers : any = [];
            wrappers.push({
            closure: () => {
                Helpers.drawImage(ctx, this.spritesheet.imgEl, rect.x1, rect.y1, rect.w, rect.h, x + offset.x + frame.offset.x, y + offset.y + frame.offset.y, flipX, flipY, options, alpha, scaleX, scaleY);
            },
            zIndex: 0
            });

            for (let childFrame of frame.childFrames)
            {
                wrappers.push({
                closure: () => {
                    let childOffsetX = x + offset.x + frame.offset.x + childFrame.offset.x;
                    let childOffsetY = y + offset.y + frame.offset.y + childFrame.offset.y;
                    Helpers.drawImage(ctx, this.spritesheet.imgEl, childFrame.rect.x1, childFrame.rect.y1, childFrame.rect.w, childFrame.rect.h, childOffsetX, childOffsetY, childFrame.xDir, childFrame.yDir, options, alpha, scaleX, scaleY);
                },
            zIndex: childFrame.zIndex
                });
        }

        wrappers.sort((a: any, b: any) => {
          return a.zIndex - b.zIndex;
        });

        for(let wrapper of wrappers) {
          wrapper.closure();
        }
      }

        drawFrame(ctx: CanvasRenderingContext2D, frame: Frame, x: number, y: number, flipX?: number, flipY?: number, options?: string, alpha?: number, scaleX?: number, scaleY?: number)
        {
            flipX = flipX || 1;
            flipY = flipY || 1;
            let rect = frame.rect;
            let offset = this.getAlignOffset(frame, flipX, flipY);
            Helpers.drawImage(ctx, this.spritesheet.imgEl, rect.x1, rect.y1, rect.w, rect.h, x + offset.x + frame.offset.x, y + offset.y + frame.offset.y, flipX, flipY, options, alpha, scaleX, scaleY);
        }

        //Returns actual width and heights, not 0-1 number
        getAlignOffset(frame: Frame, flipX?: number, flipY?: number) : Point {
            let rect = frame.rect;
            flipX = flipX || 1;
            flipY = flipY || 1;

            let w = rect.w;
            let h = rect.h;

            let halfW = w * 0.5;
            let halfH = h * 0.5;

            if(flipX > 0) halfW = Math.floor(halfW);
            else halfW = Math.ceil(halfW);
            if(flipY > 0) halfH = Math.floor(halfH);
            else halfH = Math.ceil(halfH);

            let x; let y;

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
                throw "No alignment provided";
            }
            return new Point(x, y);
        }

        getParentFrames()
        {
            let frames = [];
            for (let frame of this.frames)
            {
                if (frame.parentFrameIndex == undefined)
                {
                    frames.push(frame);
                }
            }
            return frames;
        }

        getChildFrames(parentFrameIndex: number)
        {
            let frames = [];
            for (let frame of this.frames)
            {
                if (frame.parentFrameIndex == parentFrameIndex)
                {
                    frames.push(frame);
                }
            }
            return frames;
        }
        */
    }
}

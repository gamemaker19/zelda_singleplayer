﻿using GameEditor.Editor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace GameEditor.Models
{
    public class Sprite
    {
        public string name { get; set; } = "";
        public ObservableCollection<Hitbox> hitboxes { get; set; } = new ObservableCollection<Hitbox>();
        public float loopStartFrame { get; set; } = 0;
        public ObservableCollection<Frame> frames { get; set; } = new ObservableCollection<Frame>();
        public string alignment { get; set; } = "center";
        public string wrapMode { get; set; } = "once"; //Can be "once", "loop" or "pingpong"
        public string spriteJson { get; set; } = "";
        public string spritesheetPath { get; set; }

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

        public override string ToString()
        {
            return name;
        }

        //Given the sprite's alignment, get the offset x and y on where to actually draw the sprite
        public Point getAnchor()
        {
            float x = 0, y = 0;
            if (this.alignment == "topleft")
            {
                x = 0; y = 0;
            }
            else if (this.alignment == "topmid")
            {
                x = 0.5f; y = 0;
            }
            else if (this.alignment == "topright")
            {
                x = 1; y = 0;
            }
            else if (this.alignment == "midleft")
            {
                x = 0; y = 0.5f;
            }
            else if (this.alignment == "center")
            {
                x = 0.5f; y = 0.5f;
            }
            else if (this.alignment == "midright")
            {
                x = 1; y = 0.5f;
            }
            else if (this.alignment == "botleft")
            {
                x = 0; y = 1;
            }
            else if (this.alignment == "botmid")
            {
                x = 0.5f; y = 1;
            }
            else if (this.alignment == "botright")
            {
                x = 1; y = 1;
            }
            return new Point(x, y);
        }

        public void draw(Graphics canvas, int frameIndex, float x, float y, int flipX = 1, int flipY = 1, string options = "", float alpha = 1, float scaleX = 1, float scaleY = 1)
        {
            var frame = this.frames[frameIndex];
            var rect = frame.rect;
            var offset = this.getAlignOffset(frame, flipX, flipY);

            this.spritesheet.init(false);

            var wrappers = new List<Tuple<Action, float>>();
            wrappers.Add(new Tuple<Action, float>(
                () =>
                {
                    Helpers.drawImage(canvas, this.spritesheet.image, x + offset.x + frame.offset.x, y + offset.y + frame.offset.y, rect.x1, rect.y1, rect.w, rect.h, flipX, flipY, options, alpha, scaleX, scaleY);
                }, 0));

            foreach (var childFrame in frame.childFrames)
            {
                wrappers.Add(new Tuple<Action, float>(
                () =>
                {
                    var childOffsetX = x + offset.x + frame.offset.x + childFrame.offset.x;
                    var childOffsetY = y + offset.y + frame.offset.y + childFrame.offset.y;
                    Helpers.drawImage(canvas, this.spritesheet.image, childOffsetX, childOffsetY, childFrame.rect.x1, childFrame.rect.y1, childFrame.rect.w, childFrame.rect.h, childFrame.xDir, childFrame.yDir, options, alpha, scaleX, scaleY);
                },
                childFrame.zIndex));
            }

            wrappers.Sort((Tuple<Action, float> a, Tuple<Action, float> b) =>
            {
                return Math.Sign(a.Item2 - b.Item2);
            });

            foreach (var wrapper in wrappers)
            {
                wrapper.Item1();
            }
        }

        public void drawFrame(Graphics canvas, Frame frame, float x, float y, int flipX = 1, int flipY = 1, string options = "", float alpha = 1, float scaleX = 1, float scaleY = 1)
        {
            var rect = frame.rect;
            var offset = this.getAlignOffset(frame, flipX, flipY);
            Helpers.drawImage(canvas, this.spritesheet.image, x + offset.x + frame.offset.x, y + offset.y + frame.offset.y, rect.x1, rect.y1, rect.w, rect.h, flipX, flipY, options, alpha, scaleX, scaleY);
        }

        //Returns actual width and heights, not 0-1 number
        public Point getAlignOffset(Frame frame, int flipX = 1, int flipY = 1)
        {
            var rect = frame.rect;

            var w = rect.w;
            var h = rect.h;

            var halfW = w * 0.5f;
            var halfH = h * 0.5f;

            if (flipX > 0) halfW = Mathf.Floor(halfW);
            else halfW = Mathf.Ceil(halfW);
            if (flipY > 0) halfH = Mathf.Floor(halfH);
            else halfH = Mathf.Ceil(halfH);

            float x = 0;
            float y = 0;

            if (this.alignment == "topleft")
            {
                x = 0; y = 0;
            }
            else if (this.alignment == "topmid")
            {
                x = -halfW; y = 0;
            }
            else if (this.alignment == "topright")
            {
                x = -w; y = 0;
            }
            else if (this.alignment == "midleft")
            {
                x = flipX == -1 ? -w : 0; y = -halfH;
            }
            else if (this.alignment == "center")
            {
                x = -halfW; y = -halfH;
            }
            else if (this.alignment == "midright")
            {
                x = flipX == -1 ? 0 : -w; y = -halfH;
            }
            else if (this.alignment == "botleft")
            {
                x = 0; y = -h;
            }
            else if (this.alignment == "botmid")
            {
                x = -halfW; y = -h;
            }
            else if (this.alignment == "botright")
            {
                x = -w; y = -h;
            }
            else
            {
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

        public List<Frame> getAllFrames()
        {
            var retFrames = new List<Frame>();
            for (int i = 0; i < frames.Count; i++)
            {
                Frame frame = frames[i];
                if (frame.parentFrameIndex == -1)
                {
                    retFrames.Add(frame);
                    retFrames.AddRange(frame.childFrames);
                }
            }
            return retFrames;
        }
    }
}

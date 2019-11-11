﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor_CS.Models
{
    public class SpriteInstance
    {
        public string name;
        public string properties;
        public Point pos;
        public Sprite sprite;
        public Obj obj;

        public SpriteInstance(string name, float x, float y, Obj obj, List<Sprite> sprites)
        {
            this.name = name;
            this.pos = new Point(x, y);
            this.obj = obj;
            this.sprite = sprites.Where((sprite) => {
                return sprite.name == this.obj.spriteOrImage;
            }).FirstOrDefault();
        }

        public void setSprite(List<Sprite> sprites)
        {
            this.sprite = sprites.Where((sprite) => {
                return sprite.name == this.obj.spriteOrImage;
            }).FirstOrDefault();
        }
        public Rect getPositionalRect()
        {
            float w = this.sprite.frames[0].rect.w;
            float h = this.sprite.frames[0].rect.h;
            float x1 = this.pos.x - w / 2;
            float y1 = this.pos.y - h / 2;
            if (this.sprite.alignment == "topleft")
            {
                x1 += w / 2;
                y1 += h / 2;
            }
            float x2 = x1 + w;
            float y2 = y1 + h;
            var rect = new Rect(x1, y1, x2, y2);
            return rect;
        }
        /*
        getNonSerializedKeys()
        {
            return ["sprite"];
        }
        */

        /*
        draw(ctx: CanvasRenderingContext2D)
        {
            if (this.sprite && this.sprite.spritesheet && this.sprite.spritesheet.imgEl)
            {
                this.sprite.draw(ctx, 0, this.pos.x, this.pos.y);
            }
            else if (this.sprite)
            {
            }
        }
        */
    }
}

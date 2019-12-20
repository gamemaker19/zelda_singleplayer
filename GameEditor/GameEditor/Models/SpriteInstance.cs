using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameEditor.Models
{
    public class SpriteInstance
    {
        public string name { get; set; }
        public Dictionary<string, string> properties;
        public Point pos;
        [JsonIgnore]
        public Sprite sprite;
        public Obj obj;

        public SpriteInstance(string name, float x, float y, Obj obj, List<Sprite> sprites)
        {
            this.name = name;
            this.pos = new Point(x, y);
            this.obj = obj;
            if (sprites != null)
            {
                this.sprite = sprites.Where((sprite) =>
                {
                    return sprite != null && sprite.name == this.obj.spriteOrImage;
                }).FirstOrDefault();
            }
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

        public void draw(Graphics graphics)
        {
            if (this.sprite != null && this.sprite.spritesheet != null && this.sprite.spritesheet.image != null)
            {
                this.sprite.draw(graphics, 0, this.pos.x, this.pos.y);
            }
            else if (this.sprite != null)
            {
            }
        }
    }
}

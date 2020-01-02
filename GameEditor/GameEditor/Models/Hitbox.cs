using GameEditor.Editor;
using Newtonsoft.Json;

namespace GameEditor.Models
{
    public class Hitbox : Selectable
    {
        public string tags { get; set; }
        public float width { get; set; }
        public float height { get; set; }
        public Point offset { get; set; }

        [JsonIgnore]
        public bool isSelected { get; set; }

        public Hitbox()
        {
            this.tags = "";
            this.width = 20;
            this.height = 40;
            this.offset = new Point(0, 0);
        }

        public void move(float deltaX, float deltaY)
        {
            this.offset.x += deltaX;
            this.offset.y += deltaY;
        }

        public void resizeCenter(float w, float h)
        {
            this.width += w;
            this.height += h;
        }

        public Rect getRect()
        {
            return new Rect(this.offset.x, this.offset.y, this.offset.x + this.width, this.offset.y + this.height);
        }

    }
}

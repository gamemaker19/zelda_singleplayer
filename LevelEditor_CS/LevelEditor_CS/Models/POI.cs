using LevelEditor_CS.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor_CS.Models
{
    public class POI : Selectable
    {
        public string tags;
        public float x;
        public float y;
        public POI(string tags, float x, float y) 
        {
            this.tags = tags;
            this.x = x;
            this.y = y;
        }

        public void move(float deltaX, float deltaY) 
        {
            this.x += deltaX;
            this.y += deltaY;
        }
        
        public void resizeCenter(float w, float h) 
        {
        }

        public Rect getRect
        {
            get
            {
                return new Rect(this.x - 2, this.y - 2, this.x + 2, this.y + 2);
            }
        }
    }
}

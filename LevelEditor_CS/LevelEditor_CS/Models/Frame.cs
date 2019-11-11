using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor_CS.Models
{
    public class Frame
    {
        public Rect rect;
        public float duration;
        public Point offset;
        public List<Hitbox> hitboxes;
        public List<POI> POIs;
        public List<Frame> childFrames;
        public float parentFrameIndex;
        public float zIndex = 0;
        public float xDir = 1;
        public float yDir = 1;
        public string tags = "";

        public Frame(Rect rect, float duration, Point offset)
        {
            this.rect = rect;
            this.duration = duration;
            this.offset = offset;
            this.hitboxes = new List<Hitbox>();
            this.POIs = new List<POI>();
        }

    }
}

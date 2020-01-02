using GameEditor.Editor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GameEditor.Models
{
    [Serializable]
    public class Frame : Selectable
    {
        public Rect rect { get; set; }
        public float duration { get; set; }
        public Point offset { get; set; }
        public List<Hitbox> hitboxes { get; set; }
        public List<POI> POIs { get; set; }
        public List<Frame> childFrames { get; set; }
        public int parentFrameIndex { get; set; } = -1;
        public float zIndex { get; set; } = 0;
        public int xDir { get; set; } = 1;
        public int yDir { get; set; } = 1;
        public string tags { get; set; } = "";

        [JsonIgnore]
        public bool isSelected { get; set; }

        public Frame(Rect rect, float duration, Point offset)
        {
            this.rect = rect;
            this.duration = duration;
            this.offset = offset;
            this.hitboxes = new List<Hitbox>();
            this.POIs = new List<POI>();
        }

        public void move(float deltaX, float deltaY)
        {
        }

        public void resizeCenter(float w, float h)
        {
        }
    }
}

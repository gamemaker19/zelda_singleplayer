using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Frame
    {
        public Rect rect;
        public float duration = 0;
        public Point offset;
        public List<Collider> hitboxes = new List<Collider>();
        public List<Point> POIs = new List<Point>();
        public float zIndex = 0;
        public List<Frame> childFrames = new List<Frame>();
        public bool enabled = true;
        //These offsets are used for child hitbox processing only.
        public Point topLeftOffset;
        public Point botRightOffset;
        public int xDir = 1;
        public int yDir = 1;
        public string tags = "";

        public Frame(Rect rect, float duration, Point offset, List<Collider> hitboxes, List<Point> POIs, List<Frame> childFrames)
        {
	        this.rect = rect;
	        this.duration = duration;
	        this.offset = offset;
	        this.hitboxes = hitboxes;
	        this.POIs = POIs;
	        this.childFrames = childFrames;
        }

        public Frame clone()
        {
            var clonedFrame = (Frame)MemberwiseClone();
            clonedFrame.hitboxes = new List<Collider>();
            foreach (Collider collider in hitboxes)
            {
                clonedFrame.hitboxes.Add(collider.clone());
            }
            clonedFrame.childFrames = new List<Frame>();
            foreach (Frame frame in childFrames)
            {
                clonedFrame.childFrames.Add(frame.clone());
            }
            return clonedFrame;
        }
    }
}
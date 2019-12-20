using System;

namespace GameEditor.Models
{
    public class Point
    {
        public float x { get; set; }
        public float y { get; set; }

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float ix
        {
            get
            {
                return (float)Math.Round(this.x);
            }
        }
        public float iy
        {
            get
            {
                return (float)Math.Round(this.y);
            }
        }

        public Point addxy(float x, float y)
        {
            var point = new Point(this.x + x, this.y + y);
            return point;
        }

        public bool Equals(Point point)
        {
            return x == point.x && y == point.y;
        }
    }
}

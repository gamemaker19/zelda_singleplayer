using System.Collections.Generic;

namespace GameEditor.Models
{
    public class Rect
    {
        public Point topLeftPoint { get; set; }
        public Point botRightPoint { get; set; }

        public static Rect Create(Point topLeftPoint, Point botRightPoint)
        {
            return new Rect(topLeftPoint.x, topLeftPoint.y, botRightPoint.x, botRightPoint.y);
        }

        public static Rect CreateWH(float x, float y, float w, float h)
        {
            return new Rect(x, y, x + w, y + h);
        }

        public static Rect CreateFromStringKey(string key)
        {
            var pieces = key.Split('_');
            return new Rect(float.Parse(pieces[0]), float.Parse(pieces[1]), float.Parse(pieces[2]), float.Parse(pieces[3]));
        }

        public Rect(float x1, float y1, float x2, float y2)
        {
            this.topLeftPoint = new Point(x1, y1);
            this.botRightPoint = new Point(x2, y2);
        }

        public Shape GetShape
        {
            get
            {
                return new Shape(new List<Point>() { this.topLeftPoint, new Point(this.x2, this.y1), this.botRightPoint, new Point(this.x1, this.y2) });
            }
        }

        public float midX
        {
            get
            {
                return (this.topLeftPoint.x + this.botRightPoint.x) * 0.5f;
            }
        }

        public float x1
        {
            get
            {
                return this.topLeftPoint.x;
            }
        }

        public float y1
        {
            get
            {
                return this.topLeftPoint.y;
            }
        }

        public float x2
        {
            get
            {
                return this.botRightPoint.x;
            }
        }

        public float y2
        {
            get
            {
                return this.botRightPoint.y;
            }
        }

        public float w
        {
            get
            {
                return this.botRightPoint.x - this.topLeftPoint.x;
            }
        }

        public float h
        {
            get
            {
                return this.botRightPoint.y - this.topLeftPoint.y;
            }
        }

        public float area
        {
            get
            {
                return this.w * this.h;
            }
        }

        public List<Point> GetPoints
        {
            get
            {
                return new List<Point>()
                {
                    new Point(this.topLeftPoint.x, this.topLeftPoint.y),
                    new Point(this.botRightPoint.x, this.topLeftPoint.y),
                    new Point(this.botRightPoint.x, this.botRightPoint.y),
                    new Point(this.topLeftPoint.x, this.botRightPoint.y),
                };
            }
        }

        public bool overlaps(Rect other)
        {
            // If one rectangle is on left side of other
            if (this.x1 > other.x2 || other.x1 > this.x2)
                return false;
            // If one rectangle is above other
            if (this.y1 > other.y2 || other.y1 > this.y2)
                return false;
            return true;
        }

        public bool equals(Rect other)
        {
            return this.x1 == other.x1 && this.x2 == other.x2 && this.y1 == other.y1 && this.y2 == other.y2;
        }

        public Rect clone(float x, float y)
        {
            return new Rect(this.x1 + x, this.y1 + y, this.x2 + x, this.y2 + y);
        }

        public string toString()
        {
            return this.x1 + "_" + this.y1 + "_" + this.x2 + "_" + this.y2;
        }

    }
}

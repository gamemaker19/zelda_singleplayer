using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Line
    {
        public Point point1;
        public Point point2;
        public Line(Point point1, Point point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }
        public bool isVertical()
        {
            return point1.x == point2.x;
        }
        public bool isHorizontal()
        {
            return point1.y == point2.y;
        }
        public float topY()
        {
            return Math.Min(point1.y, point2.y);
        }
        public float botY()
        {
            return Math.Max(point1.y, point2.y);
        }
        public float leftX()
        {
            return Math.Min(point1.x, point2.x);
        }
        public float rightX()
        {
            return Math.Max(point1.x, point2.x);
        }
        public string toString()
        {
            return point1.toString() + "," + point2.toString();
        }
    }
}

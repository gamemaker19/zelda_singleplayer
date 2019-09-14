using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Rect
    {
        public float x1 = 0;
        public float y1 = 0;
        public float x2 = 0;
        public float y2 = 0;
        public Rect() { }
        public Rect(float x1, float y1, float x2, float y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }
        public Rect(Point topLeft, Point size)
        {
            x1 = topLeft.x;
            y1 = topLeft.y;
            x2 = x1 + size.x;
            y2 = y1 + size.y;
            if (size.x < 0)
            {
                float temp = x1;
                x1 = x2;
                x2 = temp;
            }
            if (size.y < 0)
            {
                float temp = y1;
                y1 = y2;
                y2 = temp;
            }
        }
        public float w()
        {
            return Math.Abs(x2 - x1);
        }

        public float h()
        {
            return Math.Abs(y2 - y1);
        }
        public float area()
        {
            return w() * h();
        }
        public Point center()
        {
            return new Point((x1 + x2) / 2, (y1 + y2) / 2);
        }
        public string toString()
        {
            return x1.ToString() + ", " + y1.ToString() + ", " + x2.ToString() + ", " + y2.ToString();
        }
        
        public Shape toShape()
        {
            List<Point> points = new List<Point>() { new Point(x1, y1), new Point(x2, y1), new Point(x2, y2), new Point(x1, y2) };
            return new Shape(points);
        }
    }
}

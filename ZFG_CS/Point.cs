using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public struct Point
    {
        public float x;
        public float y;
        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Point Zero
        {
            get
            {
                return new Point(0, 0);
            }
        }

        public float magnitude
        {
            get
            {
                return (float)Math.Sqrt((x * x) + (y * y));
            }
        }
        public Point normalized
        {
            get
            {
                if (isZero()) return Zero;
                float mag = magnitude;
                return new Point(x / mag, y / mag);
            }
        }
        public Point incMag(float incAmount)
        {
            Point norm = normalized;
            norm *= incAmount;
            return new Point(x + norm.x, y + norm.y);
        }
        public void normalize()
        {
            Point normalizedPoint = normalized;
            x = normalizedPoint.x;
            y = normalizedPoint.y;
        }
        public Point addxy(float x, float y)
        {
            return new Point(this.x + x, this.y + y);
        }
        public static Point operator +(Point point1, Point point2)
        {
            return new Point(point1.x + point2.x, point1.y + point2.y);
        }
        public static Point operator -(Point point1, Point point2)
        {
            return new Point(point1.x - point2.x, point1.y - point2.y);
        }
        public static Point operator *(Point point1, float val)
        {
            return new Point(point1.x * val, point1.y * val);
        }
        public static Point operator /(Point point1, float val)
        {
            return new Point(point1.x / val, point1.y / val);
        }
        public static bool operator ==(Point point1, Point point2)
        {
            return point1.x == point2.x && point1.y == point2.y;
        }
        public static bool operator !=(Point point1, Point point2)
        {
            return point1.x != point2.x || point1.y != point2.y;
        }
        public float angle()
        {
            return (float)Math.Atan2(y, x);
        }
        
        public float angle(Point other)
        {
            float ang1 = angle();
            float ang2 = other.angle();
            if (ang1 < 0) ang1 += (float)Math.PI * 2;
            if (ang2 < 0) ang2 += (float)Math.PI * 2;
            if (ang1 >= (float)Math.PI * 2) ang1 -= (float)Math.PI * 2;
            if (ang2 >= (float)Math.PI * 2) ang2 -= (float)Math.PI * 2;
            float ang = Math.Abs(ang1 - ang2);
            return ang;
        }
        public Point dirTo(Point point)
        {
            return (point - (this)).normalized;
        }
        public Point rayTo(Point point)
        {
            return (point - (this));
        }
        public float distTo(Point point)
        {
            return (float)Math.Sqrt(Math.Pow(point.x - x, 2) + (float)Math.Pow(point.y - y, 2));
        }
        public bool isNonZero()
        {
            return x != 0 || y != 0;
        }
        public bool isZero()
        {
            return !isNonZero();
        }
        public Point project(Point other)
        {
            float ang = angle(other);
            return other.normalized * (magnitude * (float)Math.Cos(ang));
        }
        public Point withoutComponent(Point other)
        {
            return this - project(other);
        }
        public Point right()
        {
            return new Point(-y, x);
        }
        public string toString()
        {
            return x.ToString() + "," + y.ToString();
        }
    }
}

using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFG_CS
{
    public struct Shape
    {
        public List<Point> points;

        public Shape(List<Point> points)
        {
            this.points = points;
        }

        public Shape clone()
        {
            List<Point> newPoints = new List<Point>();
            foreach (Point point in points)
            {
                newPoints.Add(new Point(point.x, point.y));
            }
            return new Shape(newPoints);
        }

        public Rect toRect()
        {
            return new Rect(minX(), minY(), maxX(), maxY());
        }

        public List<float> deltaXs()
        {
            float minX = getMinX();
            List<float> deltas = new List<float>(3);
            deltas[0] = Math.Sign(points[0].x - minX);
            deltas[1] = Math.Sign(points[1].x - minX);
            deltas[2] = Math.Sign(points[2].x - minX);
            deltas.Sort();
            return deltas;
        }

        public string toString()
        {
            string s = "";
            foreach (Point point in points)
            {
                float px = point.x;
                float py = point.y;
                s += "(" + px.ToString() + "," + py.ToString() + "),";
            }
            s += "\n";
            return s;
        }

        public List<float> deltaYs()
        {
            float minY = getMinY();
            List<float> deltas = new List<float>(3);
            deltas[0] = Math.Sign(points[0].y - minY);
            deltas[1] = Math.Sign(points[1].y - minY);
            deltas[2] = Math.Sign(points[2].y - minY);
            deltas.Sort();
            return deltas;
        }

        public bool isTopLeftTriangle()
        {
            if (points.Count != 3) return false;
            var dxs = deltaXs();
            var dys = deltaYs();
            return dxs[0] == 0 && dxs[1] == 0 && dxs[2] == 1 && dys[0] == 0 && dys[1] == 0 && dys[2] == 1;
        }
        public bool isTopRightTriangle()
        {
            if (points.Count != 3) return false;
            var dxs = deltaXs();
            var dys = deltaYs();
            return dxs[0] == 0 && dxs[1] == 1 && dxs[2] == 1 && dys[0] == 0 && dys[1] == 0 && dys[2] == 1;
        }
        public bool isBotLeftTriangle()
        {
            if (points.Count != 3) return false;
            var dxs = deltaXs();
            var dys = deltaYs();
            return dxs[0] == 0 && dxs[1] == 0 && dxs[2] == 1 && dys[0] == 0 && dys[1] == 1 && dys[2] == 1;
        }
        public bool isBotRightTriangle()
        {
            if (points.Count != 3) return false;
            var dxs = deltaXs();
            var dys = deltaYs();
            return dxs[0] == 0 && dxs[1] == 1 && dxs[2] == 1 && dys[0] == 0 && dys[1] == 1 && dys[2] == 1;
        }

        public float minY()
        {
            float min = points[0].y;
            foreach (Point point in points)
            {
                if (point.y < min) min = point.y;
            }
            return min;
        }

        public float maxY()
        {
            float max = points[0].y;
            foreach (Point point in points)
            {
                if (point.y > max) max = point.y;
            }
            return max;
        }

        public float minX()
        {
            float min = points[0].x;
            foreach (Point point in points)
            {
                if (point.x < min) min = point.x;
            }
            return min;
        }

        public float maxX()
        {
            float max = points[0].x;
            foreach (Point point in points)
            {
                if (point.x > max) max = point.x;
            }
            return max;
        }

        public Point center()
        {
            float sumX = 0;
            float sumY = 0;
            foreach (Point point in points)
            {
                sumX += point.x;
                sumY += point.y;
            }
            return new Point(sumX / points.Count, sumY / points.Count);
        }

        public bool isRect()
        {
            return points.Count == 4;
        }

        public Polygon2D toPolygon()
        {
            List<Point2D> p2ds = new List<Point2D>();
            foreach (Point point in points)
            {
                p2ds.Add(new Point2D(point.x, point.y));
            }
            return new Polygon2D(p2ds);
        }

        ////////////////////////////////////
        //TILE COLLISION SUBDIVISION SECTION
        ////////////////////////////////////

        public float getMinX()
        {
            return points.Min(p => p.x);
        }

        public float getMinY()
        {
            return points.Min(p => p.y);
        }
    }
}

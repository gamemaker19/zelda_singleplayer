using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZFG_CS
{
    public class Helpers
    {
        //shape1 MUST be convex
        public static CollideData shapesIntersect(Shape shape1, Shape shape2)
        {
            List<Point> intersectionPoints = new List<Point>();
            bool isRect = false;
            if (shape1.isRect() && shape2.isRect())
	        {
		        Rect rect1 = shape1.toRect();
		        Rect rect2 = shape2.toRect();
		
		        if (rect1.x2 <= rect2.x1 || rect1.y2 <= rect2.y1 || rect2.x2 <= rect1.x1 || rect2.y2 <= rect1.y1)
		        {
			        return null;
		        }
                isRect = true;
	        }

            if(!isRect)
            {
                var a = 0;
            }

            Polygon2D p1 = shape1.toPolygon();
            Polygon2D p2 = shape2.toPolygon();

            foreach(LineSegment2D edge1 in p1.Edges)
            {
                foreach(LineSegment2D edge2 in p2.Edges)
                {
                    if (edge1.TryIntersect(edge2, out Point2D intersection, Angle.FromDegrees(5)))
                    {
                        intersectionPoints.Add(intersection.toPoint());
                    }
                }
            }

            foreach (Point2D point in p1.Vertices)
            {
                if(p2.EnclosesPoint(point))
                {
                    intersectionPoints.Add(point.toPoint());
                }
            }
            foreach (Point2D point in p2.Vertices)
            {
                if (p1.EnclosesPoint(point))
                {
                    intersectionPoints.Add(point.toPoint());
                }
            }

            intersectionPoints = intersectionPoints.Distinct().ToList();

            List<Point> intersectionPointsClone = new List<Point>(intersectionPoints);
            //There must be at least one intersection point that isn't part of one of the shapes
            //The only exception is if ALL the intersection points are in one of the shapes, AND there are 3 or more points, so removeCount is tracked for that purpose
            int removeCount = 0;
            for(int i = intersectionPointsClone.Count - 1; i >= 0; i--)
            {
                if(p1.Vertices.Any(v => v.toPoint() == intersectionPointsClone[i]) ||
                   p2.Vertices.Any(v => v.toPoint() == intersectionPointsClone[i]))
                {
                    intersectionPointsClone.RemoveAt(i);
                    removeCount++;
                }
            }

            if (intersectionPointsClone.Count == 0 && (intersectionPoints.Count < 3 || intersectionPoints.Count != removeCount))
            {
                return null;
            }

            Point normal = shape1.center().dirTo(shape2.center());
            return new CollideData(intersectionPoints, normal);
        }

        public static CollideData rectIntersectsLine(Rect rect, Line line, Point origin)
        {
            List<Point> intersectionPoints = new List<Point>();
            LineSegment2D edge = new LineSegment2D(new Point2D(line.point1.x, line.point1.y), new Point2D(line.point2.x, line.point2.y));

            Polygon2D p1 = rect.toShape().toPolygon();
            foreach (LineSegment2D edge1 in p1.Edges)
            {
                if (edge1.TryIntersect(edge, out Point2D intersection, Angle.FromDegrees(5)))
                {
                    intersectionPoints.Add(intersection.toPoint());
                }
            }

            if (intersectionPoints.Count == 0) return null;

            Point closestPoint = Point.Zero;
            float closestDist = 0;
            for (int i = 0; i < intersectionPoints.Count; i++)
            {
                float dist = intersectionPoints[i].distTo(origin);
                if (i == 0 || dist < closestDist)
                {
                    closestDist = dist;
                    closestPoint = intersectionPoints[i];
                }
            }

            CollideData retCollideData = new CollideData(intersectionPoints, closestPoint.dirTo(origin));
            retCollideData.intersectionPoints.Clear();
            retCollideData.intersectionPoints.Add(closestPoint);

            return retCollideData;
        }

        public static float getIntersectArea(Rect rect1, Rect rect2)
        {
            Shape s1 = rect1.toShape();
            Shape s2 = rect2.toShape();
            var collideData = shapesIntersect(s1, s2);
            if (collideData == null) return 0;
            Shape intersectShape = new Shape(collideData.intersectionPoints);
            Rect intersectRect = intersectShape.toRect();
            return intersectRect.area();
        }

        public static Rect getBoundingBox(List<Shape> shapes)
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            foreach (Shape shape in shapes)
            {
                foreach (Point point in shape.points)
                {
                    if (point.x < minX) minX = point.x;
                    if (point.y < minY) minY = point.y;
                    if (point.x > maxX) maxX = point.x;
                    if (point.y > maxY) maxY = point.y;
                }
            }
            return new Rect(minX, minY, maxX, maxY);
        }

        public static Point dirToVec(Direction dir)
        {
            if (dir == Direction.Up) return new Point(0, -1);
            if (dir == Direction.Down) return new Point(0, 1);
            if (dir == Direction.Left) return new Point(-1, 0);
            if (dir == Direction.Right) return new Point(1, 0);
            if (dir == Direction.UpLeft) return new Point(-1, -1);
            if (dir == Direction.UpRight) return new Point(1, -1);
            if (dir == Direction.DownLeft) return new Point(-1, 1);
            if (dir == Direction.DownRight) return new Point(1, 1);
            return new Point(0, 0);
        }

        public static Direction vecToDir(Point dir)
        {
            if (dir.x < 0 && dir.y < 0) return Direction.UpLeft;
            if (dir.x > 0 && dir.y < 0) return Direction.UpRight;
            if (dir.x < 0 && dir.y > 0) return Direction.DownLeft;
            if (dir.x > 0 && dir.y > 0) return Direction.DownRight;
            if (dir.x < 0 && dir.y == 0) return Direction.Left;
            if (dir.x > 0 && dir.y == 0) return Direction.Right;
            if (dir.x == 0 && dir.y < 0) return Direction.Up;
            if (dir.x == 0 && dir.y > 0) return Direction.Down;
            return Direction.Right;
        }

        public static float dirToAngle(Direction dir)
        {
            if (dir == Direction.Up) return 270;
            if (dir == Direction.Down) return 90;
            if (dir == Direction.Left) return 180;
            if (dir == Direction.Right) return 0;
            if (dir == Direction.UpLeft) return 225;
            if (dir == Direction.UpRight) return 315;
            if (dir == Direction.DownLeft) return 135;
            if (dir == Direction.DownRight) return 45;
            return 0;
        }

        public static int clampInt(int val, int min, int max)
        {
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }

        public static float clamp(float val, float min, float max)
        {
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }

        public static float clampMin(float val, float min)
        {
            if (val < min) return min;
            return val;
        }

        public static float clampMax(float val, float max)
        {
            if (val > max) return max;
            return val;
        }

        public static float clamp01(float val)
        {
            return clamp(val, 0, 1);
        }

        public static void drawTextStd(string textStr, float x, float y, Alignment alignment = Alignment.Left)
        {
            Global.window.SetView(Global.fontView);
            x *= 4;
            y *= 4;
            /*
            al_set_target_bitmap(global.fontBuffer);
            ALLEGRO_COLOR outlineColor = al_map_rgb(0, 0, 115);
            al_hold_bitmap_drawing(true);
            int i = 3;
            {
                al_draw_text(global.font, outlineColor, x - i, y - i, alignment, textStr.c_str());
                al_draw_text(global.font, outlineColor, x - i, y + i, alignment, textStr.c_str());
                al_draw_text(global.font, outlineColor, x + i, y - i, alignment, textStr.c_str());
                al_draw_text(global.font, outlineColor, x + i, y + i, alignment, textStr.c_str());

                al_draw_text(global.font, outlineColor, x + i, y, alignment, textStr.c_str());
                al_draw_text(global.font, outlineColor, x - i, y, alignment, textStr.c_str());
                al_draw_text(global.font, outlineColor, x, y + i, alignment, textStr.c_str());
                al_draw_text(global.font, outlineColor, x, y - i, alignment, textStr.c_str());
            }
            al_draw_text(global.font, al_color_name("white"), x, y, alignment, textStr.c_str());
            al_hold_bitmap_drawing(false);
            al_set_target_bitmap(global.buffer);
            */
            Text text = new Text(textStr, Global.font, 48);
            text.Position = new Vector2f(x, y);
            text.FillColor = Color.White;
            text.OutlineColor = new Color(0, 0, 115);
            text.OutlineThickness = 4;
            
            if (alignment == Alignment.Center)
            {
                FloatRect bounds = text.GetLocalBounds();
                text.Position = new Vector2f(x - bounds.Width/2, y);
            }
            else if (alignment == Alignment.Right)
            {
                FloatRect bounds = text.GetLocalBounds();
                text.Position = new Vector2f(x - bounds.Width, y);
            }

            Global.window.Draw(text);
            Global.window.SetView(Global.view);
        }

        public static void drawTextUI(string textStr, float x, float y, Alignment alignment = Alignment.Left)
        {
            Global.window.SetView(Global.fontView);
            x *= 4;
            y *= 4;

            Text text = new Text(textStr, Global.font, 32);
            text.Position = new Vector2f(x, y);
            text.FillColor = Color.White;
            text.OutlineColor = Color.Black;
            text.OutlineThickness = 3;

            if (alignment == Alignment.Center)
            {
                FloatRect bounds = text.GetLocalBounds();
                text.Position = new Vector2f(x - bounds.Width / 2, y);
            }
            else if (alignment == Alignment.Right)
            {
                FloatRect bounds = text.GetLocalBounds();
                text.Position = new Vector2f(x - bounds.Width, y);
            }

            Global.window.Draw(text);
            Global.window.SetView(Global.view);
        }

        public static int getGridCoordKey(short x, short y)
        {
            return x << 16 | y;
        }
        
        public static Point getRandPointInCircle(Point center, float radius, float insideCircleRadius)
        {
            radius -= insideCircleRadius;
            int randRadius = randomRange(0, (int)radius);
            int randAngle = randomRange(0, 360);

            return center + new Point(randRadius * Mathf.Cos(randAngle), randRadius * Mathf.Sin(randAngle));
        }

        public static int randomRange(int start, int end)
        {
            Random rnd = new Random();
            int rndNum = rnd.Next(start, end + 1);
            return rndNum;
        }

        public static List<T> RepeatedDefault<T>(int count)
        {
            return Repeated(default(T), count);
        }

        public static List<T> Repeated<T>(T value, int count)
        {
            List<T> ret = new List<T>(count);
            ret.AddRange(Enumerable.Repeat(value, count));
            return ret;
        }

        public static Shader cloneShader(string name)
        {
            //Console.WriteLine(Global.shaderCodes[name]);
            byte[] byteArray = Encoding.ASCII.GetBytes(Global.shaderCodes[name]);
            MemoryStream stream = new MemoryStream(byteArray);
            return new Shader(null, null, stream);
        }

    }

}

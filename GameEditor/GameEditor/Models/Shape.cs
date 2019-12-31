using System.Collections.Generic;

namespace GameEditor.Models
{
    public class Shape
    {
        public List<Point> points { get; set; }

        public Shape(List<Point> points)
        {
            this.points = points;
        }

        public Shape clone(float x, float y)
        {
            var points = new List<Point>();
            for (var i = 0; i < this.points.Count; i++)
            {
                var point = this.points[i];
                points.Add(new Point(point.x + x, point.y + y));
            }
            return new Shape(points);
        }
    }
}

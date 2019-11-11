using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor_CS.Models
{
    public class Shape
    {
        List<Point> points;

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

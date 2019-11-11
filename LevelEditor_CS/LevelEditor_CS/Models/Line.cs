using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor_CS.Models
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

        public float x1 { get { return this.point1.x; } }
        public float y1 { get { return this.point1.y; } }
        public float x2 { get { return this.point2.x; } }
        public float y2 { get { return this.point2.y; } }
    }
}

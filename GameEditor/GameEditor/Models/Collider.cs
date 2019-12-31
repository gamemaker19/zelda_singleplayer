using System.Collections.Generic;

namespace GameEditor.Models
{
    public class Collider
    {
        public Shape _shape { get; set; }
        public bool isTrigger { get; set; }
        public bool wallOnly { get; set; } = false;
        public bool isClimbable { get; set; } = true;
        //gameObject: GameObject;
        public Point offset { get; set; }
        public bool isStatic { get; set; } = false;
        public int flag { get; set; } = 0;

        public Collider(List<Point> points, bool isTrigger, bool isClimbable, bool isStatic, int flag, Point offset)
        {
            this._shape = new Shape(points);
            this.isTrigger = isTrigger;
            //this.gameObject = gameObject;
            this.isClimbable = isClimbable;
            this.isStatic = isStatic;
            this.flag = flag;
            this.offset = offset;
        }

        public Shape shape()
        {
            var offset = new Point(0, 0);
            return this._shape.clone(offset.x, offset.y);
        }
    }
}

using System.Collections.Generic;

namespace GameEditor.Models
{
    public class Collider
    {
        Shape _shape;
        public bool isTrigger;
        public bool wallOnly = false;
        public bool isClimbable = true;
        //gameObject: GameObject;
        public Point offset;
        public bool isStatic = false;
        public int flag = 0;

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

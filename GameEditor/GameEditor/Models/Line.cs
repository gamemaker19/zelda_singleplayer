namespace GameEditor.Models
{
    public class Line
    {
        public Point point1 { get; set; }
        public Point point2 { get; set; }
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

using MathNet.Spatial.Euclidean;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFG_CS
{
    public static class Extensions
    {
        public static Point toPoint(this Vector2f vector2f)
        {
            return new Point(vector2f.X, vector2f.Y);
        }

        public static Point toPoint(this Point2D point2d)
        {
            return new Point((float)point2d.X, (float)point2d.Y);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Mathf
    {
        public static float PI = (float)Math.PI;

        public static float Ceil(float num)
        {
            return (float)Math.Ceiling(num);
        }

        public static float Floor(float num)
        {
            return (float)Math.Floor(num);
        }

        public static float Abs(float num)
        {
            return (float)Math.Abs(num);
        }

        public static int Sign(float num)
        {
            return Math.Sign(num);
        }

        public static float RadToDeg(float rad)
        {
            return rad * 180 / PI;
        }

        public static float DegToRad(float deg)
        {
            return deg * PI / 180;
        }

        public static float Sin(float ang)
        {
            float rads = DegToRad(ang);
            return (float)Math.Sin(rads);
        }

        public static float Cos(float ang)
        {
            float rads = DegToRad(ang);
            return (float)Math.Cos(rads);
        }

        public static float Tan(float ang)
        {
            float rads = DegToRad(ang);
            return (float)Math.Tan(rads);
        }

        public static float Atan(float tan)
        {
            float rads = (float)Math.Atan(tan);
            return RadToDeg(rads);
        }

        public static float Sqrt(float num)
        {
            return (float)Math.Sqrt(num);
        }

        /*
        public static float Atan2(float tan)
        {
            float rads = (float)Math.Atan2(tan);
            return RadToDeg(rads);
        }
        */

        public static int RoundInt(float f)
        {
            return (int)Math.Round(f);
        }
    }
}

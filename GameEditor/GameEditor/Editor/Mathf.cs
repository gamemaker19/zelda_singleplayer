using System;

namespace GameEditor.Editor
{
    public class Mathf
    {
        public static float Floor(float val)
        {
            return (float)Math.Floor(val);
        }

        public static float Ceil(float val)
        {
            return (float)Math.Ceiling(val);
        }

        public static int Round(float val)
        {
            return (int)Math.Round(val);
        }
    }
}

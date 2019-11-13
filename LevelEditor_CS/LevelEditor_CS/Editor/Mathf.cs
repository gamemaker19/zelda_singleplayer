using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor_CS.Editor
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

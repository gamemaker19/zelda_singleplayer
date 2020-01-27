﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEditor.Editor
{
    public class Layer
    {
        public Bitmap bitmap { get; set; }

        public bool isHidden { get; set; }

        public Layer(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }
    }
}

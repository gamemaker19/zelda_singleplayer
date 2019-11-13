using LevelEditor_CS.Editor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor_CS.Models
{
    public class Spritesheet
    {
        public Bitmap image;
        public string path = "";
        public List<List<PixelData>> imgArr;

        public Spritesheet(string path)
        {
            this.path = path;       
        }

        public void init()
        {
            if (image == null)
            {
                image = new Bitmap(path);
                imgArr = Helpers.get2DArrayFromImage(image);
            }
        }

        public string getName()
        {
            return Path.GetFileNameWithoutExtension(path);
        }
    }
}

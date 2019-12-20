﻿using GameEditor.Editor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace GameEditor.Models
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

        public void init(bool getArray)
        {
            if (image == null)
            {
                image = new Bitmap(path);
                if (getArray)
                {
                    imgArr = Helpers.get2DArrayFromImage(image);
                }
            }
        }

        [JsonIgnore]
        public string name
        {
            get
            {
                return getName();
            }
        }

        public string getName()
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public string getBasePath()
        {
            string basePath = "assets/" + this.path.Split(new string[] { "assets/" }, StringSplitOptions.None)[1];
            return basePath;
        }

        public override string ToString()
        {
            return name;
        }
    }
}

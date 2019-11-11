using LevelEditor_CS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor_CS.Editor
{
    public class Ghost
    {
        public Sprite sprite;
        public Frame frame;
        public Ghost(Sprite sprite, Frame frame)
        {
            this.sprite = sprite;
            this.frame = frame;
        }
    }
}

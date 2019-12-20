using GameEditor.Models;

namespace GameEditor.Editor
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

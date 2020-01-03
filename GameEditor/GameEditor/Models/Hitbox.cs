using GameEditor.Editor;
using Newtonsoft.Json;
using System.ComponentModel;

namespace GameEditor.Models
{
    public class Hitbox : Selectable, INotifyPropertyChanged
    {
        public string tags { get; set; }
        public float width { get; set; }
        public float height { get; set; }
        public Point offset { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private bool _isSelected;
        [JsonIgnore]
        public bool isSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged("isSelected");
            }
        }

        public Hitbox()
        {
            this.tags = "";
            this.width = 20;
            this.height = 40;
            this.offset = new Point(0, 0);
        }

        public void move(float deltaX, float deltaY)
        {
            this.offset.x += deltaX;
            this.offset.y += deltaY;
        }

        public void resizeCenter(float w, float h)
        {
            this.width += w;
            this.height += h;
        }

        public Rect getRect
        {
            get
            {
                return new Rect(this.offset.x, this.offset.y, this.offset.x + this.width, this.offset.y + this.height);
            }
        }

    }
}

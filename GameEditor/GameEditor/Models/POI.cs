using GameEditor.Editor;
using Newtonsoft.Json;
using System.ComponentModel;

namespace GameEditor.Models
{
    public class POI : Selectable, INotifyPropertyChanged
    {
        public string tags { get; set; }
        public float x { get; set; }
        public float y { get; set; }

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

        public POI(string tags, float x, float y)
        {
            this.tags = tags;
            this.x = x;
            this.y = y;
        }

        public void move(float deltaX, float deltaY)
        {
            this.x += deltaX;
            this.y += deltaY;
        }

        public void resizeCenter(float w, float h)
        {
        }

        public Rect getRect
        {
            get
            {
                return new Rect(this.x - 2, this.y - 2, this.x + 2, this.y + 2);
            }
        }
    }
}

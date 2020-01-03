using GameEditor.Editor;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace GameEditor.Models
{
    [Serializable]
    public class Frame : Selectable, INotifyPropertyChanged
    {
        public Rect rect { get; set; }

        private float _duration;
        public float duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
                NotifyPropertyChanged("duration");
            }
        }

        public Point offset { get; set; }
        public ObservableCollection<Hitbox> hitboxes { get; set; }
        public ObservableCollection<POI> POIs { get; set; }
        public ObservableCollection<Frame> childFrames { get; set; } = new ObservableCollection<Frame>();
        public int parentFrameIndex { get; set; }
        public float zIndex { get; set; } = 0;
        public int xDir { get; set; } = 1;
        public int yDir { get; set; } = 1;
        public string tags { get; set; } = "";

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

        [JsonIgnore]
        public bool isChild
        {
            get
            {
                return parentFrameIndex > -1;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public Frame(Rect rect, float duration, Point offset)
        {
            this.rect = rect;
            this.duration = duration;
            this.offset = offset;
            this.hitboxes = new ObservableCollection<Hitbox>();
            this.POIs = new ObservableCollection<POI>();
        }

        [OnDeserialized]
        public void onDeserialize(StreamingContext context)
        {
            if (parentFrameIndex == 0)
            {
                parentFrameIndex = -1;
            }
        }

        public void move(float deltaX, float deltaY)
        {
        }

        public void resizeCenter(float w, float h)
        {
        }

        public Rect getRect
        {
            get { return rect; }
        }
    }
}
using SFML.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZFG_CS
{
    public class MusicWrapper
    {
        public Music music;
        public float startPos;
        public float endPos;
        public string name;

        private float _volume = 100;
        public float volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
                updateVolume();
            }
        }

        public MusicWrapper(string musicPath)
        {
            music = new Music(musicPath);
            name = Path.GetFileNameWithoutExtension(musicPath);
            music.Loop = false;
        }

        public MusicWrapper(string musicPath, double startPos, double endPos)
        {
            music = new Music(musicPath);
            name = Path.GetFileNameWithoutExtension(musicPath);
            this.startPos = (float)(startPos);
            this.endPos = (float)(endPos);
            music.Loop = true;
        }

        public void play()
        {
            music.Play();
            while (music.Status != SoundStatus.Playing) { }
        }

        public void update()
        {
            if(music.Loop && music.PlayingOffset.AsSeconds() > endPos)
            {
                music.PlayingOffset = SFML.System.Time.FromSeconds(startPos);
            }
        }

        public void setNearEnd()
        {
            music.PlayingOffset = SFML.System.Time.FromSeconds(endPos - 1);
        }

        public void updateVolume()
        {
            music.Volume = volume * Options.main.musicVolume;
        }
    }
}

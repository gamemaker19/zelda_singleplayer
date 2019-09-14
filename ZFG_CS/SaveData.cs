using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZFG_CS
{
    public class SaveData
    {
        public int wins = 0;
        public string skin = "link2";
        
        private static SaveData _saveData;
        public static SaveData saveData
        {
            get
            {
                if(_saveData == null)
                {
                    string text = null;
                    if (File.Exists("saveData.txt"))
                    {
                        text = File.ReadAllText("saveData.txt");
                    }
                    if (string.IsNullOrEmpty(text))
                    {
                        _saveData = new SaveData();
                    }
                    else
                    {
                        _saveData = JsonConvert.DeserializeObject<SaveData>(text);
                    }
                }
                return _saveData;
            }
        }

        //Does nothing, just dummy method to load singleton
        public void load()
        {

        }

        public void incWins()
        {
            wins++;
            save();
        }

        public void setSkin(string skin)
        {
            this.skin = skin;
            save();
        }

        public void save()
        {
            string text = JsonConvert.SerializeObject(_saveData);
            File.WriteAllText("saveData.txt", text);
        }
    }

}

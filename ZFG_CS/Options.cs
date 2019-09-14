using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZFG_CS
{
    public class Options
    {
        public int numCPUs = 99;
        public bool enableMasterSword = true;
        public float musicVolume = 1;
        public float soundVolume = 1;

        private static Options _main;
        public static Options main
        {
            get
            {
                if (_main == null)
                {
                    string text = "";
                    if (File.Exists("options.txt"))
                    {
                        text = File.ReadAllText("options.txt");
                    }
                    if (string.IsNullOrEmpty(text))
                    {
                        _main = new Options();
                    }
                    else
                    {
                        _main = JsonConvert.DeserializeObject<Options>(text);
                    }
                }
                return _main;
            }
        }

        public void saveToFile()
        {
            string text = JsonConvert.SerializeObject(_main);
            File.WriteAllText("options.txt", text);
        }
    }
}

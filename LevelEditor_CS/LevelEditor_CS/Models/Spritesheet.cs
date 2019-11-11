using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor_CS.Models
{
    public class Spritesheet
    {
        //imgEl: HTMLImageElement = undefined;
        public string path = "";

        public Spritesheet(string path)
        {
            this.path = path;
        }

        public string getName()
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /*
        get imgArr()
        {
            //@ts-ignore
            return window.imgArrMap[this.path];
        }

        set imgArr(imgArr: any)
        {
            //@ts-ignore
            if (!window.imgArrMap) window.imgArrMap = { };
            //@ts-ignore
            window.imgArrMap[this.path] = imgArr;
        }

        loadImage(callback: Function)
        {
            this.imgEl = document.createElement("img");
            this.imgEl.onload = () => {
                callback();
            };
            this.imgEl.src = this.path;
        }
        */
    }
}

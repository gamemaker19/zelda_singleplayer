using GameEditor.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace GameEditor.Editor
{
    public class PixelData
    {
        public float x;
        public float y;
        public Color rgb;
        public List<PixelData> neighbors;

        public PixelData(float x, float y, Color rgb, List<PixelData> neighbors)
        {
            this.x = x;
            this.y = y;
            this.rgb = rgb;
            this.neighbors = neighbors;
        }
    }

    public static class Helpers
    {
        public static List<Spritesheet> getSpritesheets(string folder)
        {
            var spritesheetsFiles = Directory.GetFiles(Consts.ASSETS_PATH + "/" + folder);
            List<Spritesheet> spritesheets = new List<Spritesheet>();
            foreach (string spritesheetFile in spritesheetsFiles)
            {
                string path = spritesheetFile.Replace('\\', '/');
                var spritesheet = new Spritesheet(path);
                spritesheets.Add(spritesheet);
            }
            return spritesheets;
        }

        public static List<Sprite> getSprites()
        {
            var spritefiles = Directory.GetFiles(Consts.ASSETS_PATH + "/sprites");
            List<Sprite> sprites = new List<Sprite>();
            foreach (string spriteFile in spritefiles)
            {
                string spriteJson = File.ReadAllText(spriteFile);
                var sprite = JsonConvert.DeserializeObject<Sprite>(spriteJson);
                sprites.Add(sprite);
            }
            return sprites;
        }

        public static List<Level> getLevels()
        {
            var levelFiles = Directory.GetFiles(Consts.ASSETS_PATH + "/levels");
            List<Level> levels = new List<Level>();
            foreach (string spriteFile in levelFiles)
            {
                string levelJson = File.ReadAllText(spriteFile);
                var level = JsonConvert.DeserializeObject<Level>(levelJson);
                levels.Add(level);
            }
            return levels;
        }

        public static List<TileData> getTileDatas()
        {
            string tileDataJson = File.ReadAllText(Consts.ASSETS_PATH + "/tiledatas.json");
            var tileDatas = JsonConvert.DeserializeObject<List<TileData>>(tileDataJson);
            return tileDatas;
        }

        public static void saveTileDatas(List<TileData> tileDatas)
        {
            string path = Consts.ASSETS_PATH + "/tiledatas.json";
            string json = JsonConvert.SerializeObject(tileDatas);
            File.WriteAllText(path, json);
        }

        public static void saveSprite(Sprite sprite)
        {
            string path = Consts.ASSETS_PATH + "/sprites/" + sprite.name + ".json";
            string json = JsonConvert.SerializeObject(sprite);
            File.WriteAllText(path, json);
        }

        public static void saveLevel(Level level)
        {
            string path = Consts.ASSETS_PATH + "/levels/" + level.name + ".json";
            string json = JsonConvert.SerializeObject(level);
            File.WriteAllText(path, json);
        }

        public static bool IsBinaryEqualTo(this object obj, object obj1)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                if (obj == null || obj1 == null)
                {
                    if (obj == null && obj1 == null)
                        return true;
                    else
                        return false;
                }

                BinaryFormatter binaryFormatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                binaryFormatter.Serialize(memStream, obj);
                byte[] b1 = memStream.ToArray();
                memStream.SetLength(0);

                binaryFormatter.Serialize(memStream, obj1);
                byte[] b2 = memStream.ToArray();

                if (b1.Length != b2.Length)
                    return false;

                for (int i = 0; i < b1.Length; i++)
                {
                    if (b1[i] != b2[i])
                        return false;
                }

                return true;
            }
        }

        public static string storageKeysPath = "storage.json";
        public static Dictionary<string, string> storageKeys = null;

        public static void setStorageKeyIfNull()
        {
            if (storageKeys == null)
            {
                if (!File.Exists(storageKeysPath))
                {
                    File.CreateText("storage.json");
                }
                string storageJson = File.ReadAllText(storageKeysPath);
                if (string.IsNullOrEmpty(storageJson))
                {
                    storageKeys = new Dictionary<string, string>();
                }
                else
                {
                    storageKeys = JsonConvert.DeserializeObject<Dictionary<string, string>>(storageJson);
                }
            }
        }

        public static string getStorageKey(string key)
        {
            setStorageKeyIfNull();
            return storageKeys.ContainsKey(key) ? storageKeys[key] : "";
        }

        public static void setStorageKey(string key, string val)
        {
            setStorageKeyIfNull();
            storageKeys[key] = val;
        }

        public static void saveStorageKeys()
        {
            setStorageKeyIfNull();
            string json = JsonConvert.SerializeObject(storageKeys);
            File.WriteAllText(storageKeysPath, json);
        }

        public static byte[] WriteObject<T>(T thingToSave)
        {
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(stream, thingToSave);
                bytes = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(bytes, 0, (int)stream.Length);
            }
            return bytes;

        }

        public static T ReadObject<T>(byte[] data)
        {
            T deserializedThing = default(T);

            using (var stream = new MemoryStream(data))
            using (var reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas()))
            {
                var serializer = new DataContractSerializer(typeof(T));

                // Deserialize the data and read it from the instance.
                deserializedThing = (T)serializer.ReadObject(reader, true);
            }
            return deserializedThing;
        }

        public static void drawRect(Graphics canvas, Rect rect, Color? fillColor, Color? strokeColor = null, int strokeWidth = 0, float fillAlpha = 1)
        {
            if (fillColor != null)
            {
                fillColor = Color.FromArgb((byte)(int)(255 * fillAlpha), fillColor.Value.R, fillColor.Value.G, fillColor.Value.B);
                SolidBrush fillBrush = new SolidBrush((Color)fillColor);
                canvas.FillRectangle(fillBrush, rect.x1, rect.y1, rect.w, rect.h);
            }

            if (strokeColor != null)
            {
                strokeColor = Color.FromArgb((byte)(int)(255 * fillAlpha), strokeColor.Value.R, strokeColor.Value.G, strokeColor.Value.B);
                SolidBrush strokeBrush = new SolidBrush((Color)strokeColor);
                canvas.DrawRectangle(new Pen(strokeBrush), rect.x1, rect.y1, rect.w, rect.h);
            }
        }

        public static void drawText(Graphics canvas, string text, float x, float y, Color? fillColor, int size)
        {
            FontFamily fontFamily = new FontFamily("Arial");
            Font font = new Font(fontFamily, size, FontStyle.Regular, GraphicsUnit.Pixel);
            canvas.DrawString(text, font, new SolidBrush((Color)fillColor), new PointF(x, y));
        }

        public static void drawCircle(Graphics canvas, float x, float y, float r, Color? fillColor, Color? lineColor = null, int lineThicknessfloat = 0)
        {
            if (fillColor != null)
            {
                SolidBrush fillBrush = new SolidBrush((Color)fillColor);
                canvas.FillEllipse(fillBrush, new Rectangle((int)(x - r), (int)(y - r), (int)(r * 2), (int)(r * 2)));
            }

            if (lineColor != null)
            {
                SolidBrush strokeBrush = new SolidBrush((Color)lineColor);
                canvas.DrawEllipse(new Pen(strokeBrush), new Rectangle((int)(x - r), (int)(y - r), (int)(r * 2), (int)(r * 2)));
            }
        }

        public static void drawPolygon(Graphics canvas, Shape shape, bool v1, Color? fillColor, Color? lineColor, int lineThickness = 1, float alpha = 1)
        {
            if (fillColor != null)
            {
                SolidBrush fillBrush = new SolidBrush((Color)fillColor);
                canvas.FillPolygon(fillBrush, shape.points.Select(point => new PointF(point.x, point.y)).ToArray());
            }

            if (lineColor != null)
            {
                SolidBrush strokeBrush = new SolidBrush((Color)lineColor);
                canvas.DrawPolygon(new Pen(strokeBrush), shape.points.Select(point => new PointF(point.x, point.y)).ToArray());
            }
        }

        public static void drawLine(Graphics canvas, float x, float y, float x2, float y2, Color? color, int thickness)
        {
            canvas.DrawLine(new Pen(new SolidBrush((Color)color), thickness), x, y, x2, y2);
        }

        public static void drawImage(Graphics canvas, Bitmap bitmap, float dx, float dy, float sx = 0, float sy = 0, float sw = -1, float sh = -1, int flipX = 1, int flipY = 1, string options = "", float alpha = 1, float scaleX = 1, float scaleY = 1)
        {
            if (sw == -1) sw = bitmap.Width;
            if (sh == -1) sh = bitmap.Height;

            if (flipX == -1)
            {
                dx += sw;
            }
            if (flipY == -1)
            {
                dy += sh;
            }

            Rectangle destRect = new Rectangle((int)dx, (int)dy, (int)(sw * flipX), (int)(sh * flipY));

            if (alpha == 1)
            {
                canvas.DrawImage(bitmap, destRect, sx, sy, sw, sh, GraphicsUnit.Pixel);
            }
            else
            {
                ColorMatrix cm = new ColorMatrix();
                cm.Matrix33 = alpha;
                ImageAttributes ia = new ImageAttributes();
                ia.SetColorMatrix(cm);
                canvas.DrawImage(bitmap, destRect, sx, sy, sw, sh, GraphicsUnit.Pixel, ia);
            }
        }

        public static List<List<PixelData>> get2DArrayFromImage(Bitmap bitmap)
        {
            var arr = new List<List<PixelData>>();

            for (int i = 0; i < bitmap.Height; i++)
            {
                var row = new List<PixelData>();
                arr.Add(row);
                for (int j = 0; j < bitmap.Width; j++)
                {
                    var pixel = bitmap.GetPixel(j, i);
                    row.Add(new PixelData(j, i, pixel, new List<PixelData>()));
                }
            }

            for (var i = 0; i < arr.Count; i++)
            {
                for (var j = 0; j < arr[i].Count; j++)
                {
                    arr[i][j].neighbors.Add(get2DArrayEl(arr, i - 1, j - 1));
                    arr[i][j].neighbors.Add(get2DArrayEl(arr, i - 1, j));
                    arr[i][j].neighbors.Add(get2DArrayEl(arr, i - 1, j + 1));
                    arr[i][j].neighbors.Add(get2DArrayEl(arr, i, j - 1));
                    arr[i][j].neighbors.Add(get2DArrayEl(arr, i, j));
                    arr[i][j].neighbors.Add(get2DArrayEl(arr, i, j + 1));
                    arr[i][j].neighbors.Add(get2DArrayEl(arr, i + 1, j - 1));
                    arr[i][j].neighbors.Add(get2DArrayEl(arr, i + 1, j));
                    arr[i][j].neighbors.Add(get2DArrayEl(arr, i + 1, j + 1));
                    arr[i][j].neighbors.RemoveAll(p => p == null);
                }
            }

            return arr;
        }

        public static Rect getPixelClumpRect(float x, float y, List<List<PixelData>> imageArr)
        {
            int ix = Mathf.Round(x);
            int iy = Mathf.Round(y);
            var selectedNode = imageArr[iy][ix];
            if (selectedNode == null)
            {
                return null;
            }
            if (selectedNode.rgb.A == 0)
            {
                Console.WriteLine("Clicked transparent pixel");
                return null;
            }

            var queue = new List<PixelData>();
            queue.Add(selectedNode);

            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = -1f;
            var maxY = -1f;

            var num = 0;
            var visitedNodes = new HashSet<PixelData>();
            while (queue.Count > 0)
            {
                var node = queue[0];
                queue.RemoveAt(0);
                num++;
                if (node.x < minX) minX = node.x;
                if (node.y < minY) minY = node.y;
                if (node.x > maxX) maxX = node.x;
                if (node.y > maxY) maxY = node.y;

                foreach (var neighbor in node.neighbors)
                {
                    if (visitedNodes.Contains(neighbor)) continue;
                    if (!queue.Contains(neighbor))
                    {
                        queue.Add(neighbor);
                    }
                }
                visitedNodes.Add(node);
            }
            //console.log(num);
            return new Rect(Mathf.Round(minX), Mathf.Round(minY), Mathf.Round(maxX + 1), Mathf.Round(maxY + 1));

        }

        public static Rect getSelectedPixelRect(float x, float y, float endX, float endY, List<List<PixelData>> imageArr)
        {
            x = Mathf.Round(x);
            y = Mathf.Round(y);

            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = -1f;
            var maxY = -1f;

            for (int i = (int)y; i <= endY; i++)
            {
                for (int j = (int)x; j <= endX; j++)
                {
                    if (imageArr[i][j].rgb.A != 0)
                    {
                        if (i < minY) minY = i;
                        if (i > maxY) maxY = i;
                        if (j < minX) minX = j;
                        if (j > maxX) maxX = j;
                    }
                }
            }

            if (minX == float.MaxValue || minY == float.MaxValue || maxX == -1 || maxY == -1) return null;

            return new Rect(Mathf.Round(minX), Mathf.Round(minY), Mathf.Round(maxX + 1), Mathf.Round(maxY + 1));
        }

        public static PixelData get2DArrayEl(List<List<PixelData>> arr, int i, int j)
        {
            if (i < 0 || i >= arr.Count) return null;
            if (j < 0 || j >= arr[0].Count) return null;
            if (arr[i][j].rgb.A == 0) return null;
            return arr[i][j];
        }

        public static bool inRect(float x, float y, Rect rect)
        {
            var rx = rect.x1;
            var ry = rect.y1;
            var rx2 = rect.x2;
            var ry2 = rect.y2;
            return x >= rx && x <= rx2 && y >= ry && y <= ry2;
        }

        public static bool inCircle(float x, float y, float circleX, float circleY, float r)
        {
            if (Math.Sqrt(Math.Pow(x - circleX, 2) + Math.Pow(y - circleY, 2)) <= r)
            {
                return true;
            }
            return false;
        }

        public static T DeepClone<T>(this T a)
        {
            /*
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
            */
            string json = JsonConvert.SerializeObject(a);
            return JsonConvert.DeserializeObject<T>(json);
        }



        /*
        void toZero(float num, float inc, float dir)
        {
            if (dir === 1)
            {
                num -= inc;
                if (num < 0) num = 0;
                return num;
            }
            else if (dir === -1)
            {
                num += inc;
                if (num > 0) num = 0;
                return num;
            }
            else
            {
                throw "Must pass in -1 or 1 for dir";
            }
        }

        void incrementRange(float num, float min, float max)
        {
            num++;
            if (num >= max) num = min;
            return num;
        }

        void decrementRange(float num, float min, float max)
        {
            num--;
            if (num < min) num = max - 1;
            return num;
        }

        void clamp01(float num)
        {
            if (num < 0) num = 0;
            if (num > 1) num = 1;
            return num;
        }

        //Inclusive
        void randomRange(float start, float end)
        {
            return _.random(start, end);
        }

        void clampMax(float num, float max)
        {
            return num < max ? num : max;
        }

        void clampMin(float num, float min)
        {
            return num > min ? num : min;
        }

        void clampMin0(float num)
        {
            return clampMin(num, 0);
        }

        void clamp(float num, float min, float max)
        {
            if (num < min) return min;
            if (num > max) return max;
            return num;
        }

        void sin(float degrees)
        {
            let rads = degrees * Math.PI / 180;
            return Math.sin(rads);
        }

        void cos(float degrees)
        {
            let rads = degrees * Math.PI / 180;
            return Math.cos(rads);
        }

        void atan(float value)
        {
            return Math.atan(value) * 180 / Math.PI;
        }

        void moveTo(float num, float dest, float inc)
        {
            inc *= Math.sign(dest - num);
            num += inc;
            return num;
        }

        void lerp(float num, float dest, float timeScale)
        {
            num = num + (dest - num) * timeScale;
            return num;
        }

        void lerpNoOver(float num, float dest, float timeScale)
        {
            num = num + (dest - num) * timeScale;
            if (Math.abs(num - dest) < 1) num = dest;
            return num;
        }

        //Expects angle and destAngle to be > 0 and < 360
        void lerpAngle(float angle, float destAngle, float timeScale)
        {
            let dir = 1;
            if (Math.abs(destAngle - angle) > 180)
            {
                dir = -1;
            }
            angle = angle + dir * (destAngle - angle) * timeScale;
            return to360(angle);
        }

        void to360(float angle)
        {
            if (angle < 0) angle += 360;
            if (angle > 360) angle -= 360;
            return angle;
        }

        void getHex(float r, float g, float b, float a)
        {
            return "#" + r.toString(16) + g.toString(16) + b.toString(16) + a.toString(16);
        }

        void roundEpsilon(float num)
        {
            let numRound = Math.round(num);
            let diff = Math.abs(numRound - num);
            if (diff < 0.0001)
            {
                return numRound;
            }
            return num;
        }

        let autoInc = 0;
        void getAutoIncId()
        {
            autoInc++;
            return autoInc;
        }

        void stringReplace(str: string, pattern: string, replacement: string)
        {
            return str.replace(new RegExp(pattern, 'g'), replacement);
        }

        void noCanvasSmoothing(c: CanvasRenderingContext2D)
        {
            c.webkitImageSmoothingEnabled = false;
            c.mozImageSmoothingEnabled = false;
            c.imageSmoothingEnabled = false; /// future
        }

        let helperCanvas = document.createElement("canvas");
        let helperCtx = helperCanvas.getContext("2d");
        noCanvasSmoothing(helperCtx);

        let helperCanvas2 = document.createElement("canvas");
        let helperCtx2 = helperCanvas2.getContext("2d");
        noCanvasSmoothing(helperCtx2);

        let helperCanvas3 = document.createElement("canvas");
        let helperCtx3 = helperCanvas3.getContext("2d");
        noCanvasSmoothing(helperCtx3);

        void drawPolygon(ctx: CanvasRenderingContext2D, shape: Shape, closed: boolean, fillColor?: string, lineColor?: string, lineThicknessfloat ?, fillAlphafloat ?) : void {

          let vertices = shape.points;

          if(fillAlpha) {
            ctx.globalAlpha = fillAlpha;
          }

          ctx.beginPath();
          ctx.moveTo(vertices[0].x, vertices[0].y);

          for(let float i = 1; i<vertices.length; i++) {
              ctx.lineTo(vertices[i].x, vertices[i].y);
          }

          if(closed) {
              ctx.closePath();

              if(fillColor) {
                  ctx.fillStyle = fillColor;
                  ctx.fill();
              }
          }

          if(lineColor) {
              ctx.lineWidth = lineThickness;
              ctx.strokeStyle = lineColor;
              ctx.stroke();
          }

          ctx.globalAlpha = 1;
        }

        void linepointNearestMouse(float x0, float y0, float x1, float y1, float x, float y) : Point {
          function lerp(float a, float b, float x):number{ return(a+x* (b-a)); };
          let float dx=x1-x0;
          let float dy=y1-y0;
          let float t = ((x-x0)* dx+(y-y0)* dy)/(dx* dx+dy* dy);
          let float lineX = lerp(x0, x1, t);
        let float lineY = lerp(y0, y1, t);
          return new Point(lineX, lineY);
        }

        void inLine(float mouseX, float mouseY, float x0, float y0, float x1, float y1) : boolean {

          let float threshold = 4;

          let float small_x = Math.min(x0, x1);
          let float big_x = Math.max(x0, x1);

          if(mouseX<small_x - (threshold*0.5) || mouseX > big_x + (threshold*0.5)){
            return false;
          }

          let linepoint: Point = linepointNearestMouse(x0, y0, x1, y1, mouseX, mouseY);
        let float dx = mouseX - linepoint.x;
          let float dy = mouseY - linepoint.y;
          let float distance = Math.abs(Math.sqrt(dx* dx+dy* dy));
          if(distance<threshold) {
            return true;
          }
          else {
            return false;
          }
        }

        void getInclinePushDir(inclineNormal: Point, pushDir: Point)
        {
            let bisectingPoint = inclineNormal.normalize().add(pushDir.normalize());
            bisectingPoint = bisectingPoint.normalize();
            //Snap to the nearest axis
            if (Math.abs(bisectingPoint.x) >= Math.abs(bisectingPoint.y))
            {
                bisectingPoint.y = 0;
            }
            else
            {
                bisectingPoint.x = 0;
            }
            return bisectingPoint.normalize();
        }

        void keyCodeToString(float charCode)
        {

            if (charCode === 0) return "left mouse";
            if (charCode === 1) return "middle mouse";
            if (charCode === 2) return "right mouse";
            if (charCode === 3) return "wheel up";
            if (charCode === 4) return "wheel down";

            if (charCode == 8) return "backspace"; //  backspace
            if (charCode == 9) return "tab"; //  tab
            if (charCode == 13) return "enter"; //  enter
            if (charCode == 16) return "shift"; //  shift
            if (charCode == 17) return "ctrl"; //  ctrl
            if (charCode == 18) return "alt"; //  alt
            if (charCode == 19) return "pause/break"; //  pause/break
            if (charCode == 20) return "caps lock"; //  caps lock
            if (charCode == 27) return "escape"; //  escape
            if (charCode == 33) return "page up"; // page up, to avoid displaying alternate character and confusing people	         
            if (charCode == 34) return "page down"; // page down
            if (charCode == 35) return "end"; // end
            if (charCode == 36) return "home"; // home
            if (charCode == 37) return "left arrow"; // left arrow
            if (charCode == 38) return "up arrow"; // up arrow
            if (charCode == 39) return "right arrow"; // right arrow
            if (charCode == 40) return "down arrow"; // down arrow
            if (charCode == 45) return "insert"; // insert
            if (charCode == 46) return "delete"; // delete
            if (charCode == 91) return "left window"; // left window
            if (charCode == 92) return "right window"; // right window
            if (charCode == 93) return "select key"; // select key
            if (charCode == 96) return "numpad 0"; // numpad 0
            if (charCode == 97) return "numpad 1"; // numpad 1
            if (charCode == 98) return "numpad 2"; // numpad 2
            if (charCode == 99) return "numpad 3"; // numpad 3
            if (charCode == 100) return "numpad 4"; // numpad 4
            if (charCode == 101) return "numpad 5"; // numpad 5
            if (charCode == 102) return "numpad 6"; // numpad 6
            if (charCode == 103) return "numpad 7"; // numpad 7
            if (charCode == 104) return "numpad 8"; // numpad 8
            if (charCode == 105) return "numpad 9"; // numpad 9
            if (charCode == 106) return "multiply"; // multiply
            if (charCode == 107) return "add"; // add
            if (charCode == 109) return "subtract"; // subtract
            if (charCode == 110) return "decimal point"; // decimal point
            if (charCode == 111) return "divide"; // divide
            if (charCode == 112) return "F1"; // F1
            if (charCode == 113) return "F2"; // F2
            if (charCode == 114) return "F3"; // F3
            if (charCode == 115) return "F4"; // F4
            if (charCode == 116) return "F5"; // F5
            if (charCode == 117) return "F6"; // F6
            if (charCode == 118) return "F7"; // F7
            if (charCode == 119) return "F8"; // F8
            if (charCode == 120) return "F9"; // F9
            if (charCode == 121) return "F10"; // F10
            if (charCode == 122) return "F11"; // F11
            if (charCode == 123) return "F12"; // F12
            if (charCode == 144) return "num lock"; // num lock
            if (charCode == 145) return "scroll lock"; // scroll lock
            if (charCode == 186) return ";"; // semi-colon
            if (charCode == 187) return "="; // equal-sign
            if (charCode == 188) return ","; // comma
            if (charCode == 189) return "-"; // dash
            if (charCode == 190) return "."; // period
            if (charCode == 191) return "/"; // forward slash
            if (charCode == 192) return "`"; // grave accent
            if (charCode == 219) return "["; // open bracket
            if (charCode == 220) return "\\"; // back slash
            if (charCode == 221) return "]"; // close bracket
            if (charCode == 222) return "'"; // single quote
            if (charCode == 32) return "space";
            return String.fromCharCode(charCode);
        }

        void deserializeES6(obj: any)
        {
            if (Array.isArray(obj))
            {
                for (var i = 0; i < obj.length; i++)
                {
                    obj[i] = deserializeES6(obj[i]);
                }
            }
            else if (typeof obj === "object")
            {

                let className: string = obj.className;
                if (className)
                {
                    //@ts-ignore
                    //var tempObj = Object.create(window[className].prototype);
                    let tempObj = createClassFromName(className);
                    Object.assign(tempObj, obj);
                    obj = tempObj;
                }

                if (obj.onDeserialize)
                {
                    obj.onDeserialize();
                }

            for(var key in obj)
                {
                    if (!obj.hasOwnProperty(key)) continue;
                    obj[key] = deserializeES6(obj[key]);
                }
            }
            if (typeof obj === "string" && $.isNumeric(obj)) {
                obj = Number(obj);
            }
            if (typeof obj === "string" && obj === "true")
            {
                obj = true;
            }
            if (typeof obj === "string" && obj === "false")
            {
                obj = false;
            }
            return obj;

        }

        void serializeES6(obj: any)
        {

            var retStr = "";

            if (Array.isArray(obj))
            {
                retStr += "[";
                for (var i = 0; i < obj.length; i++)
                {
                    retStr += serializeES6(obj[i]) + ",";
                }
                if (retStr[retStr.length - 1] === ",") retStr = retStr.substring(0, retStr.length - 1);
                retStr += "]";
            }
            else if (typeof obj === "object")
            {
                if (obj.onSerialize)
                {
                    obj.onSerialize();
                }
                retStr += "{";
                let nonSerializedKeys = obj.getNonSerializedKeys ? obj.getNonSerializedKeys() : [];
            for(var key in obj)
                {
                    if (nonSerializedKeys.indexOf(key) > -1) continue;
                    if (!obj.hasOwnProperty(key)) continue;
                    retStr += '"' + key + '":' + serializeES6(obj[key]) + ",";
                }
                retStr += '"className":"' + obj.constructor.name + '"}';
            }
            else
            {
                if (obj === undefined || obj === undefined || obj === "")
                {
                    retStr += '""';
                }
                else if (isNaN(obj))
                {
                    retStr = '"' + String(obj) + '"';
                }
                else
                {
                    retStr = String(obj);
                }
            }
            return retStr;
        }
        */

        public static List<List<T>> make2DArray<T>(int rowCount, int colCount, T val)
        {
            var retList = new List<List<T>>();
            for (int i = 0; i < rowCount; i++)
            {
                retList.Add(null);
            }
            for (int i = 0; i < retList.Count; i++)
            {
                retList[i] = new List<T>();
                for (int j = 0; j < colCount; j++)
                {
                    retList[i].Add(val);
                }
            }
            return retList;
        }

        public static string baseName(string filepath)
        {
            var baseNameStr = filepath.Substring(filepath.LastIndexOf('/') + 1);
            if (baseNameStr.LastIndexOf(".") != -1)
                baseNameStr = baseNameStr.Substring(0, baseNameStr.LastIndexOf("."));
            return baseNameStr;
        }

        public static List<T> getEnumList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

    }
}

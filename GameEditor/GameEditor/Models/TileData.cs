using GameEditor.Editor;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GameEditor.Models
{
    public enum HitboxMode
    {
        None = 0,
        Tile = 1,
        BoundingRect = 2,
        DiagTopLeft = 3,
        DiagTopRight = 4,
        DiagBotLeft = 5,
        DiagBotRight = 6,
        Pixels = 7,
        Custom = 8,
        BoxTop = 9,
        BoxBot = 10,
        BoxLeft = 11,
        BoxRight = 12,
        BoxTopLeft = 13,
        BoxTopRight = 14,
        BoxBotLeft = 15,
        BoxBotRight = 16,
        SmallDiagTopLeft = 17,
        SmallDiagTopRight = 18,
        SmallDiagBotLeft = 19,
        SmallDiagBotRight = 20,
        LargeDiagTopLeft = 21,
        LargeDiagTopRight = 22,
        LargeDiagBotLeft = 23,
        LargeDiagBotRight = 24
    }

    public enum ZIndex
    {
        Background3 = -3,
        Background2 = -2,
        Background1 = -1,
        Default = 0,
        Foreground1 = 1,
        Foreground2 = 2,
        Foreground3 = 3,
        Foreground1TopHalf = 4,
        Foreground1Unmasked = 5
    }

    public class TileData
    {
        public string name { get; set; } = "";
        [JsonIgnore]
        public Spritesheet tileset { get; set; }
        public string tilesetPath { get; set; } = "";
        public GridCoords gridCoords { get; set; }
        public Hitbox hitbox { get; set; }

        private HitboxMode _hitboxMode;
        public HitboxMode hitboxMode
        {
            get
            {
                return _hitboxMode;
            }
            set
            {
                if (tag == "indoorvoid")
                {
                    var a = 0;
                }
                _hitboxMode = value;
            }
        }
        public string customHitboxPoints { get; set; } = "";
        public string tag { get; set; }
        public ZIndex zIndex { get; set; } = 0;
        public string spriteName { get; set; } = "";

        public TileData(Spritesheet tileset, GridCoords gridCoords, string name)
        {
            this.tileset = tileset;
            this.gridCoords = gridCoords;
            this.name = name;
        }

        public string getId()
        {
            return Helpers.baseName(this.tileset.path) + "_" + (this.gridCoords.i).ToString() + "_" + (this.gridCoords.j).ToString();
        }

        [OnSerialized]
        public void onSerialize(StreamingContext context)
        {
            this.tilesetPath = this.tileset.path;
            if (this.hitboxMode == HitboxMode.SmallDiagTopLeft) { this.customHitboxPoints = "130"; this.hitboxMode = HitboxMode.Custom; }
            if (this.hitboxMode == HitboxMode.SmallDiagTopRight) { this.customHitboxPoints = "125"; this.hitboxMode = HitboxMode.Custom; }
            if (this.hitboxMode == HitboxMode.SmallDiagBotLeft) { this.customHitboxPoints = "367"; this.hitboxMode = HitboxMode.Custom; }
            if (this.hitboxMode == HitboxMode.SmallDiagBotRight) { this.customHitboxPoints = "578"; this.hitboxMode = HitboxMode.Custom; }
            if (this.hitboxMode == HitboxMode.LargeDiagTopLeft) { this.customHitboxPoints = "25760"; this.hitboxMode = HitboxMode.Custom; }
            if (this.hitboxMode == HitboxMode.LargeDiagTopRight) { this.customHitboxPoints = "28730"; this.hitboxMode = HitboxMode.Custom; }
            if (this.hitboxMode == HitboxMode.LargeDiagBotLeft) { this.customHitboxPoints = "15860"; this.hitboxMode = HitboxMode.Custom; }
            if (this.hitboxMode == HitboxMode.LargeDiagBotRight) { this.customHitboxPoints = "12863"; this.hitboxMode = HitboxMode.Custom; }
        }

        [OnDeserialized]
        public void onDeserialize(StreamingContext context)
        {
            if (this.tag.StartsWith(",")) this.tag = this.tag.Substring(1);
            /*
            if(this.customHitboxPoints == "013") { this.hitboxMode = HitboxMode.SmallDiagTopLeft; this.customHitboxPoints = ""; }
            if(this.customHitboxPoints == "125") { this.hitboxMode = HitboxMode.SmallDiagTopRight; this.customHitboxPoints = ""; }
            if(this.customHitboxPoints == "367") { this.hitboxMode = HitboxMode.SmallDiagBotLeft; this.customHitboxPoints = ""; }
            if(this.customHitboxPoints == "578") { this.hitboxMode = HitboxMode.SmallDiagBotRight; this.customHitboxPoints = ""; }
            if(this.customHitboxPoints == "02576") { this.hitboxMode = HitboxMode.LargeDiagTopLeft; this.customHitboxPoints = ""; }
            if(this.customHitboxPoints == "02873") { this.hitboxMode = HitboxMode.LargeDiagTopRight; this.customHitboxPoints = ""; }
            if(this.customHitboxPoints == "01586") { this.hitboxMode = HitboxMode.LargeDiagBotLeft; this.customHitboxPoints = ""; }
            if(this.customHitboxPoints == "12863") { this.hitboxMode = HitboxMode.LargeDiagBotRight; this.customHitboxPoints = ""; }
            */
        }

        public void setTileset(List<Spritesheet> tilesets)
        {
            this.tileset = tilesets.Where((Spritesheet tileset) =>
            {
                return tileset.getBasePath() == this.tilesetPath;
            }).FirstOrDefault();
        }

        public void setTag(string tagToSet)
        {
            var tags = this.tag.Split(',').ToList();
            tags.RemoveAll(t => string.IsNullOrEmpty(t));
            foreach (var tag in tags)
            {
                if (tag == tagToSet) return;
            }
            this.tag = tagToSet;
        }

        public bool hasTag(string tagToCheck)
        {
            var tags = this.tag.Split(',');
            foreach (var tag in tags)
            {
                if (tag == tagToCheck) return true;
            }
            return false;
        }
    }
}

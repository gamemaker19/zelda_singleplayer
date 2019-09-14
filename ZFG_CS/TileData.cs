using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZFG_CS
{
    public class TileData
    {
        public string name = "";
        public string allTags = "";
        public List<string> tags;
        public string imageBaseName = "";
        public Rect rect;
        public int zIndex = 0;
        public bool blockSameElevationProjs = true;
        public HitboxMode hitboxMode;
        public Texture bitmap;
        public string customHitboxPoints = "";
        public Animation sprite;
        public string spriteName;
        public Point spriteOffset;
        public string key = "";
        public string tagStr = "";
        public Shader shader;
        
        public TileData(string name, string tag, string imagePath, Rect rect, int zIndex, HitboxMode hitboxMode, string spriteName, string customHitboxPoints)
        {
	        this.name = name;
	        this.allTags = tag;
	        this.tags = tag.Split(',').ToList();
	        this.hitboxMode = hitboxMode;
	        this.customHitboxPoints = customHitboxPoints;
	        this.imageBaseName = Path.GetFileNameWithoutExtension(imagePath);
	        this.rect = rect;
	        this.zIndex = zIndex;
	        this.bitmap = Global.textures[imageBaseName];
	        this.spriteName = spriteName;
	        if (spriteName != "")
	        {
		        sprite = Global.animations[spriteName].clone();
		        var pieces = tag.Split('_');
		        if (pieces.Length == 3 && pieces[0] == "to")
		        {
			        spriteOffset = new Point(int.Parse(pieces[1]), int.Parse(pieces[2]));
                }
            }

            this.key = imageBaseName + "_" + ((int)(rect.y1 / 8)).ToString() + "_" + ((int)(rect.x1 / 8)).ToString();
            this.tagStr = tag;
        }

        public bool hasTag(string tagToFind)
        {
            foreach (string tag in tags)
            {
                if (tag == tagToFind)
                {
                    return true;
                }
            }
            return false;
        }

        public bool hasAnyTag(List<string> tagsVec)
        {
            foreach (string tag in tagsVec)
            {
                if (hasTag(tag))
                {
                    return true;
                }
            }
            return false;
        }

        public Collider getCollider(int i, int j, Level level)
        {
            if (level.tileSlots[i][j].noCollision)
            {
                return null;
            }

            float x1 = j * 8;
            float y1 = i * 8;
            float x2 = (j + 1) * 8;
            float y2 = (i + 1) * 8;
            if (hitboxMode == HitboxMode.Tile)
            {
                Rect colliderRect = new Rect(x1, y1, x2, y2);
                return new Collider(colliderRect);
            }
            else if (hitboxMode == HitboxMode.BoundingRect)
            {
                Rect colliderRect = new Rect(x1, y1, x2, y2);
                return new Collider(colliderRect);
            }
            else if (hitboxMode == HitboxMode.DiagBotLeft)
            {
                return new Collider(new List<Point>() { new Point(x1, y1), new Point(x2, y2), new Point(x1, y2) });
            }
            else if (hitboxMode == HitboxMode.DiagBotRight)
            {
                return new Collider(new List<Point>() { new Point(x2, y1), new Point(x2, y2), new Point(x1, y2) });
            }
            else if (hitboxMode == HitboxMode.DiagTopLeft)
            {
                return new Collider(new List<Point>() { new Point(x1, y1), new Point(x2, y1), new Point(x1, y2) });
            }
            else if (hitboxMode == HitboxMode.DiagTopRight)
            {
                return new Collider(new List<Point>() { new Point(x1, y1), new Point(x2, y1), new Point(x2, y2) });
            }
            else if (hitboxMode == HitboxMode.BoxTop)
            {
                Rect colliderRect = new Rect(x1, y1, x1 + 8, y1 + 4);
                return new Collider(colliderRect);
            }
            else if (hitboxMode == HitboxMode.BoxBot)
            {
                Rect colliderRect = new Rect(x1, y1 + 4, x1 + 8, y1 + 8);
                return new Collider(colliderRect);
            }
            else if (hitboxMode == HitboxMode.BoxLeft)
            {
                Rect colliderRect = new Rect(x1, y1, x1 + 4, y1 + 8);
                return new Collider(colliderRect);
            }
            else if (hitboxMode == HitboxMode.BoxRight)
            {
                Rect colliderRect = new Rect(x1 + 4, y1, x1 + 8, y1 + 8);
                return new Collider(colliderRect);
            }
            else if (hitboxMode == HitboxMode.BoxTopLeft)
            {
                Rect colliderRect = new Rect(x1, y1, x1 + 4, y1 + 4);
                return new Collider(colliderRect);
            }
            else if (hitboxMode == HitboxMode.BoxTopRight)
            {
                Rect colliderRect = new Rect(x1 + 4, y1, x1 + 8, y1 + 4);
                return new Collider(colliderRect);
            }
            else if (hitboxMode == HitboxMode.BoxBotLeft)
            {
                Rect colliderRect = new Rect(x1, y1 + 4, x1 + 4, y1 + 8);
                return new Collider(colliderRect);
            }
            else if (hitboxMode == HitboxMode.BoxBotRight)
            {
                Rect colliderRect = new Rect(x1 + 4, y1 + 4, x1 + 8, y1 + 8);
                return new Collider(colliderRect);
            }
            return null;
        }

        public string getKey()
        {
            return key;
        }

        public bool isDiag()
        {
            bool isCustomDiag = hitboxMode == HitboxMode.Custom &&
                (customHitboxPoints == "34286" || customHitboxPoints == "68540" || customHitboxPoints == "25460" || customHitboxPoints == "28430" || customHitboxPoints == "1530" || customHitboxPoints == "3486" || customHitboxPoints == "4586");
            return hitboxMode == HitboxMode.DiagBotLeft || hitboxMode == HitboxMode.DiagTopLeft || hitboxMode == HitboxMode.DiagBotLeft || hitboxMode == HitboxMode.DiagBotRight || isCustomDiag;
        }

        public bool isLedge()
        {
            return this is LedgeTile;
        }
    }

    public class LedgeTile : TileData
    {
	    public Point jumpOffset;
        public int xDir = 0;
        public int yDir = 0;
        public LedgeTile(string name, string tag, string imagePath, Rect rect, int zIndex, HitboxMode hitboxMode, int xDir, int yDir, string customHitboxPoints)
	: base(name, tag, imagePath, rect, zIndex, hitboxMode, "", customHitboxPoints)
        {
            this.xDir = xDir;
            this.yDir = yDir;
        }
    };
}

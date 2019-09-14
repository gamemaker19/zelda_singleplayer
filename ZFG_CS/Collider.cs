using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Collider
    {
	    public Shape shape;
        public bool isTrigger = false;
        public bool isPixelPerfect = false;
        public bool isStatic = false;
        public bool isGlobal = false;
        public bool syncedToActor = false;
        public bool isDamageHitbox = false;
        public string tags = "";
        public Damager damager;

        public Collider()
        {
        }

        public Collider(Rect rect)
        {
	        shape = rect.toShape();
        }

        public Collider(List<Point> points)
        {
	        shape = new Shape(points);
        }

        public Collider clone()
        {
            var clonedCollider = (Collider)MemberwiseClone();
            clonedCollider.shape = shape.clone();
            return clonedCollider;
        }

        public Shape getShapeNoOffset()
        {
            return shape;
        }

        public void shrinkShape(float amount)
        {
            shape.points[0] += new Point(amount, amount);
            shape.points[1] += new Point(0, amount);
            shape.points[2] += new Point(0, 0);
            shape.points[3] += new Point(amount, 0);
        }

        public void growShape(float amount)
        {
            shrinkShape(-amount);
        }

        public Shape getShape(TileData tileData)
        {
            return shape;
        }

        public Shape getShape(Actor actor)
        {
            if (syncedToActor) return shape;

            float cx = 0;
            float cy = 0;

            Animation sprite = actor.sprite;
            if (sprite.alignment == "topleft")
            {
                cx = 0; cy = 0;
            }
            else if (sprite.alignment == "topmid")
            {
                cx = 0.5f; cy = 0;
            }
            else if (sprite.alignment == "topright")
            {
                cx = 1; cy = 0;
            }
            else if (sprite.alignment == "midleft")
            {
                cx = 0; cy = 0.5f;
            }
            else if (sprite.alignment == "center")
            {
                cx = 0.5f; cy = 0.5f;
            }
            else if (sprite.alignment == "midright")
            {
                cx = 1; cy = 0.5f;
            }
            else if (sprite.alignment == "botleft")
            {
                cx = 0; cy = 1;
            }
            else if (sprite.alignment == "botmid")
            {
                cx = 0.5f; cy = 1;
            }
            else if (sprite.alignment == "botright")
            {
                cx = 1; cy = 1;
            }

            if (actor.globalCollider == null)
            {
                cx *= actor.getCurrentFrame().rect.w();
                cy *= actor.getCurrentFrame().rect.h();
            }
            else
            {
                cx *= actor.globalColliderW;
                cy *= actor.globalColliderH;
            }

            Shape shapeClone = shape.clone();
            int dir = actor.getXDir(true);
            if (!isTrigger) dir = 1;
            for (int i = 0; i < shapeClone.points.Count; i++)
            {
                Point point = shapeClone.points[i];
                point.x = (point.x * dir) + actor.pos.x - (Mathf.Floor(cx) * dir) + actor.getOffset(true).x * dir;
                point.y = point.y + actor.pos.y - Mathf.Floor(cy) + actor.getOffset(true).y;
                shapeClone.points[i] = point;
            }
            if (dir != 1 && shapeClone.isRect())
            {
                //This is a lot of overhead just for the perf improvement of comparing rects in Helpers.shapesIntersect. If you have to do this many times just delete this
                Rect rect = shapeClone.toRect();
                
                if(rect.x1 > rect.x2)
                {
                    var temp = rect.x1;
                    rect.x1 = rect.x2;
                    rect.x2 = temp;
                }

                return rect.toShape();
            }
            return shapeClone;
        }

        public void halveHeight()
        {
            float height = shape.maxY() - shape.minY();
            shape.points[2].addxy(0, -height / 2);
            shape.points[3].addxy(0, -height / 2);
        }
    }

    public class CollideData
    {
        public List<Point> intersectionPoints = new List<Point>();
        public Actor collidedActor;
        public TileData collidedTile;
        public Collider myCollider;
        public Collider collider;
        public Point normal;
        public int tileI = 0;
        public int tileJ = 0;
        public int diagDir = 0;    //1 = top left, 2 = top right, 3 = bot left, 4 = bot right
        public bool isTrigger = false;
        public Point rayTo;
        
        public CollideData() { }

        public CollideData(List<Point> intersectionPoints, Point normal)
        {
	        this.intersectionPoints = intersectionPoints;
	        this.normal = normal;
        }
    }
}

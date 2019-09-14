using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class TileSlot
    {
        public int zIndex = 0;
        public bool noCollision = false;
        public List<TileData> tileInstances = new List<TileData>();
        public HashSet<Actor> actors = new HashSet<Actor>();
        public List<TileSlot> nodeNeighbors = new List<TileSlot>();
        //public ClusterExitNode exitNode = nullptr;
        public GridCoords gridCoords;
        public bool noLand = false;    //Only used in init from properties
        public bool canLand = false;   //Cached source of truth
        public bool noProjCollide = false;

        public float gScore = 1000000;
        public float fScore = 1000000;
        public TileSlot cameFrom;

        public void setNodeNeighbors(string nodeNeighborString)
        {
        }

        public bool hasCollisionTile()
        {
            foreach (var tileInstance in tileInstances)
            {
                if (tileInstance.hitboxMode != HitboxMode.None)
                {
                    return true;
                }
            }
            return false;
        }

        public bool canLandMisc()
        {
            foreach (Actor actor in actors)
            {
                if (actor.getMainCollider(true) != null && actor.getMainCollider(true).isTrigger) continue;
                return false;
            }
            return true;
        }

        public bool canAIMoveTo(Character character, Actor target)
        {
            if (!noCollision && hasCollisionTile()) return false;
            foreach (var tileInstance in tileInstances)
            {
                if (tileInstance.hasTag("water"))
                {
                    return false;
                }
            }
            foreach (Actor actor in actors)
            {
                if (!actor.level.canCollide(character, actor)) continue;
                if (actor.getMainCollider(true) != null && actor.getMainCollider(true).isTrigger) continue;
                if (actor is FieldItem) continue;
                if (actor.getChar() != null) continue;
                if (actor == target) continue;

                if (actor is Door) continue;
                if (actor is Entrance) continue;

                if (actor.throwable != null)
                {
                    if (actor.throwable.itemRequired == Item.powerGlove && !character.hasItem(Item.powerGlove)) return false;
                    if (actor.throwable.itemRequired == Item.titansMitt && !character.hasItem(Item.titansMitt)) return false;
                    continue;
                }
                if (actor.name == "Stake" && actor.sprite.frameIndex == 0)
                {
                    if (!character.hasItem(Item.hammer)) return false;
                }
                if (actor.isSolid)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

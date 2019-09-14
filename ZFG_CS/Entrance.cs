using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Entrance : Actor
    {
        public string entranceId = "";
        public string exitLevel = "";
        public string exitId = "";
        public Direction enterDir;
        public bool fall = false;
        public bool oneWay = false;
        public string overrideMusicKey = "";
        public bool noMusicChange = false;
        public Door door;

        public Entrance(Level level, Point point, string entranceId, string exitLevel, string exitId, Direction enterDir, bool fall, bool oneWay) : base(level, point, "Entrance")
        {
            this.entranceId = entranceId;
            this.exitLevel = exitLevel;
            this.exitId = exitId;
            this.enterDir = enterDir;
            this.fall = fall;
            this.oneWay = oneWay;
        }

        public void enter(Character character)
        {
            if (oneWay)
            {
                return;
            }
            if (!fall)
            {
                if (exitLevel != character.level.name)
                {
                    Global.game.changeLevel(this, character);
                }
                else if (character.getState() != "LinkHurt")
                {
                    Entrance exitPos = character.level.entrances[exitId];
                    Point offset = Helpers.dirToVec(enterDir) * 15;
                    if (offset.y > 0) offset.y *= 2;
                    offset.x = 8;

                    character.changePos(exitPos.pos + offset, false);
                    character.changeDir(Direction.Down);
                    character.vel *= -1;
                    if (character == Global.game.camCharacter)
                    {
                        character.stateManager.invertUp = true;
                        character.updateCamera();
                    }
                    if (exitPos.door != null)
                    {
                        exitPos.door.open();
                    }
                }
            }
            else
            {
                var entranceShape = getMainCollider(true).getShape(this);
                var charShape = character.getMainCollider(false).getShape(character);
                float area = Helpers.getIntersectArea(entranceShape.toRect(), charShape.toRect());
                if (area > 32)
                {
                    character.stateManager.changeState(new LinkFall(this), true);
                }
            }
        }

    }
}

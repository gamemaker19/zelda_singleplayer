using SFML.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class Game
    {
        //Per-game instance data
        public Level overworld;
        public List<KillFeedEntry> killFeed = new List<KillFeedEntry>();
        public string currentMessage = "";
        public float currentMessageTime = 0;
        public int remainingCharacters = 0;
        public DialogBox dialogBox;
        public BRDropMenu menu = null;
        public int dialogFrameDelay = 0;
        public Character character;
        public Character camCharacter;
        public Character masterSwordChar;
        public Actor masterSword;
        public Actor unpulledMasterSword;
        public HashSet<Character> characters = new HashSet<Character>();
        public HashSet<Chest> chests = new HashSet<Chest>();
        public HashSet<FieldItem> fieldItems = new HashSet<FieldItem>();
        public Dictionary<Character, float> throttledChars = new Dictionary<Character, float>();
        public Point lostWoodsFogOffset;
        public Point woodsBackdropOffset;
        public bool masterSwordPulled = false;
        public bool masterSwordBeingPulled = false;
        public int numCPUsActive = 5;
        public bool inDropPhase = true;
        public Point currentStormCenter;
        public Point nextStormCenter;
        public Point nextCenterDir;
        public float centerMoveRate = 0;
        public float currentStormRadius = 0;
        public float nextStormRadius = 0;
        public bool isStormWait = true;
        public float stormTime = 0;
        public float stormShrinkRate = 0;
        public int stormPhase = 0;
        public List<TileSlot> shortestPath = new List<TileSlot>();
        public Actor stormKiller = new Actor();
        public bool enterGoesToMainMenu;

        public HUD hud;
        public int autoIncId = 0;

        public Game()
        {
            remainingCharacters = Options.main.numCPUs + 1;
        }

        public void initStorm()
        {
            //nextStormRadius = 100;
            //nextStormCenter = character.pos;

            isStormWait = true;

            stormTime = 30;

            currentStormRadius = getCurrentLevel().pixelWidth() * 0.75f;
            currentStormCenter = new Point(getCurrentLevel().pixelWidth() / 2, getCurrentLevel().pixelHeight() / 2);

            nextStormRadius = currentStormRadius;
            nextStormCenter = currentStormCenter;
        }

        public float getStormDamage()
        {
            if (stormPhase < 4) return 0.25f;
            else if (stormPhase == 4) return 0.5f;
            else if (stormPhase == 5) return 0.75f;
            else return 1;
        }

        public void setNextStorm()
        {
            int stormShrinkTime = 120;
            int stormWaitTime = 120;

            int phaseBeforeMove = 4;
            if (stormPhase >= phaseBeforeMove - 1)
            {
                stormWaitTime = 60;
                stormShrinkTime = 60;
            }
            if (stormPhase >= phaseBeforeMove)
            {
                stormWaitTime = 0;
                stormShrinkTime = 30;
            }

            if (isStormWait)
            {
                setCurrentMessage("Twilight moves in " + stormShrinkTime.ToString() + " seconds", 5);
                stormTime = stormWaitTime;
                currentStormCenter = nextStormCenter;
                currentStormRadius = nextStormRadius;

                if (stormPhase == 0)
                {
                    nextStormRadius = currentStormRadius * 0.5f;
                }
                else if (stormPhase < phaseBeforeMove)
                {
                    nextStormRadius = currentStormRadius * 0.5f;
                }
                else
                {
                    nextStormRadius = currentStormRadius;
                }

                int loop = 0;
                while (true)
                {
                    loop++; if (loop > 10000) { throw new Exception("INFINITE LOOP IN SETNEXTSTORM!"); }

                    //Select a random point in the current circle
                    Point prospectiveNext = Helpers.getRandPointInCircle(currentStormCenter, currentStormRadius, nextStormRadius);

                    if (stormPhase >= phaseBeforeMove)
                    {
                        Point offset2 = Point.Zero;
                        int loop2 = 0;
                        while (offset2.magnitude < 512 && loop2 < 100)
                        {
                            loop2++;
                            offset2 = new Point(Helpers.randomRange(-1024, 1024), Helpers.randomRange(-1024, 1024));
                        }
                        prospectiveNext = new Point(currentStormCenter.x + offset2.x, currentStormCenter.y + offset2.y);
                    }

                    if (prospectiveNext.x - nextStormRadius < 0 || prospectiveNext.y - nextStormRadius < 0 || prospectiveNext.x + nextStormRadius > 4096 || prospectiveNext.y + nextStormRadius > 4096)
                    {
                        continue;
                    }

                    nextStormCenter = prospectiveNext;
                    break;
                }

                nextCenterDir = currentStormCenter.dirTo(nextStormCenter);
                stormShrinkRate = (currentStormRadius - nextStormRadius) / stormShrinkTime;
                centerMoveRate = currentStormCenter.distTo(nextStormCenter) / stormShrinkTime;

                stormPhase++;
            }
            else
            {
                camCharacter.playSound("warning");
                setCurrentMessage("Warning: Twilight now moving!", 5);
                stormTime = stormShrinkTime;
            }
        }

        public void update()
        {
            if (menu != null)
            {
                menu.update();
            }
            if (dialogBox != null)
            {
                dialogBox.update();
                if (dialogBox.isDone())
                {
                    dialogBox = null;
                    dialogFrameDelay = 1;
                }
            }
            if (dialogFrameDelay > 0)
            {
                dialogFrameDelay++;
                if (dialogFrameDelay > 5) dialogFrameDelay = 0;
            }

            if (Global.input.isPressed(Control.main.Map))
            {
                if (!Global.hideMap) camCharacter.playSound("map off");
                else camCharacter.playSound("map on");
                Global.hideMap = !Global.hideMap;
            }

            for (int i = killFeed.Count - 1; i >= 0; i--)
            {
                killFeed[i].time += Global.spf;
                if (killFeed[i].time > 5)
                {
                    killFeed.RemoveAt(i);
                }
            }

            if (!masterSwordPulled)
            {
                lostWoodsFogOffset.x += Global.spf * 6;
                lostWoodsFogOffset.y += Global.spf * 6;
                if (lostWoodsFogOffset.x > 256)
                {
                    lostWoodsFogOffset.x = 0;
                    lostWoodsFogOffset.y = 0;
                }
            }

            //cout << Global.calledPerFrame << endl;
            foreach (var character in Global.game.characters)
            {
                var input = character.stateManager.input;
                //AI: clear held
                if (character.aiStateManager != null)
                {
                    foreach (var key in input.keyHeld.Keys.ToList())
                    {
                        if (input.keyHeld[key])
                        {
                            input.keyHeld[key] = false;
                        }
                    }
                    foreach (var key in input.keyPressed.Keys.ToList())
                    {
                        if (input.keyPressed[key])
                        {
                            input.keyPressed[key] = false;
                        }
                    }
                }
            }

            /*
            if (changeInput)
            {
                Character cpu1 = null;
                foreach (Character c in Global.game.characters)
                {
                    if (c.aiStateManager != null)
                    {
                        cpu1 = c;
                        cpu1.aiStateManager = nullopt;
                        break;
                    }
                }
                if (Global.input == Global.game.camCharacter.stateManager.input)
                {
                    Global.input = cpu1.stateManager.input;
                }
                else
                {
                    Global.input = Global.game.camCharacter.stateManager.input;
                }
            }
            */

            //Tile animation update
            foreach (var pair in Global.tileDatas)
            {
                TileData tileData = pair.Value;
                if (tileData != null && tileData.spriteName != "")
                {
                    tileData.sprite.update();
                }
            }

            //Update the levels
            foreach (var pair in Global.levels)
            {
                pair.Value.update();
            }

            //Storm update

            if (inDropPhase)
            {
                stormTime -= Global.spf;
                if (stormTime <= 0)
                {
                    inDropPhase = false;
                    setNextStorm();
                }
            }
            else
            {
                stormTime -= Global.spf * (Global.fastStormTimer ? 10 : 1);
                if (!isStormWait)
                {
                    currentStormCenter += nextCenterDir * Global.spf * centerMoveRate;
                    currentStormRadius -= Global.spf * stormShrinkRate;
                }

                if (stormTime <= 0)
                {
                    isStormWait = !isStormWait;
                    setNextStorm();
                }
            }

            if (currentMessageTime > 0)
            {
                currentMessageTime -= Global.spf;
                if (currentMessageTime <= 0)
                {
                    currentMessageTime = 0;
                    currentMessage = "";
                }
            }

            List<Character> toErase = new List<Character>();
            foreach (var key in throttledChars.Keys.ToList())
            {
                throttledChars[key] -= Global.spf;
                if (throttledChars[key] <= 0)
                {
                    toErase.Add(key);
                }
            }
            foreach (Character character in toErase)
            {
                throttledChars.Remove(character);
            }
        }

        public void render()
        {
            if (getCurrentLevel() == null)
            {
                return;
            }

            //game loop logic here

            if (getCurrentLevel().inLostWoods())
            {
                if (!masterSwordPulled)
                {
                    Global.animations["LostWoodsFog"].draw(lostWoodsFogOffset.x, lostWoodsFogOffset.y, 1, 1, 0, 1, null, (int)ZIndex.HUD - 5, false);
                    Global.animations["LostWoodsFog"].draw(lostWoodsFogOffset.x - 512, lostWoodsFogOffset.y - 256, 1, 1, 0, 1, null, (int)ZIndex.HUD - 5, false);
                    Global.animations["LostWoodsFog"].draw(lostWoodsFogOffset.x - 512, lostWoodsFogOffset.y, 1, 1, 0, 1, null, (int)ZIndex.HUD - 5, false);
                    Global.animations["LostWoodsFog"].draw(lostWoodsFogOffset.x, lostWoodsFogOffset.y - 256, 1, 1, 0, 1, null, (int)ZIndex.HUD - 5, false);
                }
                else if (getCurrentLevel().name == "lttp_overworld")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            Global.animations["Woods"].draw(woodsBackdropOffset.x + (i * 512), woodsBackdropOffset.y + (j * 448), 1, 1, 0, 1, null, (int)ZIndex.HUD - 5, true);
                        }
                    }
                }
            }

            getCurrentLevel().render();

            //Draw menu/dialog
            if (character != null && (menu == null || menu.disabled)) hud.draw();
            if (dialogBox != null) dialogBox.render();
            if (menu != null) menu.render();
        }

        public Level getCurrentLevel()
        {
            if (camCharacter != null)
            {
                return camCharacter.level;
            }
            return overworld;
        }

        public void setCurrentMessage(string message, float time)
        {
            currentMessage = message;
            currentMessageTime = time;
        }

        public void loadLevels()
        {
            var levelPaths = Directory.GetFiles(Global.assetPath + "assets/levels");

            foreach (string levelPath in levelPaths)
            {
                Level level = new Level();
                level.init(levelPath);
                Global.levels[level.name] = level;
                if (level.name == "lttp_overworld")
                {
                    level.musicKey = "overworld";
                }
                else if (level.name == "house")
                {
                    level.musicKey = "house";
                }
                else if (level.name == "cave")
                {
                    level.musicKey = "cave";
                }
            }
        }
        
        public void changeLevel(Entrance entrance, Character character)
        {
            Level currentLevel = character.level;
            Level newLevel = Global.levels[entrance.exitLevel];
            currentLevel.removeActor(character);

            Entrance exitPos = newLevel.entrances[entrance.exitId];

            Point offset = Helpers.dirToVec(entrance.enterDir) * 15;
            if (offset.y > 0) offset.y *= 2;
            offset.x = 8;
            if (entrance.fall) offset = Point.Zero;

            //Transition cleanup: move things that should move, delete others
            Actor bryanaRing = character.stateManager.bryanaRing;
            if (bryanaRing != null && !bryanaRing.isDeleted())
            {
                currentLevel.removeActor(bryanaRing);
                newLevel.addActor(bryanaRing);
                bryanaRing.changePos(exitPos.pos + offset, false);
            }
            Actor liftedObject = character.stateManager.actorState.throwable;
            if (liftedObject != null && !liftedObject.isDeleted())
            {
                currentLevel.removeActor(liftedObject);
                newLevel.addActor(liftedObject);
                liftedObject.changePos(exitPos.pos + offset, false);
            }
            Actor boomerang = character.boomerang;
            if (boomerang != null && !boomerang.isDeleted())
            {
                currentLevel.removeActor(boomerang);
                character.boomerang = null;
            }

            newLevel.addActor(character);
            character.changePos(exitPos.pos + offset, false);
            if (character == camCharacter)
            {
                character.updateCamera();

                if (!entrance.noMusicChange)
                {
                    newLevel.startMusic(entrance.overrideMusicKey);
                    character.checkMusicChange();
                }
            }

            if (!entrance.level.isIndoor && character.aiStateManager != null)
            {
                character.aiStateManager.entrancesUsed.Add(entrance);
                character.aiStateManager.currentExit = exitPos;
            }

            if (exitPos.door != null)
            {
                exitPos.door.open();
            }
        }

    }
}

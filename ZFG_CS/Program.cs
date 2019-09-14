using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using SFML.Window;
using Newtonsoft.Json;
using static SFML.Window.Keyboard;
using SFML.Graphics.Glsl;
using System.Threading;

namespace ZFG_CS
{
    class Program
    {
        static void Main(string[] args)
        {
            Global.Init();

            Global.window = new RenderWindow(new VideoMode(Global.windowW, Global.windowH), "Zelda: Battle Royale");
            var window = Global.window;
            Global.view = new View(new Vector2f(0, 0), new Vector2f(Global.screenW, Global.screenH));
            Global.fontView = new View(new Vector2f(Global.windowW / 2, Global.windowH / 2), new Vector2f(Global.windowW, Global.windowH));
            Global.hudView = new View(new Vector2f(Global.screenW / 2, Global.screenH / 2), new Vector2f(Global.screenW, Global.screenH));
            Global.mapCircleBuffer = new RenderTexture(75, 75);
            Global.circleBuffer = new RenderTexture(4096, 4096);
            window.SetView(Global.view);
            window.SetFramerateLimit(Global.fps);

            window.SetActive();

            CircleShape shape = new CircleShape(100);
            shape.FillColor = Color.Green;

            window.Closed += new EventHandler(onClosed);
            window.KeyPressed += new EventHandler<KeyEventArgs>(onKeyPressed);
            window.KeyReleased += new EventHandler<KeyEventArgs>(onKeyReleased);

            string fontPath = Global.assetPath + "assets/fonts/font.ttf";
            Global.font = new Font(fontPath);
            
            loadShaders();
            loadImages();
            loadAnims();
            loadTiles();
            loadSounds();
            loadMusics();
            SaveData.saveData.load();

            Global.input = new Input();

#if !DEBUG
            Thread.Sleep(1000);
#endif

            Global.goToMainMenu();
            
            /*
            Global.game.overworld = Global.levels["lttp_overworld"];
            Global.game.overworld.startMusic();
            Global.startPos = new Point(303, 363) * 8;
            Character character = new Character(Global.game.overworld, Global.startPos, Direction.Down, true, "Link", "Player1", Global.input);
            Global.game.character = character;
            Global.game.camCharacter = character;
            Global.hud = new HUD(character);
            //Global.game.characters.Add(character);
            //Character cpu1 = new Character(Global.game.overworld, Global.startPos.addxy(32, 32), Direction.Down, false, "Link", "CPU1", new Input());
            //cpu1.aiStateManager = new AIStateManager(cpu1);
            //Global.game.characters.Add(cpu1);
            Global.initStorm();
            */

            while (window.IsOpen)
            {
                Global.spf2 = Global.clock.ElapsedTime.AsSeconds();
                if (Global.spf2 > 0.667f) Global.spf2 = 0.667f;
                Global.clock.Restart();

                window.DispatchEvents();
                window.Clear();

                update();
                render();

                window.Display();
            }
        }

        private static void update()
        {
            if (Global.mainMenu != null)
            {
                Global.mainMenu.update();
            }
            if (Global.game != null)
            {
                if (Global.input.isPressed(Key.Enter) && Global.game.enterGoesToMainMenu)
                {
                    Global.goToMainMenu(false);
                }
                else
                {
                    Global.game.update();
                }
            }

            bool isPaused = false; //(Global.menu != null || Global.dialogBox != null);
            if (!isPaused)
            {
                Global.frameCount++;
                Global.calledPerFrame = 0;
                bool changeInput = false;

                if (!Global.paused)
                {
#if DEBUG
                    cheats();
#endif
                    for (int i = Global.sounds.Count - 1; i >= 0; i--)
                    {
                        if (Global.sounds[i].Status == SoundStatus.Stopped)
                        {
                            Global.sounds.RemoveAt(i);
                        }
                    }
                    
                    Global.music.update();
                }

                foreach (var key in Global.input.keyPressed.Keys.ToList())
                {
                    if (Global.input.keyPressed[key])
                    {
                        Global.input.keyPressed[key] = false;
                    }
                }
            }
        }
        
        private static void render()
        {
            if (Global.mainMenu != null)
            {
                Global.mainMenu.render();
            }

            if (Global.game != null)
            {
                Global.game.render();
            }

#if DEBUG
            //Draw debug strings
            Global.debugString1 = ((int)Math.Round(1.0f / Global.spf2)).ToString();
            if(Global.game != null && Global.game.character != null) Global.debugString2 = Mathf.Floor(Global.game.character.pos.x / 8).ToString("0") + "," + Mathf.Floor(Global.game.character.pos.y / 8).ToString("0");

            Helpers.drawTextStd(Global.debugString1, 20, 20);
            Helpers.drawTextStd(Global.debugString2, 20, 40);
            Helpers.drawTextStd(Global.debugString3, 20, 60);
#endif
        }

        private static void cheats()
        {
            if (Global.mainMenu != null) return;

            if(Global.input.isPressed(Key.Escape))
            {
                Global.goToMainMenu(false);
            }

            if (Global.input.isPressed(Key.P))
            {
                Global.paused = !Global.paused;
            }

            //Debug Cheats section
            if (Global.input.isPressed(Key.F1))
            {
                Global.game.camCharacter.isSolid = !Global.game.camCharacter.isSolid;
            }
            else if (Global.input.isPressed(Key.F2))
            {
                //Global.game.character.changePos(Global.startPos, false);
                foreach (var c in Global.game.characters) c.hasEverything = true;
            }
            else if (Global.input.isPressed(Key.F3))
            {
                Global.showHitboxes = !Global.showHitboxes;
            }
            else if (Global.input.isPressed(Key.F4))
            {
                Global.game.camCharacter.magic.value = Global.game.camCharacter.magic.maxValue;
                Global.game.camCharacter.arrows.value = 99;
            }
            else if (Global.input.isPressed(Key.F5))
            {
                Global.nextItem = Item.bombos;
                //foreach (var c in Global.game.characters) c.health.value = 10;
            }
            else if (Global.input.isPressed(Key.F6))
            {
                foreach (var c in Global.game.characters)
                {
                    if (c == Global.game.camCharacter) continue;
                    c.changePos(Global.game.camCharacter.pos.addxy(0, 200), false);
                    c.items[0] = null;
                    c.items[1] = null;
                    c.items[2] = null;
                    c.items[3] = null;
                    c.items[4] = null;

                    c.addItem(new InventoryItem(Item.sword3), 0);
                    c.addItem(new InventoryItem(Item.shield3), 1);
                    c.addItem(new InventoryItem(Item.silverBow), 2);
                    c.addItem(new InventoryItem(Item.bottledFairy), 3);
                    //c.addItem(new InventoryItem(Item.caneOfBryana), 4);
                    c.health.maxValue = 10;
                    c.health.value = 10;
                    c.aiStateManager.decided = false;
                    break;
                }
            }
            else if (Global.input.isPressed(Key.F7))
            {
                foreach (var c in Global.game.characters)
                {
                    if (c == Global.game.camCharacter) continue;
                    if (c.pos.distTo(Global.game.camCharacter.pos) < 100)
                    {
                        c.items[0] = null;
                        c.addItem(new InventoryItem(Item.sword1), 0);
                        c.aiStateManager.decided = false;
                    }
                }
                //for (var c in Global.game.characters) c.applyDamage(Damager(null, null, 1), Point());
            }
            else if (Global.input.isPressed(Key.F8))
            {
                foreach (var c in Global.game.characters)
                {
                    if (c == Global.game.camCharacter) continue;
                    if (c.pos.distTo(Global.game.camCharacter.pos) < 512)
                    {
                        c.items[0] = null;
                        int rand = Helpers.randomRange(0, 3);
                        if (rand == 0) c.addItem(new InventoryItem(Item.sword1), 0);
                        if (rand == 1) c.addItem(new InventoryItem(Item.bow), 0);
                        if (rand == 2) c.addItem(new InventoryItem(Item.lamp), 0);
                        if (rand == 3) c.addItem(new InventoryItem(Item.firerod), 0);
                        c.magic.value = c.magic.maxValue;
                        c.aiStateManager.decided = false;
                    }
                }
                //Global.fastStormTimer = !Global.fastStormTimer;
                //Global.stormTime = 0.1;
            }
            else if (Global.input.isPressed(Key.F9))
            {
                //Global.fastStormTimer = !Global.fastStormTimer;
                Global.game.character.arrows.value = 99;
            }
            else if (Global.input.isPressed(Key.F10))
            {
                Global.debugAIDownSwitch = true;
            }
            else if (Global.input.isPressed(Key.F11))
            {
                Global.music.setNearEnd();
            }
            else if (Global.input.isPressed(Key.Quote))
            {
                //changeInput = true;
                Global.game.stormTime = 0.1f;
            }
            else if (Global.input.isPressed(Key.Dash))
            {
                //Global.music.loopEnd -= 1000;
                //cout << Global.music.loopEnd << endl;
            }
            else if (Global.input.isPressed(Key.Equal))
            {
                //Global.music.loopEnd += 1000;
                //cout << Global.music.loopEnd << endl;
            }
            else if (Global.input.isPressed(Key.K))
            {
                Global.game.character.stateManager.changeState(new LinkWin(), true);
            }
        }

        /// <summary>
        /// Function called when the window is closed
        /// </summary>
        private static void onClosed(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        private static void onKeyPressed(object sender, KeyEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            //if (e.Code == Keyboard.Key.Escape)
            //    window.Close();
            Global.input.keyPressed[e.Code] = !Global.input.keyHeld.ContainsKey(e.Code) || !Global.input.keyHeld[e.Code];
            Global.input.keyHeld[e.Code] = true;

            var controlMenu = Global.mainMenu as ControlMenu;
            if (controlMenu != null && controlMenu.listenForKey && controlMenu.bindFrames == 0)
            {
                controlMenu.bindKey(e.Code);
            }
        }

        private static void onKeyReleased(object sender, KeyEventArgs e)
        {
            Global.input.keyHeld[e.Code] = false;
            Global.input.keyPressed[e.Code] = false;
        }

        static Texture CombineTextures(Texture tex1, Texture tex2)
        {
            Image image1 = tex1.CopyToImage();
            Image image2 = tex2.CopyToImage();

            uint width = Math.Max(image1.Size.X, image2.Size.X);
            uint height = image1.Size.Y + image2.Size.Y;
            Image image3 = new Image(width, height);
            for (uint x = 0; x < width; x++)
            {
                for (uint y = 0; y < height; y++)
                {
                    Color pixel;
                    if (y < image1.Size.Y)
                    {
                        if(x < image1.Size.X)
                        {
                            pixel = image1.GetPixel(x, y);
                        }
                        else
                        {
                            pixel = Color.Transparent;
                        }
                    }
                    else pixel = image2.GetPixel(x, y - image1.Size.Y);
                    image3.SetPixel(x, y, pixel);
                }
            }

            Texture texture = new Texture(image3);
            
            image1.Dispose();
            image2.Dispose();
            image3.Dispose();

            return texture;
        }

        static void loadImages()
        {
            var spritesheets = Directory.GetFiles(Global.assetPath + "assets/spritesheets").ToList();
            //var skins = new string[] { };
            var skins = Directory.GetFiles(Global.assetPath + "assets/spritesheets/skins").ToList();
            spritesheets.AddRange(skins);

            foreach (string skin in skins)
            {
                Global.skins.Add(Path.GetFileNameWithoutExtension(skin));
            }
            
            for (int i = 0; i < spritesheets.Count; i++)
            {
                string path = spritesheets[i];
                if (Path.GetFileNameWithoutExtension(path) == "equipment") continue;

                Texture texture = new Texture(path);
                Texture newTexture = null;
                
                if (skins.Contains(path))
                {
                    string equipmentPath = Global.assetPath + "assets/spritesheets/equipment.png";
                    Texture equipment = new Texture(equipmentPath);
                    newTexture = CombineTextures(texture, equipment);
                    texture.Dispose();
                    equipment.Dispose();
                }
                else
                {
                    newTexture = texture;
                }

                Global.textures[Path.GetFileNameWithoutExtension(path)] = newTexture;
            }
            List<string> tilesets = Directory.GetFiles(Global.assetPath + "assets/tilesets").ToList();
            for (int i = 0; i < tilesets.Count; i++)
            {
                Texture texture = new Texture(tilesets[i]);
                Global.textures[Path.GetFileNameWithoutExtension(tilesets[i])] = texture;

                //ALLEGRO_BITMAP* maskedBitmap = al_load_bitmap(tilesets[i].c_str());
                //al_convert_mask_to_alpha(maskedBitmap, al_map_rgb(72, 160, 72));
                //Global.maskedImages[Path.GetFileNameWithoutExtension(tilesets[i])] = maskedBitmap;
            }
        }

        static void loadAnims()
        {
            string path = Global.assetPath + "assets/sprites";
            List<string> files = Directory.GetFiles(path).ToList();
            for (int i = 0; i < files.Count; i++)
            {
                Animation anim = new Animation(files[i]);
                Global.animations[anim.name] = anim;
            }
        }

        static void loadSounds()
        {
            string path = Global.assetPath + "assets/sounds";
            List<string> files = Directory.GetFiles(path, "*", SearchOption.AllDirectories).ToList();

            for (int i = 0; i < files.Count; i++)
            {
                string name = Path.GetFileNameWithoutExtension(files[i]);
                Global.soundBuffers[name] = new SoundBuffer(files[i]);
            }
        }

        static void loadMusics()
        {
            string path = Global.assetPath + "assets/music";
            List<string> files = Directory.GetFiles(path, "*", SearchOption.AllDirectories).ToList();

            for (int i = 0; i < files.Count; i++)
            {
                string name = Path.GetFileNameWithoutExtension(files[i]);
                MusicWrapper musicWrapper = null;
                if (name == "overworld")
                {
                    //sound = new Sound("orch_" + name, 342378, 2049115);
                    musicWrapper = new MusicWrapper(files[i], 335125 / 48000.0, 3.70455e+06 / 48000.0);
                }
                else if (name == "house")
                {
                    //sound = new Sound("orch_" + name, 0, 0);
                    musicWrapper = new MusicWrapper(files[i], 0, 0);
                }
                else if (name == "cave")
                {
                    //musicWrapper = new MusicWrapper("orch_" + name, 0, 1697097);
                    musicWrapper = new MusicWrapper(files[i], 559783 / 48000.0, 1.3141e+06 / 48000.0);
                }
                else if (name == "kakariko")
                {
                    //musicWrapper = new MusicWrapper("orch_" + name, 0, 3969000);
                    musicWrapper = new MusicWrapper(files[i], 0, 4299804 / 48000f);
                }
                else if (name == "lost_woods")
                {
                    //musicWrapper = new MusicWrapper("orch_" + name, 0, 1776630);
                    musicWrapper = new MusicWrapper(files[i], 0, 975276 / 48000.0);
                }
                else if (name == "fairy_fountain")
                {
                    //musicWrapper = new MusicWrapper("orch_" + name, 1196684, 2268594 + 1000);
                    musicWrapper = new MusicWrapper(files[i], 119700 / 48000.0, 1262748 / 48000.0);
                }
                else if (name == "sanctuary")
                {
                    //musicWrapper = new MusicWrapper("orch_" + name, 529200, 3880800);
                    musicWrapper = new MusicWrapper(files[i], 471679 / 48000.0, 1.79113e+06 / 48000.0);
                }
                else if (name == "bunny")
                {
                    //musicWrapper = new MusicWrapper("orch_" + name, 0, 2140584);
                    musicWrapper = new MusicWrapper(files[i], 0, 1199051 / 48000.0);
                }
                else if (name == "master_sword")
                {
                    musicWrapper = new MusicWrapper(files[i]);
                }
                else if (name == "victory")
                {
                    musicWrapper = new MusicWrapper(files[i]);
                }
                Global.musics[name] = musicWrapper;
            }
        }

        static void loadShaders()
        {
            string path = Global.assetPath + "assets/glsl_shaders";
            List<string> files = Directory.GetFiles(path).ToList();
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].Contains("standard.vertex")) continue;
                string shaderContents = File.ReadAllText(files[i]);
                string shaderName = Path.GetFileNameWithoutExtension(files[i]);
                Global.shaderCodes[shaderName] = shaderContents;
                
                Global.shaders[shaderName] = Helpers.cloneShader(shaderName);
            }

            //SHADER SETUP
            Shader replaceColorBush = Helpers.cloneShader("replaceColor");
            replaceColorBush.SetUniform("origColor", new Vec3(41 / 255f, 123 / 255f, 57 / 255f));
            replaceColorBush.SetUniform("replaceColor", new Vec3(104 / 255f, 104 / 255f, 40 / 255f));
            replaceColorBush.SetUniform("origColor2", new Vec3(72 / 255f, 152 / 255f, 72 / 255f));
            replaceColorBush.SetUniform("replaceColor2", new Vec3(144 / 255f, 128 / 255f, 64 / 255f));
            Global.shaders["replaceColorBush"] = replaceColorBush;

            Shader replaceColorSpin = Helpers.cloneShader("replaceColor");
            replaceColorSpin.SetUniform("origColor", new Vec3(248 / 255f, 200 / 255f, 32 / 255f));
            replaceColorSpin.SetUniform("replaceColor", new Vec3(136 / 255f, 208 / 255f, 248 / 255f));
            replaceColorSpin.SetUniform("origColor2", new Vec3(248 / 255f, 112 / 255f, 48 / 255f));
            replaceColorSpin.SetUniform("replaceColor2", new Vec3(120 / 255f, 144 / 255f, 24 / 255f));
            Global.shaders["replaceColorSpin"] = replaceColorSpin;

            Shader replaceRedBlue = Helpers.cloneShader("replaceColor");
            replaceRedBlue.SetUniform("origColor", new Vec3(176 / 255f, 40 / 255f, 40 / 255f));
            replaceRedBlue.SetUniform("replaceColor", new Vec3(80 / 255f, 104 / 255f, 168 / 255f));
            replaceRedBlue.SetUniform("origColor2", new Vec3(224 / 255f, 112 / 255f, 112 / 255f));
            replaceRedBlue.SetUniform("replaceColor2", new Vec3(144 / 255f, 168 / 255f, 232 / 255f));
            Global.shaders["replaceRedBlue"] = replaceRedBlue;

            Shader replaceSilverBrown = Helpers.cloneShader("replaceColor");
            replaceSilverBrown.SetUniform("origColor", new Vec3(184 / 255f, 184 / 255f, 200 / 255f));
            replaceSilverBrown.SetUniform("replaceColor", new Vec3(255 / 255f, 181 / 255f, 82 / 255f));
            replaceSilverBrown.SetUniform("origColor2", new Vec3(176 / 255f, 40 / 255f, 40 / 255f));
            replaceSilverBrown.SetUniform("replaceColor2", new Vec3(82 / 255f, 107 / 255f, 173 / 255f));
            Global.shaders["replaceSilverBrown"] = replaceSilverBrown;

            Shader greenTransparent = Helpers.cloneShader("transparentColor");
            greenTransparent.SetUniform("color", new Vec4(72 / 255f, 160 / 255f, 72 / 255f, 1));
            Global.shaders["greenTransparent"] = greenTransparent;
        }

        static void loadTiles()
        {
            string tileDataJsons = File.ReadAllText(Global.assetPath + "assets/tiledatas.json");

            var tileDatasJson = JsonConvert.DeserializeObject<List<dynamic>>(tileDataJsons);

            foreach(dynamic tileDataJson in tileDatasJson)
            {
                string name = Convert.ToString(tileDataJson.name);
                string spriteName = Convert.ToString(tileDataJson.spriteName);
                string tilesetPath = Global.assetPath + Convert.ToString(tileDataJson.tilesetPath);
                float x1 = 8 * (float)tileDataJson.gridCoords.j;
                float y1 = 8 * (float)tileDataJson.gridCoords.i;
                float x2 = x1 + 8;
                float y2 = y1 + 8;

                Rect rect = new Rect(x1, y1, x2, y2);
                int hitboxModeInt = Convert.ToInt32(tileDataJson.hitboxMode);
                HitboxMode hitboxMode = (HitboxMode)(hitboxModeInt);
                string tags = Convert.ToString(tileDataJson.tag);
                int zIndex = Convert.ToInt32(tileDataJson.zIndex);
                string customHitboxPoints = Convert.ToString(tileDataJson.customHitboxPoints);

                //dynamic config/dynamic tile config
                var tagPieces = tags.Split(',');
                TileData tileData = null;
                foreach (string tag in tagPieces)
                {
                    if (tag == "ledge")
                    {
                        if (hitboxMode == HitboxMode.DiagTopLeft)
                        {
                            tileData = new LedgeTile(name, tag, tilesetPath, rect, zIndex, hitboxMode, -1, -1, "");
                        }
                        else if (hitboxMode == HitboxMode.DiagTopRight)
                        {
                            tileData = new LedgeTile(name, tag, tilesetPath, rect, zIndex, hitboxMode, 1, -1, "");
                        }
                        else if (hitboxMode == HitboxMode.DiagBotLeft)
                        {
                            tileData = new LedgeTile(name, tag, tilesetPath, rect, zIndex, hitboxMode, -1, 1, "");
                        }
                        else if (hitboxMode == HitboxMode.DiagBotRight)
                        {
                            tileData = new LedgeTile(name, tag, tilesetPath, rect, zIndex, hitboxMode, 1, 1, "");
                        }
                        else if (hitboxMode == HitboxMode.Tile)
                        {
                            int xDir = 0;
                            int yDir = 0;
                            if (tagPieces.Contains("left")) xDir = -1;
                            else if (tagPieces.Contains("right")) xDir = 1;
                            else if (tagPieces.Contains("up")) yDir = -1;
                            else if (tagPieces.Contains("down")) yDir = 1;
                            else if (tagPieces.Contains("downleft")) { xDir = -1; yDir = 1; }
                            else if (tagPieces.Contains("downright")) { xDir = 1; yDir = 1; }
                            else if (tagPieces.Contains("upleft")) { yDir = -1; yDir = -1; }
                            else if (tagPieces.Contains("upright")) { yDir = 1; yDir = -1; }
                            tileData = new LedgeTile(name, tag, tilesetPath, rect, zIndex, hitboxMode, xDir, yDir, "");
                        }
                    }
                }
                if (tileData == null)
                {
                    tileData = new TileData(name, tags, tilesetPath, rect, zIndex, hitboxMode, spriteName, customHitboxPoints);
                }

                Global.tileDatas[tileData.getKey()] = tileData;
            }
        }

    }
}

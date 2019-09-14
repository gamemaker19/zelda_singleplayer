using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SFML.Audio;
using SFML.System;
using System.Linq;
using System.IO;

namespace ZFG_CS
{
    public class Global
    {
        public static Dictionary<string, Level> levels = new Dictionary<string, Level>();
        public static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
        public static Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
        public static Dictionary<string, SoundBuffer> soundBuffers = new Dictionary<string, SoundBuffer>();
        public static Dictionary<string, MusicWrapper> musics = new Dictionary<string, MusicWrapper>();
        public static Dictionary<string, Shader> shaders = new Dictionary<string, Shader>();
        public static Dictionary<string, string> shaderCodes = new Dictionary<string, string>();

        public static Dictionary<string, TileData> tileDatas = new Dictionary<string, TileData>();
        public static List<string> skins = new List<string>();
        public static RenderWindow window;

        public static View view;
        public static View hudView;
        public static View fontView;
        public static RenderTexture mapCircleBuffer;
        public static RenderTexture circleBuffer;

        public static string assetPath = "../../../../../LevelEditor/";

        public static Clock clock = new Clock();

        public static Dictionary<string, List<List<string>>> bombableWallMap = new Dictionary<string, List<List<string>>>();  //Maps the top-left bombable tile id to a 2D List of bombed tiles
        public static Dictionary<string, GridCoords> bombableWallLinkage = new Dictionary<string, GridCoords>();
        
        public static uint fps = 60;
        public static float spf = 1 / 60.0f;
        public static float spf2 = 1 / 60.0f;
        public static int frameCount = 0;
        public static bool paused = false;
        public static bool debugDrop = false;
        public static bool debugCharMovement = false;
        public static uint screenW = 256;
        public static uint screenH = 224;
        public static uint windowScale = 4;
        public static uint windowW;
        public static uint windowH;
        public static bool showHitboxes = false;
        public static float gravity = 0.1f;
        
        public static Font font;
        public static Point startPos;
        public static string debugString1 = "";
        public static string debugString2 = "";
        public static string debugString3 = "";

        public static int calledPerFrame = 0;
        public static bool test = false;
        public static Input input;
        public static bool once = false;
        public static int testCounter = 0;
        
        public static bool hideMap = true;
        public static bool logAI = false;
        public static bool fastStormTimer = false;
        
        public static bool isOrchestrated = true;
        public static bool debugAIDownSwitch = false;
        public static Item nextItem = null;
        public static IMainMenu mainMenu;
        public static int defaultMusicVolume = 50;

        public static List<Sound> sounds = new List<Sound>();
        public static MusicWrapper music = null;

        public static Game game;

        public static void Init()
        {
	        fps = 60;
	        spf = 1 / 60.0f;
	        screenW = 256;
	        screenH = 224;
	        windowScale = 4;
	        windowW = screenW * windowScale;
            windowH = screenH * windowScale;
            showHitboxes = false;
	        gravity = 0.1f;
            debugDrop = false;
	        debugCharMovement = false;
	        logAI = false;
	        isOrchestrated = false;

            view = new View(new Vector2f(windowW / 2, windowH / 2), new Vector2f(windowW, windowH));
            
	        List<List<string>> caveBombDown = new List<List<string>>();
            caveBombDown.Add(new List<string>() { "cave_36_46", "cave_36_47", "cave_36_48", "cave_36_49" });
	        caveBombDown.Add(new List<string>() { "cave_37_46", "cave_16_28", "cave_16_28", "cave_37_49" });
	        caveBombDown.Add(new List<string>() { "cave_38_46", "cave_38_47", "cave_38_48", "cave_38_49" });
	        bombableWallMap["cave_36_206"] = caveBombDown;

	        List<List<string>> caveBombLeft = new List<List<string>>();
            caveBombLeft.Add(new List<string>() { "cave_9_122", "cave_15_27", "cave_15_28", "cave_15_29" });
	        caveBombLeft.Add(new List<string>() { "cave_8_122", "cave_16_27", "cave_16_28", "cave_16_29" });
	        caveBombLeft.Add(new List<string>() { "cave_9_122", "cave_17_27", "cave_16_28", "cave_17_29" });
	        caveBombLeft.Add(new List<string>() { "cave_8_122", "cave_18_27", "cave_18_28", "cave_18_29" });
	        bombableWallMap["cave_79_90"] = caveBombLeft;

	        List<List<string>> iceCaveBombDown = new List<List<string>>();
            iceCaveBombDown.Add(new List<string>() { "cave_164_14", "cave_164_15", "cave_164_16", "cave_164_17" });
	        iceCaveBombDown.Add(new List<string>() { "cave_165_14", "cave_144_28", "cave_144_28", "cave_165_17" });
	        iceCaveBombDown.Add(new List<string>() { "cave_166_14", "cave_166_15", "cave_166_16", "cave_166_17" });
	        bombableWallMap["cave_164_46"] = iceCaveBombDown;

	        List<List<string>> houseBombLeft = new List<List<string>>();
            houseBombLeft.Add(new List<string>() { "house_47_27", "house_47_28", "house_47_29" });
	        houseBombLeft.Add(new List<string>() { "house_48_27", "house_5_143", "house_47_29" });
	        houseBombLeft.Add(new List<string>() { "house_49_27", "house_5_143", "house_49_29" });
	        houseBombLeft.Add(new List<string>() { "house_50_27", "house_50_28", "house_49_29" });
	        bombableWallMap["house_15_27"] = houseBombLeft;

	        List<List<string>> houseBombRight = new List<List<string>>();
            houseBombRight.Add(new List<string>() { "house_47_34", "house_47_35", "house_47_36" });
	        houseBombRight.Add(new List<string>() { "house_47_34", "house_5_143", "house_48_36" });
	        houseBombRight.Add(new List<string>() { "house_49_34", "house_5_143", "house_49_36" });
	        houseBombRight.Add(new List<string>() { "house_49_34", "house_50_35", "house_50_36" });
	        bombableWallMap["house_15_34"] = houseBombRight;

	        //Note: unlike examples above, the i comes first, then j!
	        bombableWallLinkage["house_15_27"] = new GridCoords(15, 34);
            bombableWallLinkage["house_15_34"] = new GridCoords(15, 27);

            Item.initItemList();
        }

        public static void update()
        {
            if(game != null)
            {
                game.update();
            }
        }
        
        public static void playSound(string soundKey)
        {
            Sound sound = new Sound(Global.soundBuffers[soundKey]);
            sound.Volume *= Options.main.soundVolume;
            Global.sounds.Add(sound);
            sound.Play();
        }

        public static void goToMainMenu(bool firstTime = true)
        {
            if (!firstTime)
            {
                Global.game = null;
            }
            mainMenu = new MainMenu();
            if(music != null) music.music.Stop();
            music = musics["fairy_fountain"];
            music.updateVolume();
            music.play();
        }

    }
}

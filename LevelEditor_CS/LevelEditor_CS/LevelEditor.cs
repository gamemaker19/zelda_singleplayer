using LevelEditor_CS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelEditor_CS
{
    public partial class LevelEditor : Form
    {
        public enum Tool
        {
            Select = 0,
            PlaceTile,
            RectangleTile,
            SelectInstance,
            CreateInstance
        }

        public List<Spritesheet> spritesheets = new List<Spritesheet>();
        public List<Sprite> sprites = new List<Sprite>();
        public List<Level> levels = new List<Level>();
        public Level selectedLevel;
        public List<Obj> objs = getObjectList();
        public Obj selectedObj = null;
        public List<SpriteInstance> selectedInstances = new List<SpriteInstance>();
        public float zoom = 1;
        public string newLevelName = "";
        public bool showInstances = true;
        public bool showNavMesh = false;
        public Spritesheet selectedTileset = null;
        public List<Spritesheet> tilesets = new List<Spritesheet>();
        public List<GridCoords> tileSelectedCoords = new List<GridCoords>();
        public List<GridCoords> levelSelectedCoords = new List<GridCoords>();
        public List<TileData> tileDatas = new List<TileData>();
        public Dictionary<string, List<List<TileData>>> tileDataGrids = new Dictionary<string, List<List<TileData>>>();
        public string multiEditName = "";
        public int multiEditHitboxMode = 0;
        public string multiEditTag = "";
        public int multiEditZIndex = 0;
        public string multiEditTileSprite = "";
        public bool showLevelGrid = true;
        public Tool selectedTool = Tool.Select;
        public int loadCount = -1;
        public int maxLoadCount = 0;
        public string selectionProperties = "";
        public List<string> undoJsons = new List<string>();
        public int undoIndex = 0;
        public int selectionElevation = 0;
        public string selectedInstanceProperties = "";
        public bool showTileHitboxes = false;
        public bool showElevations = false;
        public string showTilesWithTag = "";
        public bool showTilesWithZIndex1 = false;
        public string showTilesWithSprite = "";
        public int paintElevationHeight = 0;
        public int layerIndex = 0;
        public List<Spritesheet> levelImages = new List<Spritesheet>();
        public string lastSelectedTileSprite = "";
        public bool mode16x16 = false;
        public string customHitboxPoints = "";
        public bool showRoomLines = false;
        public GridRect clonedTiles = null;
        public string showOverridesWithKey = "";
        public string lastNavMeshCoords = "";

        public LevelEditor()
        {
            InitializeComponent();
        }

        public void redrawTileCanvas()
        {
            tileCanvas.Invalidate();
        }

        public void redrawLevelCanvas()
        {
            levelCanvas.Invalidate();
        }

        private void LevelEditor_Load(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        public void addUndoJson()
        {

        }

        public void sortInstances()
        {

        }
    }
}

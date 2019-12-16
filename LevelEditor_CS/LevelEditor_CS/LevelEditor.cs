using LevelEditor_CS.Editor;
using LevelEditor_CS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListBox;

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

        public MultiListBoxBinding<SpriteInstance> instanceListBoxBinding;
        public List<SpriteInstance> selectedInstances { get { return instanceListBoxBinding.items; } }

        public SelectBinding<Sprite> tileSpriteFilterSelectBinding;
        public SelectBinding<Spritesheet> tilesetSelectBinding;
        public Spritesheet selectedTileset { get { return tilesetSelectBinding.selected; } }

        public ListBoxBinding<Level> levelListBoxBinding;
        public List<Level> levels { get { return levelListBoxBinding.items; } }
        public Level selectedLevel { get { return levelListBoxBinding.selected; } }

        public ListBoxBinding<Obj> objListBoxBinding;
        public List<Obj> objs { get { return objListBoxBinding.items; } }
        public Obj selectedObj { get { return objListBoxBinding.selected; } }

        public List<Spritesheet> spritesheets = new List<Spritesheet>();
        public List<Sprite> sprites = new List<Sprite>();
        public List<Spritesheet> tilesets = new List<Spritesheet>();

        public SelectBinding<HitboxMode> tileHitboxModeSelectBinding;
        public SelectBinding<Sprite> tileSpriteSelectBinding;

        public float zoom = 1;
        public string newLevelName = "";
        public List<TileData> tileDatas = new List<TileData>();
        public Dictionary<string, List<List<TileData>>> tileDataGrids = new Dictionary<string, List<List<TileData>>>();
        public string multiEditName = "";
        public int multiEditHitboxMode = 0;
        public string multiEditTag = "";
        public int multiEditZIndex = 0;
        public string multiEditTileSprite = "";

        public int loadCount = -1;
        public int maxLoadCount = 0;
        public string selectionProperties = "";
        public List<string> undoJsons = new List<string>();
        public int undoIndex = 0;
        public int selectionElevation = 0;
        public string selectedInstanceProperties = "";

        public int paintElevationHeight = 0;
        public int layerIndex = 0;
        public List<Spritesheet> levelImages = new List<Spritesheet>();
        public string lastSelectedTileSprite = "";
        public string customHitboxPoints = "";
        public GridRect clonedTiles = null;
        public string lastNavMeshCoords = "";

        public bool mode16x16 { get; set; } = false;

        private bool _showTileHitboxes = false;
        public bool showTileHitboxes { get { return _showTileHitboxes; } set { _showTileHitboxes = value; redrawTileCanvas(); } }

        private string _showTilesWithTag = "";
        public string showTilesWithTag { get { return _showTilesWithTag; } set { _showTilesWithTag = value; redrawTileCanvas(); } }

        private bool _showTilesWithZIndex1 = false;
        public bool showTilesWithZIndex1 { get { return _showTilesWithZIndex1; } set { _showTilesWithZIndex1 = value; redrawTileCanvas(); } }

        private string _showTilesWithSprite = "";
        public string showTilesWithSprite { get { return _showTilesWithSprite; } set { _showTilesWithSprite = value; redrawTileCanvas(); } }

        private int _showTileWithSpriteIndex;
        public int showTileWithSpriteIndex { get { return _showTileWithSpriteIndex; } set { _showTileWithSpriteIndex = value; redrawTileCanvas(); } }

        // Level canvas
        private bool _showLevelGrid = true;
        public bool showLevelGrid { get { return _showLevelGrid; } set { _showLevelGrid = value; redrawLevelCanvas(); } }

        private bool _showInstances = true;
        public bool showInstances { get { return _showInstances; } set { _showInstances = value; redrawLevelCanvas(); } }

        private bool _showRoomLines = true;
        public bool showRoomLines { get { return _showRoomLines; } set { _showRoomLines = value; redrawLevelCanvas(); } }

        private string _showOverridesWithKey = "";
        public string showOverridesWithKey { get { return _showOverridesWithKey; } set { _showOverridesWithKey = value; redrawLevelCanvas(); } }

        public Tool selectedTool = Tool.Select;
        public int selectedToolInt { get { return (int)selectedTool; } set { selectedTool = (Tool)value; } }

        private List<GridCoords> _tileSelectedCoords = new List<GridCoords>();
        private List<GridCoords> _levelSelectedCoords = new List<GridCoords>();

        public LevelCanvasUI levelCanvasUI;
        public TileCanvasUI tileCanvasUI;

        public LevelEditor()
        {
            InitializeComponent();

            var objs = getObjectList();
            spritesheets = Helpers.getSpritesheets("spritesheets");
            tilesets = Helpers.getSpritesheets("tilesets");
            tileDatas = Helpers.getTileDatas();
            levelImages = Helpers.getSpritesheets("levelimages");
            sprites = Helpers.getSprites();
            var levels = Helpers.getLevels();

            foreach(var tileData in tileDatas)
            {
                tileData.setTileset(tilesets);
            }
            loadHashCache("");

            levelCanvasUI = new LevelCanvasUI(levelCanvas, levelPanel, this);
            tileCanvasUI = new TileCanvasUI(tileCanvas, tilePanel, this);

            levelListBoxBinding = new ListBoxBinding<Level>(levelListBox, levels, "name", onLevelChange);
            objListBoxBinding = new ListBoxBinding<Obj>(objectsListBox, objs, "name", null);

            tileSpriteFilterSelectBinding = new SelectBinding<Sprite>(tileSpriteFilterSelect, sprites, "name", () => { redrawTileCanvas(); });
            tilesetSelectBinding = new SelectBinding<Spritesheet>(tilesetSelect, tilesets, "name", () => { redrawTileCanvas(); });

            // Global level canvas UI data bindings
            instanceCheckBox.DataBindings.Add("Checked", this, nameof(showInstances), false, DataSourceUpdateMode.OnPropertyChanged);
            gridCheckBox.DataBindings.Add("Checked", this, nameof(showLevelGrid), false, DataSourceUpdateMode.OnPropertyChanged);
            roomLinesCheckBox.DataBindings.Add("Checked", this, nameof(showRoomLines), false, DataSourceUpdateMode.OnPropertyChanged);
            selectedToolPanel.DataBindings.Add("Selected", this, nameof(selectedToolInt), false, DataSourceUpdateMode.OnPropertyChanged);
            overridesTextBox.DataBindings.Add("Text", this, nameof(showOverridesWithKey), false, DataSourceUpdateMode.OnPropertyChanged);
            
            // Global tile canvas UI data bindings
            tileHitboxCheckBox.DataBindings.Add("Checked", this, nameof(showTileHitboxes), false, DataSourceUpdateMode.OnPropertyChanged);
            tileZIndexCheckBox.DataBindings.Add("Checked", this, nameof(showTilesWithZIndex1), false, DataSourceUpdateMode.OnPropertyChanged);
            tileTagFilterTextBox.DataBindings.Add("Text", this, nameof(showTilesWithTag), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public void updateLevelBindings()
        {
            widthTextBox.DataBindings.Clear();
            widthTextBox.DataBindings.Add("Text", selectedLevel, "width", false, DataSourceUpdateMode.OnPropertyChanged);

            heightTextBox.DataBindings.Clear();
            heightTextBox.DataBindings.Add("Text", selectedLevel, "height", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public void updateSelectedTilesetTileBindings()
        {
            List<TileData> selectedTilesetTiles = getTileSelectedTiles();

            tileTagTextBox.DataBindings.Clear();
            tileNameTextBox.DataBindings.Clear();
            tileHitboxModeSelect.DataBindings.Clear();
            tileTagTextBox.DataBindings.Clear();
            tileZIndexSelect.DataBindings.Clear();
            tileSpriteSelect.DataBindings.Clear();

            if (selectedTilesetTiles == null || selectedTilesetTiles.Count == 0)
            {
                tileTagTextBox.Visible = false;
                tileNameTextBox.Visible = false;
                tileHitboxModeSelect.Visible = false;
                tileTagTextBox.Visible = false;
                tileZIndexSelect.Visible = false;
                tileSpriteSelect.Visible = false;
                return;
            }

            TileData firstTile = selectedTilesetTiles[0];

            tileTagTextBox.DataBindings.Add("Text", firstTile, "tag", false, DataSourceUpdateMode.OnPropertyChanged);
            tileNameTextBox.DataBindings.Add("Text", firstTile, "name", false, DataSourceUpdateMode.OnPropertyChanged);
            tileHitboxModeSelect.DataBindings.Add("SelectedIndex", firstTile,binding: )
        }

        public void resetUI()
        {
            selectionLabel.Text = "Selection: " + getSelectionCoords();
            //offsetLabel.DataBindings.Add("Text", this, nameof(), false, DataSourceUpdateMode.OnPropertyChanged);
            tileIdLabel.Text = "Tid: " + getLevelSelectionTileData();
        }

        public void redrawTileCanvas()
        {
            tileCanvas.Invalidate();
        }

        public void redrawLevelCanvas()
        {
            levelCanvas.Invalidate();
        }

        public List<Obj> getObjectList()
        {
            return new List<Obj>()
            {
                new Obj("NPCTest", "NPCIdleDown", false, null),
                new Obj("Octorok", "OctorokWalkDown", false, null),
                new Obj("RockBigGray", "RockBigGray", true, new Models.Point(0, 0)),
                new Obj("RockPile", "RockPile", true, new Models.Point(0, 0)),
                new Obj("WaterRock", "TileWaterRock", true, new Models.Point(0, 0)),
                new Obj("Entrance", "EditorPOI", true, new Models.Point(0, 0)),
                new Obj("DoorSanctuary", "DoorSanctuary", true, new Models.Point(0, 0)),
                new Obj("DoorCastle", "DoorCastle", true, new Models.Point(0, 0)),
                new Obj("Door", "Door", true, new Models.Point(0, 0)),
                new Obj("Weathercock", "Weathercock", true, new Models.Point(0, 0)),
                new Obj("DesertPalaceStone", "DesertPalaceStone", true, new Models.Point(0, 0)),
                new Obj("Whirlpool", "TileWhirlpool", true, new Models.Point(0, 0)),
                new Obj("TorchLit", "TorchLit", true, new Models.Point(0, 0)),
                new Obj("TorchUnlit", "TorchUnlit", true, new Models.Point(0, 0)),
                new Obj("TorchBig", "TorchBig", true, new Models.Point(0, 0)),
                new Obj("ChestSmall", "ChestSmall", true, new Models.Point(0, 0)),
                new Obj("ChestBig", "ChestBig", true, new Models.Point(0, 0)),
                new Obj("Pot", "Pot", true, new Models.Point(0, 0)),
                new Obj("MasterSwordWoods", "MasterSwordWoods", true, new Models.Point(0, 0)),
                new Obj("BigFairy", "BigFairy", true, new Models.Point(0, 0)),
                new Obj("Fairy", "Fairy", true, new Models.Point(0, 0)),
                new Obj("Generic", "EditorPOI", true, new Models.Point(0, 0)),
            };
        }

        public void sortInstances()
        {
            selectedLevel.instances.Sort((SpriteInstance a, SpriteInstance b) => {
                var compare = a.name.CompareTo(b.name);
                if (compare < 0) return -1;
                if (compare > 0) return 1;
                if (compare == 0)
                {
                    if (a.properties != null && b.properties != null)
                    {
                        if (a.properties.ContainsKey("entrance") && b.properties.ContainsKey("entrance"))
                        {
                            var aId = a.properties["entrance"].Split(',')[0];
                            var bId = b.properties["entrance"].Split(',')[0];
                            return aId.CompareTo(bId);
                        }
                    }
                    return 0;
                }
                return 0;
            });
        }

        public List<GridCoords> tileSelectedCoords
        {
            get
            {
                return _tileSelectedCoords;
            }
            set
            {
                _tileSelectedCoords = value;
                updateSelectedTilesetTileBindings();
            }
        }

        public List<GridCoords> levelSelectedCoords
        {
            get
            {
                return _levelSelectedCoords;
            }
            set
            {
                _levelSelectedCoords = value;
                if (value == null || value.Count == 0)
                {
                    instancePropertiesLabel.Visible = true;
                    tileInstancePropsTextBox.Visible = true;
                }
                else
                {
                    instancePropertiesLabel.Visible = false;
                    tileInstancePropsTextBox.Visible = false;
                }
            }
        }

        public List<TileData> getTileSelectedTiles()
        {
            var tileDatas = new HashSet<TileData>();
            var grid = getTileGrid();
            foreach (var gridCoord in tileSelectedCoords)
            {
                var tileData = grid[gridCoord.i][gridCoord.j];
                if (tileData != null)
                {
                    tileDatas.Add(tileData);
                }
            }
            return tileDatas.ToList();
        }

        public List<List<TileData>> getTileGrid()
        {
            return tileDataGrids[selectedTileset.path];
        }

        public string getSelectionCoords()
        {
            var rect = getLevelSelectedGridRect();
            if (rect != null)
            {
                return "(" + (rect.topLeftGridCoords.j) + "," + (rect.topLeftGridCoords.i) + ")," +
                        "(" + (rect.botRightGridCoords.j) + "," + (rect.botRightGridCoords.i) + ")";
            }
            return "";
        }

        public string getLevelSelectionTileData()
        {
            var selectedCoords = levelSelectedCoords;
            if (selectedCoords != null && selectedCoords.Count > 0)
            {
                var selection = selectedLevel.tileInstances[selectedCoords[0].i][selectedCoords[0].j];
                if (selection != null)
                {
                    return selection;
                }
            }
            return "";
        }

        public string getTileSelectionTileData()
        {
            var selectedCoords = tileSelectedCoords;
            var grid = getTileGrid();
            if (selectedCoords != null && selectedCoords.Count > 0)
            {
                var selection = grid[selectedCoords[0].i][selectedCoords[0].j];
                if (selection != null)
                {
                    return selection.getId();
                }
            }
            return "";
        }

        public string getLevelOffset()
        {
            return "(" + levelCanvasUI.offsetX + "," + levelCanvasUI.offsetY + ")";
        }

        public void onLevelSizeChange()
        {
            selectedLevel.resize();
            levelCanvasUI.setSize(selectedLevel.width * Consts.TILE_WIDTH, selectedLevel.height * Consts.TILE_WIDTH);
            redrawLevelCanvas();
        }

        /*
        public void onSelectionElevationChange()
        {
            foreach (var gridCoords in levelSelectedCoords)
            {
                selectedLevel.coordPropertiesGrid[gridCoords.i][gridCoords.j].elevation = selectionElevation.ToString();
            }
        }
        */

        public void onSelectionPropertyChange()
        {
            foreach (var gridCoords in levelSelectedCoords)
            {
                selectedLevel.coordPropertiesGrid[gridCoords.i][gridCoords.j] = JsonConvert.DeserializeObject<Dictionary<string, string>>(selectionProperties);
            }
        }
        public void updateSelectionProperties()
        {
            var firstCoords = levelSelectedCoords[0];
            selectionProperties = JsonConvert.SerializeObject(selectedLevel.coordPropertiesGrid[firstCoords.i][firstCoords.j]);
        }

        public void onSelectedInstancePropertyChange()
        {
            foreach (var instance in selectedInstances)
            {
                instance.properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(selectedInstanceProperties);
            }
        }

        public void addLevel()
        {
            var newLevel = new Level(this.newLevelName, 30, 30);
            levels.Add(newLevel);
        }

        public void onLevelChange(Level oldLevel)
        {
            if (oldLevel != null)
            {
                oldLevel.destroy();
            }

            instanceListBoxBinding = new MultiListBoxBinding<SpriteInstance>(instanceListBox, selectedLevel.instances, "name");
            levelCanvasUI.setSize(selectedLevel.width * Consts.TILE_WIDTH, selectedLevel.height * Consts.TILE_WIDTH);
            levelCanvasUI.loadScrollPos(selectedLevel.name);
            
            var selectedTilesetIndex = 0;
            var selectedTilesetIndexStr = Helpers.getStorageKey("selected_tileset_index_" + selectedLevel.name);
            if (!string.IsNullOrEmpty(selectedTilesetIndexStr)) selectedTilesetIndex = int.Parse(selectedTilesetIndexStr);
            if (selectedTilesetIndex < 0) selectedTilesetIndex = 0;
            tilesetSelectBinding.selectedIndex = selectedTilesetIndex;
            selectedLevel.setLayers(levelImages);
            onTilesetChange(selectedTileset);
            addUndoJson();
            
            redrawLevelCanvas();
            redrawTileCanvas();

            updateLevelBindings();
        }

        public void saveLevel()
        {
            /*
            for(var instance in this.selectedLevel.instances) {
              instance.normalizePoints();
            }
            ?resourceName=tiledata
            ?resourceName=tileanimation
            ?resourceName=tileclump
            */
            saveScrollPositions();
            //var tileDataJsons = Helpers.serializeES6(getTileDatas());
            //console.log(tileDataJsons);
            //var tileAnimationJsons = Helpers.serializeES6(tileAnimations);
            //var tileClumpJsons = Helpers.serializeES6(tileClumps);

            //var levelJson = Helpers.serializeES6(selectedLevel);
        }

        public void redraw(bool redrawTileUICanvas = false)
        {
            redrawLevelCanvas();
            redrawTileCanvas();
        }

        public void changeObj(Obj newObj)
        {
            instanceListBoxBinding.clear();
            selectedTool = Tool.CreateInstance;
            redrawLevelCanvas();
        }

        public void onInstanceClick(SpriteInstance instance)
        {
            instanceListBoxBinding.clear();
            instanceListBoxBinding.add(instance);
            selectedTool = Tool.SelectInstance;
            //levelCanvas.wrapper.scrollTop = instance.pos.y - $(levelCanvas.wrapper).height() / 2;
            //levelCanvas.wrapper.scrollLeft = instance.pos.x - $(levelCanvas.wrapper).width() / 2;
            redrawLevelCanvas();
        }

        public void onTilesetChange(Spritesheet tileset)
        {
            tileset.init(false);
            if (selectedLevel != null)
            {
                Helpers.setStorageKey("selected_tileset_index_" + selectedLevel.name, tilesets.IndexOf(selectedTileset).ToString());
            }
            tileCanvasUI.setSize(selectedTileset.image.Width, selectedTileset.image.Height);

            

            redrawLevelCanvas();
            redrawTileCanvas();
            tileCanvasUI.loadScrollPos(selectedTileset.path);
        }

        public void initMultiEditParams()
        {
            var selectedTiles = getTileSelectedTiles();
            if (selectedTiles.Count == 0) return;
            var firstName = selectedTiles[0].name;
            if (selectedTiles.All(selectedTile => { return selectedTile.name == firstName; }))
            {
                multiEditName = firstName;
            }
            else
            {
                multiEditName = "";
            }
            var firstMode = selectedTiles[0].hitboxMode;
            if (selectedTiles.All(selectedTile => { return selectedTile.hitboxMode == firstMode; }))
            {
                multiEditHitboxMode = (int)firstMode;
            }
            else
            {
                multiEditHitboxMode = -1;
            }
            var firstTag = selectedTiles[0].tag;
            if (selectedTiles.All(selectedTile => { return selectedTile.tag == firstTag; }))
            {
                multiEditTag = firstTag;
            }
            else
            {
                multiEditTag = "";
            }
            var firstZIndex = selectedTiles[0].zIndex;
            if (selectedTiles.All(selectedTile => { return selectedTile.zIndex == firstZIndex; }))
            {
                multiEditZIndex = (int)firstZIndex;
            }
            else
            {
                multiEditZIndex = -1;
            }
            var firstSpriteName = selectedTiles[0].spriteName;
            if (selectedTiles.All(selectedTile => { return selectedTile.spriteName == firstSpriteName; }))
            {
                multiEditTileSprite = firstSpriteName;
            }
            else
            {
                multiEditTileSprite = null;
            }
            var customHitboxPoints = selectedTiles[0].customHitboxPoints;
            if (selectedTiles.All(selectedTile => { return selectedTile.customHitboxPoints == customHitboxPoints; }))
            {
                this.customHitboxPoints = customHitboxPoints;
            }
            else
            {
                customHitboxPoints = "";
            }
        }

        public void setMultiEditName()
        {
            var selectedTiles = getTileSelectedTiles();
            foreach (var selectedTile in selectedTiles)
            {
                selectedTile.name = multiEditName;
            }
        }

        public void setMultiEditHitboxMode()
        {
            showTileHitboxes = true;
            var selectedTiles = getTileSelectedTiles();
            foreach (var selectedTile in selectedTiles)
            {
                selectedTile.hitboxMode = (HitboxMode)multiEditHitboxMode;
            }
            redrawTileCanvas();
        }

        public void setMultiEditPoints()
        {
            var selectedTiles = getTileSelectedTiles();
            foreach (var selectedTile in selectedTiles)
            {
                selectedTile.customHitboxPoints = customHitboxPoints;
            }
            redrawTileCanvas();
        }

        public void setMultiEditTag()
        {
            var selectedTiles = getTileSelectedTiles();
            foreach (var selectedTile in selectedTiles)
            {
                selectedTile.setTag(multiEditTag);
            }
            redrawTileCanvas();
        }

        public void setMultiEditZIndex()
        {
            var selectedTiles = getTileSelectedTiles();
            foreach (var selectedTile in selectedTiles)
            {
                selectedTile.zIndex = (ZIndex)multiEditZIndex;
            }
            redrawTileCanvas();
        }

        public void setMultiEditTileSprite()
        {
            var selectedTiles = getTileSelectedTiles();
            foreach (var selectedTile in selectedTiles)
            {
                selectedTile.spriteName = multiEditTileSprite;
            }
            lastSelectedTileSprite = multiEditTileSprite;
            redrawTileCanvas();
        }

        public List<string> getTileSpriteNames()
        {
            var spriteNames = new List<string>() { "" };
            foreach (var sprite in sprites)
            {
                if (sprite.name.StartsWith("Tile"))
                {
                    spriteNames.Add(sprite.name);
                }
            }
            return spriteNames;
        }

        public void addUndoJson()
        {
            /*
            if (this.undoIndex < this.undoJsons.Count - 1)
            {
                this.undoJsons.Count = this.undoIndex + 1;
            }
            var json = Helpers.serializeES6(selectedLevel);
            this.undoJsons.Add(json);
            if (this.undoJsons.Count > Consts.MAX_UNDOS)
            {
                this.undoJsons.RemoveAt(0);
            }
            this.undoIndex = this.undoJsons.Count - 1;
            */
        }

        public void undo()
        {
            /*
            this.undoIndex--;
            if (this.undoIndex < 0) this.undoIndex = 0;
            else
            {
                var obj = JSON.parse(this.undoJsons[this.undoIndex]);
                var level = Helpers.deserializeES6(obj);
                for (var i = 0; i < levels.length; i++)
                {
                    if (levels[i].name == level.name)
                    {
                        levels[i] = level;
                    }
                }
                changeLevel(level, true);
            }
            */
        }

        public void redo()
        {
            /*
            this.undoIndex++;
            if (this.undoIndex >= this.undoJsons.Count) this.undoIndex = this.undoJsons.Count - 1;
            else
            {
                var obj = JSON.parse(this.undoJsons[this.undoIndex]);
                var level = Helpers.deserializeES6(obj);
                for (var i = 0; i < levels.Count; i++)
                {
                    if (levels[i].name == level.name)
                    {
                        levels[i] = level;
                    }
                }
                changeLevel(level, true);
            }
            */
        }

        /*
        public void downloadImage()
        {
            var link = document.getElementById('link');
            link.setAttribute('download', 'level.png');
            link.setAttribute('href', selectedLevel.layers[0].toDataURL("image/png").replace("image/png", "image/octet-stream"));
            link.click();
        }
        */

        public void loadHashCache(string json)
        {
            foreach (var tileset in tilesets)
            {
                tileset.init(false);
                var tilesetName = Helpers.baseName(tileset.path);
                /*
                var currentHashCache = json[tilesetName];
                if (!currentHashCache)
                {
                    throw "Error: tile set with name \"" + tilesetName + "\" does not have a hash cache entry! Generate one with the C# utility program.";
                }
                */
                var currentHashCache = new Dictionary<string, string>();

                var tileDataCache = new Dictionary<string, TileData>();
                foreach (var tileData in tileDatas)
                {
                    if (tileData.tilesetPath == tileset.path)
                    {
                        tileDataCache[tilesetName + "," + tileData.gridCoords.i.ToString() + "," + tileData.gridCoords.j.ToString()] = tileData;
                    }
                }

                var grid = new List<List<TileData>>();
                for (int i = 0; i < tileset.image.Height / Consts.TILE_WIDTH; i++)
                {
                    var row = new List<TileData>();
                    grid.Add(row);
                    for (var j = 0; j < tileset.image.Width / Consts.TILE_WIDTH; j++)
                    {
                        currentHashCache.TryGetValue(i.ToString() + "," + j.ToString(), out var linkedCoords);
                        if (linkedCoords == null)
                        {
                            row.Add(null);
                            continue;
                        }
                        var otherI = int.Parse(linkedCoords.Split(',')[0]);
                        var otherJ = int.Parse(linkedCoords.Split(',')[1]);

                        tileDataCache.TryGetValue(tilesetName + "," + i.ToString() + "," + j.ToString(), out var tileData);
                        if (tileData == null)
                        {
                            tileData = grid[otherI][otherJ];
                        }
                        if (tileData == null)
                        {
                            tileData = new TileData(tileset, new GridCoords(i, j), "");
                            tileDataCache[tilesetName + "," + tileData.gridCoords.i.ToString() + "," + tileData.gridCoords.j.ToString()] = tileData;
                        }
                        row.Add(tileData);
                    }
                }
                tileDataGrids[tileset.path] = grid;
            }
        }

        public List<TileData> getTileDatas()
        {
            var tileDatas = new HashSet<TileData>();
            foreach (var key in tileDataGrids.Keys)
            {
                var grid = tileDataGrids[key];
                for (var i = 0; i < grid.Count; i++)
                {
                    for (var j = 0; j < grid[i].Count; j++)
                    {
                        if (grid[i][j] != null)
                        {
                            tileDatas.Add(grid[i][j]);
                        }
                    }
                }
            }
            return tileDatas.ToList();
        }

        //If null is returned, then a rectangle wasn't selected, disjoint selection
        public GridRect getLevelSelectedGridRect()
        {
            if (levelSelectedCoords.Count == 0) return null;
            var topLeftSelectedLevelCoord = new GridCoords(levelSelectedCoords.Min(c => c.i), levelSelectedCoords.Min(c => c.j));
            var botRightSelectedLevelCoord = new GridCoords(levelSelectedCoords.Max(c => c.i), levelSelectedCoords.Max(c => c.j));
            var area = (botRightSelectedLevelCoord.j - topLeftSelectedLevelCoord.j + 1) * (botRightSelectedLevelCoord.i - topLeftSelectedLevelCoord.i + 1);
            if (area != levelSelectedCoords.Count) return null;
            return new GridRect(topLeftSelectedLevelCoord.i, topLeftSelectedLevelCoord.j, botRightSelectedLevelCoord.i, botRightSelectedLevelCoord.j);
        }

        public GridRect getTileSelectedGridRect()
        {
            if (tileSelectedCoords.Count == 0) return null;
            var topLeftSelectedTileCoord = new GridCoords(tileSelectedCoords.Min(c => c.i), tileSelectedCoords.Min(c => c.j));
            var botRightSelectedTileCoord = new GridCoords(tileSelectedCoords.Max(c => c.i), tileSelectedCoords.Max(c => c.j));
            var area = (botRightSelectedTileCoord.j - topLeftSelectedTileCoord.j + 1) * (botRightSelectedTileCoord.i - topLeftSelectedTileCoord.i + 1);
            if (area != tileSelectedCoords.Count) return null;
            return new GridRect(topLeftSelectedTileCoord.i, topLeftSelectedTileCoord.j, botRightSelectedTileCoord.i, botRightSelectedTileCoord.j);
        }

        public GridCoords getTopLeftSelectedTileGridCoord()
        {
            return new GridCoords(tileSelectedCoords.Min(c => c.i), tileSelectedCoords.Min(c => c.j));
        }

        public void saveScrollPositions()
        {
            levelCanvasUI.saveScrollPos(selectedLevel.name);
            tileCanvasUI.saveScrollPos(selectedTileset.path);
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

        private void levelListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void objectsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void instanceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void saveBtn_Click(object sender, EventArgs e)
        {

        }

        private void undoBtn_Click(object sender, EventArgs e)
        {

        }

        private void redoBtn_Click(object sender, EventArgs e)
        {

        }

        private void tilesetSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void instanceCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

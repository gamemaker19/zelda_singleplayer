using GameEditor.Editor;
using GameEditor.Models;
using LevelEditor_CS.Editor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Point = GameEditor.Models.Point;

namespace GameEditor
{
    public enum Tool
    {
        Select = 0,
        PlaceTile,
        RectangleTile,
        SelectInstance,
        CreateInstance,
        FloodfillTile
    }

    /// <summary>
    /// Interaction logic for LevelEditor.xaml
    /// </summary>
    public partial class LevelEditor : Window, INotifyPropertyChanged
    {
        public float zoom = 1;
        public string newLevelName { get; set; }
        public List<TileData> tileDatas = new List<TileData>();
        public static Dictionary<string, List<List<TileData>>> tileDataGrids = new Dictionary<string, List<List<TileData>>>();

        public int loadCount = -1;
        public int maxLoadCount = 0;
        public List<string> undoJsons = new List<string>();
        public int undoIndex = 0;
        public int selectionElevation = 0;

        public int paintElevationHeight = 0;
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

        private Tool _selectTool = Tool.Select;
        public Tool selectedTool { get { return _selectTool; } set { _selectTool = value; notifyPropertyChanged(); } }
        public int selectedToolInt { get { return (int)selectedTool; } set { selectedTool = (Tool)value; } }

        private ObservableCollection<GridCoords> _tileSelectedCoords = new ObservableCollection<GridCoords>();
        private ObservableCollection<GridCoords> _levelSelectedCoords = new ObservableCollection<GridCoords>();

        public List<Spritesheet> spritesheets { get; set; }
        public List<Spritesheet> tilesets { get; set; }
        public List<Sprite> sprites { get; set; }
        public ObservableCollection<string> spriteNames { get; set; } = new ObservableCollection<string>();

        private Level _selectedLevel;
        public Level selectedLevel { get { return _selectedLevel; } set { _selectedLevel = value; notifyPropertyChanged(); } }
        public ObservableCollection<Level> levels { get; set; }
        private Spritesheet _selectedTileset;
        public Spritesheet selectedTileset { get { return _selectedTileset; } set { _selectedTileset = value; notifyPropertyChanged(); } }

        public List<SpriteInstance> selectedInstances
        {
            get
            {
                List<SpriteInstance> _selectedInstances = new List<SpriteInstance>();
                foreach (var instance in instanceListBox.SelectedItems)
                {
                    _selectedInstances.Add((SpriteInstance)instance);
                }
                return _selectedInstances;
            }
        }

        public void addSelectedInstance(SpriteInstance instance)
        {
            if (!instanceListBox.SelectedItems.Contains(instance))
            {
                instanceListBox.SelectedItems.Add(instance);
            }
        }

        public void removeSelectedInstance(SpriteInstance instance)
        {
            instanceListBox.SelectedItems.Remove(instance);
        }

        public void clearSelectedInstances()
        {
            instanceListBox.SelectedItems.Clear();
        }

        public List<Obj> objs { get; set; }
        private Obj _selectedObj;

        public TileCanvasUI tileCanvasUI;
        public LevelCanvasUI levelCanvasUI;

        public List<HitboxMode> hitboxModes { get; set; } = Helpers.getEnumList<HitboxMode>();
        public List<ZIndex> zIndices { get; set; } = Helpers.getEnumList<ZIndex>();

        public LevelEditor()
        {
            InitializeComponent();
            this.DataContext = this;

            objs = getObjectList();
            spritesheets = Helpers.getSpritesheets("spritesheets");
            tilesets = Helpers.getSpritesheets("tilesets");
            tileDatas = Helpers.getTileDatas();
            levelImages = Helpers.getSpritesheets("levelimages");
            sprites = Helpers.getSprites();
            levels = new ObservableCollection<Level>(Helpers.getLevels());
            spriteNames.Add("");

            foreach (var sprite in sprites)
            {
                sprite.spritesheets = spritesheets;
                spriteNames.Add(sprite.name);
            }
            foreach (var tileData in tileDatas)
            {
                tileData.setTileset(tilesets);
            }
            foreach (var level in levels)
            {
                foreach (var instance in level.instances)
                {
                    instance.setSprite(sprites);
                }
            }

            setTileDataGrids();

            levelCanvasUI = new LevelCanvasUI(levelScroll, this);
            tileCanvasUI = new TileCanvasUI(tileScroll, this);

            _tileSelectedCoords.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) =>
            {
                resetUI();
            };

            _levelSelectedCoords.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) =>
            {
                resetUI();
            };

            App.applicationExitAction = onApplicationExit;
        }

        public string selectedTileId
        {
            get
            {
                var tiles = getTileSelectedTiles();
                if (tiles.Count != 1) return "";
                return tiles[0].getId();
            }
        }

        public string layerShowOrHide
        {
            get
            {
                if (selectedLevel == null || selectedLevel.layers.Count == 0) return "Hide";
                if(selectedLevel.layers[selectedLayer].isHidden)
                {
                    return "Show";
                }
                return "Hide";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void notifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public void resetUI()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(""));
            }
        }

        public List<Obj> getObjectList()
        {
            return new List<Obj>()
            {
                new Obj("NPCTest", "NPCIdleDown", false, new Point(0, 0)),
                new Obj("Octorok", "OctorokWalkDown", false, new Point(0, 0)),
                new Obj("RockBigGray", "RockBigGray", true, new Point(0, 0)),
                new Obj("RockPile", "RockPile", true, new Point(0, 0)),
                new Obj("WaterRock", "TileWaterRock", true, new Point(0, 0)),
                new Obj("Entrance", "EditorPOI", true, new Point(0, 0)),
                new Obj("DoorSanctuary", "DoorSanctuary", true, new Point(0, 0)),
                new Obj("DoorCastle", "DoorCastle", true, new Point(0, 0)),
                new Obj("Door", "Door", true, new Point(0, 0)),
                new Obj("Weathercock", "Weathercock", true, new Point(0, 0)),
                new Obj("DesertPalaceStone", "DesertPalaceStone", true, new Point(0, 0)),
                new Obj("Whirlpool", "TileWhirlpool", true, new Point(0, 0)),
                new Obj("TorchLit", "TorchLit", true, new Point(0, 0)),
                new Obj("TorchUnlit", "TorchUnlit", true, new Point(0, 0)),
                new Obj("TorchBig", "TorchBig", true, new Point(0, 0)),
                new Obj("ChestSmall", "ChestSmall", true, new Point(0, 0)),
                new Obj("ChestBig", "ChestBig", true, new Point(0, 0)),
                new Obj("Pot", "Pot", true, new Point(0, 0)),
                new Obj("MasterSwordWoods", "MasterSwordWoods", true, new Point(0, 0)),
                new Obj("BigFairy", "BigFairy", true, new Point(0, 0)),
                new Obj("Fairy", "Fairy", true, new Point(0, 0)),
                new Obj("Generic", "EditorPOI", true, new Point(0, 0)),
            };
        }

        public void redrawTileCanvas()
        {
            tileCanvasUI.redraw();
        }

        public void redrawLevelCanvas()
        {
            levelCanvasUI.redraw();
        }

        public Obj selectedObj
        {
            get
            {
                return _selectedObj;
            }
            set
            {
                _selectedObj = value;
                selectedTool = Tool.CreateInstance;
            }
        }

        public bool hasTileSelectedTiles
        {
            get { return _tileSelectedCoords.Count > 0; }
        }

        public bool hasLevelSelectedTile
        {
            get { return _levelSelectedCoords.Count > 0; }
        }

        public bool hasSelectedInstance
        {
            get { return selectedInstances?.Count > 0; }
        }

        public void sortInstances()
        {
            var instanceList = selectedLevel.instances.ToList();

            instanceList.Sort((SpriteInstance a, SpriteInstance b) =>
            {
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

            selectedLevel.instances = new ObservableCollection<SpriteInstance>(instanceList);
        }

        public ObservableCollection<GridCoords> tileSelectedCoords
        {
            get
            {
                return _tileSelectedCoords;
            }
            set
            {
                _tileSelectedCoords = value;
            }
        }

        public ObservableCollection<GridCoords> levelSelectedCoords
        {
            get
            {
                return _levelSelectedCoords;
            }
            set
            {
                _levelSelectedCoords = value;
            }
        }

        public ObservableCollection<int> layers
        {
            get
            {
                var retList = new ObservableCollection<int>();
                if (selectedLevel == null) return retList;
                for (int i = 0; i < selectedLevel.layers.Count; i++)
                {
                    retList.Add(i);
                }
                return retList;
            }
        }

        private int _selectedLayer;
        public int selectedLayer
        {
            get
            {
                return _selectedLayer;
            }
            set
            {
                _selectedLayer = value;
                notifyPropertyChanged("layerShowOrHide");
            }
        }
        public Bitmap selectedLayerBitmap 
        {
            get
            {
                return selectedLevel.layers[selectedLayer].bitmap;
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
            if (selectedTileset == null) return new List<List<TileData>>();
            return tileDataGrids[selectedTileset.getName()];
        }

        public TileData getTile(int i, int j)
        {
            var grid = getTileGrid();
            if (i >= grid.Count) return null;
            if (j >= grid[0].Count) return null;
            return grid[i][j];
        }

        public string getSelectionCoords
        {
            get
            {
                var rect = getLevelSelectedGridRect();
                if (rect != null)
                {
                    return "Selection: (" + (rect.topLeftGridCoords.j) + "," + (rect.topLeftGridCoords.i) + ")," +
                            "(" + (rect.botRightGridCoords.j) + "," + (rect.botRightGridCoords.i) + ")";
                }
                return "";
            }
        }

        public string getLevelSelectionTileData
        {
            get
            {
                var selectedCoords = levelSelectedCoords;
                if (selectedCoords != null && selectedCoords.Count > 0)
                {
                    string selection = selectedLevel.tileInstances[selectedLayer][selectedCoords[0].i][selectedCoords[0].j];
                    if (selection != null)
                    {
                        return "Selection: " + selection;
                    }
                }
                return "";
            }
        }

        public string getTileSelectionTileData
        {
            get
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
        }

        public string selectionProperties
        {
            get
            {
                if (levelSelectedCoords == null || levelSelectedCoords.Count == 0)
                {
                    return "";
                }
                var firstCoords = levelSelectedCoords[0];
                string results = JsonConvert.SerializeObject(selectedLevel.coordPropertiesGrid[firstCoords.i][firstCoords.j]);
                if (results == "{}")
                {
                    return "";
                }
                return results;
            }
            set
            {
                Dictionary<string, string> json = new Dictionary<string, string>();
                try
                {
                    json = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
                }
                catch
                {
                    Console.WriteLine("Invalid json!");
                    return;
                }
                foreach (var gridCoords in levelSelectedCoords)
                {
                    selectedLevel.coordPropertiesGrid[gridCoords.i][gridCoords.j] = json;
                }
            }
        }

        public string selectedInstanceX
        {
            get
            {
                if(selectedInstances.Count > 1)
                {
                    return "-";
                }
                else if(selectedInstances.Count == 1)
                {
                    return selectedInstances[0].pos.x.ToString();
                }
                return "";
            }
            set
            {
                foreach(var instance in selectedInstances)
                {
                    try
                    {
                        instance.pos.x = int.Parse(value);
                    }
                    catch { }
                }
            }
        }

        public string selectedInstanceY
        {
            get
            {
                if (selectedInstances.Count > 1)
                {
                    return "-";
                }
                else if (selectedInstances.Count == 1)
                {
                    return selectedInstances[0].pos.y.ToString();
                }
                return "";
            }
            set
            {
                foreach (var instance in selectedInstances)
                {
                    try
                    {
                        instance.pos.y = int.Parse(value);
                    }
                    catch { }
                }
            }
        }

        public string selectedInstanceProperties
        {
            get
            {
                if (selectedInstances.Count > 1)
                {
                    return "-";
                }
                else if (selectedInstances.Count == 1 && selectedInstances[0].properties != null)
                {
                    return JsonConvert.SerializeObject(selectedInstances[0].properties);
                }
                return "";
            }
            set
            {
                foreach (var instance in selectedInstances)
                {
                    try
                    {
                        instance.properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
                    }
                    catch
                    {

                    }
                }
            }
        }

        public string getLevelOffset()
        {
            return "(" + levelCanvasUI.offsetX + "," + levelCanvasUI.offsetY + ")";
        }

        //public void onSelectionElevationChange()
        //{
        //    foreach (var gridCoords in levelSelectedCoords)
        //    {
        //        selectedLevel.coordPropertiesGrid[gridCoords.i][gridCoords.j].elevation = selectionElevation.ToString();
        //    }
        //}

        public void onSelectedInstancePropertyChange()
        {
            foreach (var instance in selectedInstances)
            {
                instance.properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(selectedInstanceProperties);
            }
        }

        public void addLevel()
        {
            var newLevel = new Level(this.newLevelName, 128, 128);
            levels.Add(newLevel);
            resetUI();
        }

        public Spritesheet getTilesetWithName(string tilesetName)
        {
            return tilesets.Where((t) => { return t.name == tilesetName; }).FirstOrDefault();
        }

        public void onLevelChange(Level oldLevel)
        {
            if (oldLevel != null)
            {
                oldLevel.destroy();
            }

            levelCanvasUI.setSize(selectedLevel.width * Consts.TILE_WIDTH, selectedLevel.height * Consts.TILE_WIDTH);
            levelCanvasUI.loadScrollPos(selectedLevel.name);

            var selectedTilesetIndex = 0;
            var selectedTilesetIndexStr = Helpers.getStorageKey("selected_tileset_index_" + selectedLevel.name);
            if (!string.IsNullOrEmpty(selectedTilesetIndexStr)) selectedTilesetIndex = int.Parse(selectedTilesetIndexStr);
            if (selectedTilesetIndex < 0) selectedTilesetIndex = 0;
            if (selectedTilesetIndex >= tilesets.Count) selectedTilesetIndex = 0;
            selectedTileset = tilesets[selectedTilesetIndex];
            selectedLevel.setLayers(levelImages);
            onTilesetChange(selectedTileset);
            addUndoJson();
            resetUI();
            redrawLevelCanvas();
            redrawTileCanvas();
        }

        public void levelListboxChanged(object sender, SelectionChangedEventArgs e)
        {
            Level oldLevel = e.RemovedItems.Count > 0 ? (Level)e.RemovedItems[0] : null;
            onLevelChange(oldLevel);
        }

        public void instanceListboxChanged(object sender, SelectionChangedEventArgs e)
        {
            redrawLevelCanvas();
        }

        public void saveLevel()
        {
            saveScrollPositions();

            List<TileData> tileDatas = getTileDatas();
            Helpers.saveTileDatas(tileDatas);

            if (selectedLevel != null)
            {
                Helpers.saveLevel(selectedLevel);
            }

            Prompt.ShowMessage("Level successfully saved", "Level successfully saved");
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

        public void redraw(bool redrawTileUICanvas = false)
        {
            redrawLevelCanvas();
            redrawTileCanvas();
        }

        public void changeObj(Obj newObj)
        {
            //instanceListBoxBinding.clear();
            selectedTool = Tool.CreateInstance;
            redrawLevelCanvas();
        }

        public void onInstanceClick(SpriteInstance instance)
        {
            //instanceListBoxBinding.clear();
            //instanceListBoxBinding.add(instance);
            selectedTool = Tool.SelectInstance;
            //levelCanvas.wrapper.scrollTop = instance.pos.y - $(levelCanvas.wrapper).height() / 2;
            //levelCanvas.wrapper.scrollLeft = instance.pos.x - $(levelCanvas.wrapper).width() / 2;
            redrawLevelCanvas();
        }

        public void tilesetComboBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            onTilesetChange(selectedTileset);
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

        public string multiEditName
        {
            get
            {
                var selectedTiles = getTileSelectedTiles();
                if (selectedTiles.Count == 0) return "";
                var firstName = selectedTiles[0].name;
                if (selectedTiles.All(selectedTile => { return selectedTile.name == firstName; }))
                {
                    return firstName;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                var selectedTiles = getTileSelectedTiles();
                foreach (var selectedTile in selectedTiles)
                {
                    selectedTile.name = value;
                }
                resetUI();
            }
        }

        public HitboxMode multiEditHitboxMode
        {
            get
            {
                var selectedTiles = getTileSelectedTiles();
                if (selectedTiles.Count == 0) return HitboxMode.None;
                var firstMode = selectedTiles[0].hitboxMode;
                if (selectedTiles.All(selectedTile => { return selectedTile.hitboxMode == firstMode; }))
                {
                    return firstMode;
                }
                else
                {
                    return HitboxMode.None;
                }
            }
            set
            {
                showTileHitboxes = true;
                var selectedTiles = getTileSelectedTiles();
                foreach (var selectedTile in selectedTiles)
                {
                    selectedTile.hitboxMode = value;
                }
                resetUI();
                redrawTileCanvas();
            }
        }

        public string multiEditTag
        {
            get
            {
                var selectedTiles = getTileSelectedTiles();
                if (selectedTiles.Count == 0) return "";
                var firstTag = selectedTiles[0].tag;
                if (selectedTiles.All(selectedTile => { return selectedTile.tag == firstTag; }))
                {
                    return firstTag;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                var selectedTiles = getTileSelectedTiles();
                foreach (var selectedTile in selectedTiles)
                {
                    selectedTile.setTag(value);
                }
                resetUI();
                redrawTileCanvas();
            }
        }

        public ZIndex multiEditZIndex
        {
            get
            {
                var selectedTiles = getTileSelectedTiles();
                if (selectedTiles.Count == 0) return ZIndex.Default;
                var firstZIndex = selectedTiles[0].zIndex;
                if (selectedTiles.All(selectedTile => { return selectedTile.zIndex == firstZIndex; }))
                {
                    return firstZIndex;
                }
                else
                {
                    return ZIndex.Default;
                }
            }
            set
            {
                var selectedTiles = getTileSelectedTiles();
                foreach (var selectedTile in selectedTiles)
                {
                    selectedTile.zIndex = value;
                }
                resetUI();
                redrawTileCanvas();
            }
        }

        public string multiEditTileSprite
        {
            get
            {
                var selectedTiles = getTileSelectedTiles();
                if (selectedTiles.Count == 0) return "";
                var firstSpriteName = selectedTiles[0].spriteName;
                if (selectedTiles.All(selectedTile => { return selectedTile.spriteName == firstSpriteName; }))
                {
                    return firstSpriteName;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                var selectedTiles = getTileSelectedTiles();
                foreach (var selectedTile in selectedTiles)
                {
                    selectedTile.spriteName = value;
                }
                lastSelectedTileSprite = value;
                resetUI();
                redrawTileCanvas();
            }
        }

        public void addUndoJson()
        {
            //if (this.undoIndex < this.undoJsons.Count - 1)
            //{
            //    this.undoJsons.Count = this.undoIndex + 1;
            //}
            //var json = Helpers.serializeES6(selectedLevel);
            //this.undoJsons.Add(json);
            //if (this.undoJsons.Count > Consts.MAX_UNDOS)
            //{
            //    this.undoJsons.RemoveAt(0);
            //}
            //this.undoIndex = this.undoJsons.Count - 1;
        }

        public void undo()
        {
            //this.undoIndex--;
            //if (this.undoIndex < 0) this.undoIndex = 0;
            //else
            //{
            //    var obj = JSON.parse(this.undoJsons[this.undoIndex]);
            //    var level = Helpers.deserializeES6(obj);
            //    for (var i = 0; i < levels.length; i++)
            //    {
            //        if (levels[i].name == level.name)
            //        {
            //            levels[i] = level;
            //        }
            //    }
            //    changeLevel(level, true);
            //}
        }

        public void redo()
        {
            //this.undoIndex++;
            //if (this.undoIndex >= this.undoJsons.Count) this.undoIndex = this.undoJsons.Count - 1;
            //else
            //{
            //    var obj = JSON.parse(this.undoJsons[this.undoIndex]);
            //    var level = Helpers.deserializeES6(obj);
            //    for (var i = 0; i < levels.Count; i++)
            //    {
            //        if (levels[i].name == level.name)
            //        {
            //            levels[i] = level;
            //        }
            //    }
            //    changeLevel(level, true);
            //}
        }

        public void downloadImage()
        {
            //var link = document.getElementById('link');
            //link.setAttribute('download', 'level.png');
            //link.setAttribute('href', selectedLevel.layers[0].toDataURL("image/png").replace("image/png", "image/octet-stream"));
            //link.click();
        }
        
        public void setTileDataGrids()
        {
            foreach (var tileset in tilesets)
            {
                tileset.init(false);
                var tilesetName = Helpers.baseName(tileset.path);

                var currentHashCache = getHashCaches(tileset.path);

                var tileDataCache = new Dictionary<string, TileData>();
                foreach (var tileData in tileDatas)
                {
                    if (tileData.tilesetPath == tileset.getBasePath())
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
                        if (string.IsNullOrEmpty(linkedCoords))
                        {
                            row.Add(null);
                            continue;
                        }
                        var otherI = int.Parse(linkedCoords.Split(',')[0]);
                        var otherJ = int.Parse(linkedCoords.Split(',')[1]);

                        tileDataCache.TryGetValue(tilesetName + "," + otherI.ToString() + "," + otherJ.ToString(), out var tileData);
                        if (tileData == null)
                        {
                            tileData = new TileData(tileset, new GridCoords(i, j), "");
                            tileDataCache[tilesetName + "," + tileData.gridCoords.i.ToString() + "," + tileData.gridCoords.j.ToString()] = tileData;
                        }
                        row.Add(tileData);
                    }
                }
                tileDataGrids[tilesetName] = grid;
            }
        }

        static Dictionary<string, string> getHashCaches(string tileset)
        {
            string tilesetName = Helpers.baseName(tileset);
            var currentHashCache = new Dictionary<string, string>();

            Bitmap bitmap = new Bitmap(tileset);
            LockBitmap lockBitmap = new LockBitmap(bitmap);
            lockBitmap.LockBits();

            Dictionary<string, GridCoords> tileHashToCoords = getTileHash(lockBitmap, tilesetName);
            for (var i = 0; i < bitmap.Height / 8; i++)
            {
                for (var j = 0; j < bitmap.Width / 8; j++)
                {
                    var hash = getTileHash(lockBitmap, i, j, tilesetName);
                    if (string.IsNullOrEmpty(hash))
                    {
                        currentHashCache[i.ToString() + "," + j.ToString()] = "";
                        continue;
                    }
                    var coords = tileHashToCoords[hash];
                    currentHashCache[i.ToString() + "," + j.ToString()] = coords.i.ToString() + "," + coords.j.ToString();
                }
            }

            lockBitmap.UnlockBits();

            return currentHashCache;
        }

        static Dictionary<string, GridCoords> getTileHash(LockBitmap image, string basename)
        {
            var tileHashToCoords = new Dictionary<string, GridCoords>();
            int duplicates = 0;
            for (int i = 0; i < image.Height / 8; i++)
            {
                for (var j = 0; j < image.Width / 8; j++)
                {
                    var hash = getTileHash(image, i, j, basename);
                    if (!string.IsNullOrEmpty(hash) && !tileHashToCoords.ContainsKey(hash))
                    {
                        tileHashToCoords[hash] = new GridCoords(i, j);
                    }
                    else
                    {
                        duplicates++;
                    }
                }
            }
            //Console.WriteLine(duplicates);
            return tileHashToCoords;
        }


        static Dictionary<string, string> cachedTileHashes = new Dictionary<string, string>();
        //Returns empty string if entirely transparent
        static string getTileHash(LockBitmap image, int i, int j, string cacheKey)
        {
            string key = cacheKey + "," + i.ToString() + "," + j.ToString();
            if (cachedTileHashes.ContainsKey(key))
            {
                return cachedTileHashes[key];
            }

            StringBuilder sb = new StringBuilder();
            var hash = "";
            var numTransparent = 0;

            for (int y = i * 8; y < i * 8 + 8; y++)
            {
                for (int x = j * 8; x < j * 8 + 8; x++)
                {
                    //Get Both Colours at the pixel point
                    Color col1 = image.GetPixel(x, y);
                    if (col1.A == 0) numTransparent++;
                    //int colInt = col1.ToArgb();
                    char r = (char)col1.R;
                    char g = (char)col1.G;
                    char b = (char)col1.B;
                    char a = (char)col1.A;
                    sb.Append(r);
                    sb.Append(g);
                    sb.Append(b);
                    sb.Append(a);
                    sb.Append(",");
                }
            }

            hash = sb.ToString();
            if (numTransparent == 8 * 8) hash = "";
            cachedTileHashes[cacheKey + "," + i.ToString() + "," + j.ToString()] = hash;
            return hash;
        }

        //public List<TileData> getTileDatas()
        //{
        //    var tileDatas = new HashSet<TileData>();
        //    foreach (var key in tileDataGrids.Keys)
        //    {
        //        var grid = tileDataGrids[key];
        //        for (var i = 0; i < grid.Count; i++)
        //        {
        //            for (var j = 0; j < grid[i].Count; j++)
        //            {
        //                if (grid[i][j] != null)
        //                {
        //                    tileDatas.Add(grid[i][j]);
        //                }
        //            }
        //        }
        //    }
        //    return tileDatas.ToList();
        //}

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

        private void onSaveButtonClicked(object sender, RoutedEventArgs e)
        {
            saveLevel();
        }

        private void onNewLevelButtonClicked(object sender, RoutedEventArgs e)
        {
            addLevel();
        }

        private void onNewLayerButtonClicked(object sender, RoutedEventArgs e)
        {
            selectedLevel.addLayer();
            selectedLayer = selectedLevel.layers.Count - 1;
            resetUI();
            redrawLevelCanvas();
        }

        private void onDeleteLayerButtonClicked(object sender, RoutedEventArgs e)
        {
            selectedLevel.removeLayer(selectedLayer);
            if (selectedLayer >= selectedLevel.layers.Count)
            {
                selectedLayer = selectedLevel.layers.Count - 1;
            }
            resetUI();
            redrawLevelCanvas();
        }

        private void onHideLayerButtonClicked(object sender, RoutedEventArgs e)
        {
            selectedLevel.layers[selectedLayer].isHidden = !selectedLevel.layers[selectedLayer].isHidden;
            resetUI();
            redrawLevelCanvas();
        }

        public void onApplicationExit()
        {
            if (selectedLevel == null) return;
            if (selectedTileset == null) return;
            levelCanvasUI.saveScrollPos(selectedLevel.name);
            tileCanvasUI.saveScrollPos(selectedTileset.path);
            Helpers.saveStorageKeys();
        }
    }
}

using GameEditor.Editor;
using GameEditor.Models;
using LevelEditor_CS.Editor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Frame = GameEditor.Models.Frame;

namespace GameEditor
{
    /// <summary>
    /// Interaction logic for SpriteEditor.xaml
    /// </summary>
    public partial class SpriteEditor : Window, INotifyPropertyChanged
    {
        public Dictionary<string, Sprite> origSprites = new Dictionary<string, Sprite>();
        public ObservableCollection<Sprite> sprites { get; set; } = new ObservableCollection<Sprite>();
        public List<Spritesheet> spritesheets { get; set; }
        public GlobalInput globalInput;

        public bool isAnimPlaying { get; set; } = false;
        public bool addPOIMode { get; set; } = false;
        public List<string> alignments { get; set; } = new List<string> { "topleft", "topmid", "topright", "midleft", "center", "midright", "botleft", "botmid", "botright" };
        public List<string> wrapModes { get; set; } = new List<string> { "loop", "once", "pingpong", "pingpongonce" };
        public float offsetX { get; set; } = 0;
        public float offsetY { get; set; } = 0;
        public float bulkDuration { get; set; } = 0;
        public string newSpriteName { get; set; } = "";
        public Ghost ghost { get; set; }
        public int lastSelectedFrameIndex { get; set; } = 0;
        public string spriteFilter { get; set; } = "";
        public string selectedFilterMode { get; set; } = "contains";
        public float tileWidth { get; set; } = 8;
        public int animFrameIndex { get; set; } = 0;
        public float animTime { get; set; } = 0;

        private bool _hideGizmos = false;
        public bool hideGizmos { get { return _hideGizmos; } set { _hideGizmos = value; redraw(); } }

        private bool _flipX = false;
        public bool flipX { get { return _flipX; } set { _flipX = value; redraw(); } }

        private bool _flipY = false;
        public bool flipY { get { return _flipY; } set { _flipY = value; redraw(); } }

        private bool _tileMode = false;
        public bool tileMode { get { return _tileMode; } set { _tileMode = value; redraw(); } }

        private bool _tileModeOffsetX = false;
        public bool tileModeOffsetX { get { return _tileModeOffsetX; } set { _tileModeOffsetX = value; redraw(); } }

        private bool _tileModeOffsetY = false;
        public bool tileModeOffsetY { get { return _tileModeOffsetY; } set { _tileModeOffsetY = value; redraw(); } }

        public SpriteCanvasUI spriteCanvasUI;
        public SpritesheetCanvasUI spritesheetCanvasUI;

        public Sprite selectedSprite { get; set; }

        public Spritesheet selectedSpritesheet { get; set; }

        private Selectable _selection;
        public Selectable selection
        {
            get
            {
                return _selection;
            }
            set
            {
                if (_selection != null)
                {
                    _selection.isSelected = false;
                }
                _selection = value;
                if (_selection != null)
                {
                    _selection.isSelected = true;
                }
                resetUI();
            }
        }

        private Frame _selectedFrame;
        public Frame selectedFrame
        {
            get
            {
                return _selectedFrame;
            }
            set
            {
                _selectedFrame = value;
                selection = _selectedFrame;
            }
        }

        public SpriteEditor()
        {
            InitializeComponent();
            this.DataContext = this;

            spriteCanvasUI = new SpriteCanvasUI(spriteScroll, this);
            spritesheetCanvasUI = new SpritesheetCanvasUI(spriteSheetScroll, this);

            spritesheets = Helpers.getSpritesheets("spritesheets");
            sprites = new ObservableCollection<Sprite>(Helpers.getSprites());

            foreach (var sprite in sprites)
            {
                sprite.spritesheets = spritesheets;
            }

            globalInput = new GlobalInput(mainCanvas, this);
            globalInput.onKeyDown = (Key key, bool firstFrame) =>
            {
                if (firstFrame)
                {
                    if (key == Key.E)
                    {
                        selectNextFrame();
                        spriteCanvasUI.redraw();
                        resetUI();
                    }
                    else if (key == Key.Q)
                    {
                        selectPrevFrame();
                        spriteCanvasUI.redraw();
                        resetUI();
                    }
                }
            };

            //spritesheetCanvasPanel.Scroll += spritesheetCanvasPanel_Scroll;

            var task = Task.Run(async () =>
            {
                for (; ; )
                {
                    await Task.Delay(1);
                    mainLoop();
                }
            });
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

        public void resetFrames()
        {
        }

        public void resetHitboxes()
        {
        }

        /*
        private void spritesheetCanvasPanel_Scroll(Object sender, ScrollEventArgs e)
        {
            spritesheetCanvasUI.redraw();
        }
        */

        private void SpriteEditor_Load(object sender, EventArgs e)
        {

        }

        private void spritesheetSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            onSpritesheetChange(selectedSpritesheet);
        }

        /*
        private void spriteListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        */

        public ObservableCollection<Sprite> getFilteredSprites()
        {
            var filters = spriteFilter.Split(',').ToList();
            if (filters[0] == "") return sprites;
            var spriteList = sprites.Where((sprite) =>
            {
                if (selectedFilterMode == "exactmatch")
                {
                    return filters.IndexOf(sprite.name) >= 0;
                }
                else if (selectedFilterMode == "contains")
                {
                    foreach (var filter in filters)
                    {
                        if (sprite.name.ToLower().Contains(filter.ToLower()))
                        {
                            return true;
                        }
                    }
                }
                else if (selectedFilterMode == "startswith")
                {
                    foreach (var filter in filters)
                    {
                        if (sprite.name.StartsWith(filter))
                        {
                            return true;
                        }
                    }
                }
                else if (selectedFilterMode == "endswith")
                {
                    foreach (var filter in filters)
                    {
                        if (sprite.name.EndsWith(filter))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }).ToList();
            return new ObservableCollection<Sprite>(spriteList);
        }

        public void onSpritesheetChange(Spritesheet newSheet)
        {
            selectedSpritesheet = newSheet;

            if (newSheet == null) return;

            newSheet.init(true);

            if (selectedSprite != null)
            {
                selectedSprite.spritesheetPath = newSheet.getBasePath();
            }

            spritesheetCanvasUI.setImage(newSheet.image);
            spriteCanvasUI.redraw();
            spritesheetCanvasUI.redraw();
        }

        public string getSpriteDisplayName(Sprite sprite)
        {
            return sprite.name + (isSpriteChanged(sprite) ? "*" : "");
        }

        public void addSprite()
        {
            string spritename = Prompt.ShowDialog("Enter sprite name", "Enter a sprite name");
            var newSprite = new Sprite(spritename, spritesheets);
            this.changeSprite(newSprite);
            sprites.Add(newSprite);
            spriteListBox.ScrollIntoView(newSprite);
            selectedFrame = null;
            selection = null;
        }

        public void changeSprite(Sprite newSprite)
        {
            selectedSprite = newSprite;
            if (newSprite.spritesheet != selectedSpritesheet)
            {
                this.onSpritesheetChange(newSprite.spritesheet);
            }
            selection = null;
            selectedFrame = selectedSprite.frames.Count > 0 ? selectedSprite.frames[0] : null;
            lastSelectedFrameIndex = 0;
            spriteCanvasUI.redraw();
            spritesheetCanvasUI.redraw();
            resetUI();
            resetFrames();
        }

        public void addHitboxToSprite(Sprite sprite)
        {
            var hitbox = new Hitbox();
            hitbox.width = selectedFrame.rect.w;
            hitbox.height = selectedFrame.rect.h;
            sprite.hitboxes.Add(hitbox);
            this.selectHitbox(hitbox);
            spriteCanvasUI.redraw();
        }

        public void addHitboxToFrame(Frame frame)
        {
            var hitbox = new Hitbox();
            hitbox.width = selectedFrame.rect.w;
            hitbox.height = selectedFrame.rect.h;
            frame.hitboxes.Add(hitbox);
            this.selectHitbox(hitbox);
            spriteCanvasUI.redraw();
        }

        public void selectHitbox(Hitbox hitbox)
        {
            selection = hitbox;
            spriteCanvasUI.redraw();
        }

        public void deleteHitbox(List<Hitbox> hitboxArr, Hitbox hitbox)
        {
            hitboxArr.Remove(hitbox);
            spriteCanvasUI.redraw();
        }

        public bool isSelectedFrameAdded()
        {
            return selectedSprite.frames.Contains(selectedFrame);
        }

        public void addPendingFrame(int index = -1)
        {
            selectedFrame = selectedFrame.DeepClone();
            if (index == -1)
            {
                selectedSprite.frames.Add(selectedFrame);
            }
            else
            {
                selectedSprite.frames[index] = selectedFrame;
            }
            spriteCanvasUI.redraw();
            spritesheetCanvasUI.redraw();
            resetUI();
        }

        public void selectFrame(Frame frame)
        {
            selectedFrame = frame;
            if (frame.parentFrameIndex == -1)
            {
                lastSelectedFrameIndex = selectedSprite.frames.IndexOf(frame);
            }
            else
            {
                lastSelectedFrameIndex = frame.parentFrameIndex;
            }
            spriteCanvasUI.redraw();
            spritesheetCanvasUI.redraw();
        }

        public void copyFrame(Frame frame, int dir)
        {
            var index = selectedSprite.frames.IndexOf(frame);
            if (dir == -1) dir = 0;
            selectedSprite.frames.Insert(index + dir, frame.DeepClone());
        }

        public void moveFrame(Frame frame, int dir)
        {
            var index = selectedSprite.frames.IndexOf(frame);
            if (index + dir < 0 || index + dir >= selectedSprite.frames.Count) return;
            var temp = selectedSprite.frames[index];
            selectedSprite.frames[index] = selectedSprite.frames[index + dir];
            selectedSprite.frames[index + dir] = temp;
        }

        public void deleteFrame(Frame frame)
        {
            if (frame.parentFrameIndex == -1)
            {
                selectedSprite.frames.Remove(frame);
                selectedFrame = selectedSprite.frames[0];
                lastSelectedFrameIndex = 0;
            }
            else
            {
                var parentFrame = selectedSprite.frames[frame.parentFrameIndex];
                if (parentFrame != null) parentFrame = selectedSprite.frames[0];
                parentFrame.childFrames.Remove(frame);
            }
            if (selection == frame) selection = null;
            resetUI();
            redraw();
        }

        public void selectNextFrame()
        {
            var frameIndex = selectedSprite.frames.IndexOf(selectedFrame);
            if (frameIndex + 1 >= selectedSprite.frames.Count) return;
            var selFrame = selectedSprite.frames[frameIndex + 1];
            selection = null;
            this.selectFrame(selFrame);
        }

        public void selectPrevFrame()
        {
            var frameIndex = selectedSprite.frames.IndexOf(selectedFrame);
            if (frameIndex - 1 < 0) return;
            var selFrame = selectedSprite.frames[frameIndex - 1];
            selection = null;
            this.selectFrame(selFrame);
        }

        public void playAnim()
        {
            isAnimPlaying = !isAnimPlaying;
            if (!isAnimPlaying)
            {
                animFrameIndex = 0;
            }
        }

        public void saveSprite()
        {
            Helpers.saveSprite(selectedSprite);
            origSprites[selectedSprite.name] = selectedSprite.DeepClone();
        }

        public void saveSprites()
        {
            foreach (var sprite in sprites)
            {
                if (isSpriteChanged(sprite))
                {
                    Helpers.saveSprite(sprite);
                    origSprites[sprite.name] = sprite.DeepClone();
                }
            }
        }

        public void onSpriteAlignmentChange()
        {
            spriteCanvasUI.redraw();
        }

        public void redraw()
        {
            spriteCanvasUI.redraw();
            spritesheetCanvasUI.redraw();
        }

        public void onBulkDurationChange()
        {
            foreach (var frame in selectedSprite.frames)
            {
                frame.duration = bulkDuration;
            }
            resetUI();
        }

        public void onLoopStartChange()
        {
        }

        public void onWrapModeChange()
        {
            spriteCanvasUI.redraw();
        }

        public void reverseFrames()
        {
            selectedSprite.frames.Reverse();
            spriteCanvasUI.redraw();
            resetUI();
        }

        public void addPOI(Frame frame, float x, float y)
        {
            var poi = new POI("", x, y);
            frame.POIs.Add(poi);
            this.selectPOI(poi);
            spriteCanvasUI.redraw();
        }

        public void selectPOI(POI poi)
        {
            this.selection = poi;
            spriteCanvasUI.redraw();
        }

        public void deletePOI(POI poi)
        {
            selectedFrame.POIs.Remove(poi);
            spriteCanvasUI.redraw();
        }

        public List<Selectable> getSelectables()
        {
            List<Selectable> selectables = new List<Selectable>();
            foreach (var hitbox in selectedSprite.hitboxes)
            {
                selectables.Add(hitbox);
            }
            foreach (var hitbox in selectedFrame.hitboxes)
            {
                selectables.Add(hitbox);
            }
            foreach (var poi in selectedFrame.POIs)
            {
                selectables.Add(poi);
            }
            return selectables;
        }
        public bool isSpriteChanged(Sprite sprite)
        {
            return sprite.IsBinaryEqualTo(origSprites[sprite.name]);
        }

        public void getSelectedPixels()
        {
            if (selectedSpritesheet == null) return;
            var rect = Helpers.getSelectedPixelRect(spritesheetCanvasUI.dragLeftX, spritesheetCanvasUI.dragTopY, spritesheetCanvasUI.dragRightX, spritesheetCanvasUI.dragBotY, selectedSpritesheet.imgArr);
            if (rect != null)
            {
                selectedFrame = new Frame(rect, 0.066f, new Models.Point(0, 0));
                spriteCanvasUI.redraw();
                spritesheetCanvasUI.redraw();
            }
        }

        public void mainLoop()
        {
            if (isAnimPlaying)
            {
                animTime += 1000 / 60f;
                var frames = selectedSprite.frames;
                if (animTime >= frames[animFrameIndex].duration * 1000)
                {
                    animFrameIndex++;
                    if (animFrameIndex >= frames.Count)
                    {
                        animFrameIndex = 0;
                    }
                    animTime = 0;
                }
                spriteCanvasUI.redraw();
            }
        }

        public List<Hitbox> getVisibleHitboxes()
        {
            List<Hitbox> hitboxes = new List<Hitbox>();
            if (selectedSprite != null)
            {
                hitboxes.AddRange(selectedSprite.hitboxes);
            }
            if (selectedFrame != null)
            {
                hitboxes.AddRange(selectedFrame.hitboxes);
            }
            return hitboxes;
        }

        private void newSpriteButtonClicked(object sender, RoutedEventArgs e)
        {
            addSprite();
        }

        private void spriteListBoxChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            changeSprite(selectedSprite);
        }

        private void playClicked(object sender, RoutedEventArgs e)
        {
            playAnim();
        }

        private void saveClicked(object sender, RoutedEventArgs e)
        {
            saveSprite();
        }

        private void saveAllClicked(object sender, RoutedEventArgs e)
        {
            saveSprites();
        }

        private void bulkDurationApplyClicked(object sender, RoutedEventArgs e)
        {
            onBulkDurationChange();
        }

        private void addAsFrameClicked(object sender, RoutedEventArgs e)
        {
            addPendingFrame();
        }

        private void reverseFramesClicked(object sender, RoutedEventArgs e)
        {
            reverseFrames();
        }

        private void globalHitboxSelected(object sender, RoutedEventArgs e)
        {
            var hitbox = ((Button)sender).DataContext as Hitbox;
            selection = hitbox;
        }

        private void globalHitboxDeleted(object sender, RoutedEventArgs e)
        {
            var hitbox = ((Button)sender).DataContext as Hitbox;
            selectedSprite.hitboxes.Remove(hitbox);
            if (selection == hitbox) selection = null;
            redraw();
            resetUI();
        }

        private void frameHitboxSelected(object sender, RoutedEventArgs e)
        {
            var hitbox = ((Button)sender).DataContext as Hitbox;
            selection = hitbox;
        }

        private void frameHitboxDeleted(object sender, RoutedEventArgs e)
        {
            var hitbox = ((Button)sender).DataContext as Hitbox;
            selectedFrame.hitboxes.Remove(hitbox);
            if (selection == hitbox) selection = null;
            redraw();
            resetUI();
        }

        private void framePOISelected(object sender, RoutedEventArgs e)
        {
            var poi = ((Button)sender).DataContext as POI;
            selection = poi;
        }

        private void framePOIDeleted(object sender, RoutedEventArgs e)
        {
            var poi = ((Button)sender).DataContext as POI;
            selectedFrame.POIs.Remove(poi);
            if (selection == poi) selection = null;
            redraw();
            resetUI();
        }

        private void addGlobalHitboxClicked(object sender, RoutedEventArgs e)
        {
            addHitboxToSprite(selectedSprite);
        }

        private void addFrameHitboxClicked(object sender, RoutedEventArgs e)
        {
            addHitboxToFrame(selectedFrame);
        }

        private void addFramePOIClicked(object sender, RoutedEventArgs e)
        {
            addPOIMode = !addPOIMode;
        }

        private void frameCopyUpClicked(object sender, RoutedEventArgs e)
        {
            var frame = ((Button)sender).DataContext as Frame;
        }

        private void frameCopyDownClicked(object sender, RoutedEventArgs e)
        {
            var frame = ((Button)sender).DataContext as Frame;
        }

        private void frameMoveUpClicked(object sender, RoutedEventArgs e)
        {
            var frame = ((Button)sender).DataContext as Frame;
        }

        private void frameMoveDownClicked(object sender, RoutedEventArgs e)
        {
            var frame = ((Button)sender).DataContext as Frame;
        }

        private void frameSelectClicked(object sender, RoutedEventArgs e)
        {
            var frame = ((Button)sender).DataContext as Frame;
            selectedFrame = frame;
            selection = frame;
            redraw();
        }

        private void frameRowClicked(object sender, MouseButtonEventArgs e)
        {
            var frame = ((StackPanel)sender).DataContext as Frame;
            selectedFrame = frame;
            selection = frame;
            redraw();
        }

        private void frameDeleteClicked(object sender, RoutedEventArgs e)
        {
            var frame = ((Button)sender).DataContext as Frame;
            deleteFrame(frame);
        }
    }

}

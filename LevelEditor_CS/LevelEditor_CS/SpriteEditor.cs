using LevelEditor_CS.Controls;
using LevelEditor_CS.Editor;
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
    public partial class SpriteEditor : Form
    {
        public Dictionary<string, Sprite> origSprites = new Dictionary<string, Sprite>();
        public List<Sprite> sprites = new List<Sprite>();
        public List<Spritesheet> spritesheets;

        public bool isAnimPlaying = false;
        public bool addPOIMode = false;
        public List<string> alignments = new List<string> { "topleft", "topmid", "topright", "midleft", "center", "midright", "botleft", "botmid", "botright" };
        public List<string> wrapModes = new List<string> { "loop", "once", "pingpong", "pingpongonce" };
        public float offsetX = 0;
        public float offsetY = 0;
        public bool hideGizmos = false;
        public bool flipX = false;
        public bool flipY = false;
        public float bulkDuration = 0;
        public string newSpriteName = "";
        public Ghost ghost;
        public int lastSelectedFrameIndex = 0;
        public bool tileMode = false;
        public bool tileModeOffsetX = false;
        public bool tileModeOffsetY = false;
        public string spriteFilter = "";
        public string selectedFilterMode = "contains";
        public float tileWidth = 8;
        public int animFrameIndex = 0;
        public float animTime = 0;

        public SpriteCanvasUI spriteCanvasUI;
        public SpritesheetCanvasUI spritesheetCanvasUI;

        public Sprite selectedSprite
        {
            get
            {
                if (spriteListBox.SelectedIndex == -1) return null;
                return sprites[spriteListBox.SelectedIndex];
            }
            set
            {
                spriteListBox.SelectedIndex = sprites.IndexOf(value);
            }
        }

        public Spritesheet selectedSpritesheet
        {
            get
            {
                if (spritesheets.Count == 0 || spritesheetSelect.SelectedIndex < 0) return null;
                return spritesheets[spritesheetSelect.SelectedIndex];
            }
        }

        public Selectable selection;
        public Frame selectedFrame;

        public SpriteEditor()
        {
            InitializeComponent();

            spriteCanvasUI = new SpriteCanvasUI(spriteCanvas, spriteCanvasPanel, this);
            spritesheetCanvasUI = new SpritesheetCanvasUI(spritesheetCanvas, spritesheetCanvasPanel, this);

            spritesheets = Helpers.getSpritesheets();
            sprites = Helpers.getSprites();

            foreach (var sprite in sprites)
            {
                spriteListBox.Items.Add(sprite.name ?? "");
                sprite.spritesheets = spritesheets;
            }

            setSelect(spritesheetSelect, spritesheets.Select(s => s.getName()).ToList(), false);
            setSelect(alignmentSelect, alignments);
            setSelect(wrapModeSelect, wrapModes);

            spritesheetCanvasPanel.Scroll += spritesheetCanvasPanel_Scroll;

        }

        private void spritesheetCanvasPanel_Scroll(Object sender, ScrollEventArgs e)
        {
            spritesheetCanvasUI.redraw();
        }

        public void setSelect(ComboBox select, List<string> items, bool setIndex = true)
        {
            foreach (var item in items)
            {
                select.Items.Add(item);
            }
            if (setIndex)
            {
                select.SelectedIndex = 0;
            }
        }

        private void SpriteEditor_Load(object sender, EventArgs e)
        {

        }

        private void spritesheetSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            onSpritesheetChange(selectedSpritesheet);
        }

        private void spriteListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            spritesheetSelect.SelectedIndex = spritesheetSelect.Items.IndexOf(selectedSprite.spritesheet.getName());
            
            alignmentSelect.SelectedItem = selectedSprite.alignment;
            wrapModeSelect.SelectedItem = selectedSprite.wrapMode;
            selectedFrame = selectedSprite.frames.FirstOrDefault();

            globalHitboxGroup.Controls.Clear();
            framePanel.Controls.Clear();

            int yPos = 20;
            foreach (Hitbox hitbox in selectedSprite.hitboxes)
            {
                var hitboxControl = new HitboxControl(hitbox);
                hitboxControl.Location = new System.Drawing.Point(0, yPos);
                yPos += 20;
                globalHitboxGroup.Controls.Add(hitboxControl);
            }

            yPos = 20;
            foreach (Frame frame in selectedSprite.frames)
            {
                var frameControl = new FrameControl(frame);
                frameControl.Location = new System.Drawing.Point(0, yPos);
                yPos += 40;
                framePanel.Controls.Add(frameControl);
            }

            spriteCanvasUI.redraw();
            spritesheetCanvasUI.redraw();
        }

        public List<Sprite> getFilteredSprites()
        {
            var filters = spriteFilter.Split(',').ToList();
            if (filters[0] == "") return sprites;
            return sprites.Where((sprite) => {
                if (selectedFilterMode == "exactmatch")
                {
                    return filters.IndexOf(sprite.name) >= 0;
                }
                else if (selectedFilterMode == "contains")
                {
                    foreach(var filter in filters)
                    {
                        if (sprite.name.ToLower().Contains(filter.ToLower()))
                        {
                            return true;
                        }
                    }
                }
                else if (selectedFilterMode == "startswith")
                {
                    foreach(var filter in filters)
                    {
                        if (sprite.name.StartsWith(filter))
                        {
                            return true;
                        }
                    }
                }
                else if (selectedFilterMode == "endswith")
                {
                    foreach(var filter in filters)
                    {
                        if (sprite.name.EndsWith(filter))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }).ToList();
        }
        
        public void onSpritesheetChange(Spritesheet newSheet)
        {
            newSheet.init();

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
            var newSprite = new Sprite(spritename, null);
            this.changeSprite(newSprite);
            sprites.Add(newSprite);
            selectedFrame = null;
            selection = null;
        }
        
        public void changeSprite(Sprite newSprite)
        {
            selectedSprite = newSprite;
            if(newSprite.spritesheet != selectedSpritesheet)
            {
                this.onSpritesheetChange(newSprite.spritesheet);
            }
            selection = null;
            selectedFrame = selectedSprite.frames[0];
            lastSelectedFrameIndex = 0;
            spriteCanvasUI.redraw();
            spritesheetCanvasUI.redraw();
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
            spriteCanvasUI.redraw();
            spritesheetCanvasUI.redraw();
        }

        public void selectNextFrame()
        {
            selection = null;
            var frameIndex = selectedSprite.frames.IndexOf(selectedFrame);
            var selFrame = selectedSprite.frames[frameIndex + 1];
            if (selFrame != null) selFrame = selectedSprite.frames[0];
            this.selectFrame(selFrame);
        }

        public void selectPrevFrame()
        {
            selection = null;
            var frameIndex = selectedSprite.frames.IndexOf(selectedFrame);
            var selFrame = selectedSprite.frames[frameIndex - 1];
            if (selFrame != null) selFrame = selectedSprite.frames[selectedSprite.frames.Count - 1];
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
            foreach(var hitbox in selectedSprite.hitboxes) {
              selectables.Add(hitbox);
            }
            foreach(var hitbox in selectedFrame.hitboxes) {
              selectables.Add(hitbox);
            }
            foreach(var poi in selectedFrame.POIs) {
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

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

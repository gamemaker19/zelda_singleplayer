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
        public Sprite selectedSprite;
        public List<Spritesheet> spritesheets;
        public Spritesheet selectedSpritesheet;
        public Selectable selection;
        public Frame selectedFrame;
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
        public float lastSelectedFrameIndex = 0;
        public bool tileMode = false;
        public bool tileModeOffsetX = false;
        public bool tileModeOffsetY = false;
        public string spriteFilter = "";
        public string selectedFilterMode = "contains";
        public float tileWidth = 8;

        public SpriteCanvasUI spriteCanvasUI;
        public SpritesheetCanvasUI spritesheetCanvasUI;

        public SpriteEditor()
        {
            InitializeComponent();
            spritesheets = Helpers.getSpritesheets();
            sprites = Helpers.getSprites();

            foreach (var sprite in sprites)
            {
                spriteListBox.Items.Add(sprite.name ?? "");
            }

            setSelect(spritesheetSelect, spritesheets.Select(s => s.getName()).ToList());
            setSelect(alignmentSelect, alignments);
            setSelect(wrapModeSelect, wrapModes);

            spriteCanvasUI = new SpriteCanvasUI(spriteCanvas, spriteCanvasPanel, this);
            spritesheetCanvasUI = new SpritesheetCanvasUI(spritesheetCanvas, spritesheetCanvasPanel, this);
        }

        public void setSelect(ComboBox select, List<string> items)
        {
            foreach (var item in items)
            {
                select.Items.Add(item);
            }
            select.SelectedIndex = 0;
        }

        private void SpriteEditor_Load(object sender, EventArgs e)
        {

        }

        private void spritesheetSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void spriteListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public List<Sprite> getFilteredSprites()
        {
            var filters = spriteFilter.Split(',');
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
            });
        }
        
        public void onSpritesheetChange(Spritesheet newSheet, bool isNew)
        {
            var newSpriteAndSheetSel = isNew && (selectedSpritesheet != null);

            if (newSpriteAndSheetSel)
            {
                selectedSprite.spritesheet = selectedSpritesheet;
                return;
            }

            /*
            if(newSheet == selectedSpritesheet) {
              return;
            }
            */

            selectedSpritesheet = newSheet;

            if (selectedSpritesheet == null)
            {
                spriteCanvasUI.redraw();
                spritesheetCanvasUI.redraw();
                return;
            }

            if (selectedSprite != null)
            {
                selectedSprite.spritesheet = newSheet;
            }

            if (newSheet.image == null)
            {
                return;
            }

            spritesheetCanvasUI.setSize(selectedSpritesheet.image.Width, selectedSpritesheet.image.Height);
            Helpers.drawImage(spritesheetCanvasUI.canvas, selectedSpritesheet.image, 0, 0);
            var imageData = spritesheetCanvasUI.ctx.getImageData(0, 0, spritesheetCanvasUI.canvas.width, spritesheetCanvasUI.canvas.height);
            newSheet.imgArr = Helpers.get2DArrayFromImage(imageData);
            newSheet.image == null = spritesheetImg;
            spriteCanvasUI.redraw();
            spritesheetCanvasUI.redraw();

        }

        public string getSpriteDisplayName(Sprite sprite)
        {
            return sprite.name + (app1.isSpriteChanged(sprite) ? '*' : '');
        }
        
        public void addSprite()
        {
            var spritename = prompt("Enter a sprite name");
            var newSprite = new Sprite(spritename, null);
            this.changeSprite(newSprite, true);
            sprites.Add(newSprite);
            selectedFrame = null;
            selection = null;
        }
        
        public void changeSprite(Sprite newSprite, bool isNew)
        {
            selectedSprite = newSprite;
            this.onSpritesheetChange(newSprite.spritesheet, isNew);
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
            spriteCanvasUI.wrapper.focus();
            spriteCanvasUI.redraw();
        }
        
        public void devareHitbox(List<Hitbox> hitboxArr, Hitbox hitbox)
        {
            _.pull(hitboxArr, hitbox);
            spriteCanvasUI.redraw();
        }
        
        public List<Frame> isSelectedFrameAdded()
        {
            return _.includes(selectedSprite.frames, selectedFrame);
        }

        public void addPendingFrame(int index = -1)
        {
            selectedFrame = _.cloneDeep(selectedFrame);
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
        
        public void copyFrame(Frame frame, float dir)
        {
            var index = selectedSprite.frames.IndexOf(frame);
            if (dir == -1) dir = 0;
            selectedSprite.frames.splice(index + dir, 0, _.cloneDeep(frame));
        }

        public void moveFrame(Frame frame, float dir)
        {
            var index = selectedSprite.frames.IndexOf(frame);
            if (index + dir < 0 || index + dir >= selectedSprite.frames.length) return;
            var temp = selectedSprite.frames[index];
            selectedSprite.frames[index] = selectedSprite.frames[index + dir];
            selectedSprite.frames[index + dir] = temp;
        }
        
        public void devareFrame(Frame frame)
        {
            if (frame.parentFrameIndex == -1)
            {
                _.pull(selectedSprite.frames, frame);
                selectedFrame = selectedSprite.frames[0];
                lastSelectedFrameIndex = 0;
            }
            else
            {
                var parentFrame = selectedSprite.frames[frame.parentFrameIndex];
                if (parentFrame != null) parentFrame = selectedSprite.frames[0];
                _.pull(parentFrame.childFrames, frame);
            }
            spriteCanvasUI.redraw();
            spritesheetCanvasUI.redraw();
        }

        public void selectNextFrame()
        {
            selection = null;
            var frameIndex = selectedSprite.frames.IndexOf(selectedFrame);
            var selectedFrame = selectedSprite.frames[frameIndex + 1];
            if (selectedFrame != null) selectedFrame = selectedSprite.frames[0] || null;
            this.selectFrame(selectedFrame);
        }

        public void selectPrevFrame()
        {
            selection = null;
            var frameIndex = selectedSprite.frames.IndexOf(selectedFrame);
            var selectedFrame = selectedSprite.frames[frameIndex - 1];
            if (selectedFrame != null) selectedFrame = selectedSprite.frames[selectedSprite.frames.length - 1] || null;
            this.selectFrame(selectedFrame);
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
            var jsonStr = Helpers.serializeES6(selectedSprite);
            $.post("save-sprite", JSON.parse(jsonStr)).then(response => {
                Console.WriteLine("Successfully saved sprite");
                origSprites[selectedSprite.name] = _.clone(selectedSprite);
            }, error => {
                Console.WriteLine("Failed to save sprite");
            });
        }
  
        public void saveSprites()
        {
            var jsonStr = "[";
            Console.WriteLine("Saving sprites:");
            foreach (var sprite in sprites)
            {
                if (isSpriteChanged(sprite))
                {
                    jsonStr += Helpers.serializeES6(sprite);
                    jsonStr += ",";
                    Console.WriteLine(sprite.name);
                }
            }
            if (jsonStr[jsonStr.length - 1] == ",") jsonStr = jsonStr.slice(0, -1);
            jsonStr += "]";
            //Console.WriteLine(jsonStr);
            $.post("save-sprites", { "data": JSON.parse(jsonStr) }).then(response => {
                Console.WriteLine("Successfully saved sprites");
                for (var sprite of sprites)
                {
                    if (app1.isSpriteChanged(sprite))
                    {
                        origSprites[sprite.name] = _.clone(sprite);
                    }
                }
            }, error => {
                Console.WriteLine("Failed to save sprites");
            });
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
            _.reverse(selectedSprite.frames);
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
            spriteCanvasUI.wrapper.focus();
            spriteCanvasUI.redraw();
        }

        public void devarePOI(POI poi)
        {
            _.pull(selectedFrame.POIs, poi);
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
        public void isSpriteChanged(Sprite sprite)
        {
            return !_.isEqual(sprite, origSprites[sprite.name]);
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
                animTime += 1000 / 60;
                var frames = selectedSprite.frames;
                if (animTime >= frames[animFrameIndex].duration * 1000)
                {
                    animFrameIndex++;
                    if (animFrameIndex >= frames.length)
                    {
                        animFrameIndex = 0;
                    }
                    animTime = 0;
                }
                spriteCanvasUI.redraw();
            }
        }

        public void getVisibleHitboxes()
        {
            List<Hitbox> hitboxes = new List<Hitbox>();
            if (selectedSprite != null)
            {
                hitboxes = hitboxes.AddRange(selectedSprite.hitboxes);
            }
            if (selectedFrame != null)
            {
                hitboxes = hitboxes.AddRange(selectedFrame.hitboxes);
            }
            return hitboxes;
        }
    }
}

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

        public SpriteEditor()
        {
            InitializeComponent();
            spritesheets = Helpers.GetSpritesheets();
            sprites = Helpers.GetSprites();

            foreach (var sprite in sprites)
            {
                spriteListBox.Items.Add(sprite.name ?? "");
            }

            setSelect(spritesheetSelect, spritesheets.Select(s => s.getName()).ToList());
            setSelect(alignmentSelect, alignments);
            setSelect(wrapModeSelect, wrapModes);
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
    }
}

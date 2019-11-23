namespace LevelEditor_CS
{
    partial class SpriteEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.spriteListBox = new System.Windows.Forms.ListBox();
            this.Sprites = new System.Windows.Forms.Label();
            this.newSpriteBtn = new System.Windows.Forms.Button();
            this.spriteCanvas = new System.Windows.Forms.PictureBox();
            this.spritesheetCanvas = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.hideGizmosCheckbox = new System.Windows.Forms.CheckBox();
            this.flipHorizontalCheckbox = new System.Windows.Forms.CheckBox();
            this.flipVerticalCheckbox = new System.Windows.Forms.CheckBox();
            this.spritesheetSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.alignmentSelect = new System.Windows.Forms.ComboBox();
            this.wrapModeSelect = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.globalHitboxGroup = new System.Windows.Forms.GroupBox();
            this.frameHitboxGroup = new System.Windows.Forms.GroupBox();
            this.framePOIGroup = new System.Windows.Forms.GroupBox();
            this.tileModeCheckBox = new System.Windows.Forms.CheckBox();
            this.offsetXCheckBox = new System.Windows.Forms.CheckBox();
            this.offsetYCheckBox = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.spriteCanvasPanel = new System.Windows.Forms.Panel();
            this.spritesheetCanvasPanel = new System.Windows.Forms.Panel();
            this.framePanel = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.spriteCanvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spritesheetCanvas)).BeginInit();
            this.spriteCanvasPanel.SuspendLayout();
            this.spritesheetCanvasPanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spriteListBox
            // 
            this.spriteListBox.FormattingEnabled = true;
            this.spriteListBox.ItemHeight = 20;
            this.spriteListBox.Location = new System.Drawing.Point(18, 94);
            this.spriteListBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.spriteListBox.Name = "spriteListBox";
            this.spriteListBox.Size = new System.Drawing.Size(216, 804);
            this.spriteListBox.TabIndex = 0;
            this.spriteListBox.SelectedIndexChanged += new System.EventHandler(this.spriteListBox_SelectedIndexChanged);
            // 
            // Sprites
            // 
            this.Sprites.AutoSize = true;
            this.Sprites.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sprites.Location = new System.Drawing.Point(18, 14);
            this.Sprites.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Sprites.Name = "Sprites";
            this.Sprites.Size = new System.Drawing.Size(96, 29);
            this.Sprites.TabIndex = 1;
            this.Sprites.Text = "Sprites";
            // 
            // newSpriteBtn
            // 
            this.newSpriteBtn.Location = new System.Drawing.Point(20, 49);
            this.newSpriteBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.newSpriteBtn.Name = "newSpriteBtn";
            this.newSpriteBtn.Size = new System.Drawing.Size(112, 35);
            this.newSpriteBtn.TabIndex = 2;
            this.newSpriteBtn.Text = "New sprite";
            this.newSpriteBtn.UseVisualStyleBackColor = true;
            // 
            // spriteCanvas
            // 
            this.spriteCanvas.Location = new System.Drawing.Point(0, 0);
            this.spriteCanvas.Margin = new System.Windows.Forms.Padding(0);
            this.spriteCanvas.Name = "spriteCanvas";
            this.spriteCanvas.Size = new System.Drawing.Size(615, 403);
            this.spriteCanvas.TabIndex = 3;
            this.spriteCanvas.TabStop = false;
            // 
            // spritesheetCanvas
            // 
            this.spritesheetCanvas.Location = new System.Drawing.Point(0, 0);
            this.spritesheetCanvas.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.spritesheetCanvas.Name = "spritesheetCanvas";
            this.spritesheetCanvas.Size = new System.Drawing.Size(695, 935);
            this.spritesheetCanvas.TabIndex = 4;
            this.spritesheetCanvas.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(254, 840);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 35);
            this.button1.TabIndex = 5;
            this.button1.Text = "Play";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(342, 840);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 35);
            this.button2.TabIndex = 6;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(426, 840);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(90, 35);
            this.button3.TabIndex = 7;
            this.button3.Text = "Save All";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // hideGizmosCheckbox
            // 
            this.hideGizmosCheckbox.AutoSize = true;
            this.hideGizmosCheckbox.Location = new System.Drawing.Point(526, 848);
            this.hideGizmosCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.hideGizmosCheckbox.Name = "hideGizmosCheckbox";
            this.hideGizmosCheckbox.Size = new System.Drawing.Size(122, 24);
            this.hideGizmosCheckbox.TabIndex = 8;
            this.hideGizmosCheckbox.Text = "Hide gizmos";
            this.hideGizmosCheckbox.UseVisualStyleBackColor = true;
            // 
            // flipHorizontalCheckbox
            // 
            this.flipHorizontalCheckbox.AutoSize = true;
            this.flipHorizontalCheckbox.Location = new System.Drawing.Point(648, 848);
            this.flipHorizontalCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flipHorizontalCheckbox.Name = "flipHorizontalCheckbox";
            this.flipHorizontalCheckbox.Size = new System.Drawing.Size(136, 24);
            this.flipHorizontalCheckbox.TabIndex = 9;
            this.flipHorizontalCheckbox.Text = "Flip Horizontal";
            this.flipHorizontalCheckbox.UseVisualStyleBackColor = true;
            // 
            // flipVerticalCheckbox
            // 
            this.flipVerticalCheckbox.AutoSize = true;
            this.flipVerticalCheckbox.Location = new System.Drawing.Point(782, 848);
            this.flipVerticalCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flipVerticalCheckbox.Name = "flipVerticalCheckbox";
            this.flipVerticalCheckbox.Size = new System.Drawing.Size(117, 24);
            this.flipVerticalCheckbox.TabIndex = 10;
            this.flipVerticalCheckbox.Text = "Flip Vertical";
            this.flipVerticalCheckbox.UseVisualStyleBackColor = true;
            // 
            // spritesheetSelect
            // 
            this.spritesheetSelect.FormattingEnabled = true;
            this.spritesheetSelect.Location = new System.Drawing.Point(351, 885);
            this.spritesheetSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.spritesheetSelect.Name = "spritesheetSelect";
            this.spritesheetSelect.Size = new System.Drawing.Size(180, 28);
            this.spritesheetSelect.TabIndex = 11;
            this.spritesheetSelect.SelectedIndexChanged += new System.EventHandler(this.spritesheetSelect_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(252, 889);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Spritesheet";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(252, 931);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "Alignment";
            // 
            // alignmentSelect
            // 
            this.alignmentSelect.FormattingEnabled = true;
            this.alignmentSelect.Location = new System.Drawing.Point(334, 926);
            this.alignmentSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.alignmentSelect.Name = "alignmentSelect";
            this.alignmentSelect.Size = new System.Drawing.Size(180, 28);
            this.alignmentSelect.TabIndex = 14;
            // 
            // wrapModeSelect
            // 
            this.wrapModeSelect.FormattingEnabled = true;
            this.wrapModeSelect.Location = new System.Drawing.Point(639, 926);
            this.wrapModeSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.wrapModeSelect.Name = "wrapModeSelect";
            this.wrapModeSelect.Size = new System.Drawing.Size(180, 28);
            this.wrapModeSelect.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(537, 932);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 20);
            this.label3.TabIndex = 15;
            this.label3.Text = "Wrap mode";
            // 
            // globalHitboxGroup
            // 
            this.globalHitboxGroup.Location = new System.Drawing.Point(256, 982);
            this.globalHitboxGroup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.globalHitboxGroup.Name = "globalHitboxGroup";
            this.globalHitboxGroup.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.globalHitboxGroup.Size = new System.Drawing.Size(934, 98);
            this.globalHitboxGroup.TabIndex = 17;
            this.globalHitboxGroup.TabStop = false;
            this.globalHitboxGroup.Text = "Global Hitboxes";
            // 
            // frameHitboxGroup
            // 
            this.frameHitboxGroup.Location = new System.Drawing.Point(256, 1092);
            this.frameHitboxGroup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.frameHitboxGroup.Name = "frameHitboxGroup";
            this.frameHitboxGroup.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.frameHitboxGroup.Size = new System.Drawing.Size(934, 98);
            this.frameHitboxGroup.TabIndex = 18;
            this.frameHitboxGroup.TabStop = false;
            this.frameHitboxGroup.Text = "Frame Hitboxes";
            // 
            // framePOIGroup
            // 
            this.framePOIGroup.Location = new System.Drawing.Point(256, 1206);
            this.framePOIGroup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.framePOIGroup.Name = "framePOIGroup";
            this.framePOIGroup.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.framePOIGroup.Size = new System.Drawing.Size(934, 98);
            this.framePOIGroup.TabIndex = 19;
            this.framePOIGroup.TabStop = false;
            this.framePOIGroup.Text = "Frame POIs";
            // 
            // tileModeCheckBox
            // 
            this.tileModeCheckBox.AutoSize = true;
            this.tileModeCheckBox.Location = new System.Drawing.Point(1472, 997);
            this.tileModeCheckBox.Name = "tileModeCheckBox";
            this.tileModeCheckBox.Size = new System.Drawing.Size(103, 24);
            this.tileModeCheckBox.TabIndex = 20;
            this.tileModeCheckBox.Text = "Tile mode";
            this.tileModeCheckBox.UseVisualStyleBackColor = true;
            // 
            // offsetXCheckBox
            // 
            this.offsetXCheckBox.AutoSize = true;
            this.offsetXCheckBox.Location = new System.Drawing.Point(1581, 998);
            this.offsetXCheckBox.Name = "offsetXCheckBox";
            this.offsetXCheckBox.Size = new System.Drawing.Size(94, 24);
            this.offsetXCheckBox.TabIndex = 21;
            this.offsetXCheckBox.Text = "Offset X";
            this.offsetXCheckBox.UseVisualStyleBackColor = true;
            // 
            // offsetYCheckBox
            // 
            this.offsetYCheckBox.AutoSize = true;
            this.offsetYCheckBox.Location = new System.Drawing.Point(1682, 998);
            this.offsetYCheckBox.Name = "offsetYCheckBox";
            this.offsetYCheckBox.Size = new System.Drawing.Size(94, 24);
            this.offsetYCheckBox.TabIndex = 22;
            this.offsetYCheckBox.Text = "Offset Y";
            this.offsetYCheckBox.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1607, 1141);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 26);
            this.textBox1.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1472, 1138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 20);
            this.label4.TabIndex = 25;
            this.label4.Text = "Set bulk duration";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1713, 1141);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(89, 33);
            this.button4.TabIndex = 26;
            this.button4.Text = "Apply";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1472, 1186);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 20);
            this.label5.TabIndex = 27;
            this.label5.Text = "Loop start frame";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1605, 1186);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 26);
            this.textBox2.TabIndex = 28;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1476, 1226);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(89, 26);
            this.button5.TabIndex = 29;
            this.button5.Text = "Add as frame";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(1576, 1226);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(149, 26);
            this.button6.TabIndex = 30;
            this.button6.Text = "Reverse frames";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // spriteCanvasPanel
            // 
            this.spriteCanvasPanel.Controls.Add(this.spriteCanvas);
            this.spriteCanvasPanel.Location = new System.Drawing.Point(254, 94);
            this.spriteCanvasPanel.Margin = new System.Windows.Forms.Padding(0);
            this.spriteCanvasPanel.Name = "spriteCanvasPanel";
            this.spriteCanvasPanel.Size = new System.Drawing.Size(1163, 738);
            this.spriteCanvasPanel.TabIndex = 31;
            // 
            // spritesheetCanvasPanel
            // 
            this.spritesheetCanvasPanel.AutoScroll = true;
            this.spritesheetCanvasPanel.BackColor = System.Drawing.SystemColors.Control;
            this.spritesheetCanvasPanel.Controls.Add(this.spritesheetCanvas);
            this.spritesheetCanvasPanel.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.spritesheetCanvasPanel.Location = new System.Drawing.Point(1472, 21);
            this.spritesheetCanvasPanel.Name = "spritesheetCanvasPanel";
            this.spritesheetCanvasPanel.Size = new System.Drawing.Size(718, 954);
            this.spritesheetCanvasPanel.TabIndex = 32;
            // 
            // framePanel
            // 
            this.framePanel.AutoScroll = true;
            this.framePanel.AutoSize = true;
            this.framePanel.Location = new System.Drawing.Point(3, 3);
            this.framePanel.MaximumSize = new System.Drawing.Size(718, 300);
            this.framePanel.Name = "framePanel";
            this.framePanel.Size = new System.Drawing.Size(0, 0);
            this.framePanel.TabIndex = 33;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 20);
            this.label6.TabIndex = 34;
            this.label6.Text = "Frames";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.framePanel);
            this.flowLayoutPanel1.Controls.Add(this.label6);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(1861, 1040);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(735, 166);
            this.flowLayoutPanel1.TabIndex = 35;
            this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // SpriteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(2856, 1515);
            this.Controls.Add(this.spritesheetCanvasPanel);
            this.Controls.Add(this.spriteCanvasPanel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.offsetYCheckBox);
            this.Controls.Add(this.offsetXCheckBox);
            this.Controls.Add(this.tileModeCheckBox);
            this.Controls.Add(this.framePOIGroup);
            this.Controls.Add(this.frameHitboxGroup);
            this.Controls.Add(this.globalHitboxGroup);
            this.Controls.Add(this.wrapModeSelect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.alignmentSelect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.spritesheetSelect);
            this.Controls.Add(this.flipVerticalCheckbox);
            this.Controls.Add(this.flipHorizontalCheckbox);
            this.Controls.Add(this.hideGizmosCheckbox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.newSpriteBtn);
            this.Controls.Add(this.Sprites);
            this.Controls.Add(this.spriteListBox);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SpriteEditor";
            this.Text = "SpriteEditor";
            this.Load += new System.EventHandler(this.SpriteEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.spriteCanvas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spritesheetCanvas)).EndInit();
            this.spriteCanvasPanel.ResumeLayout(false);
            this.spritesheetCanvasPanel.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox spriteListBox;
        private System.Windows.Forms.Label Sprites;
        private System.Windows.Forms.Button newSpriteBtn;
        private System.Windows.Forms.PictureBox spriteCanvas;
        private System.Windows.Forms.PictureBox spritesheetCanvas;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox hideGizmosCheckbox;
        private System.Windows.Forms.CheckBox flipHorizontalCheckbox;
        private System.Windows.Forms.CheckBox flipVerticalCheckbox;
        private System.Windows.Forms.ComboBox spritesheetSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox alignmentSelect;
        private System.Windows.Forms.ComboBox wrapModeSelect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox globalHitboxGroup;
        private System.Windows.Forms.GroupBox frameHitboxGroup;
        private System.Windows.Forms.GroupBox framePOIGroup;
        private System.Windows.Forms.CheckBox tileModeCheckBox;
        private System.Windows.Forms.CheckBox offsetXCheckBox;
        private System.Windows.Forms.CheckBox offsetYCheckBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Panel spriteCanvasPanel;
        private System.Windows.Forms.Panel spritesheetCanvasPanel;
        private System.Windows.Forms.Panel framePanel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}


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
            this.spriteListBox.Location = new System.Drawing.Point(12, 63);
            this.spriteListBox.Name = "spriteListBox";
            this.spriteListBox.Size = new System.Drawing.Size(145, 537);
            this.spriteListBox.TabIndex = 0;
            this.spriteListBox.SelectedIndexChanged += new System.EventHandler(this.spriteListBox_SelectedIndexChanged);
            // 
            // Sprites
            // 
            this.Sprites.AutoSize = true;
            this.Sprites.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sprites.Location = new System.Drawing.Point(12, 9);
            this.Sprites.Name = "Sprites";
            this.Sprites.Size = new System.Drawing.Size(66, 20);
            this.Sprites.TabIndex = 1;
            this.Sprites.Text = "Sprites";
            // 
            // newSpriteBtn
            // 
            this.newSpriteBtn.Location = new System.Drawing.Point(13, 33);
            this.newSpriteBtn.Name = "newSpriteBtn";
            this.newSpriteBtn.Size = new System.Drawing.Size(75, 23);
            this.newSpriteBtn.TabIndex = 2;
            this.newSpriteBtn.Text = "New sprite";
            this.newSpriteBtn.UseVisualStyleBackColor = true;
            // 
            // spriteCanvas
            // 
            this.spriteCanvas.Location = new System.Drawing.Point(0, 0);
            this.spriteCanvas.Margin = new System.Windows.Forms.Padding(0);
            this.spriteCanvas.Name = "spriteCanvas";
            this.spriteCanvas.Size = new System.Drawing.Size(410, 269);
            this.spriteCanvas.TabIndex = 3;
            this.spriteCanvas.TabStop = false;
            // 
            // spritesheetCanvas
            // 
            this.spritesheetCanvas.Location = new System.Drawing.Point(0, 0);
            this.spritesheetCanvas.Name = "spritesheetCanvas";
            this.spritesheetCanvas.Size = new System.Drawing.Size(463, 623);
            this.spritesheetCanvas.TabIndex = 4;
            this.spritesheetCanvas.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(169, 560);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(53, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Play";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(228, 560);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(284, 560);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(60, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "Save All";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // hideGizmosCheckbox
            // 
            this.hideGizmosCheckbox.AutoSize = true;
            this.hideGizmosCheckbox.Location = new System.Drawing.Point(351, 565);
            this.hideGizmosCheckbox.Name = "hideGizmosCheckbox";
            this.hideGizmosCheckbox.Size = new System.Drawing.Size(83, 17);
            this.hideGizmosCheckbox.TabIndex = 8;
            this.hideGizmosCheckbox.Text = "Hide gizmos";
            this.hideGizmosCheckbox.UseVisualStyleBackColor = true;
            // 
            // flipHorizontalCheckbox
            // 
            this.flipHorizontalCheckbox.AutoSize = true;
            this.flipHorizontalCheckbox.Location = new System.Drawing.Point(432, 565);
            this.flipHorizontalCheckbox.Name = "flipHorizontalCheckbox";
            this.flipHorizontalCheckbox.Size = new System.Drawing.Size(92, 17);
            this.flipHorizontalCheckbox.TabIndex = 9;
            this.flipHorizontalCheckbox.Text = "Flip Horizontal";
            this.flipHorizontalCheckbox.UseVisualStyleBackColor = true;
            // 
            // flipVerticalCheckbox
            // 
            this.flipVerticalCheckbox.AutoSize = true;
            this.flipVerticalCheckbox.Location = new System.Drawing.Point(521, 565);
            this.flipVerticalCheckbox.Name = "flipVerticalCheckbox";
            this.flipVerticalCheckbox.Size = new System.Drawing.Size(80, 17);
            this.flipVerticalCheckbox.TabIndex = 10;
            this.flipVerticalCheckbox.Text = "Flip Vertical";
            this.flipVerticalCheckbox.UseVisualStyleBackColor = true;
            // 
            // spritesheetSelect
            // 
            this.spritesheetSelect.FormattingEnabled = true;
            this.spritesheetSelect.Location = new System.Drawing.Point(234, 590);
            this.spritesheetSelect.Name = "spritesheetSelect";
            this.spritesheetSelect.Size = new System.Drawing.Size(121, 21);
            this.spritesheetSelect.TabIndex = 11;
            this.spritesheetSelect.SelectedIndexChanged += new System.EventHandler(this.spritesheetSelect_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(168, 593);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Spritesheet";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(168, 621);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Alignment";
            // 
            // alignmentSelect
            // 
            this.alignmentSelect.FormattingEnabled = true;
            this.alignmentSelect.Location = new System.Drawing.Point(223, 617);
            this.alignmentSelect.Name = "alignmentSelect";
            this.alignmentSelect.Size = new System.Drawing.Size(121, 21);
            this.alignmentSelect.TabIndex = 14;
            // 
            // wrapModeSelect
            // 
            this.wrapModeSelect.FormattingEnabled = true;
            this.wrapModeSelect.Location = new System.Drawing.Point(426, 617);
            this.wrapModeSelect.Name = "wrapModeSelect";
            this.wrapModeSelect.Size = new System.Drawing.Size(121, 21);
            this.wrapModeSelect.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(358, 621);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Wrap mode";
            // 
            // globalHitboxGroup
            // 
            this.globalHitboxGroup.Location = new System.Drawing.Point(171, 655);
            this.globalHitboxGroup.Name = "globalHitboxGroup";
            this.globalHitboxGroup.Size = new System.Drawing.Size(623, 65);
            this.globalHitboxGroup.TabIndex = 17;
            this.globalHitboxGroup.TabStop = false;
            this.globalHitboxGroup.Text = "Global Hitboxes";
            // 
            // frameHitboxGroup
            // 
            this.frameHitboxGroup.Location = new System.Drawing.Point(171, 728);
            this.frameHitboxGroup.Name = "frameHitboxGroup";
            this.frameHitboxGroup.Size = new System.Drawing.Size(623, 65);
            this.frameHitboxGroup.TabIndex = 18;
            this.frameHitboxGroup.TabStop = false;
            this.frameHitboxGroup.Text = "Frame Hitboxes";
            // 
            // framePOIGroup
            // 
            this.framePOIGroup.Location = new System.Drawing.Point(171, 804);
            this.framePOIGroup.Name = "framePOIGroup";
            this.framePOIGroup.Size = new System.Drawing.Size(623, 65);
            this.framePOIGroup.TabIndex = 19;
            this.framePOIGroup.TabStop = false;
            this.framePOIGroup.Text = "Frame POIs";
            // 
            // tileModeCheckBox
            // 
            this.tileModeCheckBox.AutoSize = true;
            this.tileModeCheckBox.Location = new System.Drawing.Point(981, 665);
            this.tileModeCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tileModeCheckBox.Name = "tileModeCheckBox";
            this.tileModeCheckBox.Size = new System.Drawing.Size(72, 17);
            this.tileModeCheckBox.TabIndex = 20;
            this.tileModeCheckBox.Text = "Tile mode";
            this.tileModeCheckBox.UseVisualStyleBackColor = true;
            // 
            // offsetXCheckBox
            // 
            this.offsetXCheckBox.AutoSize = true;
            this.offsetXCheckBox.Location = new System.Drawing.Point(1054, 665);
            this.offsetXCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.offsetXCheckBox.Name = "offsetXCheckBox";
            this.offsetXCheckBox.Size = new System.Drawing.Size(64, 17);
            this.offsetXCheckBox.TabIndex = 21;
            this.offsetXCheckBox.Text = "Offset X";
            this.offsetXCheckBox.UseVisualStyleBackColor = true;
            // 
            // offsetYCheckBox
            // 
            this.offsetYCheckBox.AutoSize = true;
            this.offsetYCheckBox.Location = new System.Drawing.Point(1121, 665);
            this.offsetYCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.offsetYCheckBox.Name = "offsetYCheckBox";
            this.offsetYCheckBox.Size = new System.Drawing.Size(64, 17);
            this.offsetYCheckBox.TabIndex = 22;
            this.offsetYCheckBox.Text = "Offset Y";
            this.offsetYCheckBox.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1071, 761);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(68, 20);
            this.textBox1.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(981, 759);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Set bulk duration";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1142, 761);
            this.button4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(59, 22);
            this.button4.TabIndex = 26;
            this.button4.Text = "Apply";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(981, 791);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Loop start frame";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1070, 791);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(68, 20);
            this.textBox2.TabIndex = 28;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(984, 817);
            this.button5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(59, 17);
            this.button5.TabIndex = 29;
            this.button5.Text = "Add as frame";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(1051, 817);
            this.button6.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(99, 17);
            this.button6.TabIndex = 30;
            this.button6.Text = "Reverse frames";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // spriteCanvasPanel
            // 
            this.spriteCanvasPanel.Controls.Add(this.spriteCanvas);
            this.spriteCanvasPanel.Location = new System.Drawing.Point(169, 63);
            this.spriteCanvasPanel.Margin = new System.Windows.Forms.Padding(0);
            this.spriteCanvasPanel.Name = "spriteCanvasPanel";
            this.spriteCanvasPanel.Size = new System.Drawing.Size(775, 492);
            this.spriteCanvasPanel.TabIndex = 31;
            // 
            // spritesheetCanvasPanel
            // 
            this.spritesheetCanvasPanel.AutoScroll = true;
            this.spritesheetCanvasPanel.BackColor = System.Drawing.SystemColors.Control;
            this.spritesheetCanvasPanel.Controls.Add(this.spritesheetCanvas);
            this.spritesheetCanvasPanel.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.spritesheetCanvasPanel.Location = new System.Drawing.Point(981, 14);
            this.spritesheetCanvasPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.spritesheetCanvasPanel.Name = "spritesheetCanvasPanel";
            this.spritesheetCanvasPanel.Size = new System.Drawing.Size(479, 636);
            this.spritesheetCanvasPanel.TabIndex = 32;
            // 
            // framePanel
            // 
            this.framePanel.AutoScroll = true;
            this.framePanel.AutoSize = true;
            this.framePanel.Location = new System.Drawing.Point(2, 2);
            this.framePanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.framePanel.MaximumSize = new System.Drawing.Size(479, 200);
            this.framePanel.Name = "framePanel";
            this.framePanel.Size = new System.Drawing.Size(0, 0);
            this.framePanel.TabIndex = 33;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "Frames";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.framePanel);
            this.flowLayoutPanel1.Controls.Add(this.label6);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(1241, 693);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(490, 111);
            this.flowLayoutPanel1.TabIndex = 35;
            this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // SpriteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1904, 877);
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


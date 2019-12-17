namespace LevelEditor_CS
{
    partial class LevelEditor
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
            this.levelListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.newLevelTextBox = new System.Windows.Forms.TextBox();
            this.newLevelBtn = new System.Windows.Forms.Button();
            this.objectsListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Instances = new System.Windows.Forms.Label();
            this.instanceListBox = new System.Windows.Forms.ListBox();
            this.levelPanel = new System.Windows.Forms.Panel();
            this.levelCanvas = new System.Windows.Forms.PictureBox();
            this.tilePanel = new System.Windows.Forms.Panel();
            this.tileCanvas = new System.Windows.Forms.PictureBox();
            this.saveBtn = new System.Windows.Forms.Button();
            this.undoBtn = new System.Windows.Forms.Button();
            this.redoBtn = new System.Windows.Forms.Button();
            this.instanceCheckBox = new System.Windows.Forms.CheckBox();
            this.gridCheckBox = new System.Windows.Forms.CheckBox();
            this.roomLinesCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.widthTextBox = new System.Windows.Forms.TextBox();
            this.heightTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.selectionLabel = new System.Windows.Forms.Label();
            this.tileIdLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.overridesTextBox = new System.Windows.Forms.TextBox();
            this.tileSelectionPropsTextBox = new System.Windows.Forms.TextBox();
            this.selectionPropertiesLabel = new System.Windows.Forms.Label();
            this.instancePropertiesLabel = new System.Windows.Forms.Label();
            this.tileInstancePropsTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tilesetSelect = new System.Windows.Forms.ComboBox();
            this.tileHitboxCheckBox = new System.Windows.Forms.CheckBox();
            this.tileZIndexCheckBox = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tileTagFilterTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tileSpriteFilterSelect = new System.Windows.Forms.ComboBox();
            this.tileNameTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tileHitboxModeSelect = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tileTagTextBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tileZIndexSelect = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tileSpriteSelect = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.idLabel = new System.Windows.Forms.Label();
            this.selectedToolPanel = new LevelEditor_CS.Controls.RadioGroupBox();
            this.placeInstanceRadio = new System.Windows.Forms.RadioButton();
            this.selectTileRadio = new System.Windows.Forms.RadioButton();
            this.selectInstanceRadio = new System.Windows.Forms.RadioButton();
            this.placeTileRadio = new System.Windows.Forms.RadioButton();
            this.rectangleSelectRadio = new System.Windows.Forms.RadioButton();
            this.selectedTilePanel = new System.Windows.Forms.Panel();
            this.levelPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.levelCanvas)).BeginInit();
            this.tilePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tileCanvas)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.selectedToolPanel.SuspendLayout();
            this.selectedTilePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // levelListBox
            // 
            this.levelListBox.FormattingEnabled = true;
            this.levelListBox.Location = new System.Drawing.Point(12, 91);
            this.levelListBox.Name = "levelListBox";
            this.levelListBox.Size = new System.Drawing.Size(153, 355);
            this.levelListBox.TabIndex = 0;
            this.levelListBox.SelectedIndexChanged += new System.EventHandler(this.levelListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Levels";
            // 
            // newLevelTextBox
            // 
            this.newLevelTextBox.Location = new System.Drawing.Point(12, 65);
            this.newLevelTextBox.Name = "newLevelTextBox";
            this.newLevelTextBox.Size = new System.Drawing.Size(100, 20);
            this.newLevelTextBox.TabIndex = 2;
            // 
            // newLevelBtn
            // 
            this.newLevelBtn.Location = new System.Drawing.Point(12, 38);
            this.newLevelBtn.Name = "newLevelBtn";
            this.newLevelBtn.Size = new System.Drawing.Size(75, 23);
            this.newLevelBtn.TabIndex = 3;
            this.newLevelBtn.Text = "New Level";
            this.newLevelBtn.UseVisualStyleBackColor = true;
            // 
            // objectsListBox
            // 
            this.objectsListBox.FormattingEnabled = true;
            this.objectsListBox.Location = new System.Drawing.Point(183, 38);
            this.objectsListBox.Name = "objectsListBox";
            this.objectsListBox.Size = new System.Drawing.Size(143, 290);
            this.objectsListBox.TabIndex = 4;
            this.objectsListBox.SelectedIndexChanged += new System.EventHandler(this.objectsListBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(179, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Objects";
            // 
            // Instances
            // 
            this.Instances.AutoSize = true;
            this.Instances.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Instances.Location = new System.Drawing.Point(179, 342);
            this.Instances.Name = "Instances";
            this.Instances.Size = new System.Drawing.Size(88, 20);
            this.Instances.TabIndex = 7;
            this.Instances.Text = "Instances";
            // 
            // instanceListBox
            // 
            this.instanceListBox.FormattingEnabled = true;
            this.instanceListBox.Location = new System.Drawing.Point(183, 365);
            this.instanceListBox.Name = "instanceListBox";
            this.instanceListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.instanceListBox.Size = new System.Drawing.Size(143, 290);
            this.instanceListBox.TabIndex = 6;
            this.instanceListBox.SelectedIndexChanged += new System.EventHandler(this.instanceListBox_SelectedIndexChanged);
            // 
            // levelPanel
            // 
            this.levelPanel.Controls.Add(this.levelCanvas);
            this.levelPanel.Location = new System.Drawing.Point(341, 41);
            this.levelPanel.Name = "levelPanel";
            this.levelPanel.Size = new System.Drawing.Size(740, 562);
            this.levelPanel.TabIndex = 8;
            // 
            // levelCanvas
            // 
            this.levelCanvas.Location = new System.Drawing.Point(3, 3);
            this.levelCanvas.Name = "levelCanvas";
            this.levelCanvas.Size = new System.Drawing.Size(358, 258);
            this.levelCanvas.TabIndex = 0;
            this.levelCanvas.TabStop = false;
            // 
            // tilePanel
            // 
            this.tilePanel.Controls.Add(this.tileCanvas);
            this.tilePanel.Location = new System.Drawing.Point(1098, 44);
            this.tilePanel.Name = "tilePanel";
            this.tilePanel.Size = new System.Drawing.Size(448, 559);
            this.tilePanel.TabIndex = 9;
            // 
            // tileCanvas
            // 
            this.tileCanvas.Location = new System.Drawing.Point(4, 4);
            this.tileCanvas.Name = "tileCanvas";
            this.tileCanvas.Size = new System.Drawing.Size(238, 240);
            this.tileCanvas.TabIndex = 0;
            this.tileCanvas.TabStop = false;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(341, 610);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 10;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // undoBtn
            // 
            this.undoBtn.Location = new System.Drawing.Point(422, 610);
            this.undoBtn.Name = "undoBtn";
            this.undoBtn.Size = new System.Drawing.Size(75, 23);
            this.undoBtn.TabIndex = 11;
            this.undoBtn.Text = "Undo";
            this.undoBtn.UseVisualStyleBackColor = true;
            this.undoBtn.Click += new System.EventHandler(this.undoBtn_Click);
            // 
            // redoBtn
            // 
            this.redoBtn.Location = new System.Drawing.Point(503, 609);
            this.redoBtn.Name = "redoBtn";
            this.redoBtn.Size = new System.Drawing.Size(75, 23);
            this.redoBtn.TabIndex = 12;
            this.redoBtn.Text = "Redo";
            this.redoBtn.UseVisualStyleBackColor = true;
            this.redoBtn.Click += new System.EventHandler(this.redoBtn_Click);
            // 
            // instanceCheckBox
            // 
            this.instanceCheckBox.AutoSize = true;
            this.instanceCheckBox.Location = new System.Drawing.Point(584, 613);
            this.instanceCheckBox.Name = "instanceCheckBox";
            this.instanceCheckBox.Size = new System.Drawing.Size(72, 17);
            this.instanceCheckBox.TabIndex = 13;
            this.instanceCheckBox.Text = "Instances";
            this.instanceCheckBox.UseVisualStyleBackColor = true;
            this.instanceCheckBox.CheckedChanged += new System.EventHandler(this.instanceCheckBox_CheckedChanged);
            // 
            // gridCheckBox
            // 
            this.gridCheckBox.AutoSize = true;
            this.gridCheckBox.Location = new System.Drawing.Point(653, 613);
            this.gridCheckBox.Name = "gridCheckBox";
            this.gridCheckBox.Size = new System.Drawing.Size(45, 17);
            this.gridCheckBox.TabIndex = 14;
            this.gridCheckBox.Text = "Grid";
            this.gridCheckBox.UseVisualStyleBackColor = true;
            // 
            // roomLinesCheckBox
            // 
            this.roomLinesCheckBox.AutoSize = true;
            this.roomLinesCheckBox.Location = new System.Drawing.Point(695, 613);
            this.roomLinesCheckBox.Name = "roomLinesCheckBox";
            this.roomLinesCheckBox.Size = new System.Drawing.Size(82, 17);
            this.roomLinesCheckBox.TabIndex = 15;
            this.roomLinesCheckBox.Text = "Room Lines";
            this.roomLinesCheckBox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(344, 641);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Width:";
            // 
            // widthTextBox
            // 
            this.widthTextBox.Location = new System.Drawing.Point(379, 638);
            this.widthTextBox.Name = "widthTextBox";
            this.widthTextBox.Size = new System.Drawing.Size(100, 20);
            this.widthTextBox.TabIndex = 17;
            // 
            // heightTextBox
            // 
            this.heightTextBox.Location = new System.Drawing.Point(523, 638);
            this.heightTextBox.Name = "heightTextBox";
            this.heightTextBox.Size = new System.Drawing.Size(100, 20);
            this.heightTextBox.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(485, 641);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Height:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.selectionLabel);
            this.flowLayoutPanel1.Controls.Add(this.tileIdLabel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(629, 638);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(452, 20);
            this.flowLayoutPanel1.TabIndex = 20;
            // 
            // selectionLabel
            // 
            this.selectionLabel.AutoSize = true;
            this.selectionLabel.Location = new System.Drawing.Point(3, 3);
            this.selectionLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.selectionLabel.Name = "selectionLabel";
            this.selectionLabel.Size = new System.Drawing.Size(51, 13);
            this.selectionLabel.TabIndex = 0;
            this.selectionLabel.Text = "Selection";
            // 
            // tileIdLabel
            // 
            this.tileIdLabel.AutoSize = true;
            this.tileIdLabel.Location = new System.Drawing.Point(60, 3);
            this.tileIdLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tileIdLabel.Name = "tileIdLabel";
            this.tileIdLabel.Size = new System.Drawing.Size(25, 13);
            this.tileIdLabel.TabIndex = 2;
            this.tileIdLabel.Text = "Tid:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(347, 668);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Show overrides with key:";
            // 
            // overridesTextBox
            // 
            this.overridesTextBox.Location = new System.Drawing.Point(478, 665);
            this.overridesTextBox.Name = "overridesTextBox";
            this.overridesTextBox.Size = new System.Drawing.Size(100, 20);
            this.overridesTextBox.TabIndex = 22;
            // 
            // tileSelectionPropsTextBox
            // 
            this.tileSelectionPropsTextBox.Location = new System.Drawing.Point(350, 767);
            this.tileSelectionPropsTextBox.Multiline = true;
            this.tileSelectionPropsTextBox.Name = "tileSelectionPropsTextBox";
            this.tileSelectionPropsTextBox.Size = new System.Drawing.Size(252, 89);
            this.tileSelectionPropsTextBox.TabIndex = 24;
            this.tileSelectionPropsTextBox.Visible = false;
            // 
            // selectionPropertiesLabel
            // 
            this.selectionPropertiesLabel.AutoSize = true;
            this.selectionPropertiesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.selectionPropertiesLabel.Location = new System.Drawing.Point(348, 744);
            this.selectionPropertiesLabel.Name = "selectionPropertiesLabel";
            this.selectionPropertiesLabel.Size = new System.Drawing.Size(155, 17);
            this.selectionPropertiesLabel.TabIndex = 25;
            this.selectionPropertiesLabel.Text = "Selection Properties";
            this.selectionPropertiesLabel.Visible = false;
            // 
            // instancePropertiesLabel
            // 
            this.instancePropertiesLabel.AutoSize = true;
            this.instancePropertiesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.instancePropertiesLabel.Location = new System.Drawing.Point(718, 744);
            this.instancePropertiesLabel.Name = "instancePropertiesLabel";
            this.instancePropertiesLabel.Size = new System.Drawing.Size(149, 17);
            this.instancePropertiesLabel.TabIndex = 27;
            this.instancePropertiesLabel.Text = "Instance Properties";
            this.instancePropertiesLabel.Visible = false;
            // 
            // tileInstancePropsTextBox
            // 
            this.tileInstancePropsTextBox.Location = new System.Drawing.Point(720, 767);
            this.tileInstancePropsTextBox.Multiline = true;
            this.tileInstancePropsTextBox.Name = "tileInstancePropsTextBox";
            this.tileInstancePropsTextBox.Size = new System.Drawing.Size(252, 89);
            this.tileInstancePropsTextBox.TabIndex = 26;
            this.tileInstancePropsTextBox.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1099, 614);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "Tileset:";
            // 
            // tilesetSelect
            // 
            this.tilesetSelect.FormattingEnabled = true;
            this.tilesetSelect.Location = new System.Drawing.Point(1146, 611);
            this.tilesetSelect.Name = "tilesetSelect";
            this.tilesetSelect.Size = new System.Drawing.Size(121, 21);
            this.tilesetSelect.TabIndex = 31;
            this.tilesetSelect.SelectedIndexChanged += new System.EventHandler(this.tilesetSelect_SelectedIndexChanged);
            // 
            // tileHitboxCheckBox
            // 
            this.tileHitboxCheckBox.AutoSize = true;
            this.tileHitboxCheckBox.Location = new System.Drawing.Point(1103, 640);
            this.tileHitboxCheckBox.Name = "tileHitboxCheckBox";
            this.tileHitboxCheckBox.Size = new System.Drawing.Size(111, 17);
            this.tileHitboxCheckBox.TabIndex = 32;
            this.tileHitboxCheckBox.Text = "Show tile hitboxes";
            this.tileHitboxCheckBox.UseVisualStyleBackColor = true;
            // 
            // tileZIndexCheckBox
            // 
            this.tileZIndexCheckBox.AutoSize = true;
            this.tileZIndexCheckBox.Location = new System.Drawing.Point(1239, 638);
            this.tileZIndexCheckBox.Name = "tileZIndexCheckBox";
            this.tileZIndexCheckBox.Size = new System.Drawing.Size(141, 17);
            this.tileZIndexCheckBox.TabIndex = 33;
            this.tileZIndexCheckBox.Text = "Show tiles with z index 1";
            this.tileZIndexCheckBox.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1102, 668);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(98, 13);
            this.label10.TabIndex = 34;
            this.label10.Text = "Show tiles with tag:";
            // 
            // tileTagFilterTextBox
            // 
            this.tileTagFilterTextBox.Location = new System.Drawing.Point(1204, 666);
            this.tileTagFilterTextBox.Name = "tileTagFilterTextBox";
            this.tileTagFilterTextBox.Size = new System.Drawing.Size(100, 20);
            this.tileTagFilterTextBox.TabIndex = 35;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1315, 669);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(108, 13);
            this.label11.TabIndex = 36;
            this.label11.Text = "Show tiles with sprite:";
            // 
            // tileSpriteFilterSelect
            // 
            this.tileSpriteFilterSelect.FormattingEnabled = true;
            this.tileSpriteFilterSelect.Location = new System.Drawing.Point(1425, 666);
            this.tileSpriteFilterSelect.Name = "tileSpriteFilterSelect";
            this.tileSpriteFilterSelect.Size = new System.Drawing.Size(121, 21);
            this.tileSpriteFilterSelect.TabIndex = 37;
            this.tileSpriteFilterSelect.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // tileNameTextBox
            // 
            this.tileNameTextBox.Location = new System.Drawing.Point(62, 4);
            this.tileNameTextBox.Name = "tileNameTextBox";
            this.tileNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.tileNameTextBox.TabIndex = 39;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(2, 4);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 13);
            this.label12.TabIndex = 38;
            this.label12.Text = "Tile name:";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(168, 7);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(19, 13);
            this.label13.TabIndex = 40;
            this.label13.Text = "Id:";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // tileHitboxModeSelect
            // 
            this.tileHitboxModeSelect.FormattingEnabled = true;
            this.tileHitboxModeSelect.Location = new System.Drawing.Point(73, 30);
            this.tileHitboxModeSelect.Name = "tileHitboxModeSelect";
            this.tileHitboxModeSelect.Size = new System.Drawing.Size(121, 21);
            this.tileHitboxModeSelect.TabIndex = 42;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 33);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(66, 13);
            this.label14.TabIndex = 41;
            this.label14.Text = "Hitbox mode";
            // 
            // tileTagTextBox
            // 
            this.tileTagTextBox.Location = new System.Drawing.Point(39, 57);
            this.tileTagTextBox.Name = "tileTagTextBox";
            this.tileTagTextBox.Size = new System.Drawing.Size(100, 20);
            this.tileTagTextBox.TabIndex = 44;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 59);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(26, 13);
            this.label15.TabIndex = 43;
            this.label15.Text = "Tag";
            // 
            // tileZIndexSelect
            // 
            this.tileZIndexSelect.FormattingEnabled = true;
            this.tileZIndexSelect.Location = new System.Drawing.Point(219, 57);
            this.tileZIndexSelect.Name = "tileZIndexSelect";
            this.tileZIndexSelect.Size = new System.Drawing.Size(121, 21);
            this.tileZIndexSelect.TabIndex = 46;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(171, 61);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(43, 13);
            this.label16.TabIndex = 45;
            this.label16.Text = "Z-Index";
            // 
            // tileSpriteSelect
            // 
            this.tileSpriteSelect.FormattingEnabled = true;
            this.tileSpriteSelect.Location = new System.Drawing.Point(39, 86);
            this.tileSpriteSelect.Name = "tileSpriteSelect";
            this.tileSpriteSelect.Size = new System.Drawing.Size(121, 21);
            this.tileSpriteSelect.TabIndex = 48;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 89);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(34, 13);
            this.label17.TabIndex = 47;
            this.label17.Text = "Sprite";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label18.Location = new System.Drawing.Point(1103, 692);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(433, 13);
            this.label18.TabIndex = 49;
            this.label18.Text = "_______________________________________________________________________";
            // 
            // idLabel
            // 
            this.idLabel.AutoSize = true;
            this.idLabel.Location = new System.Drawing.Point(194, 7);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(42, 13);
            this.idLabel.TabIndex = 50;
            this.idLabel.Text = "IdLabel";
            // 
            // selectedToolPanel
            // 
            this.selectedToolPanel.Controls.Add(this.placeInstanceRadio);
            this.selectedToolPanel.Controls.Add(this.selectTileRadio);
            this.selectedToolPanel.Controls.Add(this.selectInstanceRadio);
            this.selectedToolPanel.Controls.Add(this.placeTileRadio);
            this.selectedToolPanel.Controls.Add(this.rectangleSelectRadio);
            this.selectedToolPanel.Location = new System.Drawing.Point(347, 692);
            this.selectedToolPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.selectedToolPanel.Name = "selectedToolPanel";
            this.selectedToolPanel.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.selectedToolPanel.Selected = 2;
            this.selectedToolPanel.Size = new System.Drawing.Size(573, 45);
            this.selectedToolPanel.TabIndex = 51;
            this.selectedToolPanel.TabStop = false;
            this.selectedToolPanel.Text = "Tool";
            // 
            // placeInstanceRadio
            // 
            this.placeInstanceRadio.AutoSize = true;
            this.placeInstanceRadio.Location = new System.Drawing.Point(365, 18);
            this.placeInstanceRadio.Name = "placeInstanceRadio";
            this.placeInstanceRadio.Size = new System.Drawing.Size(96, 17);
            this.placeInstanceRadio.TabIndex = 27;
            this.placeInstanceRadio.Tag = "4";
            this.placeInstanceRadio.Text = "Place Instance";
            this.placeInstanceRadio.UseVisualStyleBackColor = true;
            // 
            // selectTileRadio
            // 
            this.selectTileRadio.AutoSize = true;
            this.selectTileRadio.Location = new System.Drawing.Point(5, 18);
            this.selectTileRadio.Name = "selectTileRadio";
            this.selectTileRadio.Size = new System.Drawing.Size(75, 17);
            this.selectTileRadio.TabIndex = 23;
            this.selectTileRadio.Tag = "0";
            this.selectTileRadio.Text = "Select Tile";
            this.selectTileRadio.UseVisualStyleBackColor = true;
            // 
            // selectInstanceRadio
            // 
            this.selectInstanceRadio.AutoSize = true;
            this.selectInstanceRadio.Location = new System.Drawing.Point(263, 18);
            this.selectInstanceRadio.Name = "selectInstanceRadio";
            this.selectInstanceRadio.Size = new System.Drawing.Size(99, 17);
            this.selectInstanceRadio.TabIndex = 26;
            this.selectInstanceRadio.Tag = "3";
            this.selectInstanceRadio.Text = "Select Instance";
            this.selectInstanceRadio.UseVisualStyleBackColor = true;
            // 
            // placeTileRadio
            // 
            this.placeTileRadio.AutoSize = true;
            this.placeTileRadio.Location = new System.Drawing.Point(81, 18);
            this.placeTileRadio.Name = "placeTileRadio";
            this.placeTileRadio.Size = new System.Drawing.Size(72, 17);
            this.placeTileRadio.TabIndex = 24;
            this.placeTileRadio.Tag = "1";
            this.placeTileRadio.Text = "Place Tile";
            this.placeTileRadio.UseVisualStyleBackColor = true;
            // 
            // rectangleSelectRadio
            // 
            this.rectangleSelectRadio.AutoSize = true;
            this.rectangleSelectRadio.Checked = true;
            this.rectangleSelectRadio.Location = new System.Drawing.Point(154, 18);
            this.rectangleSelectRadio.Name = "rectangleSelectRadio";
            this.rectangleSelectRadio.Size = new System.Drawing.Size(107, 17);
            this.rectangleSelectRadio.TabIndex = 25;
            this.rectangleSelectRadio.TabStop = true;
            this.rectangleSelectRadio.Tag = "2";
            this.rectangleSelectRadio.Text = "Rectangle Select";
            this.rectangleSelectRadio.UseVisualStyleBackColor = true;
            // 
            // selectedTilePanel
            // 
            this.selectedTilePanel.Controls.Add(this.idLabel);
            this.selectedTilePanel.Controls.Add(this.label13);
            this.selectedTilePanel.Controls.Add(this.tileSpriteSelect);
            this.selectedTilePanel.Controls.Add(this.tileNameTextBox);
            this.selectedTilePanel.Controls.Add(this.label17);
            this.selectedTilePanel.Controls.Add(this.tileTagTextBox);
            this.selectedTilePanel.Controls.Add(this.tileZIndexSelect);
            this.selectedTilePanel.Controls.Add(this.label12);
            this.selectedTilePanel.Controls.Add(this.label16);
            this.selectedTilePanel.Controls.Add(this.label14);
            this.selectedTilePanel.Controls.Add(this.tileHitboxModeSelect);
            this.selectedTilePanel.Controls.Add(this.label15);
            this.selectedTilePanel.Location = new System.Drawing.Point(1106, 710);
            this.selectedTilePanel.Name = "selectedTilePanel";
            this.selectedTilePanel.Size = new System.Drawing.Size(343, 118);
            this.selectedTilePanel.TabIndex = 52;
            this.selectedTilePanel.Visible = false;
            // 
            // LevelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1577, 879);
            this.Controls.Add(this.selectedToolPanel);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.tileSpriteFilterSelect);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tileTagFilterTextBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tileZIndexCheckBox);
            this.Controls.Add(this.tileHitboxCheckBox);
            this.Controls.Add(this.tilesetSelect);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.instancePropertiesLabel);
            this.Controls.Add(this.tileInstancePropsTextBox);
            this.Controls.Add(this.selectionPropertiesLabel);
            this.Controls.Add(this.tileSelectionPropsTextBox);
            this.Controls.Add(this.overridesTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.heightTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.widthTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.roomLinesCheckBox);
            this.Controls.Add(this.gridCheckBox);
            this.Controls.Add(this.instanceCheckBox);
            this.Controls.Add(this.redoBtn);
            this.Controls.Add(this.undoBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.tilePanel);
            this.Controls.Add(this.levelPanel);
            this.Controls.Add(this.Instances);
            this.Controls.Add(this.instanceListBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.objectsListBox);
            this.Controls.Add(this.newLevelBtn);
            this.Controls.Add(this.newLevelTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.levelListBox);
            this.Controls.Add(this.selectedTilePanel);
            this.Name = "LevelEditor";
            this.Text = "LevelEditor";
            this.Load += new System.EventHandler(this.LevelEditor_Load);
            this.levelPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.levelCanvas)).EndInit();
            this.tilePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tileCanvas)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.selectedToolPanel.ResumeLayout(false);
            this.selectedToolPanel.PerformLayout();
            this.selectedTilePanel.ResumeLayout(false);
            this.selectedTilePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox levelListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox newLevelTextBox;
        private System.Windows.Forms.Button newLevelBtn;
        private System.Windows.Forms.ListBox objectsListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Instances;
        private System.Windows.Forms.ListBox instanceListBox;
        private System.Windows.Forms.Panel levelPanel;
        private System.Windows.Forms.PictureBox levelCanvas;
        private System.Windows.Forms.Panel tilePanel;
        private System.Windows.Forms.PictureBox tileCanvas;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button undoBtn;
        private System.Windows.Forms.Button redoBtn;
        private System.Windows.Forms.CheckBox instanceCheckBox;
        private System.Windows.Forms.CheckBox gridCheckBox;
        private System.Windows.Forms.CheckBox roomLinesCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox widthTextBox;
        private System.Windows.Forms.TextBox heightTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox overridesTextBox;
        private System.Windows.Forms.RadioButton selectTileRadio;
        private System.Windows.Forms.TextBox tileSelectionPropsTextBox;
        private System.Windows.Forms.Label selectionPropertiesLabel;
        private System.Windows.Forms.Label instancePropertiesLabel;
        private System.Windows.Forms.TextBox tileInstancePropsTextBox;
        private System.Windows.Forms.RadioButton placeInstanceRadio;
        private System.Windows.Forms.RadioButton selectInstanceRadio;
        private System.Windows.Forms.RadioButton rectangleSelectRadio;
        private System.Windows.Forms.RadioButton placeTileRadio;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox tilesetSelect;
        private System.Windows.Forms.CheckBox tileHitboxCheckBox;
        private System.Windows.Forms.CheckBox tileZIndexCheckBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tileTagFilterTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox tileSpriteFilterSelect;
        private System.Windows.Forms.TextBox tileNameTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox tileHitboxModeSelect;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tileTagTextBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox tileZIndexSelect;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox tileSpriteSelect;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label idLabel;
        private Controls.RadioGroupBox selectedToolPanel;
        private System.Windows.Forms.Label selectionLabel;
        private System.Windows.Forms.Label tileIdLabel;
        private System.Windows.Forms.Panel selectedTilePanel;
    }
}
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
            this.tileIdLabel = new System.Windows.Forms.Label();
            this.selectionLabel = new System.Windows.Forms.Label();
            this.levelPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.levelCanvas)).BeginInit();
            this.tilePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tileCanvas)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.selectedToolPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // levelListBox
            // 
            this.levelListBox.FormattingEnabled = true;
            this.levelListBox.ItemHeight = 20;
            this.levelListBox.Location = new System.Drawing.Point(18, 140);
            this.levelListBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.levelListBox.Name = "levelListBox";
            this.levelListBox.Size = new System.Drawing.Size(228, 544);
            this.levelListBox.TabIndex = 0;
            this.levelListBox.SelectedIndexChanged += new System.EventHandler(this.levelListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Levels";
            // 
            // newLevelTextBox
            // 
            this.newLevelTextBox.Location = new System.Drawing.Point(18, 100);
            this.newLevelTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.newLevelTextBox.Name = "newLevelTextBox";
            this.newLevelTextBox.Size = new System.Drawing.Size(148, 26);
            this.newLevelTextBox.TabIndex = 2;
            // 
            // newLevelBtn
            // 
            this.newLevelBtn.Location = new System.Drawing.Point(18, 58);
            this.newLevelBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.newLevelBtn.Name = "newLevelBtn";
            this.newLevelBtn.Size = new System.Drawing.Size(112, 35);
            this.newLevelBtn.TabIndex = 3;
            this.newLevelBtn.Text = "New Level";
            this.newLevelBtn.UseVisualStyleBackColor = true;
            // 
            // objectsListBox
            // 
            this.objectsListBox.FormattingEnabled = true;
            this.objectsListBox.ItemHeight = 20;
            this.objectsListBox.Location = new System.Drawing.Point(274, 58);
            this.objectsListBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.objectsListBox.Name = "objectsListBox";
            this.objectsListBox.Size = new System.Drawing.Size(212, 444);
            this.objectsListBox.TabIndex = 4;
            this.objectsListBox.SelectedIndexChanged += new System.EventHandler(this.objectsListBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(268, 23);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 29);
            this.label2.TabIndex = 5;
            this.label2.Text = "Objects";
            // 
            // Instances
            // 
            this.Instances.AutoSize = true;
            this.Instances.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Instances.Location = new System.Drawing.Point(268, 526);
            this.Instances.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Instances.Name = "Instances";
            this.Instances.Size = new System.Drawing.Size(123, 29);
            this.Instances.TabIndex = 7;
            this.Instances.Text = "Instances";
            // 
            // instanceListBox
            // 
            this.instanceListBox.FormattingEnabled = true;
            this.instanceListBox.ItemHeight = 20;
            this.instanceListBox.Location = new System.Drawing.Point(274, 562);
            this.instanceListBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.instanceListBox.Name = "instanceListBox";
            this.instanceListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.instanceListBox.Size = new System.Drawing.Size(212, 444);
            this.instanceListBox.TabIndex = 6;
            this.instanceListBox.SelectedIndexChanged += new System.EventHandler(this.instanceListBox_SelectedIndexChanged);
            // 
            // levelPanel
            // 
            this.levelPanel.Controls.Add(this.levelCanvas);
            this.levelPanel.Location = new System.Drawing.Point(512, 63);
            this.levelPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.levelPanel.Name = "levelPanel";
            this.levelPanel.Size = new System.Drawing.Size(1110, 865);
            this.levelPanel.TabIndex = 8;
            // 
            // levelCanvas
            // 
            this.levelCanvas.Location = new System.Drawing.Point(4, 5);
            this.levelCanvas.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.levelCanvas.Name = "levelCanvas";
            this.levelCanvas.Size = new System.Drawing.Size(537, 397);
            this.levelCanvas.TabIndex = 0;
            this.levelCanvas.TabStop = false;
            // 
            // tilePanel
            // 
            this.tilePanel.Controls.Add(this.tileCanvas);
            this.tilePanel.Location = new System.Drawing.Point(1647, 68);
            this.tilePanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tilePanel.Name = "tilePanel";
            this.tilePanel.Size = new System.Drawing.Size(672, 860);
            this.tilePanel.TabIndex = 9;
            // 
            // tileCanvas
            // 
            this.tileCanvas.Location = new System.Drawing.Point(6, 6);
            this.tileCanvas.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tileCanvas.Name = "tileCanvas";
            this.tileCanvas.Size = new System.Drawing.Size(357, 369);
            this.tileCanvas.TabIndex = 0;
            this.tileCanvas.TabStop = false;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(512, 938);
            this.saveBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(112, 35);
            this.saveBtn.TabIndex = 10;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // undoBtn
            // 
            this.undoBtn.Location = new System.Drawing.Point(633, 938);
            this.undoBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.undoBtn.Name = "undoBtn";
            this.undoBtn.Size = new System.Drawing.Size(112, 35);
            this.undoBtn.TabIndex = 11;
            this.undoBtn.Text = "Undo";
            this.undoBtn.UseVisualStyleBackColor = true;
            this.undoBtn.Click += new System.EventHandler(this.undoBtn_Click);
            // 
            // redoBtn
            // 
            this.redoBtn.Location = new System.Drawing.Point(754, 937);
            this.redoBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.redoBtn.Name = "redoBtn";
            this.redoBtn.Size = new System.Drawing.Size(112, 35);
            this.redoBtn.TabIndex = 12;
            this.redoBtn.Text = "Redo";
            this.redoBtn.UseVisualStyleBackColor = true;
            this.redoBtn.Click += new System.EventHandler(this.redoBtn_Click);
            // 
            // instanceCheckBox
            // 
            this.instanceCheckBox.AutoSize = true;
            this.instanceCheckBox.Location = new System.Drawing.Point(876, 943);
            this.instanceCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.instanceCheckBox.Name = "instanceCheckBox";
            this.instanceCheckBox.Size = new System.Drawing.Size(105, 24);
            this.instanceCheckBox.TabIndex = 13;
            this.instanceCheckBox.Text = "Instances";
            this.instanceCheckBox.UseVisualStyleBackColor = true;
            this.instanceCheckBox.CheckedChanged += new System.EventHandler(this.instanceCheckBox_CheckedChanged);
            // 
            // gridCheckBox
            // 
            this.gridCheckBox.AutoSize = true;
            this.gridCheckBox.Location = new System.Drawing.Point(980, 943);
            this.gridCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridCheckBox.Name = "gridCheckBox";
            this.gridCheckBox.Size = new System.Drawing.Size(65, 24);
            this.gridCheckBox.TabIndex = 14;
            this.gridCheckBox.Text = "Grid";
            this.gridCheckBox.UseVisualStyleBackColor = true;
            // 
            // roomLinesCheckBox
            // 
            this.roomLinesCheckBox.AutoSize = true;
            this.roomLinesCheckBox.Location = new System.Drawing.Point(1042, 943);
            this.roomLinesCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.roomLinesCheckBox.Name = "roomLinesCheckBox";
            this.roomLinesCheckBox.Size = new System.Drawing.Size(120, 24);
            this.roomLinesCheckBox.TabIndex = 15;
            this.roomLinesCheckBox.Text = "Room Lines";
            this.roomLinesCheckBox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(516, 986);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "Width:";
            // 
            // widthTextBox
            // 
            this.widthTextBox.Location = new System.Drawing.Point(568, 982);
            this.widthTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.widthTextBox.Name = "widthTextBox";
            this.widthTextBox.Size = new System.Drawing.Size(148, 26);
            this.widthTextBox.TabIndex = 17;
            // 
            // heightTextBox
            // 
            this.heightTextBox.Location = new System.Drawing.Point(784, 982);
            this.heightTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.heightTextBox.Name = "heightTextBox";
            this.heightTextBox.Size = new System.Drawing.Size(148, 26);
            this.heightTextBox.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(728, 986);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 20);
            this.label4.TabIndex = 18;
            this.label4.Text = "Height:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.selectionLabel);
            this.flowLayoutPanel1.Controls.Add(this.tileIdLabel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(944, 982);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(678, 31);
            this.flowLayoutPanel1.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(520, 1028);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(181, 20);
            this.label5.TabIndex = 21;
            this.label5.Text = "Show overrides with key:";
            // 
            // overridesTextBox
            // 
            this.overridesTextBox.Location = new System.Drawing.Point(717, 1023);
            this.overridesTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.overridesTextBox.Name = "overridesTextBox";
            this.overridesTextBox.Size = new System.Drawing.Size(148, 26);
            this.overridesTextBox.TabIndex = 22;
            // 
            // tileSelectionPropsTextBox
            // 
            this.tileSelectionPropsTextBox.Location = new System.Drawing.Point(525, 1180);
            this.tileSelectionPropsTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tileSelectionPropsTextBox.Multiline = true;
            this.tileSelectionPropsTextBox.Name = "tileSelectionPropsTextBox";
            this.tileSelectionPropsTextBox.Size = new System.Drawing.Size(376, 135);
            this.tileSelectionPropsTextBox.TabIndex = 24;
            this.tileSelectionPropsTextBox.Visible = false;
            // 
            // selectionPropertiesLabel
            // 
            this.selectionPropertiesLabel.AutoSize = true;
            this.selectionPropertiesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.selectionPropertiesLabel.Location = new System.Drawing.Point(522, 1145);
            this.selectionPropertiesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.selectionPropertiesLabel.Name = "selectionPropertiesLabel";
            this.selectionPropertiesLabel.Size = new System.Drawing.Size(206, 25);
            this.selectionPropertiesLabel.TabIndex = 25;
            this.selectionPropertiesLabel.Text = "Selection Properties";
            this.selectionPropertiesLabel.Visible = false;
            // 
            // instancePropertiesLabel
            // 
            this.instancePropertiesLabel.AutoSize = true;
            this.instancePropertiesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.instancePropertiesLabel.Location = new System.Drawing.Point(1077, 1145);
            this.instancePropertiesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.instancePropertiesLabel.Name = "instancePropertiesLabel";
            this.instancePropertiesLabel.Size = new System.Drawing.Size(198, 25);
            this.instancePropertiesLabel.TabIndex = 27;
            this.instancePropertiesLabel.Text = "Instance Properties";
            this.instancePropertiesLabel.Visible = false;
            // 
            // tileInstancePropsTextBox
            // 
            this.tileInstancePropsTextBox.Location = new System.Drawing.Point(1080, 1180);
            this.tileInstancePropsTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tileInstancePropsTextBox.Multiline = true;
            this.tileInstancePropsTextBox.Name = "tileInstancePropsTextBox";
            this.tileInstancePropsTextBox.Size = new System.Drawing.Size(376, 135);
            this.tileInstancePropsTextBox.TabIndex = 26;
            this.tileInstancePropsTextBox.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1648, 945);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 20);
            this.label9.TabIndex = 30;
            this.label9.Text = "Tileset:";
            // 
            // tilesetSelect
            // 
            this.tilesetSelect.FormattingEnabled = true;
            this.tilesetSelect.Location = new System.Drawing.Point(1719, 940);
            this.tilesetSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tilesetSelect.Name = "tilesetSelect";
            this.tilesetSelect.Size = new System.Drawing.Size(180, 28);
            this.tilesetSelect.TabIndex = 31;
            this.tilesetSelect.SelectedIndexChanged += new System.EventHandler(this.tilesetSelect_SelectedIndexChanged);
            // 
            // tileHitboxCheckBox
            // 
            this.tileHitboxCheckBox.AutoSize = true;
            this.tileHitboxCheckBox.Location = new System.Drawing.Point(1654, 985);
            this.tileHitboxCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tileHitboxCheckBox.Name = "tileHitboxCheckBox";
            this.tileHitboxCheckBox.Size = new System.Drawing.Size(162, 24);
            this.tileHitboxCheckBox.TabIndex = 32;
            this.tileHitboxCheckBox.Text = "Show tile hitboxes";
            this.tileHitboxCheckBox.UseVisualStyleBackColor = true;
            // 
            // tileZIndexCheckBox
            // 
            this.tileZIndexCheckBox.AutoSize = true;
            this.tileZIndexCheckBox.Location = new System.Drawing.Point(1858, 982);
            this.tileZIndexCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tileZIndexCheckBox.Name = "tileZIndexCheckBox";
            this.tileZIndexCheckBox.Size = new System.Drawing.Size(205, 24);
            this.tileZIndexCheckBox.TabIndex = 33;
            this.tileZIndexCheckBox.Text = "Show tiles with z index 1";
            this.tileZIndexCheckBox.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1653, 1028);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(144, 20);
            this.label10.TabIndex = 34;
            this.label10.Text = "Show tiles with tag:";
            // 
            // tileTagFilterTextBox
            // 
            this.tileTagFilterTextBox.Location = new System.Drawing.Point(1806, 1025);
            this.tileTagFilterTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tileTagFilterTextBox.Name = "tileTagFilterTextBox";
            this.tileTagFilterTextBox.Size = new System.Drawing.Size(148, 26);
            this.tileTagFilterTextBox.TabIndex = 35;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1972, 1029);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(160, 20);
            this.label11.TabIndex = 36;
            this.label11.Text = "Show tiles with sprite:";
            // 
            // tileSpriteFilterSelect
            // 
            this.tileSpriteFilterSelect.FormattingEnabled = true;
            this.tileSpriteFilterSelect.Location = new System.Drawing.Point(2138, 1025);
            this.tileSpriteFilterSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tileSpriteFilterSelect.Name = "tileSpriteFilterSelect";
            this.tileSpriteFilterSelect.Size = new System.Drawing.Size(180, 28);
            this.tileSpriteFilterSelect.TabIndex = 37;
            this.tileSpriteFilterSelect.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // tileNameTextBox
            // 
            this.tileNameTextBox.Location = new System.Drawing.Point(1740, 1098);
            this.tileNameTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tileNameTextBox.Name = "tileNameTextBox";
            this.tileNameTextBox.Size = new System.Drawing.Size(148, 26);
            this.tileNameTextBox.TabIndex = 39;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1647, 1103);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 20);
            this.label12.TabIndex = 38;
            this.label12.Text = "Tile name:";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(1900, 1103);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(27, 20);
            this.label13.TabIndex = 40;
            this.label13.Text = "Id:";
            // 
            // tileHitboxModeSelect
            // 
            this.tileHitboxModeSelect.FormattingEnabled = true;
            this.tileHitboxModeSelect.Location = new System.Drawing.Point(1754, 1143);
            this.tileHitboxModeSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tileHitboxModeSelect.Name = "tileHitboxModeSelect";
            this.tileHitboxModeSelect.Size = new System.Drawing.Size(180, 28);
            this.tileHitboxModeSelect.TabIndex = 42;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(1648, 1148);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(98, 20);
            this.label14.TabIndex = 41;
            this.label14.Text = "Hitbox mode";
            // 
            // tileTagTextBox
            // 
            this.tileTagTextBox.Location = new System.Drawing.Point(1702, 1185);
            this.tileTagTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tileTagTextBox.Name = "tileTagTextBox";
            this.tileTagTextBox.Size = new System.Drawing.Size(148, 26);
            this.tileTagTextBox.TabIndex = 44;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(1648, 1188);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(36, 20);
            this.label15.TabIndex = 43;
            this.label15.Text = "Tag";
            // 
            // tileZIndexSelect
            // 
            this.tileZIndexSelect.FormattingEnabled = true;
            this.tileZIndexSelect.Location = new System.Drawing.Point(1972, 1185);
            this.tileZIndexSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tileZIndexSelect.Name = "tileZIndexSelect";
            this.tileZIndexSelect.Size = new System.Drawing.Size(180, 28);
            this.tileZIndexSelect.TabIndex = 46;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(1900, 1191);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(63, 20);
            this.label16.TabIndex = 45;
            this.label16.Text = "Z-Index";
            // 
            // tileSpriteSelect
            // 
            this.tileSpriteSelect.FormattingEnabled = true;
            this.tileSpriteSelect.Location = new System.Drawing.Point(1702, 1229);
            this.tileSpriteSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tileSpriteSelect.Name = "tileSpriteSelect";
            this.tileSpriteSelect.Size = new System.Drawing.Size(180, 28);
            this.tileSpriteSelect.TabIndex = 48;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(1648, 1234);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(51, 20);
            this.label17.TabIndex = 47;
            this.label17.Text = "Sprite";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label18.Location = new System.Drawing.Point(1654, 1065);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(648, 20);
            this.label18.TabIndex = 49;
            this.label18.Text = "_______________________________________________________________________";
            // 
            // idLabel
            // 
            this.idLabel.AutoSize = true;
            this.idLabel.Location = new System.Drawing.Point(1934, 1103);
            this.idLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(62, 20);
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
            this.selectedToolPanel.Location = new System.Drawing.Point(520, 1065);
            this.selectedToolPanel.Name = "selectedToolPanel";
            this.selectedToolPanel.Selected = 2;
            this.selectedToolPanel.Size = new System.Drawing.Size(860, 69);
            this.selectedToolPanel.TabIndex = 51;
            this.selectedToolPanel.TabStop = false;
            this.selectedToolPanel.Text = "Tool";
            // 
            // placeInstanceRadio
            // 
            this.placeInstanceRadio.AutoSize = true;
            this.placeInstanceRadio.Location = new System.Drawing.Point(548, 27);
            this.placeInstanceRadio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.placeInstanceRadio.Name = "placeInstanceRadio";
            this.placeInstanceRadio.Size = new System.Drawing.Size(139, 24);
            this.placeInstanceRadio.TabIndex = 27;
            this.placeInstanceRadio.Tag = "4";
            this.placeInstanceRadio.Text = "Place Instance";
            this.placeInstanceRadio.UseVisualStyleBackColor = true;
            // 
            // selectTileRadio
            // 
            this.selectTileRadio.AutoSize = true;
            this.selectTileRadio.Location = new System.Drawing.Point(7, 27);
            this.selectTileRadio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectTileRadio.Name = "selectTileRadio";
            this.selectTileRadio.Size = new System.Drawing.Size(107, 24);
            this.selectTileRadio.TabIndex = 23;
            this.selectTileRadio.Tag = "0";
            this.selectTileRadio.Text = "Select Tile";
            this.selectTileRadio.UseVisualStyleBackColor = true;
            // 
            // selectInstanceRadio
            // 
            this.selectInstanceRadio.AutoSize = true;
            this.selectInstanceRadio.Location = new System.Drawing.Point(395, 27);
            this.selectInstanceRadio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectInstanceRadio.Name = "selectInstanceRadio";
            this.selectInstanceRadio.Size = new System.Drawing.Size(145, 24);
            this.selectInstanceRadio.TabIndex = 26;
            this.selectInstanceRadio.Tag = "3";
            this.selectInstanceRadio.Text = "Select Instance";
            this.selectInstanceRadio.UseVisualStyleBackColor = true;
            // 
            // placeTileRadio
            // 
            this.placeTileRadio.AutoSize = true;
            this.placeTileRadio.Location = new System.Drawing.Point(122, 27);
            this.placeTileRadio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.placeTileRadio.Name = "placeTileRadio";
            this.placeTileRadio.Size = new System.Drawing.Size(101, 24);
            this.placeTileRadio.TabIndex = 24;
            this.placeTileRadio.Tag = "1";
            this.placeTileRadio.Text = "Place Tile";
            this.placeTileRadio.UseVisualStyleBackColor = true;
            // 
            // rectangleSelectRadio
            // 
            this.rectangleSelectRadio.AutoSize = true;
            this.rectangleSelectRadio.Checked = true;
            this.rectangleSelectRadio.Location = new System.Drawing.Point(231, 27);
            this.rectangleSelectRadio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rectangleSelectRadio.Name = "rectangleSelectRadio";
            this.rectangleSelectRadio.Size = new System.Drawing.Size(156, 24);
            this.rectangleSelectRadio.TabIndex = 25;
            this.rectangleSelectRadio.TabStop = true;
            this.rectangleSelectRadio.Tag = "2";
            this.rectangleSelectRadio.Text = "Rectangle Select";
            this.rectangleSelectRadio.UseVisualStyleBackColor = true;
            // 
            // tileIdLabel
            // 
            this.tileIdLabel.AutoSize = true;
            this.tileIdLabel.Location = new System.Drawing.Point(87, 5);
            this.tileIdLabel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tileIdLabel.Name = "tileIdLabel";
            this.tileIdLabel.Size = new System.Drawing.Size(34, 20);
            this.tileIdLabel.TabIndex = 2;
            this.tileIdLabel.Text = "Tid:";
            // 
            // selectionLabel
            // 
            this.selectionLabel.AutoSize = true;
            this.selectionLabel.Location = new System.Drawing.Point(4, 5);
            this.selectionLabel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectionLabel.Name = "selectionLabel";
            this.selectionLabel.Size = new System.Drawing.Size(75, 20);
            this.selectionLabel.TabIndex = 0;
            this.selectionLabel.Text = "Selection";
            // 
            // LevelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2376, 1325);
            this.Controls.Add(this.selectedToolPanel);
            this.Controls.Add(this.idLabel);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.tileSpriteSelect);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.tileZIndexSelect);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.tileTagTextBox);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.tileHitboxModeSelect);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tileNameTextBox);
            this.Controls.Add(this.label12);
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
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
    }
}
namespace LevelEditor_CS.Controls
{
    partial class Hitbox
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.deleteBtn = new System.Windows.Forms.Button();
            this.selectBtn = new System.Windows.Forms.Button();
            this.triggerCheckbox = new System.Windows.Forms.CheckBox();
            this.flagLabel = new System.Windows.Forms.Label();
            this.flagText = new System.Windows.Forms.TextBox();
            this.yOffLabel = new System.Windows.Forms.Label();
            this.yOffText = new System.Windows.Forms.TextBox();
            this.xOffLabel = new System.Windows.Forms.Label();
            this.xOffText = new System.Windows.Forms.TextBox();
            this.heightLabel = new System.Windows.Forms.Label();
            this.heightTextBox = new System.Windows.Forms.TextBox();
            this.widthLabel = new System.Windows.Forms.Label();
            this.widthText = new System.Windows.Forms.TextBox();
            // 
            // deleteBtn
            // 
            this.deleteBtn.Location = new System.Drawing.Point(485, 19);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.Size = new System.Drawing.Size(64, 23);
            this.deleteBtn.TabIndex = 12;
            this.deleteBtn.Text = "Delete";
            this.deleteBtn.UseVisualStyleBackColor = true;
            // 
            // selectBtn
            // 
            this.selectBtn.Location = new System.Drawing.Point(415, 19);
            this.selectBtn.Name = "selectBtn";
            this.selectBtn.Size = new System.Drawing.Size(64, 23);
            this.selectBtn.TabIndex = 11;
            this.selectBtn.Text = "Select";
            this.selectBtn.UseVisualStyleBackColor = true;
            // 
            // triggerCheckbox
            // 
            this.triggerCheckbox.AutoSize = true;
            this.triggerCheckbox.Location = new System.Drawing.Point(347, 22);
            this.triggerCheckbox.Name = "triggerCheckbox";
            this.triggerCheckbox.Size = new System.Drawing.Size(59, 17);
            this.triggerCheckbox.TabIndex = 10;
            this.triggerCheckbox.Text = "Trigger";
            this.triggerCheckbox.UseVisualStyleBackColor = true;
            // 
            // flagLabel
            // 
            this.flagLabel.AutoSize = true;
            this.flagLabel.Location = new System.Drawing.Point(258, 22);
            this.flagLabel.Name = "flagLabel";
            this.flagLabel.Size = new System.Drawing.Size(24, 13);
            this.flagLabel.TabIndex = 9;
            this.flagLabel.Text = "flag";
            // 
            // flagText
            // 
            this.flagText.Location = new System.Drawing.Point(288, 19);
            this.flagText.Name = "flagText";
            this.flagText.Size = new System.Drawing.Size(39, 20);
            this.flagText.TabIndex = 8;
            // 
            // yOffLabel
            // 
            this.yOffLabel.AutoSize = true;
            this.yOffLabel.Location = new System.Drawing.Point(179, 21);
            this.yOffLabel.Name = "yOffLabel";
            this.yOffLabel.Size = new System.Drawing.Size(27, 13);
            this.yOffLabel.TabIndex = 7;
            this.yOffLabel.Text = "y-off";
            // 
            // yOffText
            // 
            this.yOffText.Location = new System.Drawing.Point(212, 19);
            this.yOffText.Name = "yOffText";
            this.yOffText.Size = new System.Drawing.Size(37, 20);
            this.yOffText.TabIndex = 6;
            // 
            // xOffLabel
            // 
            this.xOffLabel.AutoSize = true;
            this.xOffLabel.Location = new System.Drawing.Point(112, 21);
            this.xOffLabel.Name = "xOffLabel";
            this.xOffLabel.Size = new System.Drawing.Size(27, 13);
            this.xOffLabel.TabIndex = 5;
            this.xOffLabel.Text = "x-off";
            // 
            // xOffText
            // 
            this.xOffText.Location = new System.Drawing.Point(145, 19);
            this.xOffText.Name = "xOffText";
            this.xOffText.Size = new System.Drawing.Size(28, 20);
            this.xOffText.TabIndex = 4;
            // 
            // heightLabel
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(60, 22);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(13, 13);
            this.heightLabel.TabIndex = 3;
            this.heightLabel.Text = "h";
            // 
            // heightTextBox
            // 
            this.heightTextBox.Location = new System.Drawing.Point(75, 18);
            this.heightTextBox.Name = "heightTextBox";
            this.heightTextBox.Size = new System.Drawing.Size(32, 20);
            this.heightTextBox.TabIndex = 2;
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(7, 22);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(15, 13);
            this.widthLabel.TabIndex = 1;
            this.widthLabel.Text = "w";
            // 
            // widthText
            // 
            this.widthText.Location = new System.Drawing.Point(24, 18);
            this.widthText.Name = "widthText";
            this.widthText.Size = new System.Drawing.Size(29, 20);
            this.widthText.TabIndex = 0;

        }

        #endregion

        private System.Windows.Forms.Button deleteBtn;
        private System.Windows.Forms.Button selectBtn;
        private System.Windows.Forms.CheckBox triggerCheckbox;
        private System.Windows.Forms.Label flagLabel;
        private System.Windows.Forms.TextBox flagText;
        private System.Windows.Forms.Label yOffLabel;
        private System.Windows.Forms.TextBox yOffText;
        private System.Windows.Forms.Label xOffLabel;
        private System.Windows.Forms.TextBox xOffText;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.TextBox heightTextBox;
        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.TextBox widthText;
    }
}

namespace LevelEditor_CS.Controls
{
    partial class HitboxControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.widthTextBox = new System.Windows.Forms.TextBox();
            this.heightTextBox = new System.Windows.Forms.TextBox();
            this.heightText = new System.Windows.Forms.Label();
            this.xOffTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.yOffTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.triggerCheckBox = new System.Windows.Forms.CheckBox();
            this.selectBtn = new System.Windows.Forms.Button();
            this.deleteBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "w";
            // 
            // widthTextBox
            // 
            this.widthTextBox.Location = new System.Drawing.Point(32, 0);
            this.widthTextBox.Name = "widthTextBox";
            this.widthTextBox.Size = new System.Drawing.Size(44, 26);
            this.widthTextBox.TabIndex = 1;
            // 
            // heightTextBox
            // 
            this.heightTextBox.Location = new System.Drawing.Point(115, 0);
            this.heightTextBox.Name = "heightTextBox";
            this.heightTextBox.Size = new System.Drawing.Size(50, 26);
            this.heightTextBox.TabIndex = 3;
            // 
            // heightText
            // 
            this.heightText.AutoSize = true;
            this.heightText.Location = new System.Drawing.Point(91, 0);
            this.heightText.Name = "heightText";
            this.heightText.Size = new System.Drawing.Size(18, 20);
            this.heightText.TabIndex = 2;
            this.heightText.Text = "h";
            // 
            // xOffTextBox
            // 
            this.xOffTextBox.Location = new System.Drawing.Point(217, 0);
            this.xOffTextBox.Name = "xOffTextBox";
            this.xOffTextBox.Size = new System.Drawing.Size(59, 26);
            this.xOffTextBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(171, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "x-off";
            // 
            // yOffTextBox
            // 
            this.yOffTextBox.Location = new System.Drawing.Point(328, 0);
            this.yOffTextBox.Name = "yOffTextBox";
            this.yOffTextBox.Size = new System.Drawing.Size(59, 26);
            this.yOffTextBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(282, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "y-off";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(434, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(94, 26);
            this.textBox1.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(393, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "flag";
            // 
            // triggerCheckBox
            // 
            this.triggerCheckBox.AutoSize = true;
            this.triggerCheckBox.Location = new System.Drawing.Point(543, 0);
            this.triggerCheckBox.Name = "triggerCheckBox";
            this.triggerCheckBox.Size = new System.Drawing.Size(93, 24);
            this.triggerCheckBox.TabIndex = 11;
            this.triggerCheckBox.Text = "Trigger?";
            this.triggerCheckBox.UseVisualStyleBackColor = true;
            // 
            // selectBtn
            // 
            this.selectBtn.Location = new System.Drawing.Point(651, 0);
            this.selectBtn.Name = "selectBtn";
            this.selectBtn.Size = new System.Drawing.Size(75, 31);
            this.selectBtn.TabIndex = 12;
            this.selectBtn.Text = "Select";
            this.selectBtn.UseVisualStyleBackColor = true;
            // 
            // deleteBtn
            // 
            this.deleteBtn.Location = new System.Drawing.Point(732, 0);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.Size = new System.Drawing.Size(75, 31);
            this.deleteBtn.TabIndex = 13;
            this.deleteBtn.Text = "Delete";
            this.deleteBtn.UseVisualStyleBackColor = true;
            // 
            // Hitbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.deleteBtn);
            this.Controls.Add(this.selectBtn);
            this.Controls.Add(this.triggerCheckBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.yOffTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.xOffTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.heightTextBox);
            this.Controls.Add(this.heightText);
            this.Controls.Add(this.widthTextBox);
            this.Controls.Add(this.label1);
            this.Name = "Hitbox";
            this.Size = new System.Drawing.Size(896, 50);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox widthTextBox;
        private System.Windows.Forms.TextBox heightTextBox;
        private System.Windows.Forms.Label heightText;
        private System.Windows.Forms.TextBox xOffTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox yOffTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox triggerCheckBox;
        private System.Windows.Forms.Button selectBtn;
        private System.Windows.Forms.Button deleteBtn;
    }
}

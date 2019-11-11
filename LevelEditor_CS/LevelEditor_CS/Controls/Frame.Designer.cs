namespace LevelEditor_CS.Controls
{
    partial class Frame
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
            this.secsTextBox = new System.Windows.Forms.TextBox();
            this.xOffTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.yOffTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.copyUpBtn = new System.Windows.Forms.Button();
            this.copyDownBtn = new System.Windows.Forms.Button();
            this.moveUpBtn = new System.Windows.Forms.Button();
            this.moveDownBtn = new System.Windows.Forms.Button();
            this.saveBtn = new System.Windows.Forms.Button();
            this.deleteBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "secs";
            // 
            // secsTextBox
            // 
            this.secsTextBox.Location = new System.Drawing.Point(52, 2);
            this.secsTextBox.Name = "secsTextBox";
            this.secsTextBox.Size = new System.Drawing.Size(73, 26);
            this.secsTextBox.TabIndex = 1;
            // 
            // xOffTextBox
            // 
            this.xOffTextBox.Location = new System.Drawing.Point(190, 3);
            this.xOffTextBox.Name = "xOffTextBox";
            this.xOffTextBox.Size = new System.Drawing.Size(73, 26);
            this.xOffTextBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(142, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "x-off";
            // 
            // yOffTextBox
            // 
            this.yOffTextBox.Location = new System.Drawing.Point(322, 3);
            this.yOffTextBox.Name = "yOffTextBox";
            this.yOffTextBox.Size = new System.Drawing.Size(73, 26);
            this.yOffTextBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(275, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "y-off";
            // 
            // copyUpBtn
            // 
            this.copyUpBtn.Location = new System.Drawing.Point(402, 1);
            this.copyUpBtn.Name = "copyUpBtn";
            this.copyUpBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.copyUpBtn.Size = new System.Drawing.Size(42, 33);
            this.copyUpBtn.TabIndex = 6;
            this.copyUpBtn.Text = "CU";
            this.copyUpBtn.UseVisualStyleBackColor = true;
            // 
            // copyDownBtn
            // 
            this.copyDownBtn.Location = new System.Drawing.Point(449, 1);
            this.copyDownBtn.Name = "copyDownBtn";
            this.copyDownBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.copyDownBtn.Size = new System.Drawing.Size(42, 33);
            this.copyDownBtn.TabIndex = 7;
            this.copyDownBtn.Text = "CD";
            this.copyDownBtn.UseVisualStyleBackColor = true;
            // 
            // moveUpBtn
            // 
            this.moveUpBtn.Location = new System.Drawing.Point(497, 1);
            this.moveUpBtn.Name = "moveUpBtn";
            this.moveUpBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.moveUpBtn.Size = new System.Drawing.Size(42, 33);
            this.moveUpBtn.TabIndex = 8;
            this.moveUpBtn.Text = "MU";
            this.moveUpBtn.UseVisualStyleBackColor = true;
            // 
            // moveDownBtn
            // 
            this.moveDownBtn.Location = new System.Drawing.Point(545, 1);
            this.moveDownBtn.Name = "moveDownBtn";
            this.moveDownBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.moveDownBtn.Size = new System.Drawing.Size(42, 33);
            this.moveDownBtn.TabIndex = 9;
            this.moveDownBtn.Text = "MD";
            this.moveDownBtn.UseVisualStyleBackColor = true;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(592, 1);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.saveBtn.Size = new System.Drawing.Size(32, 33);
            this.saveBtn.TabIndex = 10;
            this.saveBtn.Text = "S";
            this.saveBtn.UseVisualStyleBackColor = true;
            // 
            // deleteBtn
            // 
            this.deleteBtn.Location = new System.Drawing.Point(629, 1);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.deleteBtn.Size = new System.Drawing.Size(32, 33);
            this.deleteBtn.TabIndex = 11;
            this.deleteBtn.Text = "D";
            this.deleteBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(8, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(659, 24);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Child Frames";
            // 
            // Frame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.deleteBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.moveDownBtn);
            this.Controls.Add(this.moveUpBtn);
            this.Controls.Add(this.copyDownBtn);
            this.Controls.Add(this.copyUpBtn);
            this.Controls.Add(this.yOffTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.xOffTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.secsTextBox);
            this.Controls.Add(this.label1);
            this.Name = "Frame";
            this.Size = new System.Drawing.Size(688, 83);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox secsTextBox;
        private System.Windows.Forms.TextBox xOffTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox yOffTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button copyUpBtn;
        private System.Windows.Forms.Button copyDownBtn;
        private System.Windows.Forms.Button moveUpBtn;
        private System.Windows.Forms.Button moveDownBtn;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button deleteBtn;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

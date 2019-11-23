using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LevelEditor_CS.Models;

namespace LevelEditor_CS.Controls
{
    public partial class FrameControl : UserControl
    {
        public FrameControl(Frame frame)
        {
            InitializeComponent();
            this.frame = frame;
        }

        private Frame _frame;
        public Frame frame
        {
            get
            {
                return _frame;
            }
            set
            {
                _frame = value;
                secsTextBox.DataBindings.Add("Text", _frame, "duration", false, DataSourceUpdateMode.OnPropertyChanged);
                xOffTextBox.DataBindings.Add("Text", _frame.offset, "x", false, DataSourceUpdateMode.OnPropertyChanged);
                yOffTextBox.DataBindings.Add("Text", _frame.offset, "y", false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }
    }
}

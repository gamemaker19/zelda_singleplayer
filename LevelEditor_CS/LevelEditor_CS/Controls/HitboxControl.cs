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
    public partial class HitboxControl : UserControl
    {
        public HitboxControl(Hitbox hitbox)
        {
            InitializeComponent();
            this.hitbox = hitbox;
        }

        private Hitbox _hitbox;
        public Hitbox hitbox
        {
            get
            {
                return _hitbox;
            }
            set
            {
                _hitbox = value;
                widthTextBox.DataBindings.Add("Text", _hitbox, "width", false, DataSourceUpdateMode.OnPropertyChanged);
                heightTextBox.DataBindings.Add("Text", _hitbox, "height", false, DataSourceUpdateMode.OnPropertyChanged);
                xOffTextBox.DataBindings.Add("Text", _hitbox.offset, "x", false, DataSourceUpdateMode.OnPropertyChanged);
                yOffTextBox.DataBindings.Add("Text", _hitbox.offset, "y", false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

    }
}

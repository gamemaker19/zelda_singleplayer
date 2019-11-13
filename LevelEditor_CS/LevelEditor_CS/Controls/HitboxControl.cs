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
                widthTextBox.DataBindings.Add("Text", new BindingSource(_hitbox, "width"), "width");
                heightText.DataBindings.Add("Text", new BindingSource(_hitbox, "height"), "width");
            }
        }



    }
}

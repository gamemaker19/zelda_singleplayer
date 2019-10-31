using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor_CS.Controls
{
    public partial class Hitbox : Component
    {
        public Hitbox()
        {
            InitializeComponent();
        }

        public Hitbox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}

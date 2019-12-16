using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelEditor_CS.Editor
{
    public class SelectBinding<T>
    {
        public List<T> items;
        private int _selectedIndex = -1;
        public int selectedIndex { get { return _selectedIndex; } set { _selectedIndex = value; onIndexChange?.Invoke(); } }
        public Action onIndexChange;

        public T selected
        {
            get
            {
                if (items == null || items.Count == 0 || selectedIndex < 0) return default(T);
                return items[selectedIndex];
            }
        }

        public SelectBinding(ComboBox comboBox, List<T> items, string displayName, Action onIndexChange)
        {
            this.items = items;
            comboBox.DataSource = items;
            comboBox.DisplayMember = displayName;
            this.onIndexChange = onIndexChange;
            comboBox.DataBindings.Add("SelectedIndex", this, nameof(selectedIndex), false, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}

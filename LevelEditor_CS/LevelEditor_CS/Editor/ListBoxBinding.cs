using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelEditor_CS.Editor
{
    public class ListBoxBinding<T>
    {
        public List<T> items;
        public ListBox listBox;

        private int _selectedIndex = -1;
        public int selectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (_selectedIndex == value) return;
                T oldItem = _selectedIndex >= 0 ? items[_selectedIndex] : default(T);
                _selectedIndex = value;
                onIndexChange?.Invoke(oldItem);
            }
        }

        public T selected
        {
            get
            {
                if (_selectedIndex == -1 || items == null || items.Count == 0)
                {
                    return default(T);
                }
                return items[_selectedIndex];
            }
        }

        public Action<T> onIndexChange;

        public ListBoxBinding(ListBox listBox, List<T> items, string displayName, Action<T> onIndexChange)
        {
            this.items = items;
            listBox.DataSource = items;
            listBox.DisplayMember = displayName;
            this.onIndexChange = onIndexChange;
            listBox.DataBindings.Add("SelectedIndex", this, nameof(selectedIndex), false, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListBox;

namespace LevelEditor_CS.Editor
{
    public class MultiListBoxBinding<T>
    {
        public List<T> items;
        public ListBox listBox;

        public MultiListBoxBinding(ListBox listBox, List<T> objects, string displayName)
        {
            listBox.SelectedIndex = -1;
            listBox.DataSource = objects;
            listBox.DisplayMember = displayName;
        }

        public List<T> selected
        {
            get
            {
                var instances = new List<T>();
                
                for (int i = 0; i < listBox.Items.Count; i++)
                {
                    if(listBox.GetSelected(i))
                    {
                        instances.Add(items[i]);
                    }
                }
                return instances;
            }
        }

        public void clear()
        {
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                listBox.SetSelected(i, false);
            }
        }

        public void add(T item)
        {
            listBox.SetSelected(items.IndexOf(item), true);
        }
    }
}

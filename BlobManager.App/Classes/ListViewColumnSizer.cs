using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BlobManager.App.Classes
{
    public class ListViewColumnSizer
    {                
        private readonly ListView _listView;
        private readonly Dictionary<int, double> _columnInfo;

        public ListViewColumnSizer(ListView listView)
        {                        
            _listView = listView;
            _listView.Resize += ResizeColumns;

            double width = listView.Width;
            _columnInfo = new Dictionary<int, double>();
            foreach (ColumnHeader col in listView.Columns)
            {
                _columnInfo.Add(col.Index, col.Width / width);
            }
        }

        private void ResizeColumns(object sender, EventArgs e)
        {
            foreach (var col in _columnInfo)
            {
                _listView.Columns[col.Key].Width = (int)(_listView.Width * col.Value);
            }
        }
    }
}

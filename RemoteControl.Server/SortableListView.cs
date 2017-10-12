using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemoteControl.Server
{
    class SortableListView : ListView
    {
        private Dictionary<int,SortOrder> mapping = new Dictionary<int, SortOrder>(); 
        public SortableListView()
        {
            this.ColumnClick += new ColumnClickEventHandler(SortableListView_ColumnClick);
            this.ListViewItemSorter = new ListViewItemComparer();
        }

        void SortableListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 记录每列的排序方式
            if(!mapping.ContainsKey(e.Column))
                mapping.Add(e.Column, SortOrder.Ascending);

            var comparer = this.ListViewItemSorter as ListViewItemComparer;
            comparer.SortColumn = e.Column;
            comparer.Order = mapping[e.Column];
            this.Sort();

            // 切换每列的排列方式
            if (mapping[e.Column] == SortOrder.Ascending)
            {
                mapping[e.Column] = SortOrder.Descending;
            }
            else if (mapping[e.Column] == SortOrder.Descending)
            {
                mapping[e.Column] = SortOrder.Ascending;
            }
        }

        class ListViewItemComparer:IComparer
        {
            public int SortColumn = 0;
            public SortOrder Order = SortOrder.Ascending;

            public int Compare(object x, object y)
            {
                var itemX = x as ListViewItem;
                var itemY = y as ListViewItem;

                int value = 1;

                try
                {
                    value = itemX.SubItems[SortColumn].Text.CompareTo(itemY.SubItems[SortColumn].Text);
                }
                catch (Exception ex)
                {
                    return 1;
                }

                if (Order == SortOrder.Ascending)
                    return value;
                else
                {
                    return -1*value;
                }
            }
        }
    }
}

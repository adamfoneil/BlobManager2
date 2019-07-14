using BlobManager.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlobManager.App.Controls
{
    /// <summary>
    /// Dynamically adds and removes link labels on a tool strip to act like a breadcrumb control
    /// </summary>
    public class ToolStripBreadcrumb
    {
        private List<ToolStripLabel> _labels = new List<ToolStripLabel>();

        public ToolStripBreadcrumb(ToolStrip toolStrip)
        {
            ToolStrip = toolStrip;
        }

        public ToolStrip ToolStrip { get; }

        public void Add<T>(T @object, Func<T, Task> callback = null) where T: IBreadcrumbItem
        {
            var label = new ToolStripLabel(@object.GetBreadcrumbText()) { IsLink = (callback != null) };

            if (callback != null) label.Click += async delegate (object sender, EventArgs e) { await callback(@object); };

            _labels.Add(label);

            if (_labels.Count > 1) ToolStrip.Items.Add(new BreadcrumbSeparator());

            ToolStrip.Items.Add(label);
        }

        public void Clear()
        {
            foreach (var label in _labels) ToolStrip.Items.Remove(label);            
            _labels.Clear();

            List<BreadcrumbSeparator> separators = new List<BreadcrumbSeparator>();
            foreach (var item in ToolStrip.Items)
            {
                var separator = item as BreadcrumbSeparator;
                if (separator != null) separators.Add(separator);
            }
            foreach (var sep in separators) ToolStrip.Items.Remove(sep);
        }

        public void AddPath(string path, Func<Folder, Task> callback)
        {
            var folders = path.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < folders.Length; i++)
            {
                string fullPath = string.Join("/", folders.Take(i + 1)) + "/";
                Add(new Folder(fullPath, folders[i]), callback);
            }
        }

        public class Folder : IBreadcrumbItem
        {
            public Folder(string fullPath, string displayText)
            {
                FullPath = fullPath;
                Text = displayText;
            }

            public string FullPath { get; }
            public string Text { get; }

            public string GetBreadcrumbText()
            {
                return Text;
            }
        }

        internal class BreadcrumbSeparator : ToolStripLabel
        {
            public BreadcrumbSeparator() : base(" / ")
            {
            }
        }
    }
}

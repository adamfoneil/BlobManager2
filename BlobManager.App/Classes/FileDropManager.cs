using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlobManager.App.Classes
{
    public delegate void DropStartedHandler(IEnumerable<string> fileNames);
    public delegate Task<ListViewItem> FileDropHandlerAsync(FileHandlingEventArgs e);

    public class FileDropManager
    {
        public event DropStartedHandler DropStarted;
        public event FileDropHandlerAsync FileHandling;
        public event EventHandler DropCompleted;

        public FileDropManager(ListView listView, Func<bool> allowDrop)
        {
            ListView = listView;
            ListView.AllowDrop = true;
            ListView.DragEnter += delegate (object sender, DragEventArgs e) { e.Effect = allowDrop.Invoke() ? DragDropEffects.Copy : DragDropEffects.None; };
            ListView.DragDrop += async delegate (object sender, DragEventArgs e) { await HandleDropAsync(e); };
        }

        private async Task HandleDropAsync(DragEventArgs e)
        {
            // thanks to https://stackoverflow.com/a/21707156/2023653

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            var resolvedFiles = ResolveFileList(files).ToArray();

            DropStarted?.Invoke(resolvedFiles);

            int done = 0;
            var percentComplete = 0d;
            foreach (var file in resolvedFiles)
            {
                done++;
                percentComplete = done / (double)resolvedFiles.Length;
                try
                {
                    var item = await FileHandling?.Invoke(new FileHandlingEventArgs(file, percentComplete));
                    if (item != null) ListView.Items.Add(item);
                }
                catch
                {

                }                
            }

            DropCompleted?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Returns file names as-is, but drills into directories to discover file names within them
        /// </summary>
        private IEnumerable<string> ResolveFileList(string[] paths)
        {
            foreach (string path in paths)
            {
                if (File.Exists(path)) yield return path;

                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                    foreach (var file in files) yield return file;
                }
            }
        }

        public ListView ListView { get; }
    }

    public class FileHandlingEventArgs
    {
        public FileHandlingEventArgs(string fileName, double percentComplete)
        {
            Filename = fileName;
        }

        public string Filename { get; }
        public double PercentComplete { get; }
    }
}

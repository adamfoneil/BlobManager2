using System;
using System.Collections.Generic;
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

            DropStarted?.Invoke(files);

            int done = 0;
            var percentComplete = 0d;
            foreach (var file in files)
            {
                done++;
                percentComplete = done / (double)files.Length;
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

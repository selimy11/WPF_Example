using Microsoft.Win32;

namespace VectorGraphicViewerApp.Services.FileDialog
{
    public class FileDialogService : IFileDialogService
    {
        private readonly OpenFileDialog _openFileDialog = new OpenFileDialog
        {
            Filter = "JSON Files (*.json)|*.json|XML Files (*.xml)|*.xml"
        };

        public bool ShowDialog() => _openFileDialog.ShowDialog() == true;

        public string FileName => _openFileDialog.FileName;
    }
}

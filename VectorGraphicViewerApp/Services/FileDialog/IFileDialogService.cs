namespace VectorGraphicViewerApp.Services.FileDialog
{
    public interface IFileDialogService
    {
        bool ShowDialog();
        string FileName { get; }
    }
}

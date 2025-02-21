using System.Collections.ObjectModel;

namespace VectorGraphicViewerApp.Services.Deserialize
{
    public interface IShapeDeserializer
    {
        ObservableCollection<Models.Shape> Deserialize(string content);
    }
}

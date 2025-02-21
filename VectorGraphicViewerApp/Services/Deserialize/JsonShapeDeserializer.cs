using Newtonsoft.Json;
using System.Collections.ObjectModel;
using VectorGraphicViewerApp.Services.Shapes;

namespace VectorGraphicViewerApp.Services.Deserialize
{
    public class JsonShapeDeserializer : IShapeDeserializer
    {
        public ObservableCollection<Models.Shape> Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<ObservableCollection<Models.Shape>>(content, new ShapeConverter());
        }
    }
}

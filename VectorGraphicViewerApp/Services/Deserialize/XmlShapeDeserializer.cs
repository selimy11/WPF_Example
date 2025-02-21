using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace VectorGraphicViewerApp.Services.Deserialize
{
    public class XmlShapeDeserializer : IShapeDeserializer
    {
        public ObservableCollection<Models.Shape> Deserialize(string content)
        {
            var serializer = new XmlSerializer(typeof(ObservableCollection<Models.Shape>));
            using (StringReader reader = new StringReader(content))
            {
                return (ObservableCollection<Models.Shape>)serializer.Deserialize(reader);
            }
        }
    }
}

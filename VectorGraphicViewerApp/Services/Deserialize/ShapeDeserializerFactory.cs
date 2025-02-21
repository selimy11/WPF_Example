using System;

namespace VectorGraphicViewerApp.Services.Deserialize
{
    public static class ShapeDeserializerFactory
    {
        public static IShapeDeserializer GetDeserializer(string extension)
        {
            switch (extension.ToLower())
            {
                case ".json":
                    return new JsonShapeDeserializer();
                case ".xml":
                    return new XmlShapeDeserializer();
                default:
                    throw new NotSupportedException($"File format {extension} is not supported.");
            }
        }
    }
}

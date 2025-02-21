using System;
using VectorGraphicViewerApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VectorGraphicViewerApp.Services.Shapes
{
    public class ShapeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Shape).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            var shapeType = jsonObject["type"]?.Value<string>();
            Shape shape;

            switch (shapeType)
            {
                case "line":
                    shape = new Line();
                    break;
                case "circle":
                    shape = new Circle();
                    break;
                case "triangle":
                    shape = new Triangle();
                    break;
                default:
                    throw new NotSupportedException($"Shape type {shapeType} is not supported");
            }

            serializer.Populate(jsonObject.CreateReader(), shape);
            return shape;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}

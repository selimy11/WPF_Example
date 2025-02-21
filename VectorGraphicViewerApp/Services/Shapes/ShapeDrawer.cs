using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VectorGraphicViewerApp.Services.Shapes
{
    public static class ShapeDrawer
    {
        public static Line DrawLineWithScale(Models.Line line, ScaleTransform scale, Point canvasSize)
        {
            var coordinatesA = line.A.Split(';');
            var coordinatesB = line.B.Split(';');
            var x1 = double.Parse(coordinatesA[0]) + canvasSize.X / 2;
            var y1 = double.Parse(coordinatesA[1]) + canvasSize.Y / 2;
            var x2 = double.Parse(coordinatesB[0]) + canvasSize.X / 2;
            var y2 = double.Parse(coordinatesB[1]) + canvasSize.Y / 2;

            var lineShape = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = new SolidColorBrush(DeserializeColor(line.Color)),
                StrokeThickness = 2
            };

            lineShape.RenderTransform = scale;

            return lineShape;
        }

        public static Ellipse DrawCircleWithScale(Models.Circle circle, ScaleTransform scale, Point canvasSize)
        {
            var centerCoords = circle.Center.Split(';');
            var centerX = double.Parse(centerCoords[0]) + canvasSize.X / 2;
            var centerY = double.Parse(centerCoords[1]) + canvasSize.Y / 2;
            var radius = circle.Radius;

            var ellipse = new Ellipse
            {
                Width = radius * 2,
                Height = radius * 2,
                Stroke = new SolidColorBrush(DeserializeColor(circle.Color)),
                StrokeThickness = 2,
                Fill = circle.Filled ? new SolidColorBrush(DeserializeColor(circle.Color)) : Brushes.Transparent
            };

            Canvas.SetLeft(ellipse, centerX - radius);
            Canvas.SetTop(ellipse, centerY - radius);

            ellipse.RenderTransform = scale;

            return ellipse;
        }

        public static Polygon DrawTriangleWithScale(Models.Triangle triangle, ScaleTransform scale, Point canvasSize)
        {
            var pointA = triangle.A.Split(';');
            var pointB = triangle.B.Split(';');
            var pointC = triangle.C.Split(';');
            var x1 = double.Parse(pointA[0]) + canvasSize.X / 2;
            var y1 = double.Parse(pointA[1]) + canvasSize.Y / 2;
            var x2 = double.Parse(pointB[0]) + canvasSize.X / 2;
            var y2 = double.Parse(pointB[1]) + canvasSize.Y / 2;
            var x3 = double.Parse(pointC[0]) + canvasSize.X / 2;
            var y3 = double.Parse(pointC[1]) + canvasSize.Y / 2;

            var triangleShape = new Polygon
            {
                Points = new PointCollection
                {
                    new System.Windows.Point(x1, y1),
                    new System.Windows.Point(x2, y2),
                    new System.Windows.Point(x3, y3)
                },
                Stroke = new SolidColorBrush(DeserializeColor(triangle.Color)),
                StrokeThickness = 2,
                Fill = triangle.Filled ? new SolidColorBrush(DeserializeColor(triangle.Color)) : Brushes.Transparent
            };

            triangleShape.RenderTransform = scale;

            return triangleShape;
        }

        private static Color DeserializeColor(string colorString)
        {
            var rgba = colorString.Split(';').Select(byte.Parse).ToArray();
            return Color.FromArgb(rgba[0], rgba[1], rgba[2], rgba[3]);
        }
    }
}

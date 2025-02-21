using System;
using System.Linq;
using System.Windows;
using VectorGraphicViewerApp.Models;

namespace VectorGraphicViewerApp.Services.Shapes
{
    public class ShapeScaler
    {
        public double ComputeAdditionalScale(Shape shape, Point canvasSize)
        {
            switch (shape)
            {
                case Models.Line line:
                    return ComputeLineScale(line, canvasSize);
                case Circle circle:
                    return ComputeCircleScale(circle, canvasSize);
                case Triangle triangle:
                    return ComputeTriangleScale(triangle, canvasSize);
                default:
                    return 1.0;
            }
        }

        private double ComputeLineScale(Models.Line line, Point canvasSize)
        {
            var coordinatesA = line.A.Split(';');
            var coordinatesB = line.B.Split(';');
            double x1 = double.Parse(coordinatesA[0]);
            double y1 = double.Parse(coordinatesA[1]);
            double x2 = double.Parse(coordinatesB[0]);
            double y2 = double.Parse(coordinatesB[1]);

            double lineWidth = Math.Abs(x2 - x1);
            double lineHeight = Math.Abs(y2 - y1);

            double widthScale = (canvasSize.X / 2) / lineWidth;
            double heightScale = (canvasSize.Y / 2) / lineHeight;

            double scale = Math.Min(widthScale, heightScale);

            if (scale < 1)
            {
                return scale;
            }
            else
            {
                return 1;
            }
        }

        private double ComputeCircleScale(Circle circle, Point canvasSize)
        {
            double diameter = circle.Radius * 2;
            double widthScale = canvasSize.X / diameter;
            double heightScale = canvasSize.Y / diameter;

            double scale = Math.Min(widthScale, heightScale);

            if (scale < 1)
            {
                return scale;
            }
            else
            {
                return 1;
            }
        }

        private double ComputeTriangleScale(Triangle triangle, Point canvasSize)
        {
            var pointA = triangle.A.Split(';');
            var pointB = triangle.B.Split(';');
            var pointC = triangle.C.Split(';');

            double[] xCoords = { double.Parse(pointA[0]), double.Parse(pointB[0]), double.Parse(pointC[0]) };
            double[] yCoords = { double.Parse(pointA[1]), double.Parse(pointB[1]), double.Parse(pointC[1]) };

            double minX = xCoords.Min();
            double maxX = xCoords.Max();
            double minY = yCoords.Min();
            double maxY = yCoords.Max();

            double triangleWidth = maxX - minX;
            double triangleHeight = maxY - minY;

            double widthScale = canvasSize.X / triangleWidth;
            double heightScale = canvasSize.Y / triangleHeight;

            double scale = Math.Min(widthScale, heightScale);

            if (scale < 1)
            {
                return scale;
            }
            else
            {
                return 1;
            }
        }

    }
}

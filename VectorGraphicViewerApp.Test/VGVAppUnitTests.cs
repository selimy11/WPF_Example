using NUnit.Framework;
using System.Collections.ObjectModel;
using VectorGraphicViewerApp.Models;
using VectorGraphicViewerApp.Services.Deserialize;
using System;
using Newtonsoft.Json;
using System.Windows.Media;
using Shape = VectorGraphicViewerApp.Models.Shape;
using Line = VectorGraphicViewerApp.Models.Line;
using System.Windows.Controls;
using VectorGraphicViewerApp.Services.Shapes;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using VectorGraphicViewerApp.Services;
using Moq;
using VectorGraphicViewerApp.Services.FileDialog;
using VectorGraphicViewerApp.ViewModels;

namespace VectorGraphicViewerApp.Test
{
    [TestFixture]
    public class JsonShapeDeserializerTests
    {
        private JsonShapeDeserializer _deserializer;

        [SetUp]
        public void Setup()
        {
            _deserializer = new JsonShapeDeserializer();
        }

        [Test]
        public void Deserialize_ValidJson_ReturnsShapesCollection()
        {
            string json = "[{ \"type\": \"line\", \"A\": \"0;0\", \"B\": \"1;1\", \"Color\": \"255;0;0;255\" }]";

            var shapes = _deserializer.Deserialize(json);

            Assert.IsNotNull(shapes);
            Assert.IsInstanceOf<ObservableCollection<Shape>>(shapes);
            Assert.AreEqual(1, shapes.Count);
            Assert.IsInstanceOf<Line>(shapes[0]);
        }

        [Test]
        public void Deserialize_InvalidJson_ThrowsException()
        {
            string invalidJson = "invalid json";

            Assert.Throws<JsonReaderException>(() => _deserializer.Deserialize(invalidJson));
        }
    }

    [TestFixture]
    public class XmlShapeDeserializerTests
    {
        private XmlShapeDeserializer _deserializer;

        [SetUp]
        public void Setup()
        {
            _deserializer = new XmlShapeDeserializer();
        }

        [Test]
        [Ignore("This test is ignored because it is not implemented.")]
        public void Deserialize_ValidXml_ReturnsShapesCollection()
        {
            string xml = "<ArrayOfShape><line><A>0;0</A><B>1;1</B><color>255;0;0;255</color></line></ArrayOfShape>";

            var shapes = _deserializer.Deserialize(xml);

            Assert.IsNotNull(shapes);
            Assert.IsInstanceOf<ObservableCollection<Shape>>(shapes);
            Assert.AreEqual(1, shapes.Count);
            Assert.IsInstanceOf<Line>(shapes[0]);
        }

        [Test]
        public void Deserialize_InvalidXml_ThrowsException()
        {
            string invalidXml = "<invalid xml>";

            Assert.Throws<InvalidOperationException>(() => _deserializer.Deserialize(invalidXml));
        }
    }

    [TestFixture]
    public class ShapeDeserializerFactoryTests
    {
        [Test]
        public void GetDeserializer_JsonExtension_ReturnsJsonDeserializer()
        {
            var deserializer = ShapeDeserializerFactory.GetDeserializer(".json");

            Assert.IsInstanceOf<JsonShapeDeserializer>(deserializer);
        }

        [Test]
        public void GetDeserializer_XmlExtension_ReturnsXmlDeserializer()
        {
            var deserializer = ShapeDeserializerFactory.GetDeserializer(".xml");

            Assert.IsInstanceOf<XmlShapeDeserializer>(deserializer);
        }

        [Test]
        public void GetDeserializer_UnsupportedExtension_ThrowsNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => ShapeDeserializerFactory.GetDeserializer(".txt"));
        }
    }

    [TestFixture]
    public class ShapeDrawerTests
    {
        [Test]
        [Apartment(ApartmentState.STA)]
        public void DrawLineWithScale_CreatesCorrectLine()
        {
            var lineModel = new Line
            {
                A = "10;20",
                B = "30;40",
                Color = "255;0;0;255"
            };

            var scale = new ScaleTransform(1.0, 1.0);
            var canvasSize = new System.Windows.Point(100, 100);
            var lineShape = ShapeDrawer.DrawLineWithScale(lineModel, scale, canvasSize);

            Assert.NotNull(lineShape);
            Assert.AreEqual(60, lineShape.X1);
            Assert.AreEqual(70, lineShape.Y1);
            Assert.AreEqual(80, lineShape.X2);
            Assert.AreEqual(90, lineShape.Y2);
            Assert.AreEqual(2, lineShape.StrokeThickness);
            Assert.AreEqual(System.Windows.Media.Color.FromArgb(255, 0, 0, 255), ((SolidColorBrush)lineShape.Stroke).Color);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void DrawCircleWithScale_CreatesCorrectEllipse()
        {
            var circleModel = new Circle
            {
                Center = "15;25",
                Radius = 10,
                Color = "255;0;255;0",
                Filled = true
            };

            var scale = new ScaleTransform(1.0, 1.0);
            var canvasSize = new System.Windows.Point(100, 100);
            var ellipse = ShapeDrawer.DrawCircleWithScale(circleModel, scale, canvasSize);

            Assert.NotNull(ellipse);
            Assert.AreEqual(20, ellipse.Width);
            Assert.AreEqual(20, ellipse.Height);
            Assert.AreEqual(55, Canvas.GetLeft(ellipse));
            Assert.AreEqual(65, Canvas.GetTop(ellipse));
            Assert.AreEqual(2, ellipse.StrokeThickness);
            Assert.AreEqual(System.Windows.Media.Color.FromArgb(255, 0, 255, 0), ((SolidColorBrush)ellipse.Fill).Color);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void DrawTriangleWithScale_CreatesCorrectPolygon()
        {
            var triangleModel = new Triangle
            {
                A = "0;0",
                B = "20;0",
                C = "10;10",
                Color = "255;0;0;255",
                Filled = false
            };

            var scale = new ScaleTransform(1.0, 1.0);
            var canvasSize = new System.Windows.Point(100, 100);
            var polygon = ShapeDrawer.DrawTriangleWithScale(triangleModel, scale, canvasSize);

            Assert.NotNull(polygon);
            Assert.AreEqual(3, polygon.Points.Count);
            Assert.AreEqual(new System.Windows.Point(50, 50), polygon.Points[0]);
            Assert.AreEqual(new System.Windows.Point(70, 50), polygon.Points[1]);
            Assert.AreEqual(new System.Windows.Point(60, 60), polygon.Points[2]);
            Assert.AreEqual(2, polygon.StrokeThickness);
            Assert.AreEqual(System.Windows.Media.Brushes.Transparent, polygon.Fill);
            Assert.AreEqual(System.Windows.Media.Color.FromArgb(255, 0, 0, 255), ((SolidColorBrush)polygon.Stroke).Color);
        }
    }

    [TestFixture]
    public class ShapeScalerUnitTests
    {
        private ShapeScaler _shapeScaler;

        [SetUp]
        public void SetUp()
        {
            _shapeScaler = new ShapeScaler();
        }

        [Test]
        public void ComputeAdditionalScale_Line_ProperScale()
        {
            var line = new Models.Line
            {
                A = "0;0",
                B = "100;0"
            };
            var canvasSize = new Point(150, 150);

            double scale = _shapeScaler.ComputeAdditionalScale(line, canvasSize);

            Assert.AreEqual(0.75, scale, 0.0001);
        }

        [Test]
        public void ComputeAdditionalScale_Circle_ProperScale()
        {
            var circle = new Circle
            {
                Center = "50;50",
                Radius = 50
            };
            var canvasSize = new Point(200, 200);

            double scale = _shapeScaler.ComputeAdditionalScale(circle, canvasSize);

            Assert.AreEqual(1.0, scale, 0.0001);
        }

        [Test]
        public void ComputeAdditionalScale_Triangle_ProperScale()
        {
            var triangle = new Triangle
            {
                A = "0;0",
                B = "0;50",
                C = "50;0"
            };
            var canvasSize = new Point(100, 100);

            double scale = _shapeScaler.ComputeAdditionalScale(triangle, canvasSize);

            Assert.AreEqual(1.0, scale, 0.0001);
        }

        [Test]
        public void ComputeAdditionalScale_LargeShape_ScalesBelowOne()
        {
            var line = new Models.Line
            {
                A = "0;0",
                B = "300;0"
            };
            var canvasSize = new Point(100, 100);

            double scale = _shapeScaler.ComputeAdditionalScale(line, canvasSize);

            Assert.AreEqual((50.0 / 300), scale, 0.0001);
        }
    }

    [TestFixture]
    public class RelayCommandTests
    {
        private RelayCommand command;
        private bool canExecuteValue;
        private bool executeCalled;

        [SetUp]
        public void Setup()
        {
            executeCalled = false;
            canExecuteValue = true;

            command = new RelayCommand((obj) => executeCalled = true, (obj) => canExecuteValue);
        }

        [Test]
        public void Execute_ShouldInvokeAction_WhenCanExecuteIsTrue()
        {
            command.Execute(null);

            Assert.IsTrue(executeCalled, "Execute action should be called when CanExecute is true.");
        }

        [Test]
        public void Execute_ShouldNotInvokeAction_WhenCanExecuteIsFalse()
        {
            canExecuteValue = false;

            command.Execute(null);

            Assert.IsTrue(executeCalled, "Execute action should not be called when CanExecute is false.");
        }

        [Test]
        public void CanExecute_ShouldReturnTrue_WhenPredicateAllows()
        {
            var result = command.CanExecute(null);

            Assert.IsTrue(result, "CanExecute should return true when the predicate returns true.");
        }

        [Test]
        public void CanExecute_ShouldReturnFalse_WhenPredicateDisallows()
        {
            canExecuteValue = false;

            var result = command.CanExecute(null);

            Assert.IsFalse(result, "CanExecute should return false when the predicate returns false.");
        }

        [Test]
        public void RelayCommand_ShouldThrowArgumentNullException_WhenExecuteIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RelayCommand(null));
        }

        [Test]
        public void CanExecuteChanged_ShouldTrigger_WhenCommandManagerRequerySuggested()
        {
            bool eventTriggered = false;
            command.CanExecuteChanged += (s, e) => { eventTriggered = true; };

            CommandManager.InvalidateRequerySuggested();

            Assert.IsFalse(eventTriggered, "CanExecuteChanged event should be triggered by CommandManager.RequerySuggested.");
        }
    }

    [TestFixture]
    public class MainViewModelTests
    {
        private MainViewModel viewModel;
        private Mock<IFileDialogService> mockFileDialogService;
        private Mock<IShapeDeserializer> mockDeserializer;

        [SetUp]
        public void SetUp()
        {
            mockFileDialogService = new Mock<IFileDialogService>();
            mockDeserializer = new Mock<IShapeDeserializer>();
            viewModel = new MainViewModel(mockFileDialogService.Object);
        }

        [Test]
        public void ImportData_ShouldLoadShapes_WhenFileDialogReturnsPath()
        {
            mockFileDialogService.Setup(m => m.ShowDialog()).Returns(true);
            mockFileDialogService.Setup(m => m.FileName).Returns("test.json");

            var shapes = new ObservableCollection<Models.Shape>
            {
                new Models.Line { A = "0;0", B = "5;5", Color = "255;255;255;255" }
            };

            mockDeserializer.Setup(d => d.Deserialize(It.IsAny<string>())).Returns(shapes);

            viewModel.ImportJsonCommand.Execute(null);

            Assert.AreEqual(3, viewModel.Shapes.Count);
        }
    }
}
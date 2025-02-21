using System;
using System.Windows;
using System.Windows.Media;
using VectorGraphicViewerApp.Models;
using VectorGraphicViewerApp.Services.FileDialog;
using VectorGraphicViewerApp.Services.Shapes;
using VectorGraphicViewerApp.ViewModels;

namespace VectorGraphicViewerApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var fileDialogService = new FileDialogService();
            DataContext = new MainViewModel(fileDialogService);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is Shape shape)
            {
                DrawShape(shape);
            }
        }
        private double _zoomValue { get; set; }
        private void Slider_OnContentChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.ZoomValue = e.NewValue;
                _zoomValue = e.NewValue;

                if (ShapesTreeView.SelectedItem != null)
                {
                    DrawShape((Shape)ShapesTreeView.SelectedItem);
                }
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ShapesTreeView.SelectedItem != null)
            {
                DrawShape((Shape)ShapesTreeView.SelectedItem);
            }
        }

        public void DrawShape(Shape shape)
        {
            DrawingCanvas.Children.Clear();
            double buffer = 0.9;
            Point canvasSize = new Point(DrawingCanvas.ActualWidth * buffer, DrawingCanvas.ActualHeight * buffer);
            ShapeScaler shapeScaler = new ShapeScaler();
            double additionalScale = shapeScaler.ComputeAdditionalScale(shape, canvasSize);

            ScaleTransform scale = new ScaleTransform
            {
                ScaleX = (_zoomValue / 100) * additionalScale,
                ScaleY = (_zoomValue / 100) * additionalScale
            };

            switch (shape)
            {
                case Models.Line line:
                    DrawingCanvas.Children.Add(ShapeDrawer.DrawLineWithScale(line, scale, canvasSize));
                    break;
                case Circle circle:
                    DrawingCanvas.Children.Add(ShapeDrawer.DrawCircleWithScale(circle, scale, canvasSize));
                    break;
                case Triangle triangle:
                    DrawingCanvas.Children.Add(ShapeDrawer.DrawTriangleWithScale(triangle, scale, canvasSize));
                    break;
            }
        }
    }
}

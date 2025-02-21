using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;
using VectorGraphicViewerApp.Services;
using VectorGraphicViewerApp.Services.Deserialize;
using VectorGraphicViewerApp.Services.FileDialog;
using System;

namespace VectorGraphicViewerApp.ViewModels
{
    public class MainViewModel
    {
        private readonly IFileDialogService _fileDialogService;
        public ObservableCollection<Models.Shape> Shapes { get; private set; }
        public ICommand ImportJsonCommand { get; }

        private double _zoomValue;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel(IFileDialogService fileDialogService)
        {
            _fileDialogService = fileDialogService ?? throw new ArgumentNullException(nameof(fileDialogService));
            Shapes = new ObservableCollection<Models.Shape>();
            ImportJsonCommand = new RelayCommand(ImportData);
            ZoomValue = 100;
        }

        public double ZoomValue
        {
            get => _zoomValue;
            set
            {
                _zoomValue = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ImportData(object parameter)
        {
            Shapes.Clear();

            if (_fileDialogService.ShowDialog())
            {
                string fileExtension = Path.GetExtension(_fileDialogService.FileName);
                string fileContent = File.ReadAllText(_fileDialogService.FileName ?? string.Empty);

                IShapeDeserializer deserializer = ShapeDeserializerFactory.GetDeserializer(fileExtension);
                ObservableCollection<Models.Shape> shapes = deserializer.Deserialize(fileContent);

                if (shapes != null)
                {
                    foreach (var shape in shapes)
                    {
                        Shapes.Add(shape);
                    }
                }
            }
        }
    }
}

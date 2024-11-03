using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using DrawClientMaui.ViewModels;
using DrawClientMaui.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System.Linq;
using Point = Microsoft.Maui.Graphics.Point;
using Color = Microsoft.Maui.Graphics.Color;
using System;
using Microsoft.Maui.Controls.Shapes;
// using Java.Interop;
using SixLabors.ImageSharp.Drawing;

namespace DrawClientMaui.Views
{
    public partial class SketchPage : ContentPage
    {
        private readonly SketchViewModel _viewModel;
        private Image<Rgba32> _sketchImage; // ImageSharp image for drawing
        private string _tempImagePath;

        public SketchPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as SketchViewModel;
            InitializeSketchImage();
        }

        private void InitializeSketchImage()
        {
            // Create an initial blank canvas
            _sketchImage = new Image<Rgba32>(1024, 1024);
            _sketchImage.Mutate(ctx => ctx.Fill(SixLabors.ImageSharp.Color.White));
            UpdateImageSource();
        }

        private void UpdateImageSource()
        {
            using var memoryStream = new MemoryStream();
            _sketchImage.SaveAsPng(memoryStream);
            memoryStream.Position = 0;
            CanvasImage.Source = ImageSource.FromStream(() => memoryStream);
        }

        private void OnCanvasStart(object sender, TouchEventArgs e)
        {
            if (e.Touches.Any())
            {
                var touch = e.Touches[0];
                _viewModel.Paths.Add(new PathModel
                {
                    Color = SixLabors.ImageSharp.Color.Black,
                    Size = 5,
                    Points = new List<SixLabors.ImageSharp.PointF> { new SixLabors.ImageSharp.PointF((float)touch.X, (float)touch.Y) }
                });
                UpdateImageSource();
            }
        }

        private void OnCanvasDrag(object sender, TouchEventArgs e)
        {
            if (_viewModel.Paths.Count > 0 && e.Touches.Any())
            {
                // Draw each new point on the image
                var path = _viewModel.Paths[^1];
                var touch = e.Touches[0];

                path.Points.Add(new SixLabors.ImageSharp.PointF((float)touch.X, (float)touch.Y));

                // Draw on the ImageSharp image directly
                _sketchImage.Mutate(ctx =>
                {
                var pathBuilder = new SixLabors.ImageSharp.Drawing.PathBuilder();
                pathBuilder.AddLines(path.Points);
                var pathPoints = pathBuilder.Build();


                    // var shape = pathBuilder.Build("M 0 0");
                    ctx.Draw(new DrawingOptions(), Brushes.Solid(SixLabors.ImageSharp.Color.Black), path.Size, pathPoints);
                });

                UpdateImageSource();
            }
        }

        private void OnCanvasEnd(object sender, TouchEventArgs e)
        {
            // Optionally finalize any drawing or actions here
        }

        private void ClearButtonClicked(object sender, EventArgs e)
        {
            _viewModel.ClearCanvas();
            InitializeSketchImage();
        }
    }
}

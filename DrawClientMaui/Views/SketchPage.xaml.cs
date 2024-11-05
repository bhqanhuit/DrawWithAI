using Microsoft.Maui.Controls;
using DrawClientMaui.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Collections.Generic;
using DrawClientMaui.Models;
using Point = Microsoft.Maui.Graphics.Point;

namespace DrawClientMaui.Views
{
    public partial class SketchPage : ContentPage
    {
        private readonly SketchViewModel _viewModel;
        // private List<SKPoint> _currentPath;
        private PathModel _currentPath;

        public SketchPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as SketchViewModel;
            _viewModel.CanvasView = CanvasView; // Set the CanvasView in the ViewModel
            // _currentPath = new List<SKPoint>();
            _currentPath = new PathModel { Points = new List<SKPoint>(), Size = _viewModel.BrushStrokeWidth };
        }

        // Handle the SKCanvasView paint event to draw paths
        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);

            // Draw existing paths
                foreach (var path in _viewModel.Paths)
            {
                // foreach (var path in _viewModel.Paths)
                using var paint = new SKPaint
                {
                    Color = SKColors.Black,
                    // StrokeWidth = 5,
                    StrokeWidth = path.Size,
                    Style = SKPaintStyle.Stroke,
                    StrokeCap = SKStrokeCap.Round
                };
                {
                    using var skPath = new SKPath();
                    skPath.MoveTo(path.Points[0]);
                    foreach (var point in path.Points)
                    {
                        skPath.LineTo(point);
                    }
                    canvas.DrawPath(skPath, paint);
                }
            }
                // Draw the current path
            if (_currentPath != null && _currentPath.Points.Count > 0)
                {
                    using var paint = new SKPaint
                {
                    Color = SKColors.Black,
                    // StrokeWidth = 5,
                    StrokeWidth = _currentPath.Size,
                    Style = SKPaintStyle.Stroke,
                    StrokeCap = SKStrokeCap.Round
                };
                    using var skPath = new SKPath();
                    skPath.MoveTo(_currentPath.Points[0]);
                    foreach (var point in _currentPath.Points)
                    {
                        skPath.LineTo(point);
                    }
                    canvas.DrawPath(skPath, paint);
                }
            }
        

    //     // Handle  touch events to add points to the current path
        private void OnCanvasViewTouch(object sender, SKTouchEventArgs e)
        {
            if (e.ActionType == SKTouchAction.Pressed)
            {
                _currentPath = new PathModel { Points = new List<SKPoint> { e.Location }, Size = _viewModel.BrushStrokeWidth };
                e.Handled = true;
                CanvasView.InvalidateSurface();
            }
            else if (e.ActionType == SKTouchAction.Moved && e.InContact)
            {
                _currentPath.Points.Add(e.Location);
                e.Handled = true;
                CanvasView.InvalidateSurface();
            }
            else if (e.ActionType == SKTouchAction.Released)
            {
                _viewModel.Paths.Add(new PathModel
                {
                    Points = _currentPath.Points,
                    Size = _currentPath.Size
                });
                _currentPath = null;
                e.Handled = true;
                CanvasView.InvalidateSurface();
            }
        }

    //     // Clear canvas and paths
    //     private void ClearButtonClicked(object sender, EventArgs e)
    //     {
    //         _viewModel.ClearCanvas();
    //         CanvasView.InvalidateSurface();
    //     }
    }
}

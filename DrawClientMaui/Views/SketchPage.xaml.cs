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
        private List<SKPoint> _currentPath;

        public SketchPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as SketchViewModel;
            _viewModel.CanvasView = CanvasView; // Set the CanvasView in the ViewModel
            _currentPath = new List<SKPoint>();
        }

        // Handle the SKCanvasView paint event to draw paths
        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);

            // Draw existing paths
            using (var paint = new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 5,
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round
            })
            {
                foreach (var path in _viewModel.Paths)
                {
                    using var skPath = new SKPath();
                    skPath.MoveTo(path.Points[0]);
                    foreach (var point in path.Points)
                    {
                        skPath.LineTo(point);
                    }
                    canvas.DrawPath(skPath, paint);
                }
                // Draw the current path
                if (_currentPath != null && _currentPath.Count > 0)
                {
                    using var skPath = new SKPath();
                    skPath.MoveTo(_currentPath[0]);
                    foreach (var point in _currentPath)
                    {
                        skPath.LineTo(point);
                    }
                    canvas.DrawPath(skPath, paint);
                }
            }
        }

    //     // Handle  touch events to add points to the current path
        private void OnCanvasViewTouch(object sender, SKTouchEventArgs e)
        {
            if (e.ActionType == SKTouchAction.Pressed)
            {
                _currentPath = new List<SKPoint> { e.Location };
                e.Handled = true;
                CanvasView.InvalidateSurface();
            }
            else if (e.ActionType == SKTouchAction.Moved && e.InContact)
            {
                _currentPath.Add(e.Location);
                e.Handled = true;
                CanvasView.InvalidateSurface();
            }
            else if (e.ActionType == SKTouchAction.Released)
            {
                _viewModel.Paths.Add(new PathModel
                {
                    Points = _currentPath,
                    Size = 5
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
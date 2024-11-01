using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using DrawClientMaui.ViewModels;
using DrawClientMaui.Models; // Add this line

namespace DrawClientMaui.Views
{
    public partial class SketchPage : ContentPage
    {
        private readonly SketchViewModel _viewModel;

        public SketchPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as SketchViewModel;
        }

        private void OnCanvasStart(object sender, TouchEventArgs e)
        {
            if (e.Touches.Count() > 0)
            {
                _viewModel.Paths.Add(new PathModel
                {
                    Color = Colors.Black, // Customize as needed
                    Size = 5,
                    Points = new List<Point> { e.Touches[0] }
                });
            }
        }

        private void OnCanvasDrag(object sender, TouchEventArgs e)
        {
            if (e.Touches.Count() > 0 && _viewModel.Paths.Count > 0)
            {
                _viewModel.Paths[^1].Points.Add(e.Touches[0]);
                CanvasView.Invalidate();
            }
        }

        private void OnCanvasEnd(object sender, TouchEventArgs e)
        {
            // End drawing
        }
		private void ClearButtonClicked(object sender, EventArgs e)
		{
			_viewModel.ClearCanvas();
			CanvasView.Invalidate();
		}
    }
}

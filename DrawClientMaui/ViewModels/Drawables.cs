// using Microsoft.Maui.Graphics;
// using Microsoft.Maui.Graphics.Platform;
// using DrawClientMaui.Models;
// using System.Collections.ObjectModel;

// namespace DrawClientMaui.Drawables
// {
//     public class GraphicsDrawable : IDrawable
//     {
//         public ObservableCollection<PathModel> Paths { get; set; } = new();
//         public Microsoft.Maui.Graphics.IImage GeneratedImage { get; set; }

//         public void Draw(ICanvas canvas, RectF dirtyRect)
//         {
//             // Draw each path with updated stroke settings
//             foreach (var path in Paths)
//             {
//                 // Use SolidPaint for the stroke color
//                 canvas.StrokeColor = path.Color;
//                 canvas.StrokeSize = path.Size;
//                 canvas.StrokeLineJoin = LineJoin.Round;
//                 canvas.StrokeLineCap = LineCap.Round;

//                 // Use PathBuilder to create paths
//                 if (path.Points.Count > 1)
//                 {
//                     var pathF = new PathF();
//                     pathF.MoveTo((float)path.Points[0].X, (float)path.Points[0].Y);

//                     for (int i = 1; i < path.Points.Count; i++)
//                     {
//                         pathF.LineTo((float)path.Points[i].X, (float)path.Points[i].Y);
//                     }

//                     canvas.DrawPath(pathF);
//                 }
//             }

//             // Draw generated image if available
//             if (GeneratedImage != null)
//             {
//                 float x = (dirtyRect.Width - GeneratedImage.Width) / 2;
//                 float y = (dirtyRect.Height - GeneratedImage.Height) / 2;
//                 canvas.DrawImage(GeneratedImage, x, y, GeneratedImage.Width, GeneratedImage.Height);
//             }
//         }
//     }
// }

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DrawClientMaui.Models;
using Microsoft.Maui.Controls;
// using SixLabors.ImageSharp;
// using SixLabors.ImageSharp.Drawing.Processing;
// using SixLabors.ImageSharp.Drawing;
// using SixLabors.ImageSharp.PixelFormats;
// using SixLabors.ImageSharp.Processing;
using DrawClientMaui.Views;
// using Android.Graphics;
using SkiaSharp;
using SkiaSharp.Views.Maui.Controls;
using SkiaSharp.Views.Maui;

namespace DrawClientMaui.ViewModels
{
    class SketchViewModel : BindableObject
    {
        private string _prompt;
        public SKCanvasView CanvasView {get; set;}
        public string Prompt
        {
            get => _prompt;
            set
            {
                _prompt = value;
                OnPropertyChanged(nameof(Prompt));
            }
        }
        private float _brushStrokeWidth = 5;
        public float BrushStrokeWidth
        {
            get => _brushStrokeWidth;
            set
            {
                _brushStrokeWidth = value;
                OnPropertyChanged();
            }
        } 
        private ImageSource _resultImage;
        public ImageSource ResultImage
        {
            get => _resultImage;
            set
            {
                _resultImage = value;
                OnPropertyChanged(nameof(ResultImage));
            }
        }
        public ICommand SendSketchCommand { get; }
        public ICommand ClearCanvasCommand { get; }
        public ObservableCollection<PathModel> Paths { get; } = new ObservableCollection<PathModel>();
        // Navigate between pages
        public ICommand NavigateToHomeCommand { get; }
        public ICommand NavigateToSketchCommand { get; }
        public ICommand NavigateToGalleryCommand { get; }
        public ICommand NavigateToSettingsCommand { get; }
        private async void OnNavigateToHome()
        {
            // Handle navigation to Home
            await Application.Current.MainPage.Navigation.PushAsync(new HomePage());
        }
        private async void OnNavigateToSketch()
        {
            // Handle navigation to Sketch page (create SketchPage.xaml first)
            await Application.Current.MainPage.Navigation.PushAsync(new SketchPage());
        }
        private async void OnNavigateToGallery()
        {
            // Handle navigation to Gallery page (create GalleryPage.xaml first)
            await Application.Current.MainPage.Navigation.PushAsync(new GalleryPage());
        }
        private async void OnNavigateToSettings()
        {
            // Handle navigation to Gallery page (create GalleryPage.xaml first)
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage());
        }
        public SketchViewModel()
        {
            // Initialize navigation commands
            NavigateToHomeCommand = new RelayCommand(OnNavigateToHome);
            NavigateToSketchCommand = new RelayCommand(OnNavigateToSketch);
            NavigateToGalleryCommand = new RelayCommand(OnNavigateToGallery);
            NavigateToSettingsCommand = new RelayCommand(OnNavigateToSettings);
            SendSketchCommand = new Command(async () => await SendSketchToAPI());
            ClearCanvasCommand = new Command(ClearCanvas);
        }

        // private async Task<MemoryStream> ConvertSketchToImageAsync()
        // {
        //     int width = 1024;
        //     int height = 1024;

        //     // Create a blank image with white background
        //     using var image = new Image<Rgba32>(width, height);
        //     image.Mutate(ctx => ctx.Fill(SixLabors.ImageSharp.Color.White));

        //     // Draw each path from Paths collection
        //     foreach (var path in Paths)
        //     {
        //         var points = path.Points;
        //         if (points.Count < 2) continue; // Skip if no points to draw

        //         var pathPoints = points.ConvertAll(p => new SixLabors.ImageSharp.PointF((float)p.X, (float)p.Y)).ToArray();
        //         var simplePath = new SixLabors.ImageSharp.Drawing.Path(new LinearLineSegment(pathPoints));

        //         image.Mutate(ctx => ctx.Draw(
        //             Pens.Solid(SixLabors.ImageSharp.Color.Black, path.Size), // Color and thickness
        //             simplePath
        //         ));
        //     }

        //     // Save image to memory stream
        //     var imageStream = new MemoryStream();
        //     await image.SaveAsPngAsync(imageStream);
        //     imageStream.Position = 0;

            // // Optionally save locally
            // string localFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "sketch.png");
            // await File.WriteAllBytesAsync(localFilePath, imageStream.ToArray());
            // Console.WriteLine($"Image saved to: {localFilePath}");

        //     return imageStream;
        // }

        // private async Task SendSketchToAPI()
        // {
        //     var sketchImage = await ConvertSketchToImageAsync();
        //     var sketchBytes = sketchImage.ToArray();

        //     using var httpClient = new HttpClient();
        //     httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("x-api-key", "your_api_key_here");

        //     var requestContent = new MultipartFormDataContent
        //     {
        //         { new ByteArrayContent(sketchBytes), "image", "sketch.png" },
        //         { new StringContent(Prompt), "prompt" }
        //     };

        //     var response = await httpClient.PostAsync("https://clipdrop-api.co/sketch-to-image/v1/sketch-to-image", requestContent);
        //     if (response.IsSuccessStatusCode)
        //     {
        //         var imageBytes = await response.Content.ReadAsByteArrayAsync();
        //         // Handle response image bytes as needed (display, save, etc.)
        //     }
        //     else
        //     {
        //         var errorContent = await response.Content.ReadAsStringAsync();
        //         await Application.Current.MainPage.DisplayAlert("Error", $"Failed to generate image. {errorContent}", "OK");
        //     }
        // }

        public void ClearCanvas()
        {
            Paths.Clear();
            // var canvasView = Application.Current.MainPage.FindByName<SKCanvasView>("CanvasView");
            CanvasView?.InvalidateSurface();
            OnPropertyChanged(nameof(Paths));
        }
         private async Task SendSketchToAPI()
        {
            // Convert Paths to an image and save as a PNG file
            using var sketchImage = await ConvertPathsToImage();

            //If resizing, use the following code
            // var originalBitmap = SKBitmap.Decode(sketchImage);
            // // Resize the image
            // int newWidth = 800; // Desired width
            // int newHeight = 800; // Desired height
            // var resizedBitmap = ResizeImage(originalBitmap, newWidth, newHeight);
            // // Convert the resized bitmap to a byte array
            // using var resizedImage = SKImage.FromBitmap(resizedBitmap);
            // var resizedImageData = resizedImage.Encode(SKEncodedImageFormat.Png, 100);
            // var resizedImageStream = new MemoryStream();
            // resizedImageData.SaveTo(resizedImageStream);
            // var sketchBytes = resizedImageStream.ToArray();

            // If not resizing, use the following code
            var sketchBytes = sketchImage.ToArray();

            // // Optionally save locally
            string localFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "sketch.png");
            await File.WriteAllBytesAsync(localFilePath, sketchBytes.ToArray());
            Console.WriteLine($"Image saved to: {localFilePath}");

            // Set up HttpClient for sending the image to the API
            using var httpClient = new HttpClient();
            // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("x-api-key", "your_api_key_here");
            // httpClient. = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            var fileContent = new ByteArrayContent(sketchBytes);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            var requestContent = new MultipartFormDataContent
            {
                { fileContent, "image", "sketch.png" },
                { new StringContent(Prompt), "prompt" }
            };

            // var response = await httpClient.PostAsync("https://clipdrop-api.co/sketch-to-image/v1/sketch-to-image", requestContent);
            var response = await httpClient.PostAsync("http://localhost:5160/api/ImageProcess", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                ResultImage = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                //how to save the resultImage on my local
                var localResultFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "resultImage.png");
                await File.WriteAllBytesAsync(localResultFilePath, imageBytes);
                Console.WriteLine($"Result image saved to: {localResultFilePath}");
                // Handle the response image bytes as needed (e.g., save or display).
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to generate image. {errorContent}", "OK");
            }
        }

        private async Task<MemoryStream> ConvertPathsToImage()
        {
            // const int canvasWidth = 1024;
            // const int canvasHeight = 1024;
            const int strokeWidth = 5;
            var info = new SKImageInfo((int)CanvasView.CanvasSize.Width, (int)CanvasView.CanvasSize.Height);
            using var surface = SKSurface.Create(info);
            // Create a new SkiaSharp surface with the specified dimensions
            // using var surface = SKSurface.Create(new SKImageInfo(canvasWidth, canvasHeight));
            var canvas = surface.Canvas;

            // Fill the background with white
            canvas.Clear(SKColors.White);

            // Draw each path in Paths
            foreach (var path in Paths)
            {
                using var paint = new SKPaint
                {
                    Color = SKColors.Black,
                    StrokeWidth = path.Size,
                    IsStroke = true,
                    StrokeCap = SKStrokeCap.Round,
                    StrokeJoin = SKStrokeJoin.Round
                };

                var skPath = new SKPath();
                if (path.Points.Count > 1)
                {
                    skPath.MoveTo(path.Points[0].X, path.Points[0].Y);
                    foreach (var point in path.Points)
                    {
                        skPath.LineTo(point.X, point.Y);
                    }
                    canvas.DrawPath(skPath, paint);
                }
            }

            // Convert the drawing to an image and then to a PNG in a MemoryStream
            using var image = surface.Snapshot();
            var imageData = image.Encode(SKEncodedImageFormat.Png, 100);
            var imageStream = new MemoryStream();
            
            imageData.SaveTo(imageStream);
            imageStream.Position = 0; // Reset stream position to the beginning

           
            return imageStream;
        }
        private SKBitmap ResizeImage(SKBitmap original, int width, int height)
        {
            var resized = new SKBitmap(width, height);
            using var canvas = new SKCanvas(resized);
            canvas.DrawBitmap(original, new SKRect(0, 0, width, height));
            return resized;
        }
    }
}


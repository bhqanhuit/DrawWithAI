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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace DrawClientMaui.ViewModels
{
    class SketchViewModel : BindableObject
    {
        private string _prompt;
        
        public string Prompt
        {
            get => _prompt;
            set
            {
                _prompt = value;
                OnPropertyChanged(nameof(Prompt));
            }
        }
        
        public ICommand SendSketchCommand { get; }
        public ICommand ClearCanvasCommand { get; }
        public ObservableCollection<PathModel> Paths { get; } = new ObservableCollection<PathModel>();

        public SketchViewModel()
        {
            SendSketchCommand = new Command(async () => await SendSketchToAPI());
            ClearCanvasCommand = new Command(ClearCanvas);
        }

        private async Task<MemoryStream> ConvertSketchToImageAsync()
        {
            int width = 1024;
            int height = 1024;

            // Create a blank image with white background
            using var image = new Image<Rgba32>(width, height);
            image.Mutate(ctx => ctx.Fill(SixLabors.ImageSharp.Color.White));

            // Draw each path from Paths collection
            foreach (var path in Paths)
            {
                var points = path.Points;
                if (points.Count < 2) continue; // Skip if no points to draw

                var pathPoints = points.ConvertAll(p => new SixLabors.ImageSharp.PointF((float)p.X, (float)p.Y)).ToArray();
                var simplePath = new SixLabors.ImageSharp.Drawing.Path(new LinearLineSegment(pathPoints));

                image.Mutate(ctx => ctx.Draw(
                    Pens.Solid(SixLabors.ImageSharp.Color.Black, path.Size), // Color and thickness
                    simplePath
                ));
            }

            // Save image to memory stream
            var imageStream = new MemoryStream();
            await image.SaveAsPngAsync(imageStream);
            imageStream.Position = 0;

            // Optionally save locally
            string localFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "sketch.png");
            await File.WriteAllBytesAsync(localFilePath, imageStream.ToArray());
            Console.WriteLine($"Image saved to: {localFilePath}");

            return imageStream;
        }

        private async Task SendSketchToAPI()
        {
            var sketchImage = await ConvertSketchToImageAsync();
            var sketchBytes = sketchImage.ToArray();

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("x-api-key", "your_api_key_here");

            var requestContent = new MultipartFormDataContent
            {
                { new ByteArrayContent(sketchBytes), "image", "sketch.png" },
                { new StringContent(Prompt), "prompt" }
            };

            var response = await httpClient.PostAsync("https://clipdrop-api.co/sketch-to-image/v1/sketch-to-image", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                // Handle response image bytes as needed (display, save, etc.)
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to generate image. {errorContent}", "OK");
            }
        }

        public void ClearCanvas()
        {
            Paths.Clear();
            OnPropertyChanged(nameof(Paths));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

using DrawClientMaui.Views;
using DrawClientMaui.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using DrawClientMaui.Drawables;
namespace DrawClientMaui.ViewModels
{
    class SketchViewModel:BindableObject
    {
        private readonly GraphicsDrawable _graphicsDrawable;
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
        // Navigate between pages
        public ICommand NavigateToHomeCommand { get; }
        public ICommand NavigateToSketchCommand { get; }
        public ICommand NavigateToGalleryCommand { get; }
        public ICommand NavigateToSettingsCommand { get; }
        public ICommand SendSketchCommand { get; }
        public ICommand ClearCanvasCommand { get; }

        public GraphicsDrawable GraphicsDrawable => _graphicsDrawable;

        public ObservableCollection<PathModel> Paths => _graphicsDrawable.Paths;
        public string FilePath { get; private set; }


        public SketchViewModel()
        {
            // Initialize navigation commands
            NavigateToHomeCommand = new RelayCommand(OnNavigateToHome);
            NavigateToSketchCommand = new RelayCommand(OnNavigateToSketch);
            NavigateToGalleryCommand = new RelayCommand(OnNavigateToGallery);
            NavigateToSettingsCommand = new RelayCommand(OnNavigateToSettings);
            _graphicsDrawable = new GraphicsDrawable();
            SendSketchCommand = new Command(async () => await SendSketchToAPI());
            ClearCanvasCommand = new Command(ClearCanvas);
        }
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
        private async Task<MemoryStream> ConvertSketchToImageAsync()
        {
            // var canvas = new Microsoft.Maui.Graphics.Platform.PlatformBitmapExportContext(800, 800, 1.0f, 0, 0);
            // _graphicsDrawable.Draw(canvas.Canvas, new RectF(0, 0, 800, 800));
            
            // var imageStream = new MemoryStream();
            // await canvas.Image.SaveAsync(imageStream, Microsoft.Maui.Graphics.BitmapFormat.Png);
            // imageStream.Position = 0; // Reset position for reading

            // return imageStream;
            var width = 800;
            var height = 800;

            // Create a BitmapExportContext for cross-platform bitmap handling
            using var context = new Microsoft.Maui.Graphics.Platform.PlatformBitmapExportContext(width, height, 1.0f, 0, 0);
            var canvas = context.Canvas;

            // Draw the current paths onto the canvas
            _graphicsDrawable.Draw(canvas, new RectF(0, 0, width, height));

            // Export as PNG to MemoryStream
            var imageStream = new MemoryStream();
            await context.Image.SaveAsync(imageStream, ImageFormat.Png);
            imageStream.Position = 0; // Reset position for reading
            // Save the image locally
            var currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine($"Current Directory: {currentDirectory}");
            // Define a path for saving the image in the user's Documents folder
            string localFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "sketch.png");

            // var localFilePath = Path.Combine(Directory.GetCurrentDirectory(), "sketch.png");
            try{
            using (var fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write))
            {
                await imageStream.CopyToAsync(fileStream); // Save the stream to the file
            }
            Console.WriteLine($"Image saved to: {localFilePath}");
            }

            catch(Exception ex){
                Console.WriteLine($"Error: {ex.Message}");
            }

            return imageStream;
        }
               private async Task SendSketchToAPI()
        {
            var sketchImage = await ConvertSketchToImageAsync();
            var sketchBytes = sketchImage.ToArray();

            using var httpClient = new HttpClient();
            // httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // Add API key header
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("x-api-key", "bb3f8fe8d1d7af4b0a513496e65ddb351ec65b70fcba2985b1b90d0a8054ed1891bbcb97544261ec3a11f6b9a6a8b1ce");

            var requestContent = new MultipartFormDataContent
            {
                { new ByteArrayContent(sketchBytes), "image", "sketch.png" },
                { new StringContent(Prompt), "prompt" }
            };

            var response = await httpClient.PostAsync("https://clipdrop-api.co/sketch-to-image/v1/sketch-to-image", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                _graphicsDrawable.GeneratedImage = Microsoft.Maui.Graphics.Platform.PlatformImage.FromStream(new MemoryStream(imageBytes));
                OnPropertyChanged(nameof(GraphicsDrawable)); // Notify canvas to redraw
                // Check remaining credits (optional)
                if (response.Headers.TryGetValues("x-remaining-credits", out var remainingCredits))
                {
                    Console.WriteLine($"Remaining Credits: {remainingCredits.FirstOrDefault()}");
                }
            }
            else
            { // Display error message
                var errorContent = await response.Content.ReadAsStringAsync();
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to generate image. {errorContent}", "OK");
            }
        }
              public void ClearCanvas()
        {
            Paths.Clear();
            _graphicsDrawable.GeneratedImage = null;
            OnPropertyChanged(nameof(Paths));
            OnPropertyChanged(nameof(GraphicsDrawable));
            
        }
    }
}


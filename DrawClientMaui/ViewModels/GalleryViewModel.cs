using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DrawClientMaui.Models;
using DrawClientMaui.Views;
using Microsoft.Maui.Controls;

namespace DrawClientMaui.ViewModels
{
    public class GalleryViewModel : BindableObject
    {

        public ObservableCollection<SketchItem> Sketches { get; }

        public ICommand NavigateToHomeCommand { get; }
        public ICommand NavigateToSketchCommand { get; }
        public ICommand NavigateToGalleryCommand { get; }
        public ICommand NavigateToSettingsCommand { get; }

        public GalleryViewModel()
        {
            // _httpClient = new HttpClient { BaseAddress = new Uri("https://your-api-url.com") };
            Sketches = new ObservableCollection<SketchItem>();
            InitializeAsync();

            NavigateToHomeCommand = new RelayCommand(OnNavigateToHome);
            NavigateToSketchCommand = new RelayCommand(OnNavigateToSketch);
            NavigateToGalleryCommand = new RelayCommand(OnNavigateToGallery);
            NavigateToSettingsCommand = new RelayCommand(OnNavigateToSettings);
            // IsPopupVisible = false; // Initialize to false to prevent automatic popup
        }

        private async void InitializeAsync()
        {
            await LoadSketchesAsync();
        }

        private async Task LoadSketchesAsync()
        {
            var sketches = await ApiService.Client.GetFromJsonAsync<List<SketchResponse>>("api/sketches/user-sketches");
            if (sketches != null)
            {
                foreach (var sketch in sketches)
                {
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(sketch.ImageBytes));
                    Sketches.Add(new SketchItem
                    {
                        SketchName = sketch.SketchName,
                        Prompt = sketch.Prompt,
                        ImageSource = imageSource
                    });
                }
            }
        }

        public void AddSketch(SketchItem sketch)
        {
            Sketches.Add(sketch);
        }

        private async void OnNavigateToHome()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new HomePage());
        }

        private async void OnNavigateToSketch()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SketchPage());
        }

        private async void OnNavigateToGallery()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new GalleryPage());
        }

        private async void OnNavigateToSettings()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage());
        }
    }

    public class SketchItem
    {
        public string SketchName { get; set; }
        public string Prompt { get; set; }
        public ImageSource ImageSource { get; set; }
    }

    public class SketchResponse
    {
        public string SketchName { get; set; }
        public string? Prompt { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}
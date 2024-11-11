using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

using DrawClientMaui.Views;
using DrawClientMaui.Models;
using System.ComponentModel;

namespace DrawClientMaui.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private UserModel _user;
        public UserModel User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }
        // Navigate between pages
        public ICommand NavigateToHomeCommand { get; }
        public ICommand NavigateToSketchCommand { get; }
        public ICommand NavigateToGalleryCommand { get; }
        public ICommand NavigateToSettingsCommand { get; }
        public SettingsViewModel()
        {
            // Initialize navigation commands
            NavigateToHomeCommand = new RelayCommand(OnNavigateToHome);
            NavigateToSketchCommand = new RelayCommand(OnNavigateToSketch);
            NavigateToGalleryCommand = new RelayCommand(OnNavigateToGallery);
            NavigateToSettingsCommand = new RelayCommand(OnNavigateToSettings);
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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

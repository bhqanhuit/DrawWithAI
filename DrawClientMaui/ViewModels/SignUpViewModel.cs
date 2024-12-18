﻿using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DrawClientMaui.Models; 
using DrawClientMaui.Views;
namespace DrawClientMaui.ViewModels
{
    public class SignUpPageViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _email;
        private string _password;
        private bool _isPasswordHidden = true;
        private string _eyeIconSource = "eye_closed_icon.png";

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        //---
        public ICommand SignUpCommand { get; }
        public ICommand NavigateToSignInCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }

        public SignUpPageViewModel()
        {
            SignUpCommand = new RelayCommand(OnSignUp);
            NavigateToSignInCommand = new RelayCommand(OnNavigateToSignIn);
            TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);
        }

        public bool IsPasswordHidden
        {
            get => _isPasswordHidden;
            set
            {
                _isPasswordHidden = value;
                OnPropertyChanged(nameof(IsPasswordHidden));
            }
        }

        public string EyeIconSource
        {
            get => _eyeIconSource;
            set
            {
                _eyeIconSource = value;
                OnPropertyChanged(nameof(EyeIconSource));
            }
        }

        private void TogglePasswordVisibility()
        {
            IsPasswordHidden = !IsPasswordHidden;
            EyeIconSource = IsPasswordHidden ? "eye_closed_icon.png" : "eye_open_icon.png";
        }

        private async void OnSignUp()
        {
            // Basic validation for empty fields
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            // Attempt to store credentials using UserModel
            // bool accountCreated = await UserModel.StoreCredentials(Username, Email, Password);

            // if (accountCreated)
            // {
            //     await Application.Current.MainPage.Navigation.PushAsync(new HomePage());
            // }

            // else
            // {
            //     await Application.Current.MainPage.DisplayAlert("Error", "Username already exists or account creation  failed. Please try again.", "OK");
            // }
            
        }

        private async void OnNavigateToSignIn()
        {
            // Implement navigation to the sign-in page
            await Application.Current.MainPage.Navigation.PushAsync(new SignInPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

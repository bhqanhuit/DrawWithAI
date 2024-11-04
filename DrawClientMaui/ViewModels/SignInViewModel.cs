﻿using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DrawClientMaui.Views;
using DrawClientMaui.Models;

namespace DrawClientMaui.ViewModels
{
    public class SignInViewModel : INotifyPropertyChanged
    {
        private string _username;
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
        public ICommand SignInCommand { get; }
        public ICommand NavigateToSignUpCommand { get; }

        public ICommand TogglePasswordVisibilityCommand { get; }

        public SignInViewModel()
        {
            SignInCommand = new RelayCommand(OnSignIn);
            NavigateToSignUpCommand = new RelayCommand(OnNavigateToSignUp);
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

        private async void OnSignIn()
        {
            //Validate using UserModel
            if (UserModel.ValidateCredentials(Username, Password))
            {
                //Navigate to HomePage upon successful sign-in
                await Application.Current.MainPage.Navigation.PushAsync(new HomePage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid username or passwword.", "OK");
            }
        }

        private async void OnNavigateToSignUp()
        {
            // Implement navigation to the sign-up page
            await Application.Current.MainPage.Navigation.PushAsync(new SignUpPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
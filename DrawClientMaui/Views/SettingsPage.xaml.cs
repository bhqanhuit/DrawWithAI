using DrawClientMaui.ViewModels;

namespace DrawClientMaui.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        this.BindingContext = new SettingsViewModel();
        FetchUserData();
    }

    private async void FetchUserData()
    {
        try
        {
            var viewModel = (SettingsViewModel)BindingContext;
            await viewModel.LoadUserDataAsync();
        }
        catch (Exception ex)
        {
            // Log the exception or display an alert
            await DisplayAlert("Error", $"Failed to fetch user data: {ex.Message}", "OK");
        }
    }
}
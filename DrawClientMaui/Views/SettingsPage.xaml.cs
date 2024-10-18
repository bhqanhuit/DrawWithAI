using DrawClientMaui.ViewModels;
namespace DrawClientMaui.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
		this.BindingContext = new SettingsViewModel();
	}
}
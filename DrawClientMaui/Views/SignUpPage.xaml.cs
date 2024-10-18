using DrawClientMaui.ViewModels;

namespace DrawClientMaui.Views;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
		this.BindingContext = new SignUpPageViewModel();

    }
}
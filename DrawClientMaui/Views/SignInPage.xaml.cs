using DrawClientMaui.ViewModels;
using Microsoft.Maui.Controls;

namespace DrawClientMaui.Views
{
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            InitializeComponent();
            this.BindingContext = new SignInViewModel(); // Set the ViewModel as BindingContext
        }
    }
}

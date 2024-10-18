using DrawClientMaui.Views;
using DrawClientMaui.ViewModels;

namespace DrawClientMaui.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            this.BindingContext = new HomeViewModel();
        }
    }
}


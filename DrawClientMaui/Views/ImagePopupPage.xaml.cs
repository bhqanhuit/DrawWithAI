using DrawClientMaui.Views;
using DrawClientMaui.ViewModels;

namespace DrawClientMaui.Views
{
    public partial class ImagePopupPage : ContentPage
    {
        public ImagePopupPage(ImageSource imageSource)
        {
            InitializeComponent();
            BindingContext = new ImagePopupViewModel(imageSource);
        }
    }
}
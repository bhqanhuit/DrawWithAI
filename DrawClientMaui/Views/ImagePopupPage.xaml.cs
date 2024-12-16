using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace DrawClientMaui.Views
{
    public partial class ImagePopupPage : ContentPage
    {
        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(ImagePopupPage));

        public static readonly BindableProperty CloseCommandProperty =
            BindableProperty.Create(nameof(CloseCommand), typeof(ICommand), typeof(ImagePopupPage));

        public ImagePopupPage()
        {
            InitializeComponent();
            CloseCommand = new Command(async () => await ClosePopup());
            BindingContext = this;
        }

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public ICommand CloseCommand
        {
            get => (ICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        private async Task ClosePopup()
        {
            await Navigation.PopModalAsync();
        }
    }
}
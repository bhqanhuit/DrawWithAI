using DrawClientMaui.ViewModels;
namespace DrawClientMaui.Views;

public partial class GalleryPage : ContentPage
{
	public GalleryPage()
	{
		InitializeComponent();
		this.BindingContext = new GalleryViewModel();
	}
}
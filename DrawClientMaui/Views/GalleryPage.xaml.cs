using DrawClientMaui.ViewModels;
namespace DrawClientMaui.Views;

public partial class GalleryPage : ContentPage
{
	private readonly GalleryViewModel _galleryViewModel;
	public GalleryPage() 
	{
		InitializeComponent();
		_galleryViewModel = new GalleryViewModel();
        BindingContext = _galleryViewModel;
	}
	private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var selectedSketch = e.CurrentSelection[0] as SketchItem;
                if (selectedSketch != null)
                {
                    var imagePopupPage = new ImagePopupPage
                    {
                        ImageSource = selectedSketch.ImageSource
                    };
                    await Navigation.PushModalAsync(imagePopupPage);
                }
            }
        }
}
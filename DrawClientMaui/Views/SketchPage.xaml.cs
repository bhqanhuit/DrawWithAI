using DrawClientMaui.ViewModels;
namespace DrawClientMaui.Views;

public partial class SketchPage : ContentPage
{
	public SketchPage()
	{
		InitializeComponent();
		this.BindingContext = new SketchViewModel();
	}
}
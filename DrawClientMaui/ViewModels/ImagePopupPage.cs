using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

using DrawClientMaui.Views;
using DrawClientMaui.Models;

namespace DrawClientMaui.ViewModels
{
    
    public class ImagePopupViewModel
    {
        public ImageSource ImageSource { get; }
        public Command CloseCommand { get; }

        public ImagePopupViewModel(ImageSource imageSource)
        {
            ImageSource = imageSource;
            CloseCommand = new Command(() => Application.Current.CloseWindow(Application.Current.Windows.Last()));
        }
    }
}
using Covid19App.Shared.ViewModels;
using Xamarin.Forms;

namespace Covid19App.Shared.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            BindingContext = new HomePageViewModel(Navigation);
        }
    }
}
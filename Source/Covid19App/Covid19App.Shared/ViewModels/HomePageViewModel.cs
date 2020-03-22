using Xamarin.Forms;

namespace Covid19App.Shared.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public HomePageViewModel(INavigation navigation)
        {
            Title = "Home";
            Navigation = navigation;
        }
    }
}
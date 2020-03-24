using Covid19App.Shared.ViewModels;
using Xamarin.Forms;

namespace Covid19App.Shared.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new SettingsPageViewModel(Navigation);
        }
    }
}
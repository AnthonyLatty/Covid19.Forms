using Covid19App.Shared.ViewModels;
using Xamarin.Forms;

namespace Covid19App.Shared.Views
{
    public partial class CasesPage : ContentPage
    {
        public CasesPage()
        {
            InitializeComponent();
            BindingContext = new CasesPageViewModel(Navigation);
        }
    }
}
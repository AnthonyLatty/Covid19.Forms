using Covid19App.Shared.ViewModels;
using Xamarin.Forms;

namespace Covid19App.Shared.Views
{
    public partial class StatisticsPage : ContentPage
    {
        public StatisticsPage()
        {
            InitializeComponent();
            BindingContext = new StatisticsPageViewModel(Navigation);
        }
    }
}
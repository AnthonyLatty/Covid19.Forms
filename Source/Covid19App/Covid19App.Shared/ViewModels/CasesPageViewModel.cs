using System;
using Xamarin.Forms;

namespace Covid19App.Shared.ViewModels
{
    public class CasesPageViewModel : BaseViewModel
    {
        public CasesPageViewModel(INavigation navigation)
        {
            Title = "Worldwide Cases";
            Navigation = navigation;
        }
    }
}
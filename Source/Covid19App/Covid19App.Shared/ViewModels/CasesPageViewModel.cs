using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Acr.UserDialogs;
using Covid19App.Shared.Helpers;
using Covid19App.Shared.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Location = Covid19App.Shared.Models.Location;

namespace Covid19App.Shared.ViewModels
{
    public class CasesPageViewModel : BaseViewModel
    {
        private string _searchBarText;

        public ObservableCollection<Location> CoronaVirusCasesCollection { get; set; }
        public Command AppearingCommand => new Command(ExecuteFetchCases);
        public Command SearchButtonPressedCommand => new Command(ExecuteSearchButtonPressedCommand);
        public Command TextChangedCommand => new Command(ExecuteTextChangedCommand);


        public CasesPageViewModel(INavigation navigation)
        {
            Title = "Worldwide Cases";
            Navigation = navigation;

            CoronaVirusCasesCollection = new ObservableCollection<Location>();
        }

        public string SearchBarText
        {
            get => _searchBarText;
            set
            {
                _searchBarText = value;
                OnPropertyChanged();
            }
        }

        private async void ExecuteFetchCases()
        {
            try
            {
                var connected = _permissionService.CheckNetworkConnectivity();
                if (connected != NetworkAccess.Internet)
                {
                    UserDialogs.Instance.Alert("No internet connection available.", "Oops", "Ok");
                    return;
                }
                else
                {
                    FetchCasesFromApi();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                await UserDialogs.Instance.AlertAsync("Something went wrong, please try again later.", "Error", "Ok");
            }
        }

        private async void FetchCasesFromApi()
        {
            using (UserDialogs.Instance.Loading("Fetching cases..."))
            {
                var url = AppSettings.CoronaTrackerEndpoint;

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;

                    var json = JsonConvert.DeserializeObject<CoronaVirusCases>(content);

                    var casesFound = new ObservableCollection<Location>(json.Locations);

                    foreach (var cases in casesFound)
                    {
                        CoronaVirusCasesCollection.Add(cases);
                    }
                }
            }
        }

        private void ExecuteSearchButtonPressedCommand()
        {
            GetCountryBySearch();
        }

        private async void GetCountryBySearch()
        {
            try
            {
                var keyboard = SearchBarText;

                var url = AppSettings.CoronaTrackerEndpoint;

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;

                    var json = JsonConvert.DeserializeObject<CoronaVirusCases>(content);

                    var casesFound = new ObservableCollection<Location>(json.Locations);

                    var searchedCountry = casesFound.Where(c => c.Country.ToLower().Contains(SearchBarText.ToLower()));

                    foreach (var country in searchedCountry)
                    {
                        CoronaVirusCasesCollection.Add(country);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                await UserDialogs.Instance.AlertAsync("Something went wrong, please try again later.", "Error", "Ok");
            }
        }


        private void ExecuteTextChangedCommand()
        {
            //...
        }
    }
}
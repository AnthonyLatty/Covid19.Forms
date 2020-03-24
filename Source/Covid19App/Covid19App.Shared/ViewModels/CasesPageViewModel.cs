using System;
using System.Collections.Generic;
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
using System.Windows.Input;
using System.Threading.Tasks;

namespace Covid19App.Shared.ViewModels
{
    public class CasesPageViewModel : BaseViewModel
    {
        private string _searchBarText;
        private bool _isRefreshing = false;
        public ObservableCollection<Location> CoronaVirusCasesCollection { get; set; }
        public Command AppearingCommand => new Command(ExecuteFetchCases);
        public Command DisappearingCommand => new Command(ExecuteDisappearingCommand);
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
        
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await FetchCasesFromApi();

                    IsRefreshing = false;
                });
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
                    await FetchCasesFromApi();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                await UserDialogs.Instance.AlertAsync("Something went wrong, please try again later.", "Error", "Ok");
            }
        }

        private async Task FetchCasesFromApi()
        {
            using (UserDialogs.Instance.Loading("Fetching cases..."))
            {
                // Clears collection and before fetching new data
                CoronaVirusCasesCollection.Clear();

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

                    Title = $"Worldwide Cases ({casesFound.Count})";
                }
            }
        }

        private void ExecuteSearchButtonPressedCommand()
        {
            GetCasesBySearch();
        }

        private async void GetCasesBySearch()
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

                    // Clears previous collection before search was done
                    CoronaVirusCasesCollection.Clear();

                    var casesFound = new ObservableCollection<Location>(json.Locations);

                    var searchedCountry = casesFound.Where(c => c.Country.ToLower().Contains(SearchBarText.ToLower()));

                    foreach (var country in searchedCountry)
                    {
                        CoronaVirusCasesCollection.Add(country);
                    }

                    var searchedCountryCount = new List<Location>(searchedCountry).Count;

                    Title = $"Worldwide Cases ({searchedCountryCount})";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                await UserDialogs.Instance.AlertAsync("Something went wrong, please try again later.", "Error", "Ok");
            }
        }


        private async void ExecuteTextChangedCommand()
        {
            if (_searchBarText == string.Empty)
            {
                // Clears collection and before fetching new data
                CoronaVirusCasesCollection.Clear();

                await FetchCasesFromApi();
            }
            else
            {
                GetCasesBySearch();
            }
        }


        /// <summary>
        /// Cleans up page after user has left page
        /// </summary>
        private void ExecuteDisappearingCommand()
        {
            CoronaVirusCasesCollection.Clear();
        }
    }
}
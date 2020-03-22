using System.Collections.ObjectModel;
using Acr.UserDialogs;
using Covid19App.Shared.Helpers;
using Covid19App.Shared.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using System;
using System.Diagnostics;

namespace Covid19App.Shared.ViewModels
{
    public class StatisticsPageViewModel : BaseViewModel
    {
        private string _confirmedCases;
        private string _deadCases;
        private string _recoveredCases;
        private string _lastUpdated;
        public Command AppearingCommand => new Command(ExecuteFetchStatistics);

        public StatisticsPageViewModel(INavigation navigation)
        {
            Title = "Statistics";
            Navigation = navigation;
        }

        #region Properties
        public string ConfirmedCases
        {
            get => _confirmedCases;
            set
            {
                _confirmedCases = value;
                OnPropertyChanged();
            }
        }

        public string DeadCases
        {
            get => _deadCases;
            set
            {
                _deadCases = value;
                OnPropertyChanged();
            }
        }

        public string RecoveredCases
        {
            get => _recoveredCases;
            set
            {
                _recoveredCases = value;
                OnPropertyChanged();
            }
        }

        public string LastUpdated
        {
            get => _lastUpdated;
            set
            {
                _lastUpdated = value;
                OnPropertyChanged();
            }
        }
        #endregion 

        public async void ExecuteFetchStatistics()
        {
            var connected = _permissionService.CheckNetworkConnectivity();
            if (connected != NetworkAccess.Internet)
            {
                UserDialogs.Instance.Alert("No internet connection available.", "Oops", "Ok");
                return;
            }

            try
            {
                using (UserDialogs.Instance.Loading("Fetching statistics..."))
                {
                    // call service
                    var url = AppSettings.CoronaTrackerEndpoint;

                    var response = await _httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;

                        var json = JsonConvert.DeserializeObject<CoronaVirusCases>(content);

                        var VirusCases = json;

                        // Map values to label
                        ConfirmedCases = VirusCases.Latest.Confirmed.ToString("N0");
                        DeadCases = VirusCases.Latest.Deaths.ToString("N0");
                        RecoveredCases = VirusCases.Latest.Recovered.ToString("N0");

                        // Add date
                        var currentDate = DateTime.Now.ToString("h:mm tt");
                        LastUpdated = $"Last Updated: {currentDate}";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                await UserDialogs.Instance.AlertAsync("Something went wrong, please try again later.", "Error", "Ok");
            }
        }
    }
}
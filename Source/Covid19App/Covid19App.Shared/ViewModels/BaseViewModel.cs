using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Covid19App.Shared.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private string _title { get; set; }
        private bool _isNotConnected { get; set; }
        public readonly HttpClient _httpClient;
        // Navigation property inherited in view models
        public INavigation Navigation { get; set; }

        public BaseViewModel()
        {
            _httpClient = new HttpClient();

            // Handle connectivity
            Connectivity.ConnectivityChanged += ConnectivityOnConnectivityChanged;
            IsNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
        }

        ~BaseViewModel()
        {
            Connectivity.ConnectivityChanged -= ConnectivityOnConnectivityChanged;
        }

        private void ConnectivityOnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsNotConnected = e.NetworkAccess != NetworkAccess.Internet;
        }

        public bool IsNotConnected
        {
            get => _isNotConnected;
            set
            {
                _isNotConnected = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
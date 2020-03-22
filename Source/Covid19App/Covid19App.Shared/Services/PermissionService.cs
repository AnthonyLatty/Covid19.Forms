using Covid19App.Shared.Interfaces;
using Xamarin.Essentials;


namespace Covid19App.Shared.Services
{
    public class PermissionService : IPermissionService
    {
        public PermissionService()
        {
        }

        public NetworkAccess CheckNetworkConnectivity()
        {
            var networkAccess = Connectivity.NetworkAccess;
            // returns the network status
            return networkAccess;
        }
    }
}
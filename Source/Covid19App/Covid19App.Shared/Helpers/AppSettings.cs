using System;
namespace Covid19App.Shared.Helpers
{
    public static class AppSettings
    {
        // Root API endpoint
        public static readonly string CoronaTrackerEndpoint = "https://coronavirus-tracker-api.herokuapp.com/v2/locations";

        // App Center Credentials
        public static string appCenterAndroid;
        public static string appCenteriOS;


        static AppSettings()
        {
            appCenterAndroid = "";
            appCenteriOS = "";
        }
    }
}
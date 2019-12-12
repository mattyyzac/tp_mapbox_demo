using System.Reflection;

namespace Core
{
    public class AppSettings
    {
        public string MapboxToken { get; set; }
        public string GoogleApiToken { get; set; }
        public GooglePlaceApi GooglePlaceApi { get; set; }


        public static string WebRootPath { get; set; }
        public static string ContentRootPath { get; set; }

        public static string Version
        {
            get
            {
                return typeof(AppSettings)
                    .GetTypeInfo()
                    .Assembly
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion;
            }
        }
        public static string OSDescription
        {
            get
            {
                return System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            }
        }
    }
    
    public class GooglePlaceApi
    {
        public string Nearbysearch { get; set; }
        public string FindFromText { get; set; }
        public string Detail { get; set; }
        public string Photo { get; set; }
    }
}

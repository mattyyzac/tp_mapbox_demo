using Core.Data.GoogleServices;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Data.Service.GoogleServices
{
    public class Place
    {
        private readonly IOptions<AppSettings> _options;
        private readonly IHttpClientFactory _httpClientFactory;

        public Place(IOptions<AppSettings> options, IHttpClientFactory httpClientFactory)
        {
            this._options = options;
            this._httpClientFactory = httpClientFactory;
        }
        public async Task<GooglePlaceData> Nearbysearch(double lng, double lat, string name, string lang)
        {
            return await Nearbysearch(lng, lat, name, 1000, "airport", lang);
        }

        public async Task<GooglePlaceData> Nearbysearch(double lng, double lat, string name, int radius, string type, string lang)
        {
            var api = string.Format(
                this._options.Value?.GooglePlaceApi.Nearbysearch,
                this._options.Value?.GoogleApiToken,
                lat,
                lng,
                radius,
                name,
                type,
                lang);
            var jsonStr = await this._httpClientFactory.CreateClient().GetStringAsync(api);
            var data = JsonSerializer.Deserialize<GooglePlaceData>(jsonStr);
            return data;
        }

        public async Task<GooglePlaceData2> NearbySearch(string name)
        {
            var api = string.Format(
                this._options.Value?.GooglePlaceApi.FindFromText,
                this._options.Value?.GoogleApiToken,
                name);
            var jsonStr = await this._httpClientFactory.CreateClient().GetStringAsync(api);
            try
            {
                var jn = jsonStr.Replace("\n", "");
                var temp = JsonSerializer.Deserialize<GooglePlaceData2>(jn);
                if (temp != null)
                {
                    return temp;
                }
                else
                {
                    return new GooglePlaceData2 { };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GooglePlaceDetailData> Detail(string placeid, string lang)
        {
            var api = string.Format(this._options.Value?.GooglePlaceApi.Detail, this._options.Value?.GoogleApiToken, placeid, lang);
            var jsonStr = await this._httpClientFactory.CreateClient().GetStringAsync(api);
            var data = JsonSerializer.Deserialize<GooglePlaceDetailData>(jsonStr);
            return data;
        }

        public async Task<string> Photo(string photoreference, int maxWidth)
        {
            var api = string.Format(this._options.Value?.GooglePlaceApi.Photo, this._options.Value?.GoogleApiToken, photoreference, maxWidth);
            var strm = await this._httpClientFactory.CreateClient().GetStreamAsync(api);
            var base64Str = Convert.ToBase64String(StreamToBytes(strm));
            return base64Str;
        }
        private static byte[] StreamToBytes(System.IO.Stream stream)
        {
            using var memoryStream = new System.IO.MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}

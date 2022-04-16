using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnturnedStrikeWeb.Services
{
    public class CountryService : ICountryService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public CountryService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetCountryAsync(string ipAddress)
        {
            string url = "http://ip-api.com/json/" + ipAddress;
            var obj = JObject.Parse(await httpClientFactory.CreateClient().GetStringAsync(url));
            if (obj["status"].ToString() == "success")
                return obj["countryCode"].ToString();
            else
                return null;
        }
    }
}

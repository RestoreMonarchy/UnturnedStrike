using Newtonsoft.Json;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedStrikeApiPlugin.Utilities
{
    public class ApiHttpClient
    {
        private UnturnedStrikeApiPlugin pluginInstance => UnturnedStrikeApiPlugin.Instance;

        public string BaseUrl { get; }

        public ApiHttpClient()
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertifications);
            BaseUrl = pluginInstance.Configuration.Instance.APIUrl.TrimEnd('/') + '/'; 
        }

        //private bool AcceptAllCertifications(object a, X509Certificate b, X509Chain c, SslPolicyErrors d) => true;

        public HttpWebResponse SendAsJson(string relativeUrl, object obj, string method = "POST")
        {
            var content = JsonConvert.SerializeObject(obj);
            var data = Encoding.ASCII.GetBytes(content);

            var targetUrl = BuildUrl(relativeUrl);
            var request = WebRequest.Create(targetUrl) as HttpWebRequest;

            request.Method = method;
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            request.Headers["x-api-key"] = pluginInstance.Configuration.Instance.APIKey;
            request.Timeout = pluginInstance.Configuration.Instance.TimeoutMiliseconds;

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                return request.GetResponse() as HttpWebResponse;
            }
            catch (WebException e)
            {
                Logger.LogError($"An error occurated during POST request to {targetUrl}: {e.Message}");
                return null;
            }
        }

        public T SendAsJson<T>(string relativeUrl, object obj, string method = "POST")
        {
            string content;
            using (var response = SendAsJson(relativeUrl, obj, method))
            {
                using (var stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream);
                    content = reader.ReadToEnd();
                }
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception e)
            {
                Logger.LogError($"An error occurated during deserializaion of response content: {e.Message}");
                return default;
            }
        }

        public T GetFromJson<T>(string relativeUrl)
        {
            var targetUrl = BuildUrl(relativeUrl);
            var request = WebRequest.Create(targetUrl) as HttpWebRequest;

            request.Method = "GET";

            request.Headers["x-api-key"] = pluginInstance.Configuration.Instance.APIKey;
            request.Timeout = pluginInstance.Configuration.Instance.TimeoutMiliseconds;

            string content;
            try
            {
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        var reader = new StreamReader(stream);
                        content = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException e)
            {
                Logger.LogError($"An error occurated during GET request to {targetUrl}: {e.Message}");
                return default;
            }

            return JsonConvert.DeserializeObject<T>(content);
        }

        private string BuildUrl(string relativeUrl)
        {
            return BaseUrl + relativeUrl;
        }
    }
}

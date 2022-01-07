using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Proxy
{
    public class ClientExecutor
    {
        protected readonly ILogger _log;
        public ClientExecutor(ILoggerFactory log)
        {
            _log = log.CreateLogger<ClientExecutor>();
        }
        protected async Task<string> MakeRequestAsync(string requestUrl, string httpType, Dictionary<string, string> headers, object jsonRequest = null)
        {
            try
            {
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Method = httpType;
                request.ContentType = "application/json";

                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }

                if (jsonRequest != null)
                {
                    var sb = JsonConvert.SerializeObject(jsonRequest);
                    _log.LogInformation(sb, $"{httpType}:{requestUrl}", jsonRequest);
                    if (request == null)
                    {
                        return null;
                    }

                    var bt = Encoding.UTF8.GetBytes(sb);
                    var st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                }

                using var response = request.GetResponse() as HttpWebResponse;
                if (response != null && response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");
                }

                var stream1 = response?.GetResponseStream();
                if (stream1 == null)
                {
                    return null;
                }

                var sr = new StreamReader(stream1);
                var strsb = await sr.ReadToEndAsync().ConfigureAwait(false);
                _log.LogInformation($"{requestUrl}:{strsb}");
                return strsb;
            }
            catch (Exception e)
            {
                _log.LogError(e.Message, e, requestUrl);
                throw new Exception($"Error {e.Message} From WebService call");
            }
        }
        protected async Task<T> MakeRequestAsync<T>(string requestUrl, string httpType, Dictionary<string, string> headers, object jsonRequest = null)
        {
            try
            {
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Method = httpType;
                request.ContentType = "application/json";

                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }

                if (jsonRequest != null)
                {
                    var sb = JsonConvert.SerializeObject(jsonRequest);
                    _log.LogInformation($"{httpType}:{requestUrl}:{sb}");
                    if (request == null)
                    {
                        return default;
                    }

                    var bt = Encoding.UTF8.GetBytes(sb);
                    var st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                }

                using var response = request.GetResponse() as HttpWebResponse;
                if (response != null && response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");
                }

                var stream1 = response?.GetResponseStream();
                if (stream1 == null)
                {
                    return default;
                }

                var sr = new StreamReader(stream1);
                var strsb = await sr.ReadToEndAsync().ConfigureAwait(false);
                _log.LogInformation($"{requestUrl}:{strsb}");
                return JsonConvert.DeserializeObject<T>(strsb);
            }
            catch (Exception e)
            {
                _log.LogError(e.Message, e, requestUrl);
                throw new Exception($"We are unable to connect to the remote server. Please try again.");
            }
        }

        //protected static async Task<string> GetAccessTokenAsync(string tokenUrl, string userName, string password)
        //{
        //    using var client = new HttpClient();
        //    var postData = new List<KeyValuePair<string, string>>
        //    {
        //        new("username", userName),
        //        new("password", password),
        //        new("grant_type", "password")
        //    };

        //    HttpContent content = new FormUrlEncodedContent(postData);
        //    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        //    var responseResult = await client.PostAsync(tokenUrl, content).ConfigureAwait(false);
        //    return await responseResult.Content.ReadAsStringAsync().ConfigureAwait(false);
        //}
        protected static Dictionary<string, string> GetHeader(string token)
        {
            return new Dictionary<string, string> { { HttpRequestHeader.Authorization.ToString(), $"bearer {token}" } };
        }
    }
}

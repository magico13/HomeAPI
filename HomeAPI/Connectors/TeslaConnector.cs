using HomeAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeAPI.Connectors
{
    public interface ITeslaConnector
    {
        Task<bool> RefreshTokenAsync();

        Task<TeslaVehicleListResponse> GetVehiclesAsync();

        Task<TeslaVehicleDataResponse> GetVehicleDataAsync(string vehicleId);
    }

    public class TeslaCredentials
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = "{User_Access_Token}";
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; } = "{User_Refresh_Token}";
        [JsonProperty("expires_in")]
        public long Expiration { get; set; } = 0;
        [JsonProperty("created_at")]
        public long CreationTime { get; set; } = 0;


        /// <summary>
        /// Loads the credentials from the provided file and returns a new EcobeeCredentials
        /// </summary>
        /// <param name="file">File to load from</param>
        /// <returns>Credentials object</returns>
        public static TeslaCredentials LoadFromFile(string file)
        {
            if (File.Exists(file))
            {
                string contents = File.ReadAllText(file);
                TeslaCredentials creds = JsonConvert.DeserializeObject<TeslaCredentials>(contents);
                return creds;
            }
            return new TeslaCredentials();
        }

        /// <summary>
        /// Creates the file (and directory if needed) that contains the required Tesla API information
        /// </summary>
        /// <param name="file">The filename to create</param>
        public void WriteToFile(string file)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            File.WriteAllText(file, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        /// <summary>
        /// Checks if the required values are set
        /// </summary>
        /// <returns>True if valid, false otherwise</returns>
        public bool CheckValidity()
        {
            return (!string.IsNullOrEmpty(AccessToken) && !string.Equals(AccessToken, "{User_Access_Token}")
                && !string.IsNullOrEmpty(RefreshToken) && !string.Equals(RefreshToken, "{User_Refresh_Token}")
                && Expiration != 0 && CreationTime != 0);
        }

        /// <summary>
        /// Checks if the access token is expired
        /// </summary>
        /// <returns>True if expired, false if valid</returns>
        public bool CheckExpired()
        {
            DateTime created = (new DateTime(1970, 1, 1)).AddSeconds(CreationTime);
            DateTime expires = created.AddSeconds(Expiration);
            return DateTime.UtcNow > expires;
        }
    }

    public class TeslaConnector : ConnectorBase, ITeslaConnector
    {
        private const string USER_AGENT = "Magico13_HomeAPI";
        private const string API_FILE = "Configuration/tesla.private";
        private const string API_URL = "https://owner-api.teslamotors.com/api/1";
        private const string UNCONFIGURED_EXCEPTION = "Tesla API is not yet configured.";

        private const string CLIENT_ID = "81527cff06843c8634fdc09e8ac0abefb46ac849f38fe1e431c2ef2106796384";
        private const string CLIENT_SECRET = "c7257eb71a564034f9419ee651c7d0e5f7aa6bfbd18bafb5c5c033b093bb2fa3";

        private TeslaCredentials _credentials = null;

        public TeslaConnector()
        {
            if (_client == null)
            {
                _client = new HttpClient();
            }

            _credentials = TeslaCredentials.LoadFromFile(API_FILE);
        }

        /// <summary>
        /// Call before making any API calls. Checks that the configuration is valid
        /// and that the user token is not expired. Gets a new token if needed.
        /// </summary>
        /// <returns>Awaitable Task</returns>
        private async Task Validate()
        {
            if (!_credentials.CheckValidity())
            {
                throw new UnauthorizedAccessException(UNCONFIGURED_EXCEPTION);
            }
            if (_credentials.CheckExpired())
            {
                bool refreshed = await RefreshTokenAsync();
                if (!refreshed)
                {
                    throw new Exception("Could not refresh authentication token! Will require full reauthorization!");
                }
            }
        }

        public async Task<TeslaVehicleDataResponse> GetVehicleDataAsync(string vehicleId)
        {
            await Validate();

            //use the refresh token to refresh the access token
            string url = $"{API_URL}/vehicles/{vehicleId}/vehicle_data";
            try
            {
                HttpRequestMessage request = createRequest(HttpMethod.Get, url, "");
                HttpResponseMessage response = await _client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TeslaVehicleDataResponse>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<TeslaVehicleListResponse> GetVehiclesAsync()
        {
            await Validate();

            //use the refresh token to refresh the access token
            string url = $"{API_URL}/vehicles";
            try
            {
                HttpRequestMessage request = createRequest(HttpMethod.Get, url, "");
                HttpResponseMessage response = await _client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TeslaVehicleListResponse>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<bool> RefreshTokenAsync()
        {
            if (!_credentials.CheckValidity())
            {
                throw new UnauthorizedAccessException(UNCONFIGURED_EXCEPTION);
            }

            //use the refresh token to refresh the access token
            string url = $"{API_URL}/oath/token?grant_type=refresh_token&refresh_token={_credentials.RefreshToken}&client_id={CLIENT_ID}&client_secret={CLIENT_SECRET}";
            try
            {
                HttpRequestMessage request = createRequest(HttpMethod.Post, url, "");
                HttpResponseMessage response = await _client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    _credentials = JsonConvert.DeserializeObject<TeslaCredentials>(content);
                    _credentials.WriteToFile(API_FILE);
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        private HttpRequestMessage createRequest(HttpMethod method, string url, object content)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "User-Agent", USER_AGENT },
                    { HttpRequestHeader.Authorization.ToString(), "Bearer "+_credentials.AccessToken },
                    { HttpRequestHeader.Accept.ToString(), "*/*" }
                },
                Content = new StringContent(JsonConvert.SerializeObject(content))
            };

            return request;
        }
    }
}

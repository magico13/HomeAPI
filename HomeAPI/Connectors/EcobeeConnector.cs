using HomeAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeAPI.Connectors
{
    public interface IEcobeeConnector
    {
        /// <summary>
        /// Gets a new user access token using the refresh token
        /// </summary>
        /// <returns>Success</returns>
        Task<bool> RefreshTokenAsync();
        /// <summary>
        /// Gets the Thermostat Summary, a lightweight set of info that can be polled more frequently
        /// (ideally 3 minutes)
        /// </summary>
        /// <returns>The entire summary string</returns>
        Task<string> GetThermostatSummaryAsync();
        /// <summary>
        /// Gets a Thermostat, or returns a cached one if nothing has changed since the last call
        /// (Expensive, 15 minutes recommended. Cache reduces this problem)
        /// </summary>
        /// <returns>A full Thermostat</returns>
        Task<EcobeeThermostat> GetThermostatAsync();
        /// <summary>
        /// Forcibly gets a thermostat, ignoring the cached one. 
        /// (Expensive, 15 minutes recommended)
        /// </summary>
        /// <returns>A full Thermostat</returns>
        Task<EcobeeThermostat> ForceGetThermostatAsync();
    }

    public class EcobeeCredentials
    {
        public string APIKey { get; set; } = "QXT0tt1i54n8lPmvUME5olbPUJwopJLI";
        public string AccessToken { get; set; } = "{User_Access_Token}";
        public string RefreshToken { get; set; } = "{User_Refresh_Token}";
        public DateTime Expiration { get; set; } = DateTime.MinValue;


        /// <summary>
        /// Loads the credentials from the provided file and returns a new EcobeeCredentials
        /// </summary>
        /// <param name="file">File to load from</param>
        /// <returns>Credentials object</returns>
        public static EcobeeCredentials LoadFromFile(string file)
        {
            if (File.Exists(file))
            {
                string contents = File.ReadAllText(file);
                EcobeeCredentials creds = JsonConvert.DeserializeObject<EcobeeCredentials>(contents);
                return creds;
            }
            return new EcobeeCredentials();
        }

        /// <summary>
        /// Creates the file (and directory if needed) that contains the required ecobee API information
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
            return (!string.IsNullOrEmpty(APIKey) && !string.Equals(APIKey, "{app_specific_key}"))
                && (!string.IsNullOrEmpty(AccessToken) && !string.Equals(AccessToken, "{User_Access_Token}"))
                && (!string.IsNullOrEmpty(RefreshToken) && !string.Equals(RefreshToken, "{User_Refresh_Token}"));
        }

        /// <summary>
        /// Checks if the access token is expired
        /// </summary>
        /// <returns>True if expired, false if valid</returns>
        public bool CheckExpired()
        {
            return DateTime.UtcNow >= Expiration;
        }
    }

    public class EcobeeConnector : ConnectorBase, IEcobeeConnector
    {
        private const string API_FILE = "Configuration/ecobee.private";
        private const string API_URL = "https://api.ecobee.com/";
        private const string UNCONFIGURED_EXCEPTION = "Ecobee API is not yet configured.";

        private EcobeeCredentials credentials = null;

        private readonly DefaultContractResolver _resolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() }; //use camelCase
        private readonly JsonSerializerSettings _serializerSettings = null;

        private static EcobeeThermostat _lastThermostat = null;
        private static string _lastSummary = null;

        public EcobeeConnector()
        {
            if (_client == null)
            {
                _client = new HttpClient();
            }
            _serializerSettings = new JsonSerializerSettings() { ContractResolver = _resolver };
            credentials = EcobeeCredentials.LoadFromFile(API_FILE);
        }

        /// <summary>
        /// Call before making any API calls. Checks that the configuration is valid
        /// and that the user token is not expired. Gets a new token if needed.
        /// </summary>
        /// <returns>Awaitable Task</returns>
        private async Task Validate()
        {
            if (!credentials.CheckValidity())
            {
                throw new UnauthorizedAccessException(UNCONFIGURED_EXCEPTION);
            }
            if (credentials.CheckExpired())
            {
                bool refreshed = await RefreshTokenAsync();
                if (!refreshed)
                {
                    throw new Exception("Could not refresh authentication token! Will require full reauthorization!");
                }
            }
        }

        /// <inheritdoc />
        public async Task<bool> RefreshTokenAsync()
        {
            if (!credentials.CheckValidity())
            {
                throw new UnauthorizedAccessException(UNCONFIGURED_EXCEPTION);
            }

            //use the refresh token to refresh the access token
            string url = $"{API_URL}token?grant_type=refresh_token&refresh_token={credentials.RefreshToken}&client_id={credentials.APIKey}";

            try
            {
                HttpResponseMessage response = await _client.PostAsync(url, new StringContent(""));
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject obj = JObject.Parse(content);
                    if (obj.ContainsKey("access_token"))
                    {
                        credentials.AccessToken = obj.GetValue("access_token").ToObject<string>();
                        credentials.RefreshToken = obj.GetValue("refresh_token").ToObject<string>();
                        credentials.Expiration = DateTime.UtcNow + TimeSpan.FromSeconds(obj.GetValue("expires_in").ToObject<long>());
                        credentials.WriteToFile(API_FILE); //save these tokens for future use
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        /// <inheritdoc />
        public async Task<string> GetThermostatSummaryAsync()
        {
            await Validate();
            
            string url = API_URL + "1/thermostatSummary?json={\"selection\":{\"selectionType\":\"registered\",\"selectionMatch\":\"\"}}";

            try
            {
                using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", credentials.AccessToken);

                    HttpResponseMessage response = await _client.SendAsync(message);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        return content;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <inheritdoc />
        public async Task<EcobeeThermostat> ForceGetThermostatAsync()
        {
            await Validate();

            string url = API_URL + "1/thermostat?json={\"selection\":{\"selectionType\":\"registered\",\"selectionMatch\":\"\"," +
                "\"includeRuntime\":\"true\",\"includeSettings\":\"true\",\"includeEquipmentStatus\":\"true\",\"includeSensors\":\"true\"}}";//includeSettings, includeEquipmentStatus, includeSensors

            try
            {
                using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", credentials.AccessToken);

                    HttpResponseMessage response = await _client.SendAsync(message);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        JObject obj = JObject.Parse(content);
                        if (obj.TryGetValue("thermostatList", out JToken thermostats))
                        {
                            foreach (JToken thermostat in thermostats)
                            {
                                //convert to a thermostat, but we'll actually stop after the first one because we're not handling the list right now
                                _lastThermostat = thermostat.ToObject<EcobeeThermostat>();
                                return _lastThermostat;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <inheritdoc />
        public async Task<EcobeeThermostat> GetThermostatAsync()
        {
            //get the summary, compare to the cached summary. If the same, return the cached one, otherwise update
            if (_lastThermostat != null && _lastSummary != null)
            {
                string summary = await GetThermostatSummaryAsync();

                if (string.Equals(summary, _lastSummary))
                {
                    return _lastThermostat;
                }
                _lastSummary = summary;
            }
            else if (_lastSummary == null)
            { //we have no summary cached, update it (then update the thermostat too)
                _lastSummary = await GetThermostatSummaryAsync();
            }

            return await ForceGetThermostatAsync();
        }
    }
}

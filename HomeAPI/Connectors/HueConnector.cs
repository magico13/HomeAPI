using HomeAPI.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace HomeAPI.Connectors
{
    public interface IHueConnector
    {
        /// <summary>
        /// Gets the list of all lights
        /// </summary>
        /// <returns>List of all lights</returns>
        Task<List<HueLight>> GetLightsAsync();

        /// <summary>
        /// Gets the specified light
        /// </summary>
        /// <param name="id">The light to get</param>
        /// <returns>The Light</returns>
        Task<HueLight> GetLightAsync(long id);

        /// <summary>
        /// Updates the state of the specified light
        /// </summary>
        /// <param name="id">Light to update</param>
        /// <param name="state">New state to set</param>
        /// <returns>Raw JSON response</returns>
        Task<string> UpdateLightAsync(long id, HueLight.BulbState state);

        /// <summary>
        /// Makes the specified light flash for 15 seconds.
        /// </summary>
        /// <param name="id">Light to flash</param>
        /// <returns>Success</returns>
        Task<bool> AlertLightAsync(long id);
    }

    public class HueCredentials
    {
        public string UserKey { get; set; }
        public string BaseURL { get; set; }

        [JsonIgnore]
        public string APIURL { get { return $"{BaseURL}/api/{UserKey}/"; } }
    }

    public class HueConnector : ConnectorBase, IHueConnector
    {
        private HueCredentials credentials = null;
        private const string API_FILE = "Configuration/hueAPI.private";
        private const string UNCONFIGURED_EXCEPTION = "Hue API is not yet configured.";

        public HueConnector()
        {
            if (_client == null)
            {
                _client = new HttpClient();
            }
            //load credentials from private file
            if (File.Exists(API_FILE))
            {
                string contents = File.ReadAllText(API_FILE);
                credentials = JsonConvert.DeserializeObject<HueCredentials>(contents);
                if (string.Equals(credentials.UserKey, "{hue_generated_key}", StringComparison.Ordinal))
                {
                    credentials = null;
                }
            }
            else
            {
                //make the file if it doesn't exist
                HueCredentials tmp = new HueCredentials() { BaseURL = "http://{your_hub_ip}", UserKey = "{hue_generated_key}" };
                Directory.CreateDirectory(Path.GetDirectoryName(API_FILE));
                File.WriteAllText(API_FILE, JsonConvert.SerializeObject(tmp, Formatting.Indented));
            }
        }

        /// <inheritdoc/>
        public async Task<List<HueLight>> GetLightsAsync()
        {
            if (credentials == null)
            {
                throw new UnauthorizedAccessException(UNCONFIGURED_EXCEPTION);
            }
            string url = credentials.APIURL + "lights";
            List<HueLight> lights = new List<HueLight>();
            try
            {
                HttpResponseMessage response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject obj = JObject.Parse(content);
                    //list of lights with the id and then all the data
                    foreach (KeyValuePair<string, JToken> lightKVP in obj)
                    {
                        HueLight light = lightKVP.Value.ToObject<HueLight>();
                        light.ID = long.Parse(lightKVP.Key);
                        lights.Add(light);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lights;
        }

        /// <inheritdoc/>
        public async Task<HueLight> GetLightAsync(long id)
        {
            if (credentials == null)
            {
                throw new UnauthorizedAccessException(UNCONFIGURED_EXCEPTION);
            }
            string url = $"{credentials.APIURL}lights/{id}";
            try
            {
                HttpResponseMessage response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    HueLight light = JsonConvert.DeserializeObject<HueLight>(content);
                    light.ID = id;
                    return light;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <inheritdoc/>
        public async Task<string> UpdateLightAsync(long id, HueLight.BulbState state)
        {
            if (credentials == null)
            {
                throw new UnauthorizedAccessException(UNCONFIGURED_EXCEPTION);
            }
            string url = $"{credentials.APIURL}lights/{id}/state";
            try
            {
                string stateObj = JsonConvert.SerializeObject(state).ToLower();
                HttpContent content = new StringContent(stateObj);

                HttpResponseMessage response = await _client.PutAsync(url, content);
                string final = await response.Content.ReadAsStringAsync();
                return final;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> AlertLightAsync(long id)
        {
            if (credentials == null)
            {
                throw new UnauthorizedAccessException(UNCONFIGURED_EXCEPTION);
            }
            string url = $"{credentials.APIURL}lights/{id}/state";
            try
            {
                HueLight light = await GetLightAsync(id);
                light.State.Alert = HueLight.AlertType.lselect;

                string stateObj = JsonConvert.SerializeObject(light.State).ToLower();
                HttpContent content = new StringContent(stateObj);

                HttpResponseMessage response = await _client.PutAsync(url, content);
                string final = await response.Content.ReadAsStringAsync();
                return final.Contains("success");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

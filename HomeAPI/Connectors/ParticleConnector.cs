using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeAPI.Connectors
{
    public interface IParticleConnector
    {
        /// <summary>
        /// Calls a published Particle function with the specified argument
        /// </summary>
        /// <param name="functionName">Function to call</param>
        /// <param name="arg">Argument to pass</param>
        /// <returns>Integer return</returns>
        Task<int> CallFunctionAsync(string functionName, string arg);

        /// <summary>
        /// Gets the value of the published Particle variable
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="variableName">Variable to get</param>
        /// <returns>Value or default</returns>
        Task<T> GetVariableAsync<T>(string variableName);

        /// <summary>
        /// Sets the credentials to use when connecting
        /// </summary>
        /// <param name="credentials"></param>
        void SetCredentials(ParticleCredentials credentials);
    }

    public class ParticleCredentials
    {
        public string DeviceID { get; set; }
        public string AccessToken { get; set; }

        public static ParticleCredentials LoadFromFile(string file)
        {
            if (File.Exists(file))
            {
                string contents = File.ReadAllText(file);
                ParticleCredentials creds = JsonConvert.DeserializeObject<ParticleCredentials>(contents);
                return creds;
            }
            return new ParticleCredentials();
        }
    }

    public class ParticleConnector : ConnectorBase, IParticleConnector
    {
        protected ParticleCredentials _creds;

        public ParticleConnector()
        {
            if (_client == null)
            {
                _client = new HttpClient();
            }
        }

        /// <inheritdoc />
        public async Task<int> CallFunctionAsync(string functionName, string arg)
        {
            string url = $"https://api.particle.io/v1/devices/{_creds.DeviceID}/{functionName}?access_token={_creds.AccessToken}";
            try
            {
                HttpResponseMessage response = await _client.PostAsync(url, new FormUrlEncodedContent(
                    new Dictionary<string, string>
                    {
                        ["arg"] = arg,
                        ["format"] = "raw"
                    }
                    ));
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (int.TryParse(content, out int value))
                    {
                        return value;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return -1;
        }

        /// <inheritdoc />
        public async Task<T> GetVariableAsync<T>(string variableName)
        {
            string url = $"https://api.particle.io/v1/devices/{_creds.DeviceID}/{variableName}?access_token={_creds.AccessToken}";
            try
            {
                HttpResponseMessage response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject obj = JObject.Parse(content);
                    return obj.GetValue("result").Value<T>();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return default(T);
        }

        /// <inheritdoc />
        public void SetCredentials(ParticleCredentials credentials)
        {
            _creds = credentials;
        }
    }
}

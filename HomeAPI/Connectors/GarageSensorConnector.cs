using HomeAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeAPI.Connectors
{
    public interface IGarageSensorConnector
    {
        Task<List<GarageSensor>> GetSensorsAsync();
    }

    public class GarageSensorConnector : ParticleConnector, IGarageSensorConnector
    {
        public GarageSensorConnector() : base()
        {
            ParticleCredentials creds = ParticleCredentials.LoadFromFile("Configuration/garage.private");
            SetCredentials(creds);
        }

        public async Task<List<GarageSensor>> GetSensorsAsync()
        {
            List<GarageSensor> sensors = new List<GarageSensor>();
            double temp = Math.Round(await GetVariableAsync<double>("tempf"), 1); ;
            for (int i = 1; i <= 2; i++)
            {
                sensors.Add(new GarageSensor
                {
                    Name = $"Garage Door {i}",
                    Temperature = temp,
                    Occupied = !await GetVariableAsync<bool>($"garage{i}")
                });
            }

            return sensors;
        }

    }
}

using HomeAPI.Models;
using HomeAPI.Models.Interfaces;
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

            for (int i = 1; i <= 2; i++)
            {
                GarageSensor door = new GarageSensor();
                door.Name = $"Garage Door {i}";
                door.Temperature = Math.Round(await GetVariableAsync<double>("tempf"), 1);
                door.Occupied = await CallFunctionAsync("garage", $"door{i}") != 1;
                sensors.Add(door);
            }

            return sensors;
        }

    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using HomeAPI.Connectors;
using HomeAPI.Models;
using HomeAPI.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace HomeAPI.Pages
{
    public class DevicesModel : PageModel
    {
        private readonly IHueConnector _hueConnector;

        private readonly IEcobeeConnector _ecobeeConnector;

        private readonly IGarageSensorConnector _garageSensorConnector;

        private readonly ITeslaConnector _teslaConnector;

        public DevicesModel(IHueConnector hueConnector, IEcobeeConnector ecobeeConnector, IGarageSensorConnector garageSensorConnector, ITeslaConnector teslaConnector)
        {
            _hueConnector = hueConnector;
            _ecobeeConnector = ecobeeConnector;
            _garageSensorConnector = garageSensorConnector;
            _teslaConnector = teslaConnector;
        }

        [BindProperty]
        public List<HueLight> Lights { get; set; }

        [BindProperty]
        public EcobeeThermostat Thermostat { get; set; }

        [BindProperty]
        public List<IRemoteSensor> RemoteSensors { get; set; }

        [BindProperty]
        public TeslaVehicle[] Vehicles { get; set; }

        [BindProperty]
        public Dictionary<string, VehicleData> VehicleData { get; set; }

        public async Task OnGetAsync()
        {
            Stopwatch watch = Stopwatch.StartNew();

            Lights = await _hueConnector.GetLightsAsync();
            Lights = Lights.OrderBy(l => l.Name).ToList();

            System.Console.WriteLine($"Hue took {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            Thermostat = await _ecobeeConnector.GetThermostatAsync();

            System.Console.WriteLine($"Thermostat took {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            RemoteSensors = Thermostat.RemoteSensors.Select(s => (IRemoteSensor)s).ToList();
            System.Console.WriteLine($"Thermo Sensors took {watch.ElapsedMilliseconds}ms");
            watch.Restart();
            //add garage sensors
            RemoteSensors.AddRange(await _garageSensorConnector.GetSensorsAsync());
            System.Console.WriteLine($"Garage Sensors took {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            Vehicles = (await _teslaConnector.GetVehiclesAsync())?.Vehicles ?? new TeslaVehicle[] { };

            System.Console.WriteLine($"Vehicles took {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            VehicleData = new Dictionary<string, VehicleData>();
            if (Vehicles.Any())
            {
                foreach (TeslaVehicle vehicle in Vehicles)
                {
                    TeslaVehicleDataResponse response = await _teslaConnector.GetVehicleDataAsync(vehicle.IdS);
                    if (response != null)
                    {
                        VehicleData[vehicle.IdS] = response.VehicleData;
                    }
                }
            }

            System.Console.WriteLine($"Vehicle Data took {watch.ElapsedMilliseconds}ms");
            watch.Stop();
        }
    }
}
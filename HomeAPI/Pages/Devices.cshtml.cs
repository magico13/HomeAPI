using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using HomeAPI.Connectors;
using HomeAPI.Models;
using HomeAPI.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

namespace HomeAPI.Pages
{
    public class DevicesModel : PageModel
    {
        private readonly IHueConnector _hueConnector;

        private readonly IEcobeeConnector _ecobeeConnector;

        private readonly IGarageSensorConnector _garageSensorConnector;

        public DevicesModel(IHueConnector hueConnector, IEcobeeConnector ecobeeConnector, IGarageSensorConnector garageSensorConnector)
        {
            _hueConnector = hueConnector;
            _ecobeeConnector = ecobeeConnector;
            _garageSensorConnector = garageSensorConnector;
        }

        [BindProperty]
        public List<HueLight> Lights { get; set; }

        [BindProperty]
        public EcobeeThermostat Thermostat { get; set; }

        [BindProperty]
        public List<IRemoteSensor> RemoteSensors {get; set;}

        public async Task OnGetAsync()
        {
            Lights = await _hueConnector.GetLightsAsync();
            Lights = Lights.OrderBy(l => l.Name).ToList();

            Thermostat = await _ecobeeConnector.GetThermostatAsync();

            RemoteSensors = Thermostat.RemoteSensors.Select(s => (IRemoteSensor)s).ToList();
            //add garage sensors
            RemoteSensors.AddRange(await _garageSensorConnector.GetSensorsAsync());
        }
    }
}
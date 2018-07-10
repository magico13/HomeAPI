using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAPI.Connectors;
using HomeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeAPI.Pages
{
    public class ThermostatModel : PageModel
    {
        private readonly IEcobeeConnector _ecobeeConnector;

        [BindProperty]
        public EcobeeThermostat Thermostat { get; set; }

        public ThermostatModel(IEcobeeConnector ecobeeConnector)
        {
            _ecobeeConnector = ecobeeConnector;
        }

        public async Task OnGetAsync()
        {
            Thermostat = await _ecobeeConnector.GetThermostatAsync();
        }
    }
}
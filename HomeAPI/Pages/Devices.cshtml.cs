﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using HomeAPI.Connectors;
using HomeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

namespace HomeAPI.Pages
{
    public class DevicesModel : PageModel
    {
        private readonly IHueConnector _hueConnector;

        private readonly IEcobeeConnector _ecobeeConnector;

        public DevicesModel(IHueConnector hueConnector, IEcobeeConnector ecobeeConnector)
        {
            _hueConnector = hueConnector;
            _ecobeeConnector = ecobeeConnector;
        }

        [BindProperty]
        public List<HueLight> Lights { get; set; }

        [BindProperty]
        public EcobeeThermostat Thermostat { get; set; }

        public async Task OnGetAsync()
        {
            Lights = await _hueConnector.GetLightsAsync();
            Lights = Lights.OrderBy(l => l.Name).ToList();

            Thermostat = await _ecobeeConnector.GetThermostatAsync();
        }
    }
}
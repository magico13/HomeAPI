using System;
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
    public class LightsModel : PageModel
    {
        private readonly IHueConnector _hueConnector;

        public LightsModel(IHueConnector hueConnector)
        {
            _hueConnector = hueConnector;
        }

        [BindProperty]
        public List<HueLight> Lights { get; set; }

        public async Task OnGetAsync()
        {
            Lights = await _hueConnector.GetLightsAsync();
            Lights = Lights.OrderBy(l => l.Name).ToList();
        }
    }
}
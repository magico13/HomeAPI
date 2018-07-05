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

        public async Task<IActionResult> OnPostUpdateLightAsync(long id, int index, bool state)
        {
            int actualIndex = -1;
            if (Request.Form.TryGetValue("i", out StringValues indexVals))
            {
                //find i == index, that's our actual index
                for (int i=0; i<indexVals.Count; i++)
                {
                    if (int.TryParse(indexVals[i], out int tmpIndex))
                    {
                        if (tmpIndex == index)
                        {
                            actualIndex = i;
                            break;
                        }
                    }
                }
            }
            if (actualIndex < 0)
            {
                return NotFound();
            }

            int brightness = 100;
            if (Request.Form.TryGetValue("briScaled", out StringValues vals))
            {
                if (!int.TryParse(vals[actualIndex], out brightness))
                {
                    brightness = 100;
                }
            }

            int newBrightness = (int)Math.Round(brightness * 2.54);
            newBrightness = Math.Min(254, Math.Max(1, newBrightness)); //restrict to between 0 and 254
            await _hueConnector.UpdateLightAsync(id, new HueLight.BulbState() { On = state, Bri = newBrightness });
            return RedirectToPage();
        }
        
    }
}
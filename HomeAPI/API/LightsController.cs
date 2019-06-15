using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Connectors;
using HomeAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeAPI.API
{
    [Route("api/lights")]
    [ApiController]
    public class LightsController : ControllerBase
    {
        private IHueConnector _hueConnector;

        public LightsController(IHueConnector connector)
        {
            _hueConnector = connector;
        }

        [HttpGet]
        public async Task<ActionResult<List<HueLight>>> GetLights()
        {
            return await _hueConnector.GetLightsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HueLight>> GetLight(long id)
        {
            return await _hueConnector.GetLightAsync(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateAsync(long id, [FromBody] HueLight.BulbState state)
        {
            return await _hueConnector.UpdateLightAsync(id, state);
        }

        [HttpPut("{id}/brightness/{brightness}")]
        public async Task<ActionResult<string>> SetBrightnessAsync(long id, double brightness)
        {
            int finalBrightness = 254;
            if (brightness <= 1 && brightness > 0)
            {
                //given as a float between 0 and 1
                finalBrightness = (int)Math.Max(1, Math.Min(254, brightness * 254));
            }
            else
            {
                //otherwise it's a number between 1 and 254
                finalBrightness = (int)Math.Round(Math.Max(1, Math.Min(254, brightness)));
            }
            HueLight light = await _hueConnector.GetLightAsync(id);
            light.State.Bri = finalBrightness;
            light.State.On = true; //if we're setting the brightness then we need to be on
            light.State.Alert = HueLight.AlertType.none;
            return await _hueConnector.UpdateLightAsync(id, light.State);
        }

        [HttpPut("{id}/on/{on}")]
        public async Task<ActionResult<string>> SetBrightnessAsync(long id, bool on)
        {
            HueLight light = await _hueConnector.GetLightAsync(id);
            light.State.On = on;
            light.State.Alert = HueLight.AlertType.none;
            return await _hueConnector.UpdateLightAsync(id, light.State);
        }

        [HttpPut("{id}/alert")]
        public async Task<ActionResult<bool>> AlertLightAsync(long id)
        {
            return await _hueConnector.AlertLightAsync(id);
        }

        [HttpPut("{id}/toggle")]
        public async Task<ActionResult<string>> ToggleLightAsync(long id)
        {
            HueLight light = await _hueConnector.GetLightAsync(id);
            light.State.On = !light.State.On;
            light.State.Alert = HueLight.AlertType.none;

            return await _hueConnector.UpdateLightAsync(id, light.State);
        }
    }
}
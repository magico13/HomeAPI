using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HomeAPI.Pages
{
    public class IndexModel : PageModel
    {
        public string WifiInfo { get; set; }
        public void OnGet()
        {
            if (System.IO.File.Exists("Configuration/wifi.private"))
            {
                string file = System.IO.File.ReadAllText("Configuration/wifi.private");
                JObject obj = JsonConvert.DeserializeObject<JObject>(file);
                WifiInfo = $"{obj["SSID"]} / {obj["Key"]}";
            }
        }
    }
}

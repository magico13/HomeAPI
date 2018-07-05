using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HomeAPI.Models
{
    public class HueLight
    {
        public long ID { get; set; }
        public BulbState State { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string ModelID { get; set; }
        public string ManufacturerName { get; set; }
        public string UniqueID { get; set; }
        public string SWVersion { get; set; }

        public class BulbState
        {
            public bool On { get; set; }
            public int Bri { get; set; }
            [JsonConverter(typeof(StringEnumConverter))]
            public AlertType Alert { get; set; }
            public bool Reachable { get; set; }
            public bool ShouldSerializeReachable() => false;
        }

        public enum AlertType
        {
            none,
            select,
            lselect
        }
    }

    
}

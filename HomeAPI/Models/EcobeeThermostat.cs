using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAPI.Models
{
    public class EcobeeThermostat
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string ThermostatRev { get; set; }
        public bool IsRegistered { get; set; }

    }
}

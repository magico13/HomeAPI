using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAPI.Models.Interfaces
{
    public interface IRemoteSensor
    {
        string Name { get; set; }

        double Temperature { get; }
        bool HasTemperature { get; }

        int Humidity { get; }
        bool HasHumidity { get; }

        bool Occupied { get; }
        bool HasOccupancy { get; }
    }
}

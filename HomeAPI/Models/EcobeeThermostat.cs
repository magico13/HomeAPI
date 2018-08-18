using Newtonsoft.Json;
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
        public string ModelNumber { get; set; }
        public string Brand { get; set; }
        public string Features { get; set; }
        public DateTime LastModified { get; set; } 
        public DateTime ThermostatTime { get; set; }
        public DateTime UtcTime { get; set; }
        public ThermostatSettings Settings { get; set; }
        public ThermostatRuntime Runtime { get; set; }
        public string EquipmentStatus { get; set; }
        public RemoteSensor[] RemoteSensors { get; set; }

        [JsonIgnore]
        public bool FanActive { get { return EquipmentStatus?.Contains("fan", StringComparison.OrdinalIgnoreCase) == true; } }
        [JsonIgnore]
        public bool CoolActive { get { return EquipmentStatus?.Contains("cool", StringComparison.OrdinalIgnoreCase) == true; } }
        [JsonIgnore]
        public bool HeatActive { get { return EquipmentStatus?.Contains("heat", StringComparison.OrdinalIgnoreCase) == true; } }

        public class ThermostatSettings
        {//There are a bunch of other settings that I don't really care about at the moment
            public string HvacMode { get; set; }
            public bool HasHeatPump { get; set; }
            public bool HasForcedAir { get; set; }
            public bool HasBoiler { get; set; }
            public bool HasHumidifier { get; set; }
            public bool HasErv { get; set; }
            public bool HasHrv { get; set; }
            public bool HasElectric { get; set; }
            public bool HasDehumidifier { get; set; }
        }

        public class ThermostatRuntime
        {
            public string RuntimeRev { get; set; }
            public bool Connected { get; set; }
            public DateTime FirstConnected { get; set; }
            public DateTime ConnectDateTime { get; set; }
            public DateTime DisconnectedDateTime { get; set; }
            public DateTime LastModified { get; set; }
            public DateTime LastStatusModified { get; set; }
            public DateTime RuntimeDate { get; set; }
            public int RuntimeInterval { get; set; }
            public int ActualTemperature { get; set; }
            public int ActualHumidity { get; set; }
            public int DesiredHeat { get; set; }
            public int DesiredCool { get; set; }
            public int DesiredHumidity { get; set; }
            public int DesiredDehumidity { get; set; }
            public string DesiredFanMode { get; set; }
            public int[] DesiredHeatRange { get; set; }
            public int[] DesiredCoolRange { get; set; }
        }

        public class RemoteSensor
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Code { get; set; }
            public bool InUse { get; set; }
            public RemoteSensorCapability[] Capability { get; set; }

            [JsonIgnore]
            public double Temperature
            {
                get
                {
                    foreach (RemoteSensorCapability cap in Capability)
                    {
                        if (cap.Temperature > -100)
                        {
                            return cap.Temperature;
                        }
                    }
                    return -100;
                }
            }

            [JsonIgnore]
            public int Humidity
            {
                get
                {
                    foreach (RemoteSensorCapability cap in Capability)
                    {
                        if (cap.Humidity > -100)
                        {
                            return cap.Humidity;
                        }
                    }
                    return -100;
                }
            }

            [JsonIgnore]
            public bool Occupied
            {
                get
                {
                    foreach (RemoteSensorCapability cap in Capability)
                    {
                        if (cap.Occupied != null)
                        {
                            return cap.Occupied.GetValueOrDefault();
                        }
                    }
                    return false;
                }
            }

            public class RemoteSensorCapability
            {
                public string Id { get; set; }
                public string Type { get; set; }
                public string Value { get; set; }

                [JsonIgnore]
                public double Temperature
                {
                    get
                    {
                        return string.Equals(Type, "temperature") ? int.Parse(Value) / 10.0 : -100;
                    }
                }

                [JsonIgnore]
                public int Humidity
                {
                    get
                    {
                        return string.Equals(Type, "humidity") ? int.Parse(Value) : -100;
                    }
                }

                [JsonIgnore]
                public bool? Occupied
                {
                    get
                    {
                        return string.Equals(Type, "occupancy") ? bool.Parse(Value) : (bool?)null;
                    }
                }
            }
        }
    }
}

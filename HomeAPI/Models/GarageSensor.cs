namespace HomeAPI.Models
{
    public class GarageSensor : Interfaces.IRemoteSensor
    {
        public string Name { get; set; } = "Garage";

        public bool HasTemperature => true;
        public double Temperature { get; internal set; } = -100;

        public bool HasHumidity => false;
        public int Humidity => -1;

        public bool HasOccupancy => true;
        public bool Occupied { get; internal set; }
    }
}

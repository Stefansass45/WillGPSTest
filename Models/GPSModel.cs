namespace WIllGPSTest.Models
{
    public class GPSModel
    {

        public DateTime Timestamp { get; set; }
        public string RawData { get; set; }
        //public string DeviceId { get; set; } as die nodig gaan wees dan kan jy dit uncomment
        //public double Latitude { get; set; } 
        //public double Longitude { get; set; } 
        //public double Altitude { get; set; }
        public GPSModel(DateTime timestamp, string rawData)
        {
            Timestamp = timestamp;
            RawData = rawData;
        }




    }
}
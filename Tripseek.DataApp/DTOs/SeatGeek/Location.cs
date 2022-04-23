namespace Tripseek.DataApp.DTOs.SeatGeek
{
    internal class Location
    {

        [JsonPropertyName("lat")]
        public double Latitude { get; set; }
        [JsonPropertyName("long")]
        public double Longitude { get; set; }
    }
}

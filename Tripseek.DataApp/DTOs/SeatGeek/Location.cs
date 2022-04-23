namespace Tripseek.DataApp.DTOs.SeatGeek
{
    internal class Location
    {

        [JsonPropertyName("lat")]
        public double Latitude { get; set; }
        [JsonPropertyName("lon")]
        public double Longitude { get; set; }
    }
}

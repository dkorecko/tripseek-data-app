namespace Tripseek.DataApp.DTOs.SeatGeek
{
    internal class Venue
    {
        [JsonPropertyName("display_location")]
        public string? DisplayLocation { get; set; }
        public Location? Location { get; set; }
        
    }
}

namespace Tripseek.DataApp.DTOs.SeatGeek
{
    public class Performer
    {
        [JsonPropertyName("image")]
        public string ImageLink { get; set; }
        public string Name { get; set; }
    }
}
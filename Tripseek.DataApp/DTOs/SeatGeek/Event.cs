namespace Tripseek.DataApp.DTOs.SeatGeek
{
    internal class Event
    {
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("datetime_utc")]
        public string DateTimeUtc { get; set; } = string.Empty;
    }
}

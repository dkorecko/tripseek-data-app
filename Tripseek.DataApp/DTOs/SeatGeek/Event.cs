namespace Tripseek.DataApp.DTOs.SeatGeek
{
    internal class Event
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        [JsonPropertyName("datetime_utc")]
        public DateTime? DateTimeUtc { get; set; }
        [JsonPropertyName("enddatetime_utc")]
        public DateTime? EndTimeUtc { get; set; }
        public string? Title { get; set; }
        public Venue? Venue { get; set; }
        public string? Url { get; set; }
        public List<Performer>? Performers { get; set; }    
        public double Popularity { get; set; }
    }
}

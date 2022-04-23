namespace Tripseek.DataApp.DTOs.InternalApi
{
    /// <summary>
    /// Created only by SeatGeekToInternalMapper
    /// </summary>
    internal class Event
    {
        [Key]
        public string Id { get; set; }
        public int ExternalReference { get; set; }
        public string? Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Title { get; set; }
        public string? TicketUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string? DisplayLocation { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

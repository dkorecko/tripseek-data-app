namespace Tripseek.DataApp.DTOs.InternalApi
{
    internal class Event
    {
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

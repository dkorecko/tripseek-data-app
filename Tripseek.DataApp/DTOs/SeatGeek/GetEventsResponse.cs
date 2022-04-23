namespace Tripseek.DataApp.DTOs.SeatGeek
{
    internal class GetEventsResponse
    {
        public List<Event> Events { get; set; } = new List<Event>();
        public MetaData Meta { get; set; }
    }
}

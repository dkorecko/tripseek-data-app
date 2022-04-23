namespace Tripseek.DataApp.Services.Mapper
{
    internal class SeatGeekToInternalMapper
    {
        private const int MAX_KEY_LENGTH = 38;
        public static DTOs.InternalApi.Event Map(DTOs.SeatGeek.Event seatGeekEvent)
        {
            string id = HashGenerator.GetHashString(seatGeekEvent.Id.ToString() + seatGeekEvent.Title).Substring(0,MAX_KEY_LENGTH);
            return new DTOs.InternalApi.Event
            {
                Id = id,
                Title = seatGeekEvent.Title,
                TicketUrl = seatGeekEvent.Url,
                StartDate = seatGeekEvent.DateTimeUtc,
                EndDate = seatGeekEvent.EndTimeUtc,
                ExternalReference = seatGeekEvent.Id,
                DisplayLocation = seatGeekEvent?.Venue?.DisplayLocation,
                Latitude = seatGeekEvent?.Venue?.Location?.Latitude ?? 0,
                Longitude = seatGeekEvent?.Venue?.Location?.Longitude ?? 0,
                Type = seatGeekEvent?.Type,
                ImageUrl = seatGeekEvent?.Performers?.FirstOrDefault()?.ImageLink
            };
        }
    }
}

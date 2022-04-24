namespace Tripseek.DataApp.Services.Mapper
{
    internal class SeatGeekToInternalMapper
    {
        public static DTOs.InternalApi.Event Map(DTOs.SeatGeek.Event seatGeekEvent)
        {
            if (seatGeekEvent == null)
                return new DTOs.InternalApi.Event
                {
                    Id = Guid.NewGuid().ToString(),
                    StartDate = DateTime.MinValue
                };
            
            return new DTOs.InternalApi.Event
            {
                Id = $"seatgeek_{seatGeekEvent?.Id}",
                Title = seatGeekEvent?.Title,
                TicketUrl = seatGeekEvent?.Url,
                StartDate = seatGeekEvent?.DateTimeUtc,
                EndDate = seatGeekEvent?.EndTimeUtc,
                ExternalReference = seatGeekEvent?.Id ?? 0,
                DisplayLocation = seatGeekEvent?.Venue?.DisplayLocation,
                Latitude = seatGeekEvent?.Venue?.Location?.Latitude ?? 0,
                Longitude = seatGeekEvent?.Venue?.Location?.Longitude ?? 0,
                Type = seatGeekEvent?.Type,
                ImageUrl = seatGeekEvent?.Performers?.FirstOrDefault()?.ImageLink,
                Popularity = seatGeekEvent?.Popularity ?? 0,
                ArtistName = seatGeekEvent?.Performers?.FirstOrDefault()?.Name
            };
        }
    }
}

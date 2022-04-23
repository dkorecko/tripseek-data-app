using Tripseek.DataApp.DTOs.SeatGeek;

namespace Tripseek.DataApp.Services
{
    internal class SeatGeekService
    {
        private readonly HttpClient _client;
        private readonly string _clientId = "MjY2ODg5MDN8MTY1MDcxNTEyMy45NzQxNzM";
        private readonly string _clientSecret = "c55fe143f21c96b0885ee96cc701d7dc09f58adacd2ede328a5b60ee8743a7d8";
        public SeatGeekService()
        {
            _client = new HttpClient();
        }

        public async Task<int> GetEventCount()
        {
            LoggingService.Log("Obtaining number of matching events from SeatGeek...");
           
            var path = $"https://api.seatgeek.com/2/events?datetime_utc.gt={DateTime.UtcNow.ToString("yyyy-MM-dd")}&per_page=1&page=1&client_id={_clientId}&client_secret={_clientSecret}";
            LoggingService.LogRequest(HttpMethod.Get, path);
            var response = await _client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            LoggingService.LogShortResponse(response);
            var data = JsonMapper.MapToObject<GetEventsResponse>(await response.Content.ReadAsStringAsync());
            LoggingService.Log("Number of events: " + data.Meta?.Total);
            return data.Meta?.Total ?? 0;
        }

        public async Task<List<Event>> FetchAllEventsAsync(int numberOfEvents)
        {
            var result = new List<Event>();
            const int eventsPerPage = 5000;
            int numberOfPages = (numberOfEvents / eventsPerPage) + 1;
            for (int i=1; i<=numberOfPages; i++)
            {
                LoggingService.Log($"Fetching events page... {i}/{numberOfPages}");
                var response = await GetEvents(i, eventsPerPage);
                result.AddRange(response.Events);
            }

            return result;
        }

        public async Task<GetEventsResponse> GetEvents(int page, int eventsPerPage)
        {
            var path = $"https://api.seatgeek.com/2/events?datetime_utc.gt={DateTime.UtcNow.ToString("yyyy-MM-dd")}&page={page}&per_page={eventsPerPage}&client_id={_clientId}&client_secret={_clientSecret}";
            LoggingService.LogRequest(HttpMethod.Get, path);
            var response = await _client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            LoggingService.LogShortResponse(response);
            var data = JsonMapper.MapToObject<GetEventsResponse>(await response.Content.ReadAsStringAsync());

            if (data == null)
                throw new Exception("Failed to fetch events.");
            
            LoggingService.Log("Successfully fetched events.");
            return data;
        }
    }
}

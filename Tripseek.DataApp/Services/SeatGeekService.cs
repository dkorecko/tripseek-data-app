using Tripseek.DataApp.DTOs.SeatGeek;

namespace Tripseek.DataApp.Services
{
    internal class SeatGeekService
    {
        private readonly HttpClient _client;
        private readonly string _clientId = ConfigurationManager.Configuration.SeatGeekClientId;
        private readonly string _clientSecret = ConfigurationManager.Configuration.SeatGeekClientSecret;
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

        public List<Event> FetchAllEvents(int numberOfEvents)
        {
            var result = new List<Event>();
            List<Task> tasks = new List<Task>();
            const int eventsPerPage = 5000;
            int numberOfPages = (numberOfEvents / eventsPerPage) + 1;
            for (int i=1; i<=numberOfPages; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    using (var client = new HttpClient())
                    {
                        LoggingService.Log($"Fetching events page... {i}/{numberOfPages}");
                        try
                        {
                            var response = await GetEvents(client, i, eventsPerPage);
                            result.AddRange(response.Events);
                        }
                        catch (Exception ex)
                        {
                            LoggingService.LogException(ex);
                            LoggingService.Log("Failed, skipping page.");
                        }
                    }
                }));
                Thread.Sleep(3000);
            }

            Task.WaitAll(tasks.ToArray());

            return result;
        }

        public async Task<GetEventsResponse> GetEvents(HttpClient client, int page, int eventsPerPage)
        {
            var path = $"https://api.seatgeek.com/2/events?datetime_utc.gt={DateTime.UtcNow.ToString("yyyy-MM-dd")}&page={page}&per_page={eventsPerPage}&client_id={_clientId}&client_secret={_clientSecret}";
            LoggingService.LogRequest(HttpMethod.Get, path);
            var response = await client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            LoggingService.LogShortResponse(response);
            var data = JsonMapper.MapToObject<GetEventsResponse>(await response.Content.ReadAsStringAsync());

            if (data == null)
                throw new Exception($"Failed to fetch events for page {page}.");
            
            LoggingService.Log($"Successfully fetched events for page {page}.");
            return data;
        }
    }
}

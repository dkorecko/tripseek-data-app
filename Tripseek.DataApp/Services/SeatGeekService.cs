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

        public async void TestCall()
        {
            var path = $"https://api.seatgeek.com/2/events?client_id={_clientId}&client_secret={_clientSecret}";
            LoggingService.LogRequest(HttpMethod.Get, path);
            var response = await _client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            LoggingService.LogResponse(response);
        }
    }
}

namespace Tripseek;

class Program
{
    static async Task Main()
    {
        LoggingService.Log("Starting Tripseek Data App");

        while (true)
        {
            LoggingService.Log("Initiating update...");

            try
            {
                SeatGeekService seatGeekService = new SeatGeekService();
                int eventCount = await seatGeekService.GetEventCount();
                var events = await seatGeekService.FetchAllEventsAsync(eventCount);
                LoggingService.Log($"Received {events.Count} events.");
                
            }
            catch (Exception ex)
            {
                LoggingService.LogException(ex);
                LoggingService.Log("Terminating current fetch job...");
            }

            var nextRequestTargetTime = DateTime.UtcNow.AddHours(2);
            LoggingService.Log("Targetting next request at " + nextRequestTargetTime + " UTC");
            Thread.Sleep(1000 * 60 * 60 * 2);
        }
    }
}
using AutoMapper;

namespace Tripseek;

class Program
{
    static async Task Main()
    {
        LoggingService.Log("Starting Tripseek Data App");

        while (true)
        {
            LoggingService.Log("Initiating update...");

            List<DataApp.DTOs.InternalApi.Event> events = new List<DataApp.DTOs.InternalApi.Event>();
            
            try
            {
                SeatGeekService seatGeekService = new SeatGeekService();
                int eventCount = await seatGeekService.GetEventCount();
                var seatGeekEvents = await seatGeekService.FetchAllEventsAsync(eventCount);
                LoggingService.Log($"Received {seatGeekEvents.Count} events.");
                LoggingService.Log($"Mapping {seatGeekEvents.Count} events to internal API DTOs.");
                
                foreach(var seatGeekEvent in seatGeekEvents)
                    events.Add(SeatGeekToInternalMapper.Map(seatGeekEvent));
            }
            catch (Exception ex)
            {
                LoggingService.LogException(ex);
                LoggingService.Log("Terminating current fetch job...");
            }
            finally
            {
                LoggingService.Log($"Deploying {events.Count} changed events to database...");
                var dbContext = new AppDbContext();

                dbContext.Events.RemoveRange(dbContext.Events);
                await dbContext.Events.AddRangeAsync(events);
                await dbContext.SaveChangesAsync();
                LoggingService.Log("Database insertion successful.");
            }

            var nextRequestTargetTime = DateTime.UtcNow.AddHours(2);
            LoggingService.Log("Targetting next request at " + nextRequestTargetTime + " UTC");
            Thread.Sleep(1000 * 60 * 60 * 2);
        }
    }
}
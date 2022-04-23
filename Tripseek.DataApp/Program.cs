using AutoMapper;

namespace Tripseek;

class Program
{
    static async Task Main()
    {
        LoggingService.Log("Starting Tripseek Data App");
        ConfigurationManager.Initialize();

        while (true)
        {
            LoggingService.Log("Initiating update...");

            List<DataApp.DTOs.InternalApi.Event> events = new List<DataApp.DTOs.InternalApi.Event>();
            
            try
            {
                SeatGeekService seatGeekService = new SeatGeekService();
                int eventCount = await seatGeekService.GetEventCount();
                var seatGeekEvents = seatGeekService.FetchAllEvents(eventCount);
                LoggingService.Log($"Received {seatGeekEvents.Count} events, mapping to internal API DTOs.");
                
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
                var dbContext = new AppDbContext();
                var allDbEvents = await dbContext.Events.ToListAsync();
                int failed = 0;

                if (!allDbEvents.Any())
                {
                    await dbContext.Events.AddRangeAsync(events);
                }
                else
                {
                    LoggingService.Log($"Deploying {events.Count} changed events to database...");
                    foreach (var eventDto in events)
                    {
                        try
                        {
                            if (allDbEvents.Where(x => x.Id == eventDto.Id).Any())
                            {
                                var targetEvent = allDbEvents.Where(x => x.Id == eventDto.Id).First();
                                dbContext.Events.Update(targetEvent).CurrentValues.SetValues(eventDto);
                            }
                            else
                                await dbContext.Events.AddAsync(eventDto);
                        }
                        catch(Exception ex)
                        {
                            LoggingService.LogException(ex);
                            failed++;
                        }
                    }
                }
                LoggingService.Log("Committing changes...");
                await dbContext.SaveChangesAsync();
                LoggingService.Log($"Database update successful, {failed} changes failed.");
            }

            var nextRequestTargetTime = DateTime.UtcNow.AddHours(2);
            LoggingService.Log("Targetting next request at " + nextRequestTargetTime + " UTC");
            Thread.Sleep(1000 * 60 * 60 * 2);
        }
    }
}
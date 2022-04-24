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

                foreach (var seatGeekEvent in seatGeekEvents)
                {
                    var mappedEvent = SeatGeekToInternalMapper.Map(seatGeekEvent);
                    
                    if(!events.Where(x => x.Id == mappedEvent.Id).Any())
                        events.Add(mappedEvent);
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogException(ex);
                LoggingService.Log("Terminating current fetch job...");
            }
            finally
            {
                List<DataApp.DTOs.InternalApi.Event> allDbEvents = new List<DataApp.DTOs.InternalApi.Event>();
                using (AppDbContext appDbContext = new AppDbContext())
                {
                    allDbEvents = await appDbContext.Events.ToListAsync();
                }
                int failed = 0;

                using(AppDbContext appDbContext = new AppDbContext())
                {
                    if (!allDbEvents.Any())
                    {
                        await appDbContext.Events.AddRangeAsync(events);
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
                                    appDbContext.Events.Update(targetEvent).CurrentValues.SetValues(eventDto);
                                }
                                else
                                    await appDbContext.Events.AddAsync(eventDto);
                            }
                            catch(Exception ex)
                            {
                                LoggingService.LogException(ex);
                                failed++;
                            }
                        }
                    }
                    LoggingService.Log("Committing changes...");
                    await appDbContext.SaveChangesAsync();
                    LoggingService.Log($"Database update successful, {failed} changes failed.");
                }
            }

            var nextRequestTargetTime = DateTime.UtcNow.AddHours(2);
            LoggingService.Log("Targetting next request at " + nextRequestTargetTime + " UTC");
            Thread.Sleep(1000 * 60 * 60 * 2);
        }
    }
}
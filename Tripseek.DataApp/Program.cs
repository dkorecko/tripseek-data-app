LoggingService.Log("Starting Tripseek Data App");

while(true)
{
    LoggingService.Log("Initiating update...");

    try
    {
        SeatGeekService seatGeekService = new SeatGeekService();
        seatGeekService.TestCall();
    }
    catch(Exception ex)
    {
        LoggingService.LogException(ex);
    }
    
    var nextRequestTargetTime = DateTime.UtcNow.AddHours(2);
    LoggingService.Log("Targetting next request at " + nextRequestTargetTime + " UTC");
    Thread.Sleep(1000 * 60 * 60 * 2);
}
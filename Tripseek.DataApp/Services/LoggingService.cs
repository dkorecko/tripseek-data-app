namespace Tripseek.DataApp.Services
{
    internal class LoggingService
    {

        public static void Log(string message)
        {
            Console.WriteLine($"[LOG] {message}");
        }
    }
}

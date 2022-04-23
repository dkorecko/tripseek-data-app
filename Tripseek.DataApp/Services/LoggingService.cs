namespace Tripseek.DataApp.Services
{
    internal class LoggingService
    {
        public static void Log(string message)
        {
            Console.WriteLine($"[{DateTime.UtcNow} UTC] {message}");
        }

        public static void LogRequest(HttpMethod method, string path)
        {
            Log($"Sending {method.Method} request to {path}");
        }

        public async static void LogResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            Log($"Response information: {response}\nBody:\n{content}");
        }

        public static void LogException(Exception exception)
        {
            Log($"Exception occurred: {exception}");
        }
    }
}

using System.Reflection;

namespace Tripseek.DataApp.Database
{
    internal class ConfigurationManager
    {
        public static Configuration Configuration { get; set; }
        public static void Initialize()
        {
            var runningPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string targetPath = Path.Combine(runningPath, "secrets.json");

            var receivedData = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(targetPath));

            if (receivedData == null)
                throw new Exception("Secrets not found.");

            Configuration = receivedData;
        }
    }

    public class Configuration
    {
        public string SeatGeekClientId { get; set; }
        public string SeatGeekClientSecret { get; set; }
        public string ConnectionString { get; set; }

    }
}

namespace Tripseek.DataApp.Services.Mapper
{
    internal class JsonMapper
    {
        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        public static string MapToJson(object obj)
        {
            return JsonSerializer.Serialize(obj, _serializerOptions);
        }

        public static T MapToObject<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _serializerOptions);
        }
    }
}

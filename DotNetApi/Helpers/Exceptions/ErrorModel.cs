using System.Text.Json;

namespace DotNetApi.Helpers.Exceptions
{
    public class ErrorModel
    {
        public string Message { get; set; } = string.Empty;

        public string ToJsonString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

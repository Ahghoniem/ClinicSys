using System.Text.Json;

namespace LibraryMAnagementSystem.API.Errors
{

    public class ApiExceptionResponse : ApiResponse
    {

        public string? Details { get; set; }

        public ApiExceptionResponse(int statusCode, string? message = null, string? details = null) : base(statusCode, message)
        {
            Details = details;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }

}

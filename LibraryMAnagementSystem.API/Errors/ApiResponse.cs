using System.Text.Json;

namespace LibraryMAnagementSystem.API.Errors
{

    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }


        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request - The request could not be understood or was missing required parameters.",
                401 => "Unauthorized - Authentication is required or has failed.",
                404 => "Not Found - The requested resource could not be found.",
                500 => "Internal Server Error - A generic error occurred on the server.",
                _ => "Unknown Status Code - No message available.",
            };
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}

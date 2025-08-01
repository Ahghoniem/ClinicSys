namespace LibraryMAnagementSystem.API.ResponseHandler
{
    public class OldApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        // Constructor for success responses
        public OldApiResponse(int statusCode, string message, T? data = default)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        // Constructor for error responses
        public OldApiResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
            Data = default;
        }
    }
}

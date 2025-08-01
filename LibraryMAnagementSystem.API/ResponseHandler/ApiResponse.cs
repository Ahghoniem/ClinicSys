namespace LibraryMAnagementSystem.API.ResponseHandler
{
    public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public ApiResponse() { }

    public ApiResponse(T data, string message = "Success", int statusCode = 200)
    {
        IsSuccess = true;
        Message = message;
        StatusCode = statusCode;
        Data = data;
    }

    public ApiResponse(List<string> errors, string message = "Validation Failed", int statusCode = 400)
    {
        IsSuccess = false;
        Message = message;
        StatusCode = statusCode;
        Errors = errors;
    }
}

}

using System.Net;

namespace InventoryItems.Helper
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ApiResponse(bool success, string message, object data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ApiResponse SuccessResponse(object data = null)
        {
            return new ApiResponse(true, "Success", data);
        }

        public static ApiResponse ErrorResponse(string message)
        {
            return new ApiResponse(false, message);
        }

        public static ApiResponse HandleUnauthorized(string message = "Unauthorized access")
        {
            return new ApiResponse(false, message) { HttpStatusCode = HttpStatusCode.Unauthorized };
        }

        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;
    }
}

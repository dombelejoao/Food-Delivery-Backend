using System;

namespace FoodDelivery.BusinessLogic.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> SuccessResponse(T data, string message = "")
        => new() { Success = true, Message = message, Data = data };

    public static ApiResponse<T> ErrorResponse(string message)
        => new() { Success = false, Message = message };
}

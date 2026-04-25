using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace military_guard.API.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // 1. Ghi log lỗi để dev vào xem (Rất quan trọng)
            _logger.LogError(exception, "Đã xảy ra lỗi: {Message}", exception.Message);

            // 2. Chuẩn bị khung phản hồi chuẩn ProblemDetails
            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            // 3. Phân loại Exception để trả về Status Code phù hợp
            if (exception is ValidationException fluentException)
            {
                problemDetails.Title = "Dữ liệu đầu vào không hợp lệ.";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Detail = "Vui lòng kiểm tra lại các trường bị lỗi.";

                // Lấy danh sách lỗi chi tiết từ FluentValidation trả về cho Frontend
                problemDetails.Extensions["errors"] = fluentException.Errors
                    .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                    .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
            }
            else if (exception is Exception)
            {
                problemDetails.Title = "Lỗi nghiệp vụ.";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Detail = exception.Message; 
            }
            else
            {
                // Lỗi hệ thống (NullReference, Database sập...)
                problemDetails.Title = "Lỗi hệ thống nội bộ.";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Detail = "Đã có lỗi xảy ra trong quá trình xử lý. Vui lòng thử lại sau.";
            }

            // 4. Trả về response
            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            // Trả về true báo hiệu cho .NET biết: "Tôi đã xử lý lỗi này rồi, đừng crash app nữa"
            return true;
        }
    }
}

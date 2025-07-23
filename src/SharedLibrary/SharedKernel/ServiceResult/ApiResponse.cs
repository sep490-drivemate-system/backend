using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.ServiceResult
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }
        public object? Value { get; set; }

        public ApiResponse() { }

        public ApiResponse(bool isSuccess, string message, object? value = null, string? errorCode = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Value = value;
            ErrorCode = errorCode;
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public new T? Data { get; set; }

        public ApiResponse() { }

        public ApiResponse(bool success, string message, T? data = default, string? errorCode = null)
            : base(success, message, data, errorCode)
        {
            Data = data;
        }
    }
}

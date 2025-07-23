using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.ServiceResult
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public string? Message { get; }
        public T? Data { get; }
        public ServiceError? Error { get; }

        protected Result(bool isSuccess, string? message, T? data, ServiceError? error)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            Error = error;
        }

        public static Result<T> Success(T data, string? message = "Success") =>
            new(true, message, data, null);

        public static Result<T> Failure(ServiceError error, string? message = null) =>
            new(false, message ?? error.Description, default, error);
    }
}

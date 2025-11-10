using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http.DTOs.ApiResponse
{
    public class DefaultApiResponse<T>
    {

        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; } 

        [JsonPropertyName("value")]
        public T? Value { get; set; }
    }
}

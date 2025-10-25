using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Http
{
    public class HttpService
    {
        private  readonly HttpClient _httpClient = new HttpClient();

        public  async Task<T?> GetAsync<T>(string url)
        {

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json)) return default;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(json, options);

        }
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest body)
        {
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var resultJson = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(resultJson)) return default;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<TResponse>(resultJson, options);
        }
    }
}

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
        public readonly HttpClient _httpClient = new HttpClient();

        public  async Task<T?> GetAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP {(int)response.StatusCode}: {errorContent}");
                }

                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json)) return default;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<T>(json, options);
            }
            catch (HttpRequestException)
            {
                throw; // Re-throw HTTP exceptions to be handled by caller
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Request to {url} failed: {ex.Message}", ex);
            }
        }
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest body)
        {
            try
            {
                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP {(int)response.StatusCode}: {errorContent}");
                }

                var resultJson = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(resultJson)) return default;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<TResponse>(resultJson, options);
            }
            catch (HttpRequestException)
            {
                throw; // Re-throw HTTP exceptions to be handled by caller
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Request to {url} failed: {ex.Message}", ex);
            }
        }
    }
}

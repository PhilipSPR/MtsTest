using MtsTest.Interfaces;
using System.Text.Json;

namespace MtsTest.Services
{
    public class HttpBinService : IHttpBinService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public HttpBinService(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> SendImageAsync(string imageBase64)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(new { image = imageBase64 }), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://httpbin.org/post", content);
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogWarning("При отправке изображения возникла ошибка: {0}", ex.Message);
                throw new Exception($"При отправке изображения возникла ошибка: {ex.Message}");
            }
        }
    }
}

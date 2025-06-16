using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace WebApplication1.Service
{

    public class FastApiClient
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public FastApiClient(HttpClient httpClient, IConfiguration config)
        {
            _http = httpClient;
            _config = config;
        }

        public async Task<string> SendImageAsync(string base64 = "", string? imageUrl = null)
        {
            var payload = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(base64))
                payload["image_base64"] = base64;
            else if (!string.IsNullOrEmpty(imageUrl))
                payload["image_url"] = imageUrl;

            var content = new FormUrlEncodedContent(payload);
            var response = await _http.PostAsync(_config["FastApiUrl"], content);
            return await response.Content.ReadAsStringAsync();
        }
    }

}

//public async Task<string?> SendBase64ImageAsync(string base64Image, string fileName)
//{
//    var payload = new { file_name = fileName, base64_data = base64Image };
//    var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

//    var response = await _client.PostAsync("/process-base64", content);
//    response.EnsureSuccessStatusCode();

//    return await response.Content.ReadAsStringAsync();
//}

//public async Task<string?> SendImageUrlAsync(string imageUrl)
//{
//    var payload = new { image_url = imageUrl };
//    var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

//    var response = await _client.PostAsync("/process-url", content);
//    response.EnsureSuccessStatusCode();

//    return await response.Content.ReadAsStringAsync();
//}

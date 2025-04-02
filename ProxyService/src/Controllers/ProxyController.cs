using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProxyApi.Attributes.Auth;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ProxyApi.Controllers
{
    [Route("proxy")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly long _robotId = 1;

        public ProxyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [UserAuthorize]
        [HttpPost("{*path}")]
        [Consumes("application/json", "text/json", "application/*+json", "multipart/form-data")]
        public async Task<IActionResult> ProxyPost(
            string path, 
            [FromBody] object body)
        {
            
            return await HandleProxyRequest(path, body);
        }

        [UserAuthorize]
        [HttpGet("{*path}")]
        public async Task<IActionResult> ProxyGet(string path)
        {
            return await HandleProxyRequest(path, null);
        }

        [UserAuthorize]
        [HttpPut("{*path}")]
        public async Task<IActionResult> ProxyPut(string path, [FromBody] object body)
        {
            return await HandleProxyRequest(path, body);
        }
        
        [UserAuthorize]
        [HttpDelete("{*path}")]
        public async Task<IActionResult> ProxyDelete(string path)
        {
            return await HandleProxyRequest(path, null);
        }

        private class RequestPath {
            public string BaseUrl { get; set; }
            public string Path { get; set; }
            public string Query { get; set; }
        }

        private RequestPath? ParseUrl(string url)
        {

            var decodedPath = Uri.UnescapeDataString(url);
            var pathParts = decodedPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (pathParts.Length <= 1)
            {
                return null;
            }

            // Разбираем URL с помощью Uri
            var uri = new Uri("http://temp.com/" + decodedPath);

            // Получаем отдельные части
            string service = uri.Segments[1].TrimEnd('/');
            string baseUrl = GetBaseUriByService(service);
            if (baseUrl == null)
            {
                return null;
            }
            string path = string.Join(string.Empty, uri.Segments, 2, uri.Segments.Length - 2);
            string query = uri.Query.TrimStart('?');

            return new RequestPath { BaseUrl = baseUrl, Path = path, Query = query };
        }

        private async Task<IActionResult> HandleProxyRequest(string path, object? body)
        {
            var requestPath = ParseUrl(path);

            if (requestPath == null)
            {
                return NotFound();
            }
            // Создаем базовый URI
            var baseUriObj = new Uri(requestPath.BaseUrl);
            
            // Собираем полный URI с учетом QueryString
            var fullUri = new UriBuilder(baseUriObj)
            {
                Path = $"{baseUriObj.AbsolutePath.TrimEnd('/')}/{requestPath.Path}",
                Query = requestPath.Query.TrimStart('?')
            }.Uri;

            var client = _httpClientFactory.CreateClient();
            
            var request = new HttpRequestMessage(
                new HttpMethod(Request.Method),
                fullUri
            );

            // Копирование заголовков
            foreach (var header in Request.Headers)
            {
                if (!request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                {
                    request.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }

            var userId = HttpContext.Items["UserId"] as long?;
            if (userId != null)
            {
                request.Headers.Add("UserId", userId.ToString());
            }
            request.Headers.Add("RobotId", _robotId.ToString());

            // Копирование тела запроса
            if (body != null)
            {
                var jsonBody = JsonSerializer.Serialize(body);
                Console.WriteLine("Request Body in JSON format:");
                Console.WriteLine(jsonBody);

                request.Content = new StringContent(
                    jsonBody,
                    Encoding.UTF8,
                    Request.ContentType ?? "application/json");
            }

            try
            {
                Console.WriteLine(fullUri.AbsolutePath);
                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                
                var result = Content(content, response.Content.Headers.ContentType?.MediaType);
                result.StatusCode = (int)response.StatusCode;
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        private string? GetBaseUriByService(string serviceName)
        {
            // return "http://localhost:5002";
            // TODO: change urls
            return serviceName.ToLower() switch
            {
                "sensors" => "http://sensors_api:5003",
                "processes" => "http://processes_api:5002",
                "statuses" => "http://statuses_api:5001",
                _ => null
            };
        }
    }
}
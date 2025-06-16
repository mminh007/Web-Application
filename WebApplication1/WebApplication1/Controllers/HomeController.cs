using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHttpClientFactory factory, ILogger<HomeController> logger)
        {
            _httpClientFactory = factory;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = TempData["UserId"] as string ?? "";
            var result = TempData["Result"] as string ?? "";
            var message = TempData["Message"] as string ?? "";
            var imageUrl = TempData["ImageUrl"] as string ?? "";

            ViewBag.UserId = userId;
            ViewBag.Result = result;
            ViewBag.Message = message;
            ViewBag.ImageUrl = imageUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewSession()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsync("/api/user/new_session", null);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Failed to create session";
                return RedirectToAction("Index");
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var userId = doc.RootElement.GetProperty("user_id").GetString();

            TempData["UserId"] = userId;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file, string userId)
        {
            if (file == null || string.IsNullOrEmpty(userId))
            {
                TempData["Message"] = "Missing file or user ID";
                return RedirectToAction("Index");
            }

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");

            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream());
            content.Add(fileContent, "file", file.FileName);
            content.Add(new StringContent(userId), "userId");

            var response = await client.PostAsync("/api/image/upload", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            TempData["Result"] = responseJson;
            TempData["Message"] = response.IsSuccessStatusCode ? "Processed" : "Failed";
            TempData["UserId"] = userId;

            return RedirectToAction("Index");
        }
    }
}

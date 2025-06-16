using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Service;
using WebApplication1.Extensions;

namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly RedisService _redis;
        private readonly AppDbContext _db;
        private readonly FastApiClient _client;

        public ImageController(
            IWebHostEnvironment env,
            IConfiguration config,
            RedisService redis,
            AppDbContext db,
            FastApiClient client)
        {
            _env = env;
            _config = config;
            _redis = redis;
            _db = db;
            _client = client;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string userId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file");

            var filename = $"{Guid.NewGuid()}_{file.FileName}";
            var uploadPath = Path.Combine(_env.ContentRootPath, _config["UploadSettings:UploadPath"]!);
            Directory.CreateDirectory(uploadPath);

            string result;
            var maxInlineSize = int.Parse(_config["UploadSettings:MaxInlineSize"]!);

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var bytes = ms.ToArray();
            string hashKey = Convert.ToBase64String(System.Security.Cryptography.MD5.HashData(bytes));

            var cached = await _redis.GetAsync(hashKey);
            if (cached != null)
            {
                return Ok(new ImageUploadResult { Message = "From cache", Result = cached });
            }

            if (bytes.Length <= maxInlineSize)
            {
                string base64 = Convert.ToBase64String(bytes);
                result = await _client.SendImageAsync(base64: base64);
            }
            else
            {
                string fullPath = Path.Combine(uploadPath, filename);
                await System.IO.File.WriteAllBytesAsync(fullPath, bytes);
                string imageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{filename}";
                result = await _client.SendImageAsync(imageUrl: imageUrl);
            }

            // Save metadata

            var metadata = new MetadataImage
            {
                ImageName = filename,
                ResultJson = result
            };

            await _db.SaveMetadataAsync(metadata
            );

            var user = _db.Users.FirstOrDefault(u => u.UserId == userId);

            if (user != null) return BadRequest("Invalid user ID");

            var request = new Request
            {
                UserId = user.Id,
                MetaImage_Id = metadata.MetaImage_ID,
                RequestedAt = DateTime.Now,
            };

            _db.requests.Add(request);
            await _db.SaveChangesAsync();

            await _redis.SetAsync(hashKey, result);

            return Ok(new ImageUploadResult { Message = "Processed", Result = result });
        }
    }


}

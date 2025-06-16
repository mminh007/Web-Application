using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("new_session")]
        public async Task<IActionResult> CreateNewSession()
        {
            var newUser = new User(); // Tự sinh UUID + CreatedAt
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                user_id = newUser.UserId,
                created_at = newUser.CreatedAt
            });
        }
    }

}

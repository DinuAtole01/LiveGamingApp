using Microsoft.AspNetCore.Mvc;
using LiveGamingApp.Data;
using LiveGamingApp.Models;

namespace LiveGamingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
            if (_context.Users.Any(u => u.Email == newUser.Email))
            {
                return BadRequest("Email already exists!");
            }

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok("User registered successfully!");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == loginUser.Email && u.PasswordHash == loginUser.PasswordHash);
            
            if (user == null)
            {
                return Unauthorized("Invalid email or password!");
            }

            return Ok($"Welcome back, {user.Username}! Your Wallet Balance is: {user.WalletBalance}");
        }

        // --- नवीन WALLET API ---
        [HttpPost("update-wallet")]
        public IActionResult UpdateWallet([FromBody] WalletRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
            {
                return NotFound("User not found!");
            }

            user.WalletBalance += request.Amount; // पैसे ॲड किंवा वजा करण्यासाठी
            _context.SaveChanges();

            return Ok($"Wallet updated successfully! New Balance: {user.WalletBalance}");
        }
    }

    // API ला 'Email' आणि 'Amount' समजण्यासाठी बनवलेला छोटा साचा
    public class WalletRequest
    {
        public string Email { get; set; }
        public decimal Amount { get; set; }
    }
}
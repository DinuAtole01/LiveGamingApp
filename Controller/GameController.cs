using Microsoft.AspNetCore.Mvc;
using LiveGamingApp.Data;
using LiveGamingApp.Models;

namespace LiveGamingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GameController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("start-match")]
        public IActionResult StartMatch([FromBody] MatchRequest request)
        {
            var player1 = _context.Users.FirstOrDefault(u => u.Email == request.Player1Email);
            var player2 = _context.Users.FirstOrDefault(u => u.Email == request.Player2Email);

            if (player1 == null || player2 == null) 
                return NotFound("One or both players not found!");

            if (player1.WalletBalance < request.EntryFee || player2.WalletBalance < request.EntryFee)
                return BadRequest("Insufficient wallet balance!");

            player1.WalletBalance -= request.EntryFee;
            player2.WalletBalance -= request.EntryFee;
            _context.SaveChanges();

            return Ok($"Match Started! {request.EntryFee} deducted from both. Pool: {request.EntryFee * 2}");
        }

        [HttpPost("declare-winner")]
        public IActionResult DeclareWinner([FromBody] WinnerRequest request)
        {
            var winner = _context.Users.FirstOrDefault(u => u.Email == request.WinnerEmail);
            if (winner == null) 
                return NotFound("Winner not found!");

            decimal totalPool = request.EntryFee * 2;
            decimal platformCommission = totalPool * 0.10m; // 10% commission
            decimal prizeMoney = totalPool - platformCommission;

            // १. विजेत्याला पैसे देणे
            winner.WalletBalance += prizeMoney;

            // २. मॅचचा रेकॉर्ड 'MatchRecords' टेबलमध्ये सेव्ह करणे (नवीन कोड)
            var matchRecord = new MatchRecord
            {
                Player1Email = request.Player1Email,
                Player2Email = request.Player2Email,
                WinnerEmail = request.WinnerEmail,
                EntryFee = request.EntryFee,
                PrizeMoney = prizeMoney
            };
            _context.MatchRecords.Add(matchRecord);
            
            _context.SaveChanges();

            return Ok($"Match Over! Winner {winner.Username} received {prizeMoney}. Record saved!");
        }

        // ३. मॅच हिस्ट्री पाहण्यासाठी नवीन API 
        [HttpGet("history/{email}")]
        public IActionResult GetUserHistory(string email)
        {
            var history = _context.MatchRecords
                .Where(m => m.Player1Email == email || m.Player2Email == email)
                .OrderByDescending(m => m.PlayedAt)
                .ToList();

            if (!history.Any())
                return NotFound("No match history found for this user.");

            return Ok(history);
        }
    }

    public class MatchRequest
    {
        public string Player1Email { get; set; } = string.Empty;
        public string Player2Email { get; set; } = string.Empty;
        public decimal EntryFee { get; set; }
    }

    public class WinnerRequest
    {
        public string Player1Email { get; set; } = string.Empty;
        public string Player2Email { get; set; } = string.Empty;
        public string WinnerEmail { get; set; } = string.Empty;
        public decimal EntryFee { get; set; }
    }
}
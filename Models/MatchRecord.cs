using System;

namespace LiveGamingApp.Models
{
    public class MatchRecord
    {
        public int Id { get; set; }
        public string Player1Email { get; set; } = string.Empty;
        public string Player2Email { get; set; } = string.Empty;
        public string WinnerEmail { get; set; } = string.Empty;
        public decimal EntryFee { get; set; }
        public decimal PrizeMoney { get; set; }
        public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
    }
}
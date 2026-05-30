using Microsoft.AspNetCore.SignalR;

namespace LiveGamingApp.Hubs
{
    public class GameHub : Hub
    {
        public async Task PlayMove(string user, int position)
        {
            await Clients.All.SendAsync("ReceiveMove", user, position);
        }

        public async Task MoveStriker(string user, double targetX, double targetY)
        {
            await Clients.All.SendAsync("ReceiveStrikerMove", user, targetX, targetY);
        }

        public async Task ShootStriker(string user, double velocityX, double velocityY)
        {
            await Clients.All.SendAsync("ReceiveShoot", user, velocityX, velocityY);
        }

        // --- नवीन सिस्टीम: लाईव्ह पोझिशन सिंक करण्यासाठी ---
        public async Task SyncPositions(string user, double sX, double sY, double qX, double qY)
        {
            await Clients.All.SendAsync("ReceiveSync", user, sX, sY, qX, qY);
        }
    }
}
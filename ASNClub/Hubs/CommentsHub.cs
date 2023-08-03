using Microsoft.AspNetCore.SignalR;

namespace ASNClub.Hubs
{
    public class CommentsHub : Hub
    {
        public async Task UploadComment(string username, string comment, string currentTime)
        {
            await Clients.All.SendAsync("ReceiveComment", username, comment, currentTime);
        }
    }
}

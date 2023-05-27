using Microsoft.AspNetCore.SignalR;

namespace BlazorChatSignalR.Server.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> _users = new Dictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            string username = Context.GetHttpContext().Request.Query["username"];
            _users.Add(Context.ConnectionId, username);
            await SendMessage(string.Empty, $"{username} joined the party!");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
			string username = _users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
			_users.Remove(Context.ConnectionId);
			await SendMessage(string.Empty, $"{username} left the party");
			//await base.OnDisconnectedAsync(exception);
		}

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}

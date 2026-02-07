using Microsoft.AspNetCore.SignalR;

namespace BlueCraeftBowl.Hubs;

public class LeaderboardHub : Hub
{
    public async Task SendLeaderboardUpdate()
    {
        await Clients.All.SendAsync("ReceiveLeaderboardUpdate");
    }
}

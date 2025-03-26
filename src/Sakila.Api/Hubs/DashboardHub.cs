using Microsoft.AspNetCore.SignalR;

namespace Sakila.Api.Hubs;

public class DocumentHub : Hub
{

}

public class DashboardHub : Hub
{
    private readonly ILogger<DashboardHub> logger;

    public DashboardHub(ILogger<DashboardHub> logger)
    {
        this.logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        logger.LogInformation("Connected {ConnectionId}", Context.ConnectionId);

        // TODO: dodać uwierzytelnianie
        // Groups.AddToGroupAsync(Context.ConnectionId, "Blue");

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        logger.LogInformation("Disconnected {ConnectionId}", Context.ConnectionId);

        return base.OnDisconnectedAsync(exception);
    }
}

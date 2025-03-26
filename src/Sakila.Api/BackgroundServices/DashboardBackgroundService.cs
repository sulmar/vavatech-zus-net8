
using Microsoft.AspNetCore.SignalR;
using Sakila.Api.Domain.Models;
using Sakila.Api.Hubs;

namespace Sakila.Api.BackgroundServices;

public class DashboardBackgroundService : BackgroundService
{
    private readonly ILogger<DashboardBackgroundService> logger;
    private readonly IHubContext<DashboardHub> hubContext;

    public DashboardBackgroundService(ILogger<DashboardBackgroundService> logger, IHubContext<DashboardHub> hubContext)
    {
        this.logger = logger;
        this.hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var info = new Info
            {
                ActiveUsers = Random.Shared.Next(0, 10),
                OpenedIssues = Random.Shared.Next(0,  100)
            };

            logger.LogInformation("ActiveUsers: {ActiveUsers} OpenedIssues: {OpenedIssues}", info.ActiveUsers, info.ActiveUsers);

            await hubContext.Clients.All.SendAsync("DashboardChanged", info);

            await Task.Delay(Random.Shared.Next(500, 2000));
        }
    }
}

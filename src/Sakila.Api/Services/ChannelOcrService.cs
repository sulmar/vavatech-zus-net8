using Microsoft.AspNetCore.SignalR;
using Sakila.Api.Hubs;
using System.Threading.Channels;

namespace Sakila.Api.Services;

public class ChannelOcrService : IOcrService
{
    private readonly Channel<IFormFile> files = Channel.CreateUnbounded<IFormFile>();
    private ILogger<BlockingCollectionOcrService> logger;
    private readonly IHubContext<DocumentHub> hubContext;

    public ChannelOcrService(ILogger<BlockingCollectionOcrService> logger, IHubContext<DocumentHub> hubContext)
    {
        this.logger = logger;
        this.hubContext = hubContext;
        Task.Run(() => ProcessFilesAsync());
    }

    public async Task AddAsync(IFormFile file)
    {
        await files.Writer.WriteAsync(file);
    }


    public async Task ProcessFilesAsync()
    {
        await foreach (var file in files.Reader.ReadAllAsync())
        {
            logger.LogInformation("Przetwarzanie pliku {FileName}...", file.FileName);


            await SimulateOcrProcessingAsync(file);

            logger.LogInformation("Przetworzono {FileName}", file.FileName);

            await hubContext.Clients.All.SendAsync("Processed", file.FileName);
        }
    }

    private async Task SimulateOcrProcessingAsync(IFormFile file)
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
    }

}

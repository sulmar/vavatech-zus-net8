using Microsoft.AspNetCore.SignalR;
using Sakila.Api.Hubs;
using System.Collections.Concurrent;

namespace Sakila.Api.Services;

public class OcrService
{
    private BlockingCollection<IFormFile> files = new();

    private ILogger<OcrService> logger;
    private readonly IHubContext<DocumentHub> hubContext;

    public OcrService(ILogger<OcrService> logger, IHubContext<DocumentHub> hubContext)
    {
        this.logger = logger;
        this.hubContext = hubContext;
        Task.Run(() => ProcessFilesAsync());
    }

    public void Add(IFormFile file)
    {
        files.Add(file);
    }

    public async Task ProcessFilesAsync()
    {
        foreach (var file in files.GetConsumingEnumerable())
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

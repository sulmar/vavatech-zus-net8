using Fleet;
using Grpc.Core;
using System.Collections.Concurrent;

namespace Tracking.Api.Services;

public class MyDriverService : Fleet.DriverService.DriverServiceBase
{
    private readonly ConcurrentDictionary<string, IServerStreamWriter<DriverMessage>> _drivers = new ConcurrentDictionary<string, IServerStreamWriter<DriverMessage>>();

    // Kierowca się rejestruje i otrzymuje wiadomości
    public override async Task StreamMessages(DriverRequest request, IServerStreamWriter<DriverMessage> responseStream, ServerCallContext context)
    {
        _drivers[request.DriverId] = responseStream;

        while (!context.CancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1000);
        }

    }

    // Wysyła wiadomość do konkretnego kierowcy
    public async Task SendMessageToDriver(string driverId, string title, string content)
    {
        if (_drivers.TryGetValue(driverId, out var stream))
        {
            await stream.WriteAsync(new DriverMessage {  Title = title, Content = content });
        }
    }
}
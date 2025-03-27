using Grpc.Core;

namespace Tracking.Api.Services;

public class MyTrackingService : TrackingService.TrackingServiceBase
{
    private readonly ILogger<MyTrackingService> _logger;

    public MyTrackingService(ILogger<MyTrackingService> logger)
    {
        _logger = logger;
    }

    public override Task<AddLocationResponse> AddLocation(AddLocationRequest request, ServerCallContext context)
    {
        _logger.LogInformation("PlateNumber: {PlateNumber} ({Latitude},{Longitude}) {Speed} km/h", request.PlateNumber, request.Latitude, request.Longitude, request.Speed);

        var response = new  AddLocationResponse {  IsConfirmed = true };

        return Task.FromResult(response);
    }   
}

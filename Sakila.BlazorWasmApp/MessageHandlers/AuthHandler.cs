
using System.Net.Http.Headers;

namespace Sakila.BlazorWasmApp.MessageHandlers;

public class AuthHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string accessToken = "abc";

        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        return base.SendAsync(request, cancellationToken);
    }
}

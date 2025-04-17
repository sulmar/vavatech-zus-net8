using Sakila.Api.Domain.Abstractions;

namespace Sakila.Api.Infrastructure
{
    public class FakePermissionService : IPermissionService
    {
        public Task<List<string>> GetPermissionsAsync(string username) => Task.FromResult(new List<string>
            {
                "print",
                "export",
                "create"
            });
    }
}

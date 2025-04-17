namespace Sakila.Api.Domain.Abstractions;

public interface IPermissionService
{
    Task<List<string>> GetPermissionsAsync(string username);
}
namespace Sakila.Api.Services;

public interface IOcrService
{
    Task AddAsync(IFormFile file);

}

namespace Sakila.Api.Domain.Models;

public record Info
{
    public int OpenedIssues { get; set; }
    public int ActiveUsers { get; set; }
}

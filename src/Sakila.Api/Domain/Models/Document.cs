namespace Sakila.Api.Domain.Models;

public class Document
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Owner { get; set; }
}

using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;
namespace Sakila.Api.Infrastructure;

public class FakeDocumentRepository : IDocumentRepository
{
    public Document Get(int id)
    {
        return new Document {  
            Id = 1, 
            Title = "Lorem ipsum", 
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", 
            Owner = "Kate" };
    }
}

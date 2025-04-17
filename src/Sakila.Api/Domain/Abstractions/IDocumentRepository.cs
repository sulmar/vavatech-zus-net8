using Sakila.Api.Domain.Models;

namespace Sakila.Api.Domain.Abstractions;

public interface IDocumentRepository
{
    Document Get(int id);    
}
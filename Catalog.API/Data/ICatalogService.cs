using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;
public interface ICatalogService
{
    IMongoCollection<Product> Products { get; set; }
}

using Catalog.API.Entities;

namespace Catalog.API.Repository;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProducts();
    Task<Product> GetProduct(string id);
    Task<bool> UpdateProduct(Product product);
    Task<bool> DeleteProduct(string id);
    Task CreateProduct(Product product);
}


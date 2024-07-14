using OpdrachtAPI.Models;

namespace OpdrachtAPI.Services;

public interface IProductService
{
    Task<Product?> GetAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task AddAsync(Product product);
    Task<Product?> UpdateAsync(Product product);
    Task<bool> DeleteAsync(Guid id);
}

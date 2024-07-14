using Microsoft.EntityFrameworkCore;
using OpdrachtAPI.Models;

namespace OpdrachtAPI.Services;

public class ProductService : IProductService
{
    private readonly OpdrachtAPIContext _context;

    public ProductService(OpdrachtAPIContext context)
        => _context = context;

    public async Task<Product?> GetAsync(Guid id)
        => await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<Product>> GetAllAsync()
        => await _context.Products.ToListAsync();

    public async Task AddAsync(Product product)
    {
        try
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Product?> UpdateAsync(Product product)
    {
        try
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            Product? product = await _context.Products
                .Where(product => product.Id == id)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

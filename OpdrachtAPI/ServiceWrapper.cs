using OpdrachtAPI.Services;

namespace OpdrachtAPI;

public class ServiceWrapper : IServiceWrapper
{
    private OpdrachtAPIContext _context;

    private IProductService? _productService;

    public ServiceWrapper(OpdrachtAPIContext context)
    {
        _context = context;
    }

    public IProductService Products
    {
        get
        {
            if (_productService == null)
            {
                _productService = new ProductService(_context);
            }

            return _productService;
        }
    }
}

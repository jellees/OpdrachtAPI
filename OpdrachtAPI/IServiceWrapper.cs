using OpdrachtAPI.Services;

namespace OpdrachtAPI;

public interface IServiceWrapper
{
    IProductService Products { get; }
}

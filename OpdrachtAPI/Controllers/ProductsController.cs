using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpdrachtAPI.Models;

namespace OpdrachtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IServiceWrapper _service;
        private readonly IOpdrachtAPIMetrics _metrics;

        public ProductsController(IServiceWrapper service, IOpdrachtAPIMetrics metrics)
        {
            _service = service;
            _metrics = metrics;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _service.Products.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _service.Products.GetAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _service.Products.UpdateAsync(product);

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            await _service.Products.AddAsync(product);
            _metrics.ProductAdded(1);
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _service.Products.DeleteAsync(id);

            return NoContent();
        }
    }
}

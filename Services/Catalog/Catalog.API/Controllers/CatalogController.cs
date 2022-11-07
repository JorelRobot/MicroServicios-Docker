using Catalog.API.Entities;
using Catalog.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await productRepository.CreateProduct(product);
            return Ok();
        }


        //como es una linea se puede usar flecha
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
            => Ok(await productRepository.GetProducts());
        

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await productRepository.GetProduct(id);

            if(product == null) return NotFound();

            return Ok(product);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product, string id)
        {
            var dbProduct = await productRepository.GetProduct(id);

            if (dbProduct is null) return BadRequest();

            product.Id = id;
            await productRepository.UpdateProduct(product);
            return Ok();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            var dbProduct = await productRepository.GetProduct(id);

            if (dbProduct is null) return BadRequest();

            await productRepository.DeleteProduct(id);
            return Ok();
        }
    }
}

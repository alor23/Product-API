using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products_api.Models;

namespace Products_api.Controllers
{
    [Produces("application/json")]
    [Route("Product")]
    public class ProductController : Controller
    {
        private readonly ProductsDBContext _context;

        public ProductController(ProductsDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get all products
        /// </summary>
        // GET: Product
        [HttpGet]
        public IEnumerable<Product> GetProduct()
        {
            return _context.Product;
        }
        /// <summary>
        /// Get product by id
        /// </summary>
        /// <response code = "400">Bad request</response>
        /// <response code = "404">Product not found</response>
        // GET: Product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Product.SingleOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        /// <summary>
        /// Update product by id
        /// </summary>
        /// <response code = "400">Bad request</response>
        /// <response code = "404">Product not found</response>
        // PUT: Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] Guid id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != product.Id)
            {
                return BadRequest("Product id is required");
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Product updated "+product.Id);
        }
        /// <summary>
        /// Create product
        /// </summary>
        /// <response code = "400">Bad request</response>
        // POST: Product
        [HttpPost]
        public async Task<IActionResult> GuidPostProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (ProductExists(product.Id) == true)
            {
                return BadRequest("Product with id "+product.Id+" alredy exists");
            }
            else
            {
                _context.Product.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProduct", new { id = product.Id }, "Product created with id " + product.Id);
            }
        }
        /// <summary>
        /// Delete product
        /// </summary>
        /// <response code = "400">Bad request</response>
        /// <response code = "404">Product not found</response>
        // DELETE: Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Product.SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product with id deleted " + product.Id);
        }

        private bool ProductExists(Guid id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
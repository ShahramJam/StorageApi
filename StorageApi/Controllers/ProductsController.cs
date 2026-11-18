using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageApi.Models;
using StorageApi.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using StorageApi.DTOs;

namespace StorageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(StorageApiContext context) : ControllerBase
    {
        private readonly StorageApiContext _context = context;

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            return await _context.Product.Select(p => p.ToProductDto()).ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product.ToProductDto();
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int? id, UpdateProductDto product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product.ToEntity()).State = EntityState.Modified;

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

            return NoContent();
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(CreateProductDto product)
        {
            var entity = product.ToEntity();
            _context.Product.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = entity.Id }, entity);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int? id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
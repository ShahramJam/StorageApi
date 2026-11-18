using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageApi.Data;
using StorageApi.DTOs;
using StorageApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(StorageApiContext context) : ControllerBase
    {
        private readonly StorageApiContext _context = context;

        //// GET: api/Products
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        //{
        //    return await _context.Products.Select(p => p.ToProductDto()).ToListAsync();
        //}

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product.ToProductDto();
        }

        // GET: api/Products , GET: api/products?category=KategoriNamn
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] string? category)
        {
            IQueryable<Product> query = _context.Products.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(category))
            {
                category = category.Trim().ToLower();
                query = query.Where(p => p.Category == category);
            }

            return await query.Select(p => p.ToProductDto()).ToListAsync();
        }
        // GET: api/Products/stats
        [HttpGet("stats")]
        public async Task<ActionResult<ProductStatsDto>> GetProductStats()
        {
            var stats = await _context.Products.AsNoTracking().GroupBy(_ => 1) // Grupperar alla rader till en enda SQL-fråga
                .Select(g => new ProductStatsDto(
                    g.Count(),
                    g.Sum(p => p.Price * p.Count),
                    g.Average(p => p.Price)
                )).FirstOrDefaultAsync();

            // Om tabellen är helt tom returneras null; skicka då tillbaka nollvärden
            return Ok(stats ?? new ProductStatsDto(0, 0, 0));
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
            _context.Products.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = entity.Id }, entity);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int? id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
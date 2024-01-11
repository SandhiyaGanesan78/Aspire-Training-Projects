using bloomApiProject.Data;
using bloomApiProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace bloomApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly bloomApiProjectDbContext _context;

        public ProductsController(bloomApiProjectDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
             try
            {
                var products = await _context.Products.ToListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching products.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> addProduct([FromBody] Products product){
            if (ModelState.IsValid)
    {
        try
        {
            product.id = Guid.NewGuid();
           
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return Ok(product); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while adding the product.");
        }
    }

    return BadRequest(ModelState);
}





        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> updateProduct([FromRoute] Guid id, Products products)
        {
            var product= await _context.Products.FindAsync(id);
            if(product==null)
            {
                return NotFound();
            }
            product.name=products.name;
            product.price=products.price;
            product.category=products.category;
            product.description=products.description;
            product.image=products.image;

            await _context.SaveChangesAsync();
            return Ok(product);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> deleteProduct([FromRoute] Guid id)
        {
            var product=await _context.Products.FindAsync(id);
            if(product==null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }
        [HttpGet]
        [Route("carousel")]
        public async Task<IActionResult> CarouselProducts()
        {
            var products = await _context.Products.Take(3).ToListAsync();
            return Ok(products);
        }
        [HttpGet]
        [Route("trendy")]
        public async Task<IActionResult> TrendyProducts()
        {
            var products = await _context.Products.Take(8).ToListAsync();
            return Ok(products);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string query)
        {
            var products = await _context.Products
            .Where(p => p.name.Contains(query) || p.description.Contains(query))
            .Take(10) 
            .ToListAsync();
            if (products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }
        [HttpGet]
        [Route("category")]
        public async Task<ActionResult<IEnumerable<Products>>> GetCategoryProducts(string category)
        {
            IQueryable<Products> query = _context.Products;
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.category.ToLower() == category.ToLower());
            }
            var products = await query.ToListAsync();
            return Ok(products);
        }
    }
}

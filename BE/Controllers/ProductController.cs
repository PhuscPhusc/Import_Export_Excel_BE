using BE.Database;
using BE.dto;
using BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/product
        //[HttpPost("")]
        //public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        //{
        //    if (request.Products == null || request.Products.Count == 0)
        //        return BadRequest("Danh sách sản phẩm rỗng");

        //    var entities = request.Products.Select(p => new Product
        //    {
        //        ProductName = p.ProductName,
        //        Description = p.Description,
        //        Price = p.Price,
        //        Stock = p.Stock,
        //        Images = p.Images,
        //        CreatedAt = p.CreatedAt,
        //        IsActive = p.IsActive
        //    }).ToList();

        //    _context.Products.AddRange(entities);
        //    await _context.SaveChangesAsync();

        //    return Ok(entities);
        //}
        // POST: api/product
        [HttpPost("")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            if (request.Products == null || request.Products.Count == 0)
                return BadRequest("Danh sách sản phẩm rỗng");

            var dt = new DataTable();
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("Stock", typeof(int));
            dt.Columns.Add("Images", typeof(string));
            dt.Columns.Add("CreatedAt", typeof(DateTime));
            dt.Columns.Add("IsActive", typeof(bool));

            foreach (var p in request.Products)
            {
                dt.Rows.Add(p.ProductName, p.Description, p.Price, p.Stock, p.Images, p.CreatedAt, p.IsActive);
            }

            var conn = _context.Database.GetDbConnection().ConnectionString;
            using (var sqlBulk = new SqlBulkCopy(conn))
            {
                sqlBulk.DestinationTableName = "Product"; // tên bảng

                sqlBulk.ColumnMappings.Add("ProductName", "ProductName");
                sqlBulk.ColumnMappings.Add("Description", "Description");
                sqlBulk.ColumnMappings.Add("Price", "Price");
                sqlBulk.ColumnMappings.Add("Stock", "Stock");
                sqlBulk.ColumnMappings.Add("Images", "Images");
                sqlBulk.ColumnMappings.Add("CreatedAt", "CreatedAt");
                sqlBulk.ColumnMappings.Add("IsActive", "IsActive");
                await sqlBulk.WriteToServerAsync(dt);
            }

            return Ok(new { inserted = request.Products.Count });
        }

        // GET: api/Product/products
        //[HttpGet("")]
        //public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        //{
        //    var products = await _context.Products.ToListAsync();
        //    return Ok(products);
        //}
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts(
              [FromQuery] int page = 1,   
              [FromQuery] int limit = 10   
        )
        {
            if (page <= 0) page = 1;
            if (limit <= 0) limit = 10;

            // Tổng số sản phẩm
            var totalItems = await _context.Products.CountAsync();

            var totalPages = (int)Math.Ceiling(totalItems / (double)limit);

            var products = await _context.Products
                .OrderBy(p => p.Id)                 // sắp xếp để phân trang ổn định
                .Skip((page - 1) * limit)     
                .Take(limit)                       
                .ToListAsync();

            return Ok(new
            {
                page,
                limit,
                totalItems,
                totalPages,
                data = products
            });
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

    }
}

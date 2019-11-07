using Microsoft.EntityFrameworkCore;

namespace Products_api.Models
{
    public class ProductsDBContext :DbContext
    {
        public ProductsDBContext(DbContextOptions<ProductsDBContext> options) : base(options) { }

        public DbSet<Product> Product { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace RestfulApiExample.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private static List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 10.0 },
            new Product { Id = 2, Name = "Product 2", Price = 20.0 },
            new Product { Id = 3, Name = "Product 3", Price = 30.0 }
        };

        // Ürünleri listeleme ve sıralama
        [HttpGet("list")]
        public ActionResult<IEnumerable<Product>> GetProducts([FromQuery] string name)
        {
            IEnumerable<Product> filteredProducts = products;
            if (!string.IsNullOrEmpty(name))
            {
                filteredProducts = products.Where(p => p.Name.ToLower().Contains(name.ToLower()));
            }

            List<Product> sortedProducts = filteredProducts.OrderBy(p => p.Name).ToList();
            return Ok(sortedProducts);
        }

        // Yeni ürün ekleme
        [HttpPost]
        public ActionResult<Product> AddProduct([FromBody] Product product)
        {
            if (product == null || string.IsNullOrEmpty(product.Name) || product.Price <= 0)
            {
                return BadRequest("Invalid product data.");
            }

            product.Id = products.Max(p => p.Id) + 1;
            products.Add(product);

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // Belirli bir ürünü görüntüleme
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            Product product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // Belirli bir ürünü güncelleme
        [HttpPut("{id}")]
        public ActionResult<Product> UpdateProduct(int id, [FromBody] Product product)
        {
            if (product == null || string.IsNullOrEmpty(product.Name) || product.Price <= 0)
            {
                return BadRequest("Invalid product data.");
            }

            Product existingProduct = products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;

            return Ok(existingProduct);
        }

        // Belirli bir ürünü silme
        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            Product product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            products.Remove(product);
            return NoContent();
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

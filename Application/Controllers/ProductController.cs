using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        // CRUD operations for products

        private readonly List<Product> _products = new List<Product>();
        private readonly List<User> _users = new List<User>();


        // GET: api/products
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public IActionResult GetProductById(string id)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            return Ok(product);
        }


        // Only sellers can create products
        // POST: api/products
        [HttpPost]
        [Authorize(Roles = "seller")]
        public IActionResult CreateProduct([FromBody] Product newProduct)
        {
            // Find the user who is creating the product
            var seller = _users.FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);
            if (seller == null)
            {
                return BadRequest("Seller not found");
            }

            // Assign a unique ProductId (replace with a proper mechanism in production)
            newProduct.ProductId = Guid.NewGuid().ToString();
            newProduct.SellerId = seller.UserId; // Set the seller ID

            _products.Add(newProduct);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.ProductId }, newProduct);
        }

        // Only sellers can update their own products
        // PUT: api/products/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "seller")]
        public IActionResult UpdateProduct(string id, [FromBody] Product updatedProduct)
        {
            var existingProduct = _products.FirstOrDefault(p => p.ProductId == id);
            if (existingProduct == null)
            {
                return NotFound("Product not found");
            }

            // Check if the authenticated user is the seller of the product
            var seller = _users.FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);
            if (seller == null || existingProduct.SellerId != seller.UserId)
            {
                return Forbid("You don't have permission to update this product");
            }

            // Update product information
            existingProduct.AmountAvailable = updatedProduct.AmountAvailable;
            existingProduct.Cost = updatedProduct.Cost;
            existingProduct.ProductName = updatedProduct.ProductName;

            return Ok(existingProduct);
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "seller")] // Only sellers can delete their own products
        public IActionResult DeleteProduct(string id)
        {
            var productToDelete = _products.FirstOrDefault(p => p.ProductId == id);
            if (productToDelete == null)
            {
                return NotFound("Product not found");
            }

            // Check if the authenticated user is the seller of the product
            var seller = _users.FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);
            if (seller == null || productToDelete.SellerId != seller.UserId)
            {
                return Forbid("You don't have permission to delete this product");
            }

            _products.Remove(productToDelete);
            return NoContent();
        }

    }

}

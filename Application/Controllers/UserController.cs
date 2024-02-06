using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
       
        private readonly List<User> _users = new List<User>();
        private readonly List<Product> _products = new List<Product>();

        // GET: api/users
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public IActionResult GetUserById(string id)
        {
            var user = _users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        // No authentication required for registration
        // POST: api/users
        [HttpPost]
        [AllowAnonymous] 
        public IActionResult CreateUser([FromBody] User newUser)
        {
            // Check if the username exists
            if (_users.Any(u => u.Username == newUser.Username))
            {
                return BadRequest("Username already taken");
            }

            // Assign a unique UserId 
            newUser.UserId = Guid.NewGuid().ToString();


            _users.Add(newUser);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.UserId }, newUser);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "seller")] // Only sellers can update user information
        public IActionResult UpdateUser(string id, [FromBody] User updatedUser)
        {
            var existingUser = _users.FirstOrDefault(u => u.UserId == id);
            if (existingUser == null)
            {
                return NotFound("User not found");
            }

            // Update user information
            existingUser.Username = updatedUser.Username;
            existingUser.Password = updatedUser.Password;
            existingUser.Deposit = updatedUser.Deposit;
            existingUser.Role = updatedUser.Role;

            return Ok(existingUser);
        }

        // DELETE: api/users/{id}
        // Only sellers can delete users
        [HttpDelete("{id}")]
        [Authorize(Roles = "seller")] 
        public IActionResult DeleteUser(string id)
        {
            var userToDelete = _users.FirstOrDefault(u => u.UserId == id);
            if (userToDelete == null)
            {
                return NotFound("User not found");
            }

            _users.Remove(userToDelete);
            return NoContent();
        }

       
        /// ////////////////


        [Authorize(Roles = "buyer")]
        [HttpPost("reset")]
        public IActionResult ResetDeposit()
        {
            var buyer = _users.FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);
            if (buyer == null)
            {
                return BadRequest("Buyer not found");
            }

            // Reset deposit to zero
            buyer.Deposit = 0;


            return Ok("Deposit reset successfully.");
        }

        // POST: api/users/deposit
        [HttpPost("deposit")]
        [Authorize(Roles = "buyer")]
        public IActionResult DepositCoins([FromBody] DepositRequest depositRequest)
        {
            // Find the authenticated user
            var buyer = _users.FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);
            if (buyer == null)
            {
                return BadRequest("Buyer not found");
            }

            // Deposit logic
            buyer.Deposit += depositRequest.Amount;

            return Ok($"Deposited {depositRequest.Amount} cents. Total deposit: {buyer.Deposit} cents");
        }

        //////////////////////
        ///

        // POST: api/users/buy
        [HttpPost("buy")]
        [Authorize(Roles = "buyer")]
        public IActionResult BuyProducts([FromBody] BuyRequest buyRequest)
        {
            // Find the authenticated user
            var buyer = _users.FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);
            if (buyer == null)
            {
                return BadRequest("Buyer not found");
            }

            // Find the product to buy
            var productToBuy = _products.FirstOrDefault(p => p.ProductId == buyRequest.ProductId);
            if (productToBuy == null)
            {
                return BadRequest("Product not found");
            }

            // Check if the buyer has enough deposit
            if (buyer.Deposit < productToBuy.Cost * buyRequest.Amount)
            {
                return BadRequest("Insufficient funds");
            }

            // Calculate total amount spent
            float? totalSpent = productToBuy.Cost * buyRequest.Amount;

            // Update buyer's deposit
            buyer.Deposit -= totalSpent;

            // Generate change in 5, 10, 20, 50, and 100 cent coins
            float? change = buyer.Deposit;
            int[] coins = { 100, 50, 20, 10, 5 };
            Dictionary<int, int> changeCoins = new Dictionary<int, int>();

            foreach (var coin in coins)
            {
                int coinCount = (int)(change / coin);
                if (coinCount > 0)
                {
                    changeCoins.Add(coin, coinCount);
                    change -= coinCount * coin;
                }
            }

        
            // Prepare response
            var response = new
            {
                TotalSpent = totalSpent,
                ProductsPurchased = $"{buyRequest.Amount} x {productToBuy.ProductName}",
                Change = changeCoins
            };

            return Ok(response);
        }





    }

}


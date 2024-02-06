using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class User
    {

        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public float? Deposit { get; set; }
        public string Role { get; set; }

    }
}


using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Index(nameof(UserName),  IsUnique = true)]
    public class AppUser
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Role { get; set; }
        
        public required string HashedPassword { get; set; }
    }
}
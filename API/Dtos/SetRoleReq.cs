
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class SetRoleReq
    {
        [Required]
        // Validointi hylkää muut arvot jo ennen kuin actionia ajetaan
        [RegularExpression("^(user|staff|admin)$", ErrorMessage = "role must be user, staff or admin")]
        public required string Role { get; set; }
    }
}
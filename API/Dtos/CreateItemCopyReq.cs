
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CreateItemCopyReq
    {
        // Inventaarionumero, esim. "CAM-001"
        [Required]
        [MinLength(1), MaxLength(50)]
        public required string InventoryCode { get; set; }
    }
}
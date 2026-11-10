
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    [Index(nameof(InventoryCode), IsUnique = true)]
    public class ItemCopy
    {
        public int Id { get; set; }

        // Inventaarionumero, esim. "CAM-001"
        public required string InventoryCode { get; set; }

        public int ItemId { get; set; }
        public Item? Item { get; set; }
    }
}
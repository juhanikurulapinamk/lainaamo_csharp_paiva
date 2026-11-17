
namespace API.Dtos
{
    public class ItemCopyDto
    {
        public int Id { get; set; }
        public required string InventoryCode { get; set; }
        public int ItemId { get; set; }
    }
}
using API.Models;

namespace API.Dtos;

public class ItemCopyDto
{
    public int Id { set; get; }
    public required string InventoryCode { set; get; }

    
}
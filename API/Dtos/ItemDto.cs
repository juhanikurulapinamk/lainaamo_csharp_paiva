
namespace API.Dtos
{
    public class ItemDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        
        public CategoryDto? Category {get; set; }

        public int CopyCount { get; set; }
    }
}
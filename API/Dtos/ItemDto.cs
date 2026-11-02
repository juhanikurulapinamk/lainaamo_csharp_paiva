
namespace API.Dtos
{
    public class ItemDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        // Kategorian nimi, ei koko kategoriaoliota
        public required string CategoryName { get; set; }

        public int CopyCount { get; set; }
    }
}
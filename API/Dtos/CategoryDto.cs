
namespace API.Dtos
{
    // Tämä lähtee ulos rajapinnasta
    public class CategoryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
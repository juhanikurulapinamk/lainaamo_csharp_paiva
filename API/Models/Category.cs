
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    // Nimen pitää olla yksilöllinen: kahta "Kamerat"-kategoriaa ei ole järkeä olla
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        // Muista käyttää Id-propertysta juuri tätä nimeä.
        // EF Core tekee Id-nimisestä propertysta automaattisesti
        // tietokantataulun perusavaimen
        public int Id { get; set; }

        public required string Name { get; set; }
    }
}
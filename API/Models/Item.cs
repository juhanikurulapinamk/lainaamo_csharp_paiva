
namespace API.Models
{
    public class Item
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        // Tämä on viiteavain (foreign key).
        // Nimeämiskäytäntö on <Navigaatioproperty><Perusavain>,
        // eli Category + Id = CategoryId.
        // Kun noudatat sitä, EF Core tunnistaa relaation automaattisesti.
        public int CategoryId { get; set; }

        // Tämä on navigaatioproperty.
        // Sitä EI ole tietokannassa sarakkeena — se on olemassa
        // vain koodissa, jotta pääset kategoriaan käsiksi nimikkeen kautta.
        public Category? Category { get; set; }
        
        // Item.cs
        public ICollection<ItemCopy> Copies { get; set; } = new List<ItemCopy>();
    }
}
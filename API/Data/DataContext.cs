
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    // Propertyn nimestä tulee tietokantataulun nimi.
    // Tässä siis Categories.
    public DbSet<Category> Categories { get; set; }
}
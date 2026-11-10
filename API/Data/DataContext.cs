
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    // Propertyn nimestä tulee tietokantataulun nimi.
    // Tässä siis Categories.
    public DbSet<Category> Categories { get; set; }
    public DbSet<Item> Items { get; set; }

    
    // DataContext.cs
    public DbSet<ItemCopy> ItemCopies { get; set; }
}
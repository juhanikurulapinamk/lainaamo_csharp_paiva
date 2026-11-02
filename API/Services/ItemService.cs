using API.CustomExceptions;
using API.Data;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class ItemService(DataContext _repository) : IItemService
{
    public async Task<Item> Create(CreateItemReq requestData)
    {
        // Viiteavaimen pitää osoittaa olemassa olevaan kategoriaan.
        // Ilman tätä tarkistusta tietokanta heittäisi viiteavainvirheen, josta tulisi 500.
        var categoryExists = await _repository.Categories
            .AnyAsync(c => c.Id == requestData.CategoryId);

        if (!categoryExists)
        {
            throw new NotFoundException($"category {requestData.CategoryId} not found");
        }

        var item = new Item
        {
            Name = requestData.Name,
            Description = requestData.Description,
            CategoryId = requestData.CategoryId
        };

        await _repository.Items.AddAsync(item);
        await _repository.SaveChangesAsync();

        // Haetaan tallennettu nimike uudelleen, jotta kategoria ja kappaleet tulevat mukaan
        return await GetById(item.Id);
    }

    public async Task<IEnumerable<Item>> GetAll()
    {
        return await _repository.Items.Include(i => i.Category).ToListAsync();
    }

    public async Task<Item> GetById(int id)
        {
            var item = await _repository.Items
                .Include(i => i.Category)
                
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                throw new NotFoundException($"item {id} not found");
            }

            return item;
        }
}
using API.CustomExceptions;
using API.Data;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class ItemService(DataContext _repository) : IItemService
{

    // Sama tarkistus tarvitaan kahdessa metodissa, joten se on omassa metodissaan
    private async Task EnsureNameIsFree(string inventoryCode, int? exceptId = null)
    {
        var taken = await _repository.ItemCopies
            .AnyAsync(ic => ic.InventoryCode == inventoryCode && ic.Id != exceptId);

        if (taken)
        {
            throw new BusinessRuleException($"copy {inventoryCode} already exists");
        }
    }

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

    public async Task<ItemCopy> CreateCopy(int id, CreateItemCopyReq requestData)
    {
        await EnsureNameIsFree(requestData.InventoryCode);

        var itemCopy = new ItemCopy
        {
            InventoryCode = requestData.InventoryCode,
            ItemId = id
        };

        await _repository.ItemCopies.AddAsync(itemCopy);
        await _repository.SaveChangesAsync();

        return itemCopy;
    }

    public async Task<IEnumerable<Item>> GetAll()
    {
        return await _repository.Items
        .Include(i => i.Category)
        .Include(i => i.Copies)
        .ToListAsync();
    }

    public async Task<Item> GetById(int id)
        {
            var item = await _repository.Items
                .Include(i => i.Category)
                .Include(i => i.Copies)
                
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                throw new NotFoundException($"item {id} not found");
            }

            return item;
        }
}
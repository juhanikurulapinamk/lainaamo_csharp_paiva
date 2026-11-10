using API.CustomExceptions;
using API.Data;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;



public class CategoryService(DataContext _repository) : ICategoryService
{

    // Sama tarkistus tarvitaan kahdessa metodissa, joten se on omassa metodissaan
    private async Task EnsureNameIsFree(string name, int? exceptId = null)
    {
        var taken = await _repository.Categories
            .AnyAsync(c => c.Name == name && c.Id != exceptId);

        if (taken)
        {
            throw new BusinessRuleException($"category {name} already exists");
        }
    }

    public async Task<Category> Create(CreateCategoryReq requestData)
    {

        await EnsureNameIsFree(requestData.Name);

        var category = new Category { Name = requestData.Name };

        // EF Coressa rivi lisätään ensin muutoslistaan
        await _repository.Categories.AddAsync(category);
        // ja tallennetaan vasta SaveChangesAsyncissa
        await _repository.SaveChangesAsync();

        return category;
    }

    public async Task Delete(int id)
    {

        var hasItems = await _repository.Items.AnyAsync(i => i.CategoryId == id);

        if (hasItems)
        {
            throw new BusinessRuleException("category cannot be deleted while it has items");
        }

        var category = await GetById(id);

        _repository.Categories.Remove(category);
        await _repository.SaveChangesAsync();
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        return await _repository.Categories.ToListAsync();
    }

    public async Task<Category> GetById(int id)
    {
        // FirstOrDefaultAsync palauttaa null, jos riviä ei löydy.
        // FirstAsync heittäisi poikkeuksen.
        var category = await _repository.Categories.FirstOrDefaultAsync(c => c.Id == id) ?? throw new NotFoundException($"category {id} not found");
        return category;
    }

    public async Task<Category> Update(int id, CreateCategoryReq requestData)
    {
        var category = await GetById(id);

        // tämä on uusi. Oma id jätetään pois, jotta nimen voi tallentaa ennallaan.
        await EnsureNameIsFree(requestData.Name, id);

        category.Name = requestData.Name;

        // Huomaa: muokattaessa ei tarvita AddAsyncia.
        // EF Core seuraa hakemaansa oliota ja huomaa muutoksen itse.
        await _repository.SaveChangesAsync();

        return category;
    }

    
}
using API.CustomExceptions;
using API.Data;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class CategoryService(DataContext _repository) : ICategoryService
{
    public async Task<Category> Create(CreateCategoryReq requestData)
    {
        var category = new Category { Name = requestData.Name };

        // EF Coressa rivi lisätään ensin muutoslistaan
        await _repository.Categories.AddAsync(category);
        // ja tallennetaan vasta SaveChangesAsyncissa
        await _repository.SaveChangesAsync();

        return category;
    }

    public async Task Delete(int id)
    {
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

        category.Name = requestData.Name;

        // Huomaa: muokattaessa ei tarvita AddAsyncia.
        // EF Core seuraa hakemaansa oliota ja huomaa muutoksen itse.
        await _repository.SaveChangesAsync();

        return category;
    }

    
}
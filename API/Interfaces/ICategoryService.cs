
using API.Dtos;
using API.Models;

namespace API.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(int id);
        Task<Category> Create(CreateCategoryReq requestData);
        Task<Category> Update(int id, CreateCategoryReq requestData);
        Task Delete(int id);
    }
}
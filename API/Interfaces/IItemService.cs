
using API.Dtos;
using API.Models;

namespace API.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAll();
        Task<Item> GetById(int id);
        Task<Item> Create(CreateItemReq requestData);
    }
}
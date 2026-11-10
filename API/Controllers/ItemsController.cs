using API.CustomExceptions;
using API.Dtos;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ItemsController(IItemService _itemService) : LainaamoControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ItemDto>>> GetAll()
    {
        var items = await _itemService.GetAll();
        var dtos = items.Select(i => new ItemDto
        {
            Id = i.Id,
            Name = i.Name,
            Description = i.Description,
            CopyCount = i.Copies.Count,
            Category = new CategoryDto
            {
                Id = i.Category!.Id,
                Name = i.Category!.Name
            },
           
           
            
        });
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> Create(CreateItemReq requestData)
    {
        try {
            var item = await _itemService.Create(requestData);
            return Ok(new ItemDto {
                Name = item.Name,
                Category = new CategoryDto
                {
                    Id = item.Category!.Id,
                    Name = item.Category!.Name
                }
            });
        } catch(NotFoundException)
        {
            return NotFound("category not found");
        }
    }
}
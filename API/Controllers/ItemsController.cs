using API.CustomExceptions;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
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

        // GET api/items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetById(int id)
        {
            try
            {
                var item = await _itemService.GetById(id);

                return Ok(ToDto(item));
            }
            catch (NotFoundException e)
            {
                return NotFound(new { title = "item not found", detail = e.Message });
            }
        }

        private static ItemDto ToDto(Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Category = new CategoryDto
                {
                    Id = item.Category!.Id,
                    Name = item.Category!.Name
                },
                CopyCount = item.Copies.Count
            };
        }

    
    // POST api/items/5/copies
    [HttpPost("{id}/copies")]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<ActionResult<ItemCopyDto>> AddCopy(int id, CreateItemCopyReq req)
    {
        try
        {
            var copy = await _itemService.AddCopy(id, req);

            // Location-header osoittaa nimikkeeseen, johon kappale lisättiin
            return CreatedAtAction(nameof(GetById), new { id },
                new ItemCopyDto { Id = copy.Id, InventoryCode = copy.InventoryCode, ItemId = copy.ItemId });
        }
        catch (NotFoundException e)
        {
            return NotFound(new { title = "item not found", detail = e.Message });
        }
        catch (BusinessRuleException e)
        {
            return Conflict(new { title = "cannot add copy", detail = e.Message });
        }
    }

    /*[HttpPost("{id}/copies")]
    public async Task<ActionResult<ItemCopyDto>> CreateCopy(int id, CreateItemCopyReq requestData)
    {
        
        try
        {
            var itemCopy = await _itemService.CreateCopy(id, requestData);
            return Ok(
                new ItemCopyDto
                {
                    Id = itemCopy.Id,
                    InventoryCode = itemCopy.InventoryCode,
                    

                }
            );
        } catch(BusinessRuleException e)
        {
            return Conflict(new {title = "cannot create copy of an item", detail = e.Message});
        }
    }*/


    [HttpPost]
    [Authorize]
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
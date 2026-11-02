using API.CustomExceptions;
using API.Dtos;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CategoriesController(ICategoryService _categoryService) : LainaamoControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var categories = await _categoryService.GetAll();

        // Muunnetaan modelit DTO:iksi
        var dtos = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name
        });

        return Ok(dtos);
    }

    // POST api/categories
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(CreateCategoryReq req)
    {
        var category = await _categoryService.Create(req);

        // CreatedAtAction palauttaa 201 ja Location-headerin,
        // josta luotu resurssi löytyy
        return Ok(
           
            new CategoryDto { Id = category.Id, Name = category.Name });
    }

    // GET api/categories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        try
        {
            var category = await _categoryService.GetById(id);

            return Ok(new CategoryDto { Id = category.Id, Name = category.Name });
        }
        catch (NotFoundException e)
        {
            return NotFound(new { title = "category not found", detail = e.Message });
        }
    }

    // PUT api/categories/5
    [HttpPut("{id}")]
    [HttpPatch("{id}")]
    public async Task<ActionResult<CategoryDto>> Update(int id, CreateCategoryReq req)
    {
        try
        {
            var category = await _categoryService.Update(id, req);

            return Ok(new CategoryDto { Id = category.Id, Name = category.Name });
        }
        catch (NotFoundException e)
        {
            return NotFound(new { title = "category not found", detail = e.Message });
        }
    }

    // DELETE api/categories/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _categoryService.Delete(id);

            // 204 No Content: onnistui, eikä palautettavaa ole
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(new { title = "category not found", detail = e.Message });
        }

        catch (BusinessRuleException e)
        {
            // 409 Conflict: pyyntö oli kelvollinen, mutta se on ristiriidassa
            // resurssin nykytilan kanssa
            return Conflict(new { title = "cannot delete category", detail = e.Message });
        }
    }

    

}
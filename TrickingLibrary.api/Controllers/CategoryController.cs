using Common.Models;
using Common.Utilities;
using Data.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.api.Dtos.Category;

namespace CategoryingLibrary.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ApplicationDbContext ctx) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        category.Id = Formatter.FormatePascalToKebab(category.Name).ToLowerInvariant();
        category.CreatedAt = DateTime.UtcNow;
        category.ModifiedAt = DateTime.UtcNow;
        category.IsDeleted = false;

        ctx.Categories.Add(category);
        await ctx.SaveChangesAsync();

        return Ok(category);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var categories = ctx.Categories
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .Select(CategoryProjection.ReadAs())
            .ToList();
        return Ok(categories);
    }

    [HttpGet("{categoryId}")]
    public IActionResult GetById(string categoryId)
    {
        var category = ctx.Categories
            .AsNoTracking()
            .Where(c => c.Id.Equals(categoryId) && !c.IsDeleted)
            .Select(CategoryProjection.ReadAs())
            .FirstOrDefault();

        if (category is null) return NotFound();
        return Ok(category);
    }

    [HttpGet("{categoryId}/with-data")]
    public IActionResult GetByIdWithData(string categoryId)
    {
        var category = ctx.Categories
            .AsNoTracking()
            .Where(c => c.Id.Equals(categoryId) && !c.IsDeleted)
            .Select(CategoryProjection.ReadAs(true))
            .FirstOrDefault();

        if (category is null) return NotFound();
        return Ok(category);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Category category)
    {
        var fetchedCategory = ctx.Categories.FirstOrDefault(c => c.Id.Equals(category.Id) && !c.IsDeleted);

        if (fetchedCategory is null) return NotFound();

        fetchedCategory.Id = Formatter.FormatePascalToKebab(category.Name).ToLowerInvariant();
        fetchedCategory.Name = category.Name;
        fetchedCategory.Description = category.Description;
        fetchedCategory.ModifiedAt = DateTime.UtcNow;
        await ctx.SaveChangesAsync();

        return Ok(fetchedCategory);
    }

    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> Delete(string categoryId)
    {
        var result = await ctx.Categories
            .Where(c => c.Id.Equals(categoryId) && !c.IsDeleted)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(s => s.IsDeleted, true)
            );

        return result == 0 ? NotFound() : Ok();
    }
}
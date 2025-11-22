using Common.Models;
using Common.Utilities;
using Data.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.api.Dtos.Difficulty;

namespace TrickingLibrary.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DifficultyController(ApplicationDbContext ctx) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Difficulty difficulty)
    {
        difficulty.Id = Formatter.FormatePascalToKebab(difficulty.Name).ToLowerInvariant();
        difficulty.CreatedAt = DateTime.UtcNow;
        difficulty.ModifiedAt = DateTime.UtcNow;
        difficulty.IsDeleted = false;

        ctx.Difficulties.Add(difficulty);
        await ctx.SaveChangesAsync();

        return Ok(difficulty);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var difficulties = ctx.Difficulties
            .Where(d => !d.IsDeleted)
            .AsNoTracking()
            .Select(DifficultyProjection.ReadAs())
            .ToList();
        return Ok(difficulties);
    }

    [HttpGet("{difficultyId}")]
    public IActionResult GetById(string difficultyId)
    {
        var difficulty = ctx.Difficulties
            .AsNoTracking()
            .Where(d => d.Id.Equals(difficultyId) && !d.IsDeleted)
            .Select(DifficultyProjection.ReadAs())
            .FirstOrDefault();

        if (difficulty is null) return NotFound();
        return Ok(difficulty);
    }

    [HttpGet("{difficultyId}/with-data")]
    public IActionResult GetByIdWithData(string difficultyId)
    {
        var difficulty = ctx.Difficulties
            .AsNoTracking()
            .Where(d => d.Id.Equals(difficultyId) && !d.IsDeleted)
            .Select(DifficultyProjection.ReadAs(true)).FirstOrDefault();

        if (difficulty is null) return NotFound();
        return Ok(difficulty);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Difficulty difficulty)
    {
        var fetchedDifficulty = ctx.Difficulties.FirstOrDefault(d => d.Id.Equals(difficulty.Id) && !d.IsDeleted);

        if (fetchedDifficulty is null) return NotFound();

        fetchedDifficulty.Id = Formatter.FormatePascalToKebab(difficulty.Name).ToLowerInvariant();
        fetchedDifficulty.Name = difficulty.Name;
        fetchedDifficulty.Description = difficulty.Description;
        fetchedDifficulty.ModifiedAt = DateTime.UtcNow;
        await ctx.SaveChangesAsync();

        return Ok(fetchedDifficulty);
    }

    [HttpDelete("{difficultyId}")]
    public async Task<IActionResult> Delete(string difficultyId)
    {
        var result = await ctx.Difficulties
            .Where(d => d.Id.Equals(difficultyId) && !d.IsDeleted)
            .ExecuteUpdateAsync(setter => setter.SetProperty(s => s.IsDeleted, true));

        return result == 0 ? NotFound() : Ok();
    }
}
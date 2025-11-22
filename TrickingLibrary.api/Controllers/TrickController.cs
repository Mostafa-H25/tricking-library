using Common.Enums.Trick;
using Data.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.api.Dtos.Trick;

namespace TrickingLibrary.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrickController(ApplicationDbContext ctx) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TrickCreateDto trickCreateDto)
    {
        var trick = TrickProjection.InitEntity(trickCreateDto);

        var exists = ctx.Tricks.Any(t => t.Id.Equals(trick.Id));
        if (exists) return BadRequest("Trick already exists.");

        ctx.Tricks.Add(trick);
        await ctx.SaveChangesAsync();

        return Ok(TrickProjection.ReadAs().Compile().Invoke(trick));
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var tricks = ctx.Tricks
            .AsNoTracking()
            .Where(t => !t.IsDeleted)
            .Select(TrickProjection.ReadAs())
            .ToList();
        return Ok(tricks);
    }

    [HttpGet("{trickId}")]
    public IActionResult GetById(string trickId)
    {
        var trick = ctx.Tricks
            .AsNoTracking()
            .Where(t => t.Id.Equals(trickId) && !t.IsDeleted)
            .Select(TrickProjection.ReadAs(DtoDetailLevelEnum.Semi))
            .FirstOrDefault();

        if (trick is null) return NotFound();
        return Ok(trick);
    }

    [HttpGet("{trickId}/with-data")]
    public IActionResult GetByIdWithData(string trickId)
    {
        var trick = ctx.Tricks
            .AsNoTracking()
            .Where(t => t.Id.Equals(trickId) && !t.IsDeleted)
            .Select(TrickProjection.ReadAs(DtoDetailLevelEnum.Full)).FirstOrDefault();

        if (trick is null) return NotFound();
        return Ok(trick);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TrickFormDto trickFormDto)
    {
        var trick = ctx.Tricks.FirstOrDefault(t => t.Id.Equals(trickFormDto.Id) && !t.IsDeleted);

        if (trick is null) return NotFound();

        TrickProjection.ToEntity(trickFormDto, trick);

        await ctx.SaveChangesAsync();

        return Ok(trick);
    }

    [HttpDelete("{trickId}")]
    public async Task<IActionResult> Delete(string trickId)
    {
        var trick = ctx.Tricks.FirstOrDefault(t => t.Id.Equals(trickId) && !t.IsDeleted);
        var result = await ctx.Tricks
            .Where(t => t.Id.Equals(trickId) && !t.IsDeleted)
            .ExecuteUpdateAsync(setter => setter.SetProperty(s => s.IsDeleted, true));

        return result == 0 ? NotFound() : Ok();
    }
}
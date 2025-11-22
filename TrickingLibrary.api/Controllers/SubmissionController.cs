using System.Threading.Channels;
using Data.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.api.Dtos.Submission;
using TrickingLibrary.api.Dtos.Video;
using TrickingLibrary.api.Mappers;

namespace TrickingLibrary.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubmissionController(ApplicationDbContext ctx, IWebHostEnvironment env) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SubmissionCreateDto submissionCreateDto,
        [FromServices] Channel<VideoEditingDto> channel)
    {
        var filePath = Path.Combine(env.WebRootPath, submissionCreateDto.FileName);
        if (!Path.Exists(filePath)) return BadRequest();

        var submission = SubmissionProjection.InitEntity(submissionCreateDto);
        ctx.Add(submission);
        await ctx.SaveChangesAsync();

        var message = new VideoEditingDto
        {
            SubmissionId = submission.Id,
            FileName = submission.FileName
        };

        await channel.Writer.WriteAsync(message);

        return Ok();
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var categories = ctx.Submissions
            .AsNoTracking()
            .Where(s => s.IsProcessed && !s.IsDeleted)
            .Select(SubmissionProjection.ReadAs)
            .ToList();
        return Ok(categories);
    }

    [HttpGet("{submissionId}")]
    public IActionResult GetById(string submissionId)
    {
        var submission = ctx.Submissions
            .AsNoTracking()
            .Where(s => s.Id.Equals(submissionId) && s.IsProcessed && !s.IsDeleted)
            .Select(SubmissionProjection.ReadAs)
            .FirstOrDefault();

        if (submission is null) return NotFound();
        return Ok(submission);
    }

    [HttpDelete("{submissionId}")]
    public async Task<IActionResult> Delete(string submissionId)
    {
        var result = await ctx.Submissions
            .Where(s => s.Id.Equals(submissionId) && !s.IsDeleted)
            .ExecuteUpdateAsync(setter => setter.SetProperty(s => s.IsDeleted, true));

        return result == 0 ? NotFound() : Ok();
    }
}
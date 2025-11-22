using Data.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace TrickingLibrary.api.Controllers;

[Route("api/[controller]")]
public class VideoController(ApplicationDbContext ctx, IWebHostEnvironment env) : ControllerBase
{
    private const string TempPrefix = "temp_";

    [HttpPost]
    public async Task<IActionResult> UploadVideo(IFormFile video)
    {
        var mime = Path.GetExtension(video.FileName);
        var fileName = string.Concat(TempPrefix, DateTime.Now.Ticks.ToString(), mime);
        var filePath = Path.Combine(env.WebRootPath, fileName);

        await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await video.CopyToAsync(fileStream);

        var videoResponseDto = new
        {
            FileName = fileName,
            Name = video.FileName,
        };

        return Ok(videoResponseDto);
    }

    [HttpPost]
    public async Task<IActionResult> GetVideo(string submissionId)
    {
        var fileName = ctx.Submissions
            .Where(s => s.IsProcessed && !s.IsDeleted)
            .Select(s => s.FileName)
            .FirstOrDefault();

        if (string.IsNullOrEmpty(fileName)) return BadRequest();

        var filePath = Path.Combine(env.WebRootPath, fileName);

        await using var file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return new FileStreamResult(file, "video/mp4");
    }

    [HttpDelete]
    public IActionResult DeleteVideo(string submissionId)
    {
        var fileName = ctx.Submissions
            .Where(s => s.IsProcessed && !s.IsDeleted)
            .Select(s => s.FileName)
            .FirstOrDefault();

        if (string.IsNullOrEmpty(fileName)) return BadRequest();

        var filePath = Path.Combine(env.WebRootPath, fileName);

        System.IO.File.Delete(filePath);
        return Ok();
    }
}
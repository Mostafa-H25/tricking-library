using System.Diagnostics;
using System.Threading.Channels;
using Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.api.Dtos.Video;

namespace TrickingLibrary.api.BackgroundServices;

public class VideoEditingService(
    IWebHostEnvironment env,
    ILogger<VideoEditingService> logger,
    Channel<VideoEditingDto> channel,
    IServiceProvider serviceProvider)
    : BackgroundService
{
    private const string ConstPrefix = "converted_";
    private const string ThumbnailPrefix = "thumbnail_";
    private readonly ChannelReader<VideoEditingDto> _reader = channel.Reader;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _reader.WaitToReadAsync(stoppingToken))
        {
            var message = await _reader.ReadAsync(stoppingToken);
            var inputPath = Path.Combine(env.WebRootPath, message.FileName);
            var outputName = Path.Combine(ConstPrefix, DateTime.Now.Ticks.ToString(), ".mp4");
            var outputPath = Path.Combine(env.WebRootPath, outputName);
            var thumbnailName = Path.Combine(ThumbnailPrefix, DateTime.Now.Ticks.ToString(), ".webp");
            var thumbnailPath = Path.Combine(env.WebRootPath, thumbnailName);
            var ffmpegPath = Path.Combine(env.ContentRootPath, "ffmpeg", "ffmpeg.exe");

            try
            {
                if (!File.Exists(inputPath)) throw new Exception("Temporary file is not found.");
                if (!File.Exists(ffmpegPath)) throw new Exception("FFMPEG file is not found.");

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments =
                        $"-y -i {inputPath} -an -vf scale=640*480 {outputPath} -ss 00:00:00 -farame:v 1 {thumbnailPath}",
                    WorkingDirectory = env.WebRootPath,
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                using var process = new Process();
                process.StartInfo = processStartInfo;
                process.Start();
                await process.WaitForExitAsync(stoppingToken);

                if (!File.Exists(outputPath))
                    throw new Exception("FFMPEG failed to create converted video and save in specified path.");
                if (!File.Exists(thumbnailPath))
                    throw new Exception("FFMPEG failed to create video thumbnail and save in specified path.");

                using var scope = serviceProvider.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await ctx.Submissions
                    .Where(s => s.Id == message.SubmissionId)
                    .ExecuteUpdateAsync(setters => setters
                            .SetProperty(s => s.FileName, outputName)
                            .SetProperty(s => s.ThumbnailName, thumbnailName)
                            .SetProperty(s => s.IsProcessed, true),
                        stoppingToken
                    );
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Video processing failed for video: {0}", message.FileName);
                if (File.Exists(outputPath)) File.Delete(outputPath);
                if (File.Exists(thumbnailPath)) File.Delete(thumbnailPath);
            }
            finally
            {
                File.Delete(inputPath);
            }
        }
    }
}
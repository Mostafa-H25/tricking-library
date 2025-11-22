namespace TrickingLibrary.api.Dtos.Video;

public record VideoEditingDto
{
    public required int SubmissionId { get; set; }
    public required string FileName { get; set; }
}
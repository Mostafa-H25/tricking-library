namespace TrickingLibrary.api.Dtos.Submission;

public record SubmissionCreateDto
{
    public required string Name { get; set; }
    public required string FileName { get; set; }
    public required string Trick { get; set; }
};
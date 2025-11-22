namespace TrickingLibrary.api.Dtos.Submission;

public record SubmissionReadDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
};
namespace TrickingLibrary.api.Dtos.Trick;

public record TrickReadDto()
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Difficulty { get; set; }
    public IList<string>? Categories { get; set; }
    public IList<string>? Submissions { get; set; }
    public IList<TrickReadDto>? Prerequisites { get; set; }
    public IList<TrickReadDto>? Progressions { get; set; }
};
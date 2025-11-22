namespace TrickingLibrary.api.Dtos.Trick;

public record TrickCreateDto()
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Difficulty { get; set; }
    public IList<string> Submissions { get; set; } = [];
    public IList<string> Categories { get; set; } = [];
    public IList<string> Prerequisites { get; set; } = [];
    public IList<string> Progressions { get; set; } = [];
};
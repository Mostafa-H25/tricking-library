using TrickingLibrary.api.Dtos.Trick;

namespace TrickingLibrary.api.Dtos.Category;

public record CategoryReadDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public IList<TrickMinimalDto>? Tricks { get; set; }
}
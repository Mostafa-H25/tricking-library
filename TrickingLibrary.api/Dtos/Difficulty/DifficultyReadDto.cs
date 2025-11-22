using TrickingLibrary.api.Dtos.Trick;

namespace TrickingLibrary.api.Dtos.Difficulty;

public record DifficultyReadDto()
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public IList<TrickMinimalDto>? Tricks { get; set; }
}
using System.Linq.Expressions;
using TrickingLibrary.api.Dtos.Trick;

namespace TrickingLibrary.api.Dtos.Difficulty;

public static class DifficultyProjection
{
    public static Expression<Func<global::Common.Models.Difficulty, DifficultyReadDto>> ReadAs(
        bool includeDetails = false)
    {
        return difficulty => new DifficultyReadDto
        {
            Id = difficulty.Id,
            Name = difficulty.Name,
            Description = difficulty.Description,
            Tricks = includeDetails
                ? difficulty.Tricks.Select(t => new TrickMinimalDto() { Id = t.Id, Name = t.Name })
                    .ToList()
                : null
        };
    }
}
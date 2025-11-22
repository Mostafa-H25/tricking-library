using System.Linq.Expressions;
using TrickingLibrary.api.Dtos.Trick;

namespace TrickingLibrary.api.Dtos.Category;

public static class CategoryProjection
{
    public static Expression<Func<global::Common.Models.Category, CategoryReadDto>> ReadAs(bool includeDetails = false)
    {
        return category => new CategoryReadDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Tricks = includeDetails
                ? category.TrickCategory.Select(tc => new TrickMinimalDto() { Id = tc.Trick.Id, Name = tc.Trick.Name })
                    .ToList()
                : null
        };
    }
}
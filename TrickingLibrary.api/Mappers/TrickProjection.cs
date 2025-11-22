using System.Linq.Expressions;
using Common.Enums.Trick;
using Common.Models;

namespace TrickingLibrary.api.Dtos.Trick;

public static class TrickProjection
{
    public static Expression<Func<global::Common.Models.Trick, TrickReadDto>> ReadAs(
        DtoDetailLevelEnum includeData = DtoDetailLevelEnum.None)
    {
        var isSemi = includeData == DtoDetailLevelEnum.Semi;
        var isFull = includeData == DtoDetailLevelEnum.Full;
        return trick => new TrickReadDto
        {
            Id = trick.Id,
            Name = trick.Name,
            Description = trick.Description,
            Difficulty = !isFull && !isSemi ? null : trick.Difficulty.Name,
            Categories = isFull || isSemi ? trick.TrickCategories.Select(tc => tc.Category.Name).ToList() : null,
            Submissions = isFull ? trick.Submissions.Select(s => s.Name).ToList() : null,
            Prerequisites = isFull
                ? trick.Prerequisites.Select(pp => new TrickReadDto
                {
                    Id = pp.Prerequisite.Id,
                    Name = pp.Prerequisite.Name,
                    Description = pp.Prerequisite.Description,
                    Difficulty = pp.Prerequisite.Difficulty.Name,
                    Categories = pp.Prerequisite.TrickCategories.Select(tc => tc.Category.Name).ToList()
                }).ToList()
                : null,
            Progressions = isFull
                ? trick.Progressions.Select(pp => new TrickReadDto
                {
                    Id = pp.Progression.Id,
                    Name = pp.Progression.Name,
                    Description = pp.Progression.Description,
                    Difficulty = pp.Progression.Difficulty.Name,
                    Categories = pp.Progression.TrickCategories.Select(tc => tc.Category.Name).ToList()
                }).ToList()
                : null,
        };
    }

    public static global::Common.Models.Trick InitEntity(TrickCreateDto trickCreateDto)
    {
        var id = trickCreateDto.Name.Replace(" ", "-").ToLowerInvariant();
        return new global::Common.Models.Trick
        {
            Id = id,
            Name = trickCreateDto.Name,
            Description = trickCreateDto.Description,
            DifficultyId = trickCreateDto.Difficulty,
            TrickCategories = trickCreateDto.Categories.Select(c => new TrickCategory { TrickId = id, CategoryId = c })
                .ToList(),
            Progressions = trickCreateDto.Progressions.Select(p => new PrerequisiteProgressionRelation()
                { PrerequisiteId = id, ProgressionId = p }).ToList(),
            Prerequisites = trickCreateDto.Prerequisites.Select(p => new PrerequisiteProgressionRelation()
                { PrerequisiteId = p, ProgressionId = id }).ToList(),
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public static void ToEntity(TrickFormDto trickFormDto, global::Common.Models.Trick trick)
    {
        var id = (trickFormDto.Name ?? trick.Name).Replace(" ", "-").ToLowerInvariant();

        trick.Id = id;
        trick.Name = trickFormDto.Name ?? trick.Name;
        trick.Description = trickFormDto.Description ?? trick.Description;
        trick.DifficultyId = trickFormDto.Difficulty ?? trick.DifficultyId;
        trick.TrickCategories =
            trickFormDto.Categories?.Select(c => new TrickCategory { TrickId = id, CategoryId = c }).ToList() ??
            trick.TrickCategories;
        trick.Progressions =
            trickFormDto.Progressions?.Select(p => new PrerequisiteProgressionRelation()
                { PrerequisiteId = id, ProgressionId = p }).ToList() ?? trick.Progressions;
        trick.Prerequisites =
            trickFormDto.Prerequisites?.Select(p => new PrerequisiteProgressionRelation()
                { PrerequisiteId = p, ProgressionId = id }).ToList() ?? trick.Prerequisites;
        trick.ModifiedAt = DateTime.UtcNow;
    }
}
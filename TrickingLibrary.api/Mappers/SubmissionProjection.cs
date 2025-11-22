using System.Linq.Expressions;
using Common.Models;
using TrickingLibrary.api.Dtos.Submission;

namespace TrickingLibrary.api.Mappers;

public static class SubmissionProjection
{
    public static Expression<Func<Submission, SubmissionReadDto>> ReadAs => submission => new SubmissionReadDto
    {
        Id = submission.Id,
        Name = submission.Name
    };

    public static Submission InitEntity(SubmissionCreateDto submissionCreateDto)
    {
        return new Submission
        {
            Id = Random.Shared.Next(),
            Name = submissionCreateDto.Name,
            FileName = submissionCreateDto.FileName,
            TrickId = submissionCreateDto.Trick,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
            IsProcessed = false,
            IsDeleted = false,
        };
    }
}
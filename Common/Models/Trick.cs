namespace Common.Models;

public class Trick : BaseModel<string>
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string DifficultyId { get; set; }
    public Difficulty? Difficulty { get; set; }
    public ICollection<TrickCategory> TrickCategories { get; set; } = [];
    public ICollection<Submission> Submissions { get; set; } = [];
    public ICollection<PrerequisiteProgressionRelation> Prerequisites { get; set; } = [];
    public ICollection<PrerequisiteProgressionRelation> Progressions { get; set; } = [];
}
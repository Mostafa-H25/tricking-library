namespace Common.Models;

public class Difficulty : BaseModel<string>
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Trick> Tricks { get; set; } = [];
}
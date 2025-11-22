namespace Common.Models;

public class Submission : BaseModel<int>
{
    public required string Name { get; set; }
    public required string FileName { get; set; }
    public string? ThumbnailName { get; set; }
    public string? Description { get; set; }
    public required string TrickId { get; set; }
    public bool IsProcessed { get; set; } = false;
    public Trick? Trick { get; set; }
}
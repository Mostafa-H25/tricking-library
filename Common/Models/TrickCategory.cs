namespace Common.Models;

public class TrickCategory
{
    public required string TrickId { get; set; }
    public Trick? Trick { get; set; }
    public required string CategoryId { get; set; }
    public Category? Category { get; set; }
}
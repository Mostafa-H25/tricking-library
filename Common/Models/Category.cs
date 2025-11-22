namespace Common.Models;

public class Category : BaseModel<string>
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<TrickCategory> TrickCategory { get; set; } = [];
}
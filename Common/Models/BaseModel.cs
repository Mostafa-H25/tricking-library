namespace Common.Models;

public class BaseModel<TKey>
{
    public required TKey Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
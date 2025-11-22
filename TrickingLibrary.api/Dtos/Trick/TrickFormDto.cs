public record TrickFormDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Difficulty { get; set; }
    public IList<string>? Categories { get; set; }
    public IList<string>? Submissions { get; set; }
    public IList<string>? Prerequisites { get; set; }
    public IList<string>? Progressions { get; set; }
}
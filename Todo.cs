namespace MinimalAPI_Sample;

public record Todo
{
    public int Id { get; set; }

    public string? Name { get; set; }
    
    public bool IsComplete { get; set; }
}
namespace AutoXDisplay.DAL.Entities;

public class RaceResultEntity
{
    public string? Position { get; set; }
    public string? Class { get; set; }
    public string? Number { get; set; }
    public string? Name { get; set; }
    public string? Car { get; set; }
    public string? CarColor { get; set; }
    public List<string> Runs { get; set; } = new List<string>() {};
    public string? Total { get; set; }
    public string? Diff { get; set; }
}
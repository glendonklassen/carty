namespace CartyMap.Api;

public class MapDrawRequest
{
    public int? MinX { get; set; }
    public int? MaxX { get; set; }
    public int? MinY { get; set; }
    public int? MaxY { get; set; }
    public string? Type { get; set; }
}
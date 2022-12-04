namespace CartyMap.Api;

public class MapSpace
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Q => X - (Y + (Y & 1)) / 2;
    public int R => Y;
    public int S => -Q-R;
    public int Type { get; set; }
}
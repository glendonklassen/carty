using System.Collections.Generic;
using System.Linq;

namespace GK.Carty.Api;

public class MapSpace
{
    public int X { get; set; }
    public int Y { get; set; }
    public Coordinate Location
    {
        get
        {
            var q = X - (Y + (Y & 1)) / 2;
            var r = Y;
            return new(q, Y, -q-r);
        }
    }
    public TerrainType TerrainType { get; set; }
}

public static class MapSpaceExtensions
{
    public static List<MapSpace> Neighbours(this MapSpace m, List<MapSpace> board)
    {
        var result = new List<MapSpace>();
        var se = board.SingleOrDefault(a => a.Location == m.Location.Southeast());
        var sw = board.SingleOrDefault(a => a.Location == m.Location.Southwest());
        var ne = board.SingleOrDefault(a => a.Location == m.Location.Northeast());
        var nw = board.SingleOrDefault(a => a.Location == m.Location.Northwest());
        var e = board.SingleOrDefault(a => a.Location == m.Location.East());
        var w = board.SingleOrDefault(a => a.Location == m.Location.West());
        if(se is not null) result.Add(se);
        if(sw is not null) result.Add(sw);
        if(e is not null) result.Add(e);
        if(w is not null) result.Add(w);
        if(ne is not null) result.Add(ne);
        if(nw is not null) result.Add(nw);
        return result;
    }
}
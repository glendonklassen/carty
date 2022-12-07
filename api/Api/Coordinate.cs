using System;

namespace GK.Carty.Api;

public record Coordinate(int Q, int R, int S)
{
    public static Coordinate operator -(Coordinate a, Coordinate b) => new(a.Q - b.Q, a.R - b.R, a.S - b.S);

    public void Deconstruct(out int Q, out int R, out int S)
    {
        Q = this.Q;
        R = this.R;
        S = this.S;
    }
}

public static class CoordinateExtensions
{
    public static Coordinate East(this Coordinate c) => new(c.Q, c.R + 1, c.S - 1);
    public static Coordinate Northeast(this Coordinate c) => new(c.Q + 1, c.R - 1, c.S);
    public static Coordinate Northwest(this Coordinate c) => new(c.Q, c.R - 1, c.S + 1);
    public static Coordinate West(this Coordinate c) => new(c.Q + 1, c.R, c.S - 1);
    public static Coordinate Southwest(this Coordinate c) => new(c.Q - 1, c.R + 1, c.S);
    public static Coordinate Southeast(this Coordinate c) => new(c.Q, c.R + 1, c.S - 1);
    public static int Distance(this Coordinate a, Coordinate b)
    {
        var diff = a - b;
        return Math.Max(Math.Max(diff.Q, diff.R), diff.S);
    }
}
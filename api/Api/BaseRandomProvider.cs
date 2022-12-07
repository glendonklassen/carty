using System;

namespace GK.Carty.Api;

public class BaseRandomProvider : IRandomProvider
{
    private readonly Random _random;

    public BaseRandomProvider()
    {
        _random = new Random();
    }

    public Random Random() => _random;
    public bool Flip() => _random.Next() % 2 == 0;
}
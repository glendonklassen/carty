﻿using System;

namespace GK.Carty.Api;

public class BaseRandomProvider : IRandomProvider
{
    private readonly Random _random;

    public BaseRandomProvider()
    {
        _random = new Random();
    }

    public Random GetRandom() => _random;
}
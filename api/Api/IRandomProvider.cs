using System;

namespace GK.Carty.Api;

public interface IRandomProvider
{
    Random GetRandom();
    bool Flip();
}
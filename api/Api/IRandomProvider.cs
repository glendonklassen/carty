using System;

namespace GK.Carty.Api;

public interface IRandomProvider
{
    Random Random();
    bool Flip();
}
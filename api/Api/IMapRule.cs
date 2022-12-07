using System.Collections.Generic;

namespace GK.Carty.Api;

public interface IMapRule<T>
{
    void SetSpaces(List<MapSpace> spaces, MapSpace mainSpace, T param);
}
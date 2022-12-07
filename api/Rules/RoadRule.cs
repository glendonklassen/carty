using System;
using System.Collections.Generic;
using System.Linq;
using GK.Carty.Api;

namespace GK.Carty.Rules;

public class RoadRule : IMapRule<TerrainType>
{
    private readonly IRandomProvider _random;

    public RoadRule(IRandomProvider random)
    {
        _random = random ?? throw new ArgumentNullException(nameof(random));
    }

    public void SetSpaces(List<MapSpace> spaces, MapSpace mainSpace, TerrainType param)
    {
        var location = mainSpace.Location;
        if (mainSpace.TerrainType != param) return;
        var neighbours = mainSpace.Neighbours(spaces);
        var roads = neighbours.Where(x => x.TerrainType == param).ToList();
        var roadsLength = roads.Count;
        // Roads must be single file.
        for (var i = 0; i < roadsLength; i++)
        {
            for (var j = i + 1; j < roadsLength; j++)
            {
                if (roads[i].Location.Distance(roads[j].Location) != 1) continue;
                if (roads[i].TerrainType != param || roads[j].TerrainType != param) continue;
                var flip = _random.GetRandom().Next() % 2 == 0;
                if (flip)
                    roads[i].TerrainType = TerrainType.Grass;
                else
                    roads[j].TerrainType = TerrainType.Grass;
            }
        }
        // Roads should not be alone.
        if (neighbours.Any(n => n.TerrainType == TerrainType.Road)) return;
        var randomIndex = _random.GetRandom().Next(neighbours.Count);
        neighbours[randomIndex].TerrainType = TerrainType.Road;
    }
}
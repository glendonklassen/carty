using System;
using System.Collections.Generic;
using System.Linq;
using GK.Carty.Api;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GK.Carty.Rules;

public class RoadRule : IMapRule<TerrainType>
{
    private readonly IRandomProvider _r;
    private readonly ILogger<RoadRule> _logger;
    public RoadRule(IRandomProvider r, ILogger<RoadRule> logger)
    {
        _r = r ?? throw new ArgumentNullException(nameof(r));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void SetSpaces(List<MapSpace> spaces, MapSpace mainSpace, TerrainType param)
    {
        var location = mainSpace.Location;
        _logger.LogInformation(JsonConvert.SerializeObject(location));
        if (mainSpace.TerrainType != param) return;
        var neighbours = mainSpace.Neighbours(spaces);
        var roads = neighbours.Where(x => x.TerrainType == param).ToList();
        var roadsLength = roads.Count;
        // Roads must be single file.
        for (var i = 0; i < roadsLength; i++)
        {
            for (var j = i + 1; j < roadsLength; j++)
            {
                // TODO FIX this isn't catching all instances of roads being in a bunch.
                if (roads[i].Location.Distance(roads[j].Location) != 1) continue;
                if (roads[i].TerrainType != param || roads[j].TerrainType != param) continue;
                if (_r.Flip())
                    roads[i].TerrainType = TerrainType.Grass;
                else
                    roads[j].TerrainType = TerrainType.Water;
            }
        }
        // Roads should not be alone.
        if (neighbours.Any(n => n.TerrainType == TerrainType.Road)) return;
        var randomIndex = _r.GetRandom().Next(neighbours.Count);
        neighbours[randomIndex].TerrainType = TerrainType.Road;
    }
}
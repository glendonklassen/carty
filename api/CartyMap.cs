using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GK.Carty.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace GK.Carty
{
    public class CartyMap
    {
        private readonly IMapRule<TerrainType> _rule;
        private readonly IRandomProvider _r;

        public CartyMap(IMapRule<TerrainType> rule, IRandomProvider r)
        {
            _rule = rule ?? throw new ArgumentNullException(nameof(rule));
            _r = r ?? throw new ArgumentNullException(nameof(r));
        }

        [FunctionName(nameof(CartyMap))]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, 
            "post", 
            Route = "map")] HttpRequest httpRequest)
        {
            var body = await httpRequest.ReadAsStringAsync();
            if (string.IsNullOrEmpty(body))
                return new BadRequestResult();
            var request = JsonConvert.DeserializeObject<MapDrawRequest>(body);
            if(request is null)
                return new BadRequestResult();
            var dummySpaces = new List<MapSpace>();
            for (var x = 0; x < request.Columns; x++)
            {
                for (var y = 0; y < request.Rows; y++)
                {
                    var curSpace = new MapSpace
                    {
                        X = x, Y = y,
                        TerrainType = (TerrainType)_r.Random().Next(1, 4)
                    };
                    dummySpaces.Add(curSpace);
                }
            }

            foreach (var space in dummySpaces)
            {
                _rule.SetSpaces(dummySpaces, space, TerrainType.Road);
            }
            return new OkObjectResult(dummySpaces);
        }
    }
}

using System.Net;
using CartyMap.Api;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CartyMap
{
    public class CartyMap
    {
        [Function(nameof(CartyMap))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "map")] HttpRequestData httpRequest)
        {
            var body = await httpRequest.ReadAsStringAsync();
            var response = httpRequest.CreateResponse(HttpStatusCode.OK);
            if (string.IsNullOrEmpty(body))
                return httpRequest.CreateResponse(HttpStatusCode.BadRequest);
            var request = JsonConvert.DeserializeObject<MapDrawRequest>(body);
            if(request is null)
                return httpRequest.CreateResponse(HttpStatusCode.BadRequest);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            MapSpace? prevSpace = null;
            var random = new Random();
            var dummySpaces = new List<MapSpace>();
            for (var x = 0; x < request.Columns; x++)
            {
                for (var y = 0; y < request.Rows; y++)
                {
                    var curSpace = new MapSpace{X = x, Y = y};
                    curSpace.Type = NextType(prevSpace, curSpace, request.Rows, random);
                    dummySpaces.Add(curSpace);
                    prevSpace = curSpace;
                }
            }
            response.WriteString(JsonConvert.SerializeObject(dummySpaces));
            return response;
        }

        private static int NextType(MapSpace? prevSpace, MapSpace curSpace, int rows, Random rand)
        {
            if (prevSpace is null || prevSpace.Y != curSpace.Y) return rand.Next(1, rows);
            var flip = rand.Next() % 2 == 0;
            return flip ? prevSpace.Type : rand.Next(1, rows);
        }
    }
}

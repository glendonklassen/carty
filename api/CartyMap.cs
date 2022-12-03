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
        private readonly ILogger _logger;

        public CartyMap(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CartyMap>();
        }

        [Function(nameof(CartyMap))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "map")] HttpRequestData httpRequest)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var body = await httpRequest.ReadAsStringAsync();
            var response = httpRequest.CreateResponse(HttpStatusCode.OK);
            if (string.IsNullOrEmpty(body))
                return httpRequest.CreateResponse(HttpStatusCode.BadRequest);
            var request = JsonConvert.DeserializeObject<MapDrawRequest>(body);
            if(request is null)
                return httpRequest.CreateResponse(HttpStatusCode.BadRequest);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            var dummySpaces = new List<MapSpace>();
            for (var x = 0; x < request.MaxX; x++)
            {
                for (var y = 0; y < request.MaxY; y++)
                {
                    
                    dummySpaces.Add(new MapSpace{Type = request.Type, X = x, Y = y});
                }
            }
            response.WriteString(JsonConvert.SerializeObject(dummySpaces));
            return response;
        }
    }
}

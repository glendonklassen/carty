using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

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
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "map")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}

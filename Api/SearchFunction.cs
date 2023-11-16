using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ApiIsolated
{
    public class SearchTrigger
    {
        private readonly ILogger _logger;

        public SearchTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SearchTrigger>();
        }

        [Function("Search")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post",Route = "search/{keyword}")] HttpRequestData req, string keyword)
        {
            _logger.LogInformation("C# HTTP trigger function <<Search>> processed a request.");
            
            var db = new DBContext(new LoggerFactory(), Program.FinalDbPath);
            var result = db.SearchAyas(keyword);
            var response = req.CreateResponse(HttpStatusCode.OK);
            //response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            //response.WriteString("Welcome to Azure Functions!");
            
            response.WriteAsJsonAsync(result);
            return response;
        }
    }
}

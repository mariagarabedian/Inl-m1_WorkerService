using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedLibery.Models;

namespace AzureFunctions
{
    public static class GetTemperature
    {
      
        [FunctionName("GetTemperature")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var url = req.Query["http://api.openweathermap.org/data/2.5/weather?q=Kumla,se&APPID=340a1c7e1eb2c2fac4b365398b20c7e8"];
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var data = JsonConvert.DeserializeObject<TemperaturModel>(requestBody);

            temp = temp ?? data?.Temp;

            var responseMessage = string.IsNullOrEmpty(url)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {url}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }

   
}

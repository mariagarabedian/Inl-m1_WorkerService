using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedLibery.Models;

namespace Inläm1_WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

         private HttpClient _client;
        


        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
             _client = new HttpClient();
            
            _logger.LogInformation("The service has been started ");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {

             _client.Dispose();
            
            _logger.LogInformation("The service has been stopped ");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                     var response = await _client.GetAsync("http://api.openweathermap.org/data/2.5/weather?q=Kumla,se&APPID=");// Tog bort min nyckel från github
                    

                    if (response.IsSuccessStatusCode)
                    {

                        var data = JsonConvert.DeserializeObject<TempratureModel>(await response.Content.ReadAsStringAsync());
                       

                        if(data.main.temp < 290)
                        {
                            _logger.LogInformation($" Its cold uteside! Temperature: {data.main.temp}, Humidity: {data.main.humidity}");
                        }
                        else
                            _logger.LogInformation($"Temperature: {data.main.temp}, Humidity: {data.main.humidity}");

                    }
                    else
                     _logger.LogInformation("Not SuccessStatusCod at: ",DateTimeOffset.Now);


                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Failed to get data from API - {ex.Message}");
                   
                }

               

                await Task.Delay(5*1000, stoppingToken);
            }
        }
    }
}

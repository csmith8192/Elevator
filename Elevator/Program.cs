using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace Elevator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var _host = CreateHostBuilder(args).Build();

            var _cache = _host.Services.GetRequiredService<IMemoryCache>();

            _cache.Set<Elevator>("myElevator", new Elevator() { SelectedFloorQueue = new Queue<int>() });

            _host.Services.GetService<UpdateElevatorState>().StartAsync(new System.Threading.CancellationToken());

            _host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
using Maze.Challenge.Application.Abstractions;
using Maze.Challenge.Application.Infraestructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Maze.Challenge.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IHost _host = Host
              .CreateDefaultBuilder()
              .ConfigureHostConfiguration((config) =>
              {
                  config.AddJsonFile("appsettings.json", optional: false);
              })
              .ConfigureServices((context, services) =>
              {
                  services
                    .AddMazeSolver(context.Configuration);
              })
              .Build();

            var resolver = _host.Services
                .GetServices<IMazeSolver>()
                .Where(x => x.ImplementationName() == nameof(RecursiveBacktrackingSolver))
                .First();

            resolver.Run().Wait();
        }

    }
}
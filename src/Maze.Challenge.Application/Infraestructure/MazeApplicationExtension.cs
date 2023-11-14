using Maze.Challenge.Application.Abstractions;
using Maze.Challenge.Application.Strategies;
using Maze.Challenge.Client.Infraestructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Maze.Challenge.Application.Infraestructure
{
    public static class MazeApplicationExtension
    {
        public static IServiceCollection AddMazeSolver(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddOptions<MazeApplicationSettings>()
                .BindConfiguration(nameof(MazeApplicationSettings))
                .Validate(settings =>
                {
                    if(settings.Width == 0 || settings.Height == 0)
                    {
                        throw new ArgumentException("The With and/or Height are required and should be greater than zero");
                    }
                    return true;
                });

            services.AddMazeClient(configuration)
                .AddTransient<IMazeSolver, RecursiveBacktrackingSolver>()
                .AddTransient<IMazeSolver, RecursiveSolver>();

            return services;
        }
    }
}

using Maze.Challenge.Client.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Maze.Challenge.Client.Infraestructure
{
    public static class MazeClientExtension
    {
        public static IServiceCollection AddMazeClient(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddOptions<MazeClientSettings>()
                .BindConfiguration(nameof(MazeClientSettings))
                .Validate(settings =>
                {
                    if (string.IsNullOrEmpty(settings.Code) || string.IsNullOrEmpty(settings.BaseUrl))
                    {
                        throw new ArgumentException("The Code and/or BaseUrl are required");
                    }
                    return true;
                });

            var baseUrl = configuration.GetValue<string>($"{nameof(MazeClientSettings)}:BaseUrl");

            services.AddHttpClient<IMazeClient, MazeClient>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });

            return services;
        }
    }

}

using Maze.Challenge.Client.Dtos;
using Maze.Challenge.Client.Infraestructure;
using Maze.Challenge.Client.Interfaces;
using Microsoft.Extensions.Options;

namespace Maze.Challenge.Client
{
    public class MazeClient : IMazeClient
    {
        private CustomHttpClient _client;
        private readonly MazeClientSettings _settings;

        public MazeClient(
            HttpClient httpClient,
            IOptions<MazeClientSettings> options)
        {
            _client = new CustomHttpClient(httpClient);
            _settings = options.Value;
        }

        public async Task<NewGameResponse> CreateGame(Guid mazeUid)
        {
            var url = string.Format(Urls.NEW_GAME, mazeUid, _settings.Code);
            return await _client.Post<OperationRequest, NewGameResponse>(url,
                new OperationRequest()
                {
                    Operation = Operations.Start
                });
        }

        public async Task<NewMazeReponse> CreateMaze(NewMazeRequest request)
        {
            var url = string.Format(Urls.NEW_MAZE, _settings.Code);
            return await _client.Post<NewMazeRequest, NewMazeReponse>(url, request);
        }

        public async Task<TakeALookResponse?> Move(MoveRequest request)
        {
            try
            {
                var url = string.Format(Urls.GAME, request.MazeUid, request.GameUid, _settings.Code);
                return await _client.Post<OperationRequest, TakeALookResponse>(url,
                    new OperationRequest()
                    {
                        Operation = request.Operation
                    });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<TakeALookResponse> TakeALook(Guid mazeUid, Guid gameUid)
        {
            var url = string.Format(Urls.GAME, mazeUid, gameUid, _settings.Code);
            return await _client.Get<TakeALookResponse>(url);
        }

        public async Task<NewGameResponse> RestartGame(Guid mazeUid, Guid gameUid)
        {
            var url = string.Format(Urls.GAME, mazeUid, gameUid, _settings.Code);
            return await _client.Post<OperationRequest, NewGameResponse>(url,
                new OperationRequest()
                {
                    Operation = Operations.Start
                });
        }

        public async Task<DebugResponse> Debug(Guid mazeUid)
        {
            var url = string.Format(Urls.DEBUG_MAZE, mazeUid, _settings.Code);
            return await _client.Get<DebugResponse>(url);
        }
    }
}

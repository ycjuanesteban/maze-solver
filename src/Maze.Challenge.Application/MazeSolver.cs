using Maze.Challenge.Application.Abstractions;
using Maze.Challenge.Application.Infraestructure;
using Maze.Challenge.Client;
using Maze.Challenge.Client.Dtos;
using Maze.Challenge.Client.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Maze.Challenge.Application
{
    public class MazeSolver : IMazeSolver
    {
        private readonly ILogger _logger;
        private readonly IMazeClient _mazeClient;
        private readonly MazeApplicationSettings _settings;

        //TODO: to review this because is possible to abstract more this part
        private readonly NewGameResponse _finalStatus;
        private bool[,] _visited;
        private readonly Dictionary<string, (int y, int x)> _posibleMovements = new Dictionary<string, (int y, int x)>
        {
            { Operations.GoNorth, (-1, 0) },
            { Operations.GoSouth, (1, 0) },
            { Operations.GoWest, (0, -1) },
            { Operations.GoEast, (0, 1) },
        };

        public MazeSolver(
            ILogger<MazeSolver> logger,
            IMazeClient mazeClient,
            IOptions<MazeApplicationSettings> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(ILogger));
            _mazeClient = mazeClient ?? throw new ArgumentNullException(nameof(IMazeClient));
            _settings = options.Value;

            _finalStatus = new NewGameResponse()
            {
                Completed = true,
                CurrentPositionX = _settings.Width - 1,
                CurrentPositionY = _settings.Height - 1
            };
            _visited = new bool[_settings.Height, _settings.Width];
        }

        public async Task Run()
        {
            _logger.LogInformation("Start process ...");

            _logger.LogInformation("Creating a new maze");
            var newMaze = await _mazeClient.CreateMaze(new NewMazeRequest(_settings.Width, _settings.Height));

            _logger.LogInformation("Creating a new game");
            var newGame = await _mazeClient.CreateGame(newMaze.MazeUid);

            bool solvedGame = await Resolve(newGame);
            _logger.LogInformation($"Solved {solvedGame}");

            var lookAround = await _mazeClient.TakeALook(newGame.MazeUid, newGame.GameUid);
            Console.WriteLine($"Completed {lookAround.Game.Completed}");

            PrintBoard();
        }

        //https://en.wikipedia.org/wiki/Maze-solving_algorithm#Recursive_algorithm
        private async Task<bool> Resolve(NewGameResponse game)
        {
            if (game.Completed || (game.CurrentPositionX == _finalStatus.CurrentPositionX && game.CurrentPositionY == _finalStatus.CurrentPositionY))
            {
                return true;
            }

            SetVisitedPosition(game);

            var lookAround = await _mazeClient.TakeALook(game.MazeUid, game.GameUid);
            var nextStep = GetNextStep(lookAround);

            var currentPosition = await _mazeClient.Move(new MoveRequest()
            {
                GameUid = game.GameUid,
                MazeUid = game.MazeUid,
                Operation = nextStep
            });

            if (currentPosition != null)
            {
                if (await Resolve(new NewGameResponse()
                {
                    Completed = currentPosition.Game.Completed,
                    CurrentPositionX = currentPosition.Game.CurrentPositionX,
                    CurrentPositionY = currentPosition.Game.CurrentPositionY,
                    GameUid = game.GameUid,
                    MazeUid = game.MazeUid
                }))
                    return true;
            }

            return false;
        }

        private string GetNextStep(TakeALookResponse game)
        {
            var posibleSteps = new Dictionary<string, bool>
            {
                { Operations.GoNorth, game.MazeBlockView.NorthBlocked },
                { Operations.GoSouth, game.MazeBlockView.SouthBlocked },
                { Operations.GoEast, game.MazeBlockView.EastBlocked },
                { Operations.GoWest, game.MazeBlockView.WestBlocked },
            };
            string nextStep = "";

            var futureSteps = posibleSteps.Where(x => !x.Value).ToList();

            foreach (var step in futureSteps)
            {
                int xPosition = game.Game.CurrentPositionX + _posibleMovements[step.Key].x;
                int yPosition = game.Game.CurrentPositionY + _posibleMovements[step.Key].y;

                if (!_visited[yPosition, xPosition])
                {
                    nextStep = step.Key;
                    break;
                }
            }

            //return the first because should make a movement
            if (string.IsNullOrEmpty(nextStep))
                nextStep = futureSteps.First().Key;

            Console.WriteLine($"X:{game.MazeBlockView.CoordX} - Y: {game.MazeBlockView.CoordY} - {nextStep}");

            return nextStep;

        }

        private void SetVisitedPosition(NewGameResponse game)
        {
            _visited[game.CurrentPositionY, game.CurrentPositionX] = true;
        }

        private void PrintBoard()
        {
            Console.WriteLine();
            var width = _visited.GetUpperBound(0);
            var height = _visited.GetUpperBound(1);

            for (int i = 0; i <= width; i++)
            {
                for (int j = 0; j <= height; j++)
                {
                    Console.Write(_visited[i, j] ? "-" : "#");
                }
                Console.WriteLine();
            }

        }
    }
}

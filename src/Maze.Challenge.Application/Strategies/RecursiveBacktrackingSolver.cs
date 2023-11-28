using Maze.Challenge.Application.Abstractions;
using Maze.Challenge.Application.Infraestructure;
using Maze.Challenge.Client;
using Maze.Challenge.Client.Dtos;
using Maze.Challenge.Client.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class RecursiveBacktrackingSolver : IMazeSolver
{
    private readonly ILogger _logger;
    private readonly IMazeClient _mazeClient;
    private readonly MazeApplicationSettings _settings;
    private bool[,] _visited;
    private readonly Dictionary<string, (int x, int y)> _posibleMovements = new Dictionary<string, (int x, int y)>
        {
            { Operations.GoNorth, (0, -1) },
            { Operations.GoSouth, (0, 1) },
            { Operations.GoWest, (-1, 0) },
            { Operations.GoEast, (1, 0) },
        };
    private Stack<(int x, int y)> _rightPath = new Stack<(int x, int y)>();

    public RecursiveBacktrackingSolver(
        ILogger<RecursiveBacktrackingSolver> logger,
        IMazeClient mazeClient,
        IOptions<MazeApplicationSettings> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(ILogger));
        _mazeClient = mazeClient ?? throw new ArgumentNullException(nameof(IMazeClient));
        _settings = options.Value;
        _visited = new bool[_settings.Width, _settings.Height];
    }

    public string ImplementationName()
    {
        return nameof(RecursiveBacktrackingSolver);
    }

    public async Task Run()
    {
        _logger.LogInformation("Start process ...");

        _logger.LogInformation("Creating a new maze");
        var newMaze = await _mazeClient.CreateMaze(new NewMazeRequest(_settings.Width, _settings.Height));

        _logger.LogInformation("Creating a new game");
        var game = await _mazeClient.CreateGame(newMaze.MazeUid);

        var currentStatus = await _mazeClient.TakeALook(game.MazeUid, game.GameUid);
        _visited[currentStatus.MazeBlockView.CoordX, currentStatus.MazeBlockView.CoordY] = true;

        while (!game.Completed)
        {
            var nextStep = GetNextStep(currentStatus);
            Console.WriteLine($"X:{currentStatus.MazeBlockView.CoordX} - Y: {currentStatus.MazeBlockView.CoordY} - {nextStep}");

            await _mazeClient.Move(new MoveRequest()
            {
                GameUid = game.GameUid,
                MazeUid = game.MazeUid,
                Operation = nextStep
            });

            currentStatus = await _mazeClient.TakeALook(game.MazeUid, game.GameUid);
            _visited[currentStatus.MazeBlockView.CoordX, currentStatus.MazeBlockView.CoordY] = true;
            game = currentStatus.Game;
        }

        Console.WriteLine("Done");
        Console.WriteLine($"Solved {game.Completed}");
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

        var futureSteps = posibleSteps.Where(x => !x.Value).ToList();

        foreach (var step in futureSteps)
        {
            int xPosition = game.Game.CurrentPositionX + _posibleMovements[step.Key].x;
            int yPosition = game.Game.CurrentPositionY + _posibleMovements[step.Key].y;

            if (!_visited[xPosition, yPosition])
            {
                _rightPath.Push((xPosition, yPosition));
                return step.Key;
            }
        }

        //Go back
        var currentPosition = _rightPath.Pop();
        var lastVisitedPosition = _rightPath.First();

        var xLastVisitedPosition = lastVisitedPosition.x - currentPosition.x;
        var yLastVisitedPosition = lastVisitedPosition.y - currentPosition.y;

        var stepToGoBack = _posibleMovements.Where(x => x.Value == (xLastVisitedPosition, yLastVisitedPosition)).First().Key;

        return stepToGoBack;
    }
}
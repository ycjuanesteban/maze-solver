using Maze.Challenge.Client.Dtos;

namespace Maze.Challenge.Client.Interfaces
{
    public interface IMazeClient
    {
        Task<NewMazeReponse> CreateMaze(NewMazeRequest request);

        Task<NewGameResponse> CreateGame(Guid mazeUid);

        Task<TakeALookResponse> TakeALook(Guid mazeUid, Guid gameUid);

        Task<TakeALookResponse?> Move(MoveRequest request);

        Task<NewGameResponse> RestartGame(Guid mazeUid, Guid gameUid);

        Task<DebugResponse> Debug(Guid mazeUid);

    }
}

namespace Maze.Challenge.Application.Abstractions
{
    public interface IMazeSolver
    {
        string ImplementationName();
        Task Run();
    }
}

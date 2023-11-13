namespace Maze.Challenge.Client.Dtos
{
    public class MoveRequest : OperationRequest
    {
        public Guid MazeUid { get; set; }
        public Guid GameUid { get; set; }
    }
}

namespace Maze.Challenge.Client.Dtos
{
    public class DebugResponse
    {
        public Guid MazeUid { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<MazeBlockViewResponse> Blocks { get; set; }
    }
}

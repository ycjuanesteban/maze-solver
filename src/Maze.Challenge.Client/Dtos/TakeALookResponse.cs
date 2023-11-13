namespace Maze.Challenge.Client.Dtos
{
    public class TakeALookResponse
    {
        public NewGameResponse Game { get; set; }
        public MazeBlockViewResponse MazeBlockView { get; set; }
    }
}

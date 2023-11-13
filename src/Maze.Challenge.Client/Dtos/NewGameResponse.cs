namespace Maze.Challenge.Client.Dtos
{
    public class NewGameResponse
    {
        public Guid MazeUid { get; set; }
        public Guid GameUid { get; set; }
        public bool Completed { get; set; }
        public int CurrentPositionY { get; set; }
        public int CurrentPositionX { get; set; }
    }
}

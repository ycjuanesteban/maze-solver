namespace Maze.Challenge.Client.Dtos
{
    public class NewMazeRequest
    {
        public int Width { get; }
        public int Height { get; }

        public NewMazeRequest(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}

namespace Maze.Challenge.Client
{
    public static class Urls
    {
        public static string NEW_MAZE = "/api/maze?code={0}";
        public static string DEBUG_MAZE = "/api/maze/{0}?code={1}";
        public static string NEW_GAME = "/api/game/{0}?code={1}";
        public static string GAME = "/api/game/{0}/{1}?code={2}";
    }
}

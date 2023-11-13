# Maze resolver

## Execute
To execute the project, you just need to open it with Visual Studio and click on play button.

## Configuration
If you want to change a posible configuration parameter, you need to open the `appsettings.json` file that you can find in `Maze.Challenge.Console` project and change it.

This object below is a configuration object example.
```json
{
  "MazeClientSettings": {
    "BaseUrl": "", //Base service url ex: www.maze-challenge.com
    "Code": ""     // secure code, go at the end of each http call
  },
  "MazeApplicationSettings": {
    "Width": , // width of the maze
    "Height":  // height of the maze
  }
}
```

## Algorithm used
To solve the maze problem, I implemented the recursive algorithm that is part of a set of "maze-solving algorithm".

This algorithm test all the posible paths acting like a binary search tree and checking one by one the nodes and your neighbour.

### Problems and future solutions
After iterate many times, the current logic has a problem because the api save the status each time that you make a movement, that is, every time the game is being updated with the newest current position and when the recursion return to a old node to start the new path this can't be execute because that status doesn't exist anymore. 

A possible solution is the wall follower algorithm.
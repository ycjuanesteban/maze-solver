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

## Algorithms used

After search in many sources, the firts place that you will find in [wikipedia](https://en.wikipedia.org/wiki/Maze-solving_algorithm). That information is the principal approach to start the deep search of the information and algorithms.

At the end, I found the book [Mazes for Programmers](https://www.amazon.es/Mazes-Programmers-Twisty-Little-Passages/dp/1680500554) that help me to understand what was the problem when I implement the initial algorithm.

All the next iterations and commits will try to implement a different algorithm and use the `strategy` pattern like the way choose the one that you can try.

* The Recursive Backtracker Algorithm (book chaper 5).
using Common;
using System;

namespace Drawfael.Services
{
    public class BoardService
    {
        private Board Board { get; set; }
        public UserService UserService { get; }

        public event EventHandler<Cell>? CellChanged;
        public BoardService(UserService userService)
        {
            Board = new Board();
            UserService = userService;
            CheckAllOfTheBoard();
            new Timer((s) => { 
                var rnd = s as Random;

                    var x = rnd.Next() % Board.SIZE;
                    var y = rnd.Next() % Board.SIZE;
                ColorCellRequest(x,y,UserService.him);
                
            }, new Random(),0,5000);
        }

        public async Task<Board> GetBoard()
        {
            Console.WriteLine("Fetching Board");
            return Board;
        }

        public async Task<Cell> GetCell(int x, int y)
        {
            if (x < 0 || y < 0 || x > Board.SIZE || y > Board.SIZE)
            {
                return null;
            }
            return Board.Cells[x, y];
        }
        public void ColorCellRequest(int x, int y, User user)
        {
            ChangeCell(x, y, user.Color);
            UserService.UserPlaced(user);
            CheckBoard(user.Color);
            //CheckAllOfTheBoard();
        }
        private void ChangeCell(int x, int y, CellColor color)
        {
            //todo - dont get color, by username.
            //todo - validate can change (time)
            if (x < 0 || y < 0 || x > Board.SIZE || y > Board.SIZE)
            {
                return;
            }

            Board.Cells[x, y].Color = color;

            CellChanged?.Invoke(this, Board.Cells[x, y]);
        }
        private void CheckAllOfTheBoard()
        {
            CheckBoard(CellColor.Red);
            CheckBoard(CellColor.Blue);
            CheckBoard(CellColor.Green);
            CheckBoard(CellColor.Yellow);
        }
        private void CheckBoard(CellColor color)
        {

            bool dfs_fill(Cell[,] matrix, bool[,] visited, bool[,] island, int x,
                int y, int n, int m, CellColor islandColor)
            {

                //if the island reached the edge - then it is not enclosed inside
                if (x < 0 || y < 0 || x >= n || y >= m)
                    return false;

                if (island[x, y] == true || matrix[x, y].Color == islandColor)
                    return true;

                // Mark land as visited and as an island
                visited[x, y] = true;
                island[x, y] = true;


                // Traverse to all adjacent elements
                return dfs_fill(matrix, visited, island, x + 1, y, n, m, islandColor) &&
                dfs_fill(matrix, visited, island, x, y + 1, n, m, islandColor) &&
                dfs_fill(matrix, visited, island, x - 1, y, n, m, islandColor) &&
                dfs_fill(matrix, visited, island, x, y - 1, n, m, islandColor);
            }
            // Function that counts the closed island
            void colorIslands(Cell[,] matrix, int n,
                                         int m, CellColor islandColor)
            {

                // Create bool 2D visited matrix
                // to keep track of visited cell

                // Initially all elements are
                // unvisited.
                bool[,] visited = new bool[n, m];

                // Mark visited all lands
                // that are reachable from edge
                for (int i = 0; i < n; ++i)
                {
                    for (int j = 0; j < m; ++j)
                    {

                        // traverse only unvisited nodes and "water"
                        if (matrix[i, j].Color != islandColor&& visited[i, j] == false)
                        {

                            var island = new bool[n, m];
                            if (dfs_fill(matrix, visited, island, i, j, n, m, islandColor))
                            {
                                for (int k = 0; k < n; ++k)
                                {
                                    for (var l = 0; l < m; ++l)
                                    {
                                        if (island[k, l])
                                        {
                                            ChangeCell(k, l, islandColor);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

            colorIslands(Board.Cells, Board.SIZE, Board.SIZE, color);
        }
    }
}

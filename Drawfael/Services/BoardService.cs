using Common;
using System;

namespace Drawfael.Services
{
    public class BoardService
    {
        private Board Board { get; set; }
        public event EventHandler<Cell> CellChanged;
        public BoardService()
        {
            Board = new Board();
            CheckAllOfTheBoard();
            new Timer(s =>
            {
                //var rnd = s as Random;
                //int x = rnd.Next() % Board.SIZE;
                //int y = rnd.Next() % Board.SIZE;
                //CellColor color = Cell.Colors[rnd.Next() % Cell.Colors.Length];
                //ChangeCell(x, y, color).Wait();


            }, new Random(), 0, 500000);
        }

        public async Task<Board> GetBoard()
        {
            Console.WriteLine("Fetching Board");
            return Board;
        }

        public async Task<Cell> GetCell(int x, int y)
        {
            if(x < 0 || y < 0 || x > Board.SIZE || y> Board.SIZE)
            {
                return null;
            }
            return Board.Cells[x, y];
        }
        public void ColorCellRequest(int x, int y, CellColor color)
        {
            ChangeCell(x, y, color);
            CheckAllOfTheBoard();
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
            //CheckBoard(CellColor.Red);
            //CheckBoard(CellColor.Blue);
            CheckBoard(CellColor.Green);
            //CheckBoard(CellColor.Yellow);
        } 
        private void CheckBoard(CellColor color)
        {
            void dfs(Cell[,] matrix, bool[,] visited, int x,
                    int y, int n, int m, CellColor islandColor)
            {

                // If the land is already visited
                // or there is no land or the
                // coordinates gone out of matrix
                // break function as there
                // will be no islands
                if (x < 0 || y < 0 || x >= n || y >= m
                    || visited[x, y] == true || matrix[x, y].Color != islandColor)
                    return;

                // Mark land as visited
                visited[x, y] = true;


                // Traverse to all adjacent elements
                dfs(matrix, visited, x + 1, y, n, m, islandColor);
                dfs(matrix, visited, x, y + 1, n, m, islandColor);
                dfs(matrix, visited, x - 1, y, n, m, islandColor);
                dfs(matrix, visited, x, y - 1, n, m, islandColor);
            }

            bool dfs_fill(Cell[,] matrix, bool[,] visited, bool[,] island, int x,
                int y, int n, int m, CellColor islandColor)
            {

                //if the island reached the edge - then it is not enclosed inside
                if (x < 0 || y < 0 || x >= n || y >= m) return false;

                if (visited[x, y] == true || matrix[x, y].Color == islandColor)
                    return true;

                // Mark land as visited
                visited[x, y] = true;
                island[x, y] = true;


                // Traverse to all adjacent elements
                return dfs_fill(matrix, visited, island, x + 1, y, n, m, islandColor) &&
                dfs_fill(matrix, visited, island, x, y + 1, n, m, islandColor) &&
                dfs_fill(matrix, visited, island, x - 1, y, n, m, islandColor) &&
                dfs_fill(matrix, visited, island, x, y - 1, n, m, islandColor);
            }
            // Function that counts the closed island
            int colorIslands(Cell[,] matrix, int n,
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

                        // Traverse corners
                        if ((i * j == 0 || i == n - 1 || j == m - 1)
                            && matrix[i, j].Color == islandColor
                            && visited[i, j] == false)
                            dfs(matrix, visited, i, j, n, m, islandColor);
                    }
                }

                // To stores number of closed islands
                int result = 0;

                for (int i = 0; i < n; ++i)
                {
                    for (int j = 0; j < m; ++j)
                    {

                        // If the land not visited
                        // then there will be atleast
                        // one closed island
                        if (visited[i, j] == false
                            && matrix[i, j].Color == islandColor)
                        {
                            result++;

                            // Mark all lands associated
                            // with island visited.
                            dfs(matrix, visited, i, j, n, m, islandColor);

                            //fill inner - define inner as button right?
                            var island = new bool[n, m];
                            var isIsland = dfs_fill(matrix, visited, island, i + 1, j + 1, n, m, islandColor);
                            if (isIsland)
                            {
                                for (int k = 0; k < n; ++k)
                                {
                                    for (int l = 0; l < m; ++l)
                                    {
                                        if (island[k, l])
                                        {
                                            ChangeCell(k, l, color);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Return the readonly count
                return result;
            }

             colorIslands(Board.Cells, Board.SIZE, Board.SIZE, color);
        }
    }
}

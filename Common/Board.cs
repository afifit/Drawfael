using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class Board
    {
        public const int SIZE = 20;

        public Board()
        {
            Cells = new Cell[SIZE, SIZE];
            for(int i = 0; i < SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    Cells[i, j] = new Cell(i,j);
                }
            }
            int topx = 10;
            int topy = 10;
            int width = 4;
            for (int i = 0; i <= width; i++)
            {
                Cells[topx + i, topy].Color = CellColor.Green;
                Cells[topx, topy+ i].Color = CellColor.Green;
                Cells[topx + width, topy+ i].Color = CellColor.Green;
                Cells[topx + i, topy + width].Color = CellColor.Green;

            }
        }

        public Cell[,] Cells { get; }
    }

    public class Cell
    {
        public static CellColor[] Colors = new CellColor[] { CellColor.Blue,CellColor.Red,CellColor.Green,CellColor.Yellow,CellColor.White };
        public Cell(int x, int y)
        {
            Color = CellColor.White;
            //Color = Colors[new Random().Next() % 5];
            Cords = (x, y);
        }

        public (int x ,int y) Cords { get; }
        public CellColor Color { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string UpdatedBy { get; set; }
    }
}

using Common;
using System;

namespace Services
{
    public class BoardService
    {
        private Board Board { get; set; }
        public BoardService()
        {
            Board = new Board();
        }
    }
}

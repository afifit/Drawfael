using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class User
    {
        public string Username { get; set; }
        public DateTime LastColor { get; set; }
        public CellColor Color { get; set; }

        public User()
        {
            Username = "Dor";
            LastColor = DateTime.Now;
            Color = CellColor.Red;
        }
    }
}

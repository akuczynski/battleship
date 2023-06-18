using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class BoardCell : Button
    {
        public BoardCell()
        {
            Dock = DockStyle.Fill;
            Text = string.Empty;
            Size = new Size(30, 30);
        }
    }
}

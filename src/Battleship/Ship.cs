using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public enum ShipLayout
    {
        Horizontal,
        Vertical
    }

    internal class Ship
    {
        public int StartPosX { get; }
        public int StartPosY { get; }
        public int EndPosX { get; }
        public int EndPosY { get; }
        public int Size { get; }

        public Ship(int startPosX, int startPosY, int size, ShipLayout layout)
        {
            StartPosX = startPosX;
            StartPosY = startPosY;
            Size = size;

            if (layout == ShipLayout.Horizontal)
            {
                EndPosX = StartPosX + size;
                EndPosY = StartPosY;
            }
            if (layout == ShipLayout.Vertical)
            {
                EndPosX = StartPosX;
                EndPosY = StartPosY + size;
            }
        } 
    }
}

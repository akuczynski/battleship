using Battleship.Controls;
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

    public enum ShipType
    {
        Battleship = 5,
        Destroyer = 4
    }

    internal class Ship : IDisposable
    {
        public int StartPosX { get; }
        public int StartPosY { get; }
        public int EndPosX { get; }
        public int EndPosY { get; }
        public int Size { get; }
        public bool IsSunk => boardCells.All(x => x.State == BoardState.Uncovered);

        public event EventHandler Sunk;

        public IList<IBoardCell> boardCells { get; private set; }

        public Ship(int startPosX, int startPosY, ShipType shipType, ShipLayout layout)
        {
            StartPosX = startPosX;
            StartPosY = startPosY;
            Size = (int)shipType;

            if (layout == ShipLayout.Horizontal)
            {
                EndPosX = StartPosX + Size;
                EndPosY = StartPosY;
            }
            if (layout == ShipLayout.Vertical)
            {
                EndPosX = StartPosX;
                EndPosY = StartPosY + Size;
            }

            boardCells = new List<IBoardCell>();
        }

        protected virtual void OnShipSunk(EventArgs e)
        {
            Sunk?.Invoke(this, e);
        }         

        public void Dispose()
        {
            foreach (var boardCell in boardCells)
            {
                boardCell.Uncovered -= OnBoardCellUncovered;
            }
        }

        public void AddBoardCell(IBoardCell boardCell)
        {
            boardCells.Add(boardCell);
            boardCell.Uncovered += OnBoardCellUncovered;
        }

        private void OnBoardCellUncovered(object? sender, EventArgs e)
        {
            var boardCell = sender as IBoardCell;

            if (IsSunk)
            {
                OnShipSunk(EventArgs.Empty);
            }

            // react to click event only once ! 
            boardCell!.Uncovered -= OnBoardCellUncovered;
        }
    }
}

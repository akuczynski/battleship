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

    internal class Ship : IDisposable
    {
        public int StartPosX { get; }
        public int StartPosY { get; }
        public int EndPosX { get; }
        public int EndPosY { get; }
        public int Size { get; }
        public bool IsSunk => boardCells.All(x => x.State == BoardState.Uncovered);

        private Action _onShipSunk;

        public IList<BoardCell> boardCells { get; private set; }

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

            boardCells = new List<BoardCell>();
        }
        
        public void SetOnShipSunkAction(Action onShipSunk)
        {
            _onShipSunk = onShipSunk;
        }

        public void Dispose()
        {
            foreach (var boardCell in boardCells)
            {
                boardCell.Click -= OnShipCellClicked;
            }
        }

        public void AddBoardCell(BoardCell boardCell)
        {
            boardCells.Add(boardCell);
            boardCell.Click += OnShipCellClicked;
        }

        private void OnShipCellClicked(object? sender, EventArgs e)
        {
            var boardCell = sender as BoardCell;

            if (IsSunk)
            {
                _onShipSunk();
            }

            // react to click event only once ! 
            boardCell!.Click -= OnShipCellClicked;
        }
    }
}

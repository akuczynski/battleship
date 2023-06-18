using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship.Controls;

namespace Battleship
{

    // 1 x Battleship  (5 squares)
    // 2 x Destroyers ( 4 squares) 

    internal class GameBoard
    {
        public IReadOnlyList<Ship> Ships => _ships; 

        private bool[,] _gameMatrix;

        private List<Ship> _ships = new List<Ship>();

        public void StartNewGame(IEnumerator cells)
        {
            GenerateNewBoard();

            InitializeBoardCells(cells); 
        }

        private void InitializeBoardCells(IEnumerator cells)
        {
            while (cells.MoveNext())
            {
                var boardCell = (BoardCell)cells.Current;

                boardCell.Init(BoardCellType.ShipCell);
            }

            cells.Reset();
        }

        private void GenerateNewBoard()
        {
            // clean board
            _gameMatrix = new bool[10, 10];
            _ships = new List<Ship>();

            // create ships 
            var battleship = new Ship(0, 0, 5, ShipLayout.Horizontal);
            _ships.Add(battleship);

            AddShipToTheBoard(battleship); 
        }

        private void AddShipToTheBoard(Ship ship)
        {
            // for horizontal ship layout 
            for(int i = ship.StartPosX; i <= ship.EndPosX; i++)
                _gameMatrix[i, ship.StartPosY] = true;

            // for vertical ship layout
            for (int j = ship.StartPosY; j <= ship.EndPosY; j++)
                _gameMatrix[ship.StartPosX, j] = true;

        }
    } 
}

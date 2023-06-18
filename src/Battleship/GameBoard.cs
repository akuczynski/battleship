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
        private bool[,] _gameMatrix;

        private List<Ship> _ships;

        private Label _gameStatus;

        public GameBoard(Label gameStatus)
        {
            _gameMatrix = new bool[10, 10];
            _ships = new List<Ship>();
            _gameStatus = gameStatus;
        }

        public void StartNewGame(TableLayoutPanel gamePanel)
        {
            GenerateNewBoard();
            InitializeBoardCells(gamePanel);

            _gameStatus.Text = "Number of uncovered ships: 3"; 
        }

        private void InitializeBoardCells(TableLayoutPanel gamePanel)
        {
            var cells = gamePanel.Controls.GetEnumerator();

            while (cells.MoveNext())
            {
                var boardCell = (BoardCell)cells.Current;
                var row = gamePanel.GetRow(boardCell);
                var column = gamePanel.GetColumn(boardCell);

                var hasShip = _gameMatrix[column, row];
                boardCell.Init(hasShip ? BoardCellType.ShipCell: BoardCellType.Empty);
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
            for(int i = ship.StartPosX; i < ship.EndPosX; i++)
                _gameMatrix[i, ship.StartPosY] = true;

            // for vertical ship layout
            for (int j = ship.StartPosY; j < ship.EndPosY; j++)
                _gameMatrix[ship.StartPosX, j] = true;

        }
    } 
}

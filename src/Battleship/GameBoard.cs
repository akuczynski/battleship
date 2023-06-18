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
            UpdateGameStatusLabel();
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

                if (hasShip)
                {
                    var ship = GetShipOnPosXY(column, row);

                    ship.AddBoardCell(boardCell);
                    boardCell.Init(BoardCellType.ShipCell);

                }
                else
                {
                    boardCell.Init(BoardCellType.Empty);
                }
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
            var destroyer1 = new Ship(0, 1, 4, ShipLayout.Horizontal);
            var destroyer2 = new Ship(0, 2, 4, ShipLayout.Horizontal);

            _ships.Add(battleship);
            _ships.Add(destroyer1);
            _ships.Add(destroyer2);

            _ships.ForEach(ship =>
            {
                ship.SetOnShipSunkAction(() => UpdateGameStatusLabel());
                AddShipToTheBoard(ship);
            });
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

        private Ship? GetShipOnPosXY(int column, int row)
        {
            foreach(var ship in _ships)
            {
                if (column >= ship.StartPosX && column <= ship.EndPosX)
                    if (row >= ship.StartPosY && row <= ship.EndPosY)
                        return ship;
            }

            return null;
        }

        private void UpdateGameStatusLabel()
        {
            var numberOfUncoveredShips =  _ships.Where(x => !x.IsSunk).Count();

            if (numberOfUncoveredShips == 0) 
            {
                _gameStatus.Text = $"Congratulations !\r\nThere are no more ships to uncover.";
            }
            else
            {
                _gameStatus.Text = $"Number of uncovered ships: {numberOfUncoveredShips}";
            }           
        }
    } 
} 

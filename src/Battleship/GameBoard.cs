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

#if DEBUG
                boardCell.PerformClick();  //    for debug only 
#endif
            }

            cells.Reset();
        }


        private void GenerateNewBoard()
        {
            // clean board
            _gameMatrix = new bool[10, 10];
            _ships = new List<Ship>();

            // create ships 
            CreateNewShip(ShipType.Battleship); 
            CreateNewShip(ShipType.Destroyer); 
            CreateNewShip(ShipType.Destroyer); 

            _ships.ForEach(ship =>
            {
                ship.SetOnShipSunkAction(() => UpdateGameStatusLabel());
                AddShipToTheBoard(ship);
            });
        }

        private void AddShipToTheBoard(Ship ship)
        {
            // for horizontal ship layout 
            for (int i = ship.StartPosX; i < ship.EndPosX; i++)
                _gameMatrix[i, ship.StartPosY] = true;

            // for vertical ship layout
            for (int j = ship.StartPosY; j < ship.EndPosY; j++)
                _gameMatrix[ship.StartPosX, j] = true;
        }

        private Ship? GetShipOnPosXY(int column, int row)
        {
            foreach (var ship in _ships)
            {
                if (column >= ship.StartPosX && column <= ship.EndPosX)
                    if (row >= ship.StartPosY && row <= ship.EndPosY)
                        return ship;
            }

            return null;
        }

        private void CreateNewShip(ShipType shipType)
        {
            Random random = new Random();
            int startPosX, startPosY;
            var isOverlappingPosition = false;
            int size = (int) shipType ;

            ShipLayout layout;

            do
            {
                // generate some random position for a new ship 
                layout = random.Next() % 2 == 0 ? ShipLayout.Horizontal : ShipLayout.Vertical;

                if (layout == ShipLayout.Horizontal)
                {
                    startPosX = random.Next(0, 9 - size);
                    startPosY = random.Next(0, 9);
                }
                else
                {
                    startPosX = random.Next(0, 9);
                    startPosY = random.Next(0, 9 - size);
                }

                // check ships position overlapping 
                isOverlappingPosition = false;
                if (layout == ShipLayout.Horizontal)
                {
                    for (int i = Math.Max(startPosX - 1, 0); i < Math.Min(startPosX + size + 1, 9); i++)
                    {
                        // there should be one empty cell between ships 
                        if (GetShipOnPosXY(i, startPosY) != null
                         || GetShipOnPosXY(i, startPosY - 1) != null
                         || GetShipOnPosXY(i, startPosY + 1) != null)
                        {
                            isOverlappingPosition = true;
                            break;
                        }
                    }
                } 
                else
                {
                    for (int j = Math.Max(startPosY - 1, 0); j < Math.Min(startPosY + size + 1, 9); j++)
                    {
                        // there should be one empty cell between ships 
                        if (GetShipOnPosXY(startPosX, j) != null 
                        || GetShipOnPosXY(startPosX - 1, j) != null
                        || GetShipOnPosXY(startPosX + 1, j) != null)
                        {
                            isOverlappingPosition = true;
                            break;
                        }
                    }
                }

            } while (isOverlappingPosition);

            _ships.Add(new Ship(startPosX, startPosY, size, layout));
        }


        private void UpdateGameStatusLabel()
        {
            var numberOfUncoveredShips = _ships.Where(x => !x.IsSunk).Count();

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

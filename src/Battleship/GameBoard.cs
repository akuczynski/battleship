using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship.Controls;

namespace Battleship
{
    // Game board has only 3 ships: 
    // 1 x Battleship  (5 squares)
    // 2 x Destroyers ( 4 squares) 
    internal class GameBoard
    {
        private bool[,] _gameMatrix;

        private List<Ship> _ships;
        
        private Label _gameStatus;

        private TableLayoutPanel _gamePanel;

        public GameBoard(Label gameStatus, TableLayoutPanel gamePanel)
        {
            _gameStatus = gameStatus;
            _gamePanel = gamePanel; 
        }

        public void StartNewGame()
        {
            InitializeBoardCells();
            GenerateNewBoard();
            UpdateGameStatusLabel();

           // TestMode(); // this is only for the debugging
        }

        private void InitializeBoardCells()
        {
            var cells = _gamePanel.Controls.GetEnumerator();

            while (cells.MoveNext())
            {
                var boardCell = (BoardCell)cells.Current;
                boardCell.Init(BoardCellType.Empty);
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
        }

        private void CreateNewShip(ShipType shipType)
        {
            Random random = new Random();
            int startPosX, startPosY;
            var isOverlappingPosition = false;
            int size = (int)shipType;

            ShipLayout layout;

            do
            {
                // generate some random position for a new ship 
                layout = random.Next() % 2 == 0 ? ShipLayout.Horizontal : ShipLayout.Vertical;

                if (layout == ShipLayout.Horizontal)
                {
                    startPosX = random.Next(0, 11 - size);
                    startPosY = random.Next(0, 10);
                }
                else
                {
                    startPosX = random.Next(0, 10);
                    startPosY = random.Next(0, 11 - size);
                }

                // check ships position overlapping 
                isOverlappingPosition = false;
                if (layout == ShipLayout.Horizontal)
                {
                    for (int i = Math.Max(startPosX - 1, 0); i < Math.Min(startPosX + size + 1, 9); i++)
                    {
                        // there should be one empty cell between ships 
                        if (IsThereAShipOnPosXY(i, startPosY) 
                         || IsThereAShipOnPosXY(i, startPosY - 1) 
                         || IsThereAShipOnPosXY(i, startPosY + 1))
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
                        if (IsThereAShipOnPosXY(startPosX, j) 
                        ||  IsThereAShipOnPosXY(startPosX - 1, j) 
                        ||  IsThereAShipOnPosXY(startPosX + 1, j))
                        {
                            isOverlappingPosition = true;
                            break;
                        }
                    }
                }

            } while (isOverlappingPosition);

            var ship = new Ship(startPosX, startPosY, size, layout);
            ship.SetOnShipSunkAction(() => UpdateGameStatusLabel());

            AddShipToTheBoard(ship);
        }

        private void AddShipToTheBoard(Ship ship)
        {
            _ships.Add(ship);

            // for horizontal ship layout 
            for (int i = ship.StartPosX; i < ship.EndPosX; i++)
            {
                _gameMatrix[i, ship.StartPosY] = true;
                var boardCell = (BoardCell)_gamePanel.GetControlFromPosition(i, ship.StartPosY);
                ship.AddBoardCell(boardCell);
                boardCell.Init(BoardCellType.ShipCell);
            }

            // for vertical ship layout
            for (int j = ship.StartPosY; j < ship.EndPosY; j++)
            {
                _gameMatrix[ship.StartPosX, j] = true;
                var boardCell = (BoardCell)_gamePanel.GetControlFromPosition(ship.StartPosX, j);
                ship.AddBoardCell(boardCell);
                boardCell.Init(BoardCellType.ShipCell);
            }
        }

        private bool IsThereAShipOnPosXY(int posX, int posY)
        {
            if (posX < 0 || posX > 9 || posY < 0 || posY > 9)
            {
                return false;
            }

            return _gameMatrix[posX, posY];
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

        private void TestMode()
        {
            var cells = _gamePanel.Controls.GetEnumerator();
            while (cells.MoveNext())
            {
                var boardCell = (BoardCell)cells.Current;
                boardCell.PerformClick();
            }

            cells.Reset();
        }
    }
}

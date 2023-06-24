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
    internal class GameBoard : IDisposable 
    {
        private bool[,] _gameMatrix;

        private List<Ship> _ships;
        
        private IAppGUI _appGUI;

        public GameBoard(IAppGUI appGUI)
        {
            _appGUI = appGUI; 
        }

        public void StartNewGame()
        {
            InitializeBoardCells();
            GenerateNewBoard();
            UpdateGameStatusLabel();
        }

        public void Dispose()
        {
            _ships.ForEach(x => UnsubscribeToShipEvents(x));
        }

        private void InitializeBoardCells()
        {
            _appGUI.CleanBoard();
        }

        private void GenerateNewBoard()
        {
            // clean board
            _gameMatrix = new bool[10, 10];
            _ships?.ForEach( ship => UnsubscribeToShipEvents(ship));
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

            var ship = new Ship(startPosX, startPosY, shipType, layout);
            SubscribeToShipEvents(ship);

            AddShipToTheBoard(ship);
        }

        private void AddShipToTheBoard(Ship ship)
        {
            _ships.Add(ship);

            // for horizontal ship layout 
            for (int i = ship.StartPosX; i < ship.EndPosX; i++)
            {
                _gameMatrix[i, ship.StartPosY] = true;
                var boardCell = _appGUI.GetBoardCellFromPosition(i, ship.StartPosY);
                ship.AddBoardCell(boardCell);
                boardCell.Init(BoardCellType.ShipCell);
            }

            // for vertical ship layout
            for (int j = ship.StartPosY; j < ship.EndPosY; j++)
            {
                _gameMatrix[ship.StartPosX, j] = true;
                var boardCell = _appGUI.GetBoardCellFromPosition(ship.StartPosX, j);
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
                _appGUI.SetMessage($"Congratulations !\r\nThere are no more ships to uncover.");
            }
            else
            {
                _appGUI.SetMessage($"Number of uncovered ships: {numberOfUncoveredShips}");
            }
        }

        private void SubscribeToShipEvents(Ship ship)
        {
            ship.Sunk += OnShipSunk;
        }

        private void UnsubscribeToShipEvents(Ship ship)
        {
            ship.Sunk -= OnShipSunk;
        }

        private void OnShipSunk(object? sender, EventArgs e)
        {
            UpdateGameStatusLabel();
        }
    }
}

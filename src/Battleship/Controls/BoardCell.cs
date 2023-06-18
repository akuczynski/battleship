namespace Battleship.Controls
{
    public enum BoardCellType
    {
        Empty,

        ShipCell
    }

    public enum BoardState
    {
        Covered,

        Uncovered
    }

    public class BoardCell : Button
    {
        public BoardCellType Type { get; }

        public BoardState State { get; }

        public int Row { get; set; }

        public int Column { get; set; }

        public BoardCell()
        {
            State = BoardState.Covered;
        }
    }
}

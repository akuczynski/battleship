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
        public BoardCellType Type { get; private set; }

        public BoardState State { get; private set; }

        public int Row { get; set; }

        public int Column { get; set; }

        public BoardCell()
        {
            this.Click += BoardCell_Click;
        }

        public new void Dispose()
        {
            this.Click -= BoardCell_Click;
            base.Dispose();
        }

        public void Init(BoardCellType type)
        {
            Type = type;

            State = BoardState.Covered;
            BackColor = Button.DefaultBackColor;
        }

        public void Uncover()
        {
            State = BoardState.Uncovered;
            
            if (Type == BoardCellType.Empty)
            {
                BackColor = Color.Blue;
            }
            else if (Type == BoardCellType.ShipCell)
            {
                BackColor = Color.Red;
            }
        }

        private void BoardCell_Click(object? sender, EventArgs e)
        {
            var boardCell = sender as BoardCell;
            boardCell?.Uncover();
        }
    }
}

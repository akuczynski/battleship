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

        public BoardCell()
        {
            Enabled = false;
            Click += BoardCell_Click;
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
            Enabled = true;
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

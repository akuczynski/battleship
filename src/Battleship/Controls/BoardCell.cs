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

    public interface IBoardCell
    {
       void Init(BoardCellType type);

       event EventHandler Uncovered;

       BoardState State { get; }
    }

    public class BoardCell : Button, IBoardCell
    {
        public BoardCellType Type { get; private set; }

        public BoardState State { get; private set; }

        public event EventHandler? Uncovered;

        public BoardCell()
        {
            Enabled = false;
            Click += BoardCell_Click;
        }
      
        public void Init(BoardCellType type)
        {
            Type = type;

            State = BoardState.Covered;
            BackColor = Button.DefaultBackColor;
            Enabled = true;
        }

        public new void Dispose()
        {
            this.Click -= BoardCell_Click;
            base.Dispose();
        }

        private void BoardCell_Click(object? sender, EventArgs e)
        {
            var boardCell = sender as BoardCell;
            boardCell?.OnUncover(EventArgs.Empty);
        }


        protected virtual void OnUncover(EventArgs e)
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

            Uncovered?.Invoke(this, e);
        }
    }
}

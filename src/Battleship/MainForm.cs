using Battleship.Controls;

namespace Battleship
{
    public interface IAppGUI
    {
        void SetMessage(string msg);

        void CleanBoard();

        IBoardCell GetBoardCellFromPosition(int column, int row);
    }

    public partial class MainForm : Form, IAppGUI
    {
        private GameBoard _gameBoard;

        public MainForm()
        {
            InitializeComponent();
            _gameBoard = new GameBoard(this);
        }

        public IBoardCell GetBoardCellFromPosition(int column, int row)
        {
            return (IBoardCell)GameBoardPanel.GetControlFromPosition(column, row);
        }

        public void CleanBoard()
        {
            var cells = GameBoardPanel.Controls.GetEnumerator();

            while (cells.MoveNext())
            {
                var boardCell = (BoardCell)cells.Current;
                boardCell.Init(BoardCellType.Empty);
            }

            cells.Reset();
        }


        public void SetMessage(string msg)
        {
            GameStatusLabel.Text = msg;
        }

        private void NewGameButton_Click_1(object sender, EventArgs e)
        {
            _gameBoard.StartNewGame();
        }
    }
}
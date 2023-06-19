using Battleship.Controls;

namespace Battleship
{
    public partial class MainForm : Form
    {
        private GameBoard _gameBoard;

        public MainForm()
        {
            InitializeComponent();
            _gameBoard = new GameBoard(GameStatusLabel, GameBoardPanel);
        }

        private void NewGameButton_Click_1(object sender, EventArgs e)
        {
            _gameBoard.StartNewGame();
        }
    }
}